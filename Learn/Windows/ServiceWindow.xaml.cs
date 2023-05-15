using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Learn.Windows
{
    /// <summary>
    /// Логика взаимодействия для ServiceWindow.xaml
    /// </summary>
    public partial class ServiceWindow : Window
    {
        private readonly LearnEntities _db;
        public Services Service;

        private Session _session = Session.Get();

        private ObservableCollection<Photos> _photos;

        public ServiceWindow(int serviceId)
        {
            InitializeComponent();

            _db = new LearnEntities();
            Service = _db.Services.Where(x => x.Id == serviceId).FirstOrDefault();

            if (Service == null)
            {
                MessageBox.Show("Услуга не найдена!");
                Close();
            }
            DataContext = Service;

            InitializeFieldsFromService(Service);

            _photos = new ObservableCollection<Photos>(new List<Photos>() { Service.Photos }.Concat(Service.ServicePhotos.Select(x => x.Photos)));
            images.ItemsSource = _photos;

            if (!_session.IsAdmin)
            {
                DisableInputs();
            }
        }

        private void InitializeFieldsFromService(Services service)
        {
            serviceId.Text = service.Id.ToString();
            serviceName.Text = service.ServiceName;
            serviceCost.Text = service.Cost.ToString();
            serviceDiscount.Text = service.CurrentDiscount.ToString();
            serviceDuration.Text = service.DurationMinutes.ToString();
        }

        private void DisableInputs()
        {
            var toDisable = new List<TextBox>()
            {
                serviceName, serviceCost, serviceDiscount, serviceDuration
            };

            foreach (var tb in toDisable)
            {
                tb.IsEnabled = false;
            }

            changeAdditionalImages.IsEnabled = false;
            changeMainImage.IsEnabled = false;
            save.Visibility = Visibility.Collapsed;
            register.Visibility = Visibility.Collapsed;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            Service.ServiceName = serviceName.Text;
            Service.Cost = int.Parse(serviceCost.Text);
            Service.CurrentDiscount = int.Parse(serviceDiscount.Text);
            Service.DurationMinutes = int.Parse(serviceDuration.Text);

            try
            {
                _db.SaveChanges();
            } catch
            {
                foreach (var ve in _db.GetValidationErrors())
                {
                    foreach (var m in ve.ValidationErrors)
                    {
                        MessageBox.Show(m.ErrorMessage);
                    }
                }
                return;
            }

            DialogResult = true;
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            var window = new CreateServiceClientWindow(Service.Id);
            window.ShowDialog();
        }

        private void changeMainImage_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "JPEG Images|*.jpeg";

            var result = ofd.ShowDialog();

            if (result == true)
            {
                var data = File.ReadAllBytes(ofd.FileName);
                Service.Photos = new Photos()
                {
                    ImagePath = ofd.FileName.Substring(0, ofd.FileName.Length > 250 ? 250 : ofd.FileName.Length),
                    ImageData = data
                };
                _db.SaveChanges();
            }
        }

        private void changeAdditionalImages_Click(object sender, RoutedEventArgs e)
        {
            var window = new ServiceAdditionalImagesWindow(Service.Id);
            window.ShowDialog();
        }
    }
}

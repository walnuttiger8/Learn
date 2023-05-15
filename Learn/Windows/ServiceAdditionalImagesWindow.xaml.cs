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
    /// Логика взаимодействия для ServiceAdditionalImagesWindow.xaml
    /// </summary>
    public partial class ServiceAdditionalImagesWindow : Window
    {
        private readonly LearnEntities _db;
        private int _serviceId;
        private bool _updated = false;

        public ServiceAdditionalImagesWindow(int serviceId)
        {
            InitializeComponent();
            _db = new LearnEntities();

            _serviceId = serviceId;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _db.ServicePhotos.Where(x => x.ServiceId == _serviceId).Load();
            images.ItemsSource = _db.ServicePhotos.Local;
        }

        private void images_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (images.SelectedItem != null)
            {
                deleteImage.IsEnabled = true;
            }
        }

        private void deleteImage_Click(object sender, RoutedEventArgs e)
        {
            var image = images.SelectedItem as ServicePhotos;
            if (image == null)
            {
                MessageBox.Show("Выберите изображение");
                return;
            }
            _db.ServicePhotos.Remove(image);
            _db.SaveChanges();
            _updated = true;
        }

        private void addImage_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "JPEG Images|*.jpeg";

            var result = ofd.ShowDialog();

            if (result == true)
            {
                var data = File.ReadAllBytes(ofd.FileName);
                var servicePhoto = new ServicePhotos()
                {
                    Photos = new Photos()
                    {
                        ImagePath = ofd.FileName.Substring(0, ofd.FileName.Length > 250 ? 250 : ofd.FileName.Length),
                        ImageData = data
                    },
                    ServiceId = _serviceId
                };
                _db.ServicePhotos.Add(servicePhoto);
                _db.SaveChanges();
                _updated = true;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DialogResult = _updated;
        }
    }
}

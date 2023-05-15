using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для CreateServiceClientWindow.xaml
    /// </summary>
    public partial class CreateServiceClientWindow : Window
    {
        private readonly LearnEntities _db;
        private Services _service;

        public CreateServiceClientWindow(int serviceId)
        {
            InitializeComponent();

            _db = new LearnEntities();

            _service = _db.Services.Where(x => x.Id == serviceId).FirstOrDefault();
            if (serviceDate == null)
            {
                MessageBox.Show("Услуга не найдена!");
                Close();
            }

            InitializeServiceInfo();
            InitializeClients();
        }

        private void InitializeServiceInfo()
        {
            serviceName.Text = _service.ServiceName;
            serviceDuration.Text = _service.DurationMinutes.ToString();
        }

        private void InitializeClients()
        {
            client.ItemsSource = _db.Clients.ToList();
        }

        private int GetServiceTimeMinutes()
        {
            var hour = int.Parse(serviceTime.Text.Split(':')[0]);
            var minute = int.Parse(serviceTime.Text.Split(':')[1]);

            return hour * 60 + minute;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (client.SelectedItem == null)
            {
                MessageBox.Show("Необходимо выбрать клиента");
                return;
            }

            DateTime date;
            try
            {
                date = DateTime.Parse(serviceDate.Text);
                date.AddMinutes(GetServiceTimeMinutes());
            } catch
            {
                MessageBox.Show("Дата или время заполнены некорректно");
                return;
            }
            

            var serviceClient = new ServiceClients()
            {
                ServiceId = _service.Id,
                ClientId = (client.SelectedItem as Clients).Id,
                ServiceStartDate = date,
            };

            try
            {
                _db.ServiceClients.Add(serviceClient);
                _db.SaveChanges();
                DialogResult = true;
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
        }
    }
}

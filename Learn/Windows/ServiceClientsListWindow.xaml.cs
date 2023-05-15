using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Threading;

namespace Learn.Windows
{
    /// <summary>
    /// Логика взаимодействия для ServiceClientsListWindow.xaml
    /// </summary>
    public partial class ServiceClientsListWindow : Window
    {
        private readonly LearnEntities _db;
        public ServiceClientsListWindow()
        {
            InitializeComponent();
            _db = new LearnEntities();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayServiceClients();

            var timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 30);
            timer.Tick += new EventHandler(OnTimer);
            timer.Start();
        }

        private void OnTimer(object sender, EventArgs e)
        {
            DisplayServiceClients();
        }

        private void DisplayServiceClients()
        {
            _db.ServiceClients.Load();
            serviceClients.ItemsSource = _db.ServiceClients.Local.Where(x => x.ServiceStartDate > DateTime.Now).OrderBy(x => x.ServiceStartDate);
            lastUpdateDate.Text = DateTime.Now.ToString();
        }

        private void leftUntil_Initialized(object sender, EventArgs e)
        {
            var current = sender as TextBlock;
            var serviceClient = current.DataContext as ServiceClients;

            var left = serviceClient.ServiceStartDate.Subtract(DateTime.Now);

            current.Text = $"{Math.Floor(left.TotalHours)} часов {left.Minutes} минут";

            if (left.TotalMinutes < 60)
            {
                current.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}

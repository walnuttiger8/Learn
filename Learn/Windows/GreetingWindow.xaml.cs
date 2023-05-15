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
    /// Логика взаимодействия для GreetingWindow.xaml
    /// </summary>
    public partial class GreetingWindow : Window
    {
        private Session _session = Session.Get();
        public GreetingWindow()
        {
            InitializeComponent();
        }

        private void continueAsGuest_Click(object sender, RoutedEventArgs e)
        {
            OpenServicesList();
            Close();
        }

        private void continueAsAdmin_Click(object sender, RoutedEventArgs e)
        {
            var window = new AdministratorAuthorizationWindow();
            var result = window.ShowDialog();
            if (result == true)
            {
                _session.IsAdmin = true;
                OpenServicesList();
                
            } else
            {
                MessageBox.Show("Неверные данные");
            }
        }

        private void OpenServicesList()
        {
            var window = new ServicesListWindow();
            window.Show();
            Close();
        }
    }
}

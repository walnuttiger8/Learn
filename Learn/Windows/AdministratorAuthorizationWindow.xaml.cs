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
    /// Логика взаимодействия для AdministratorAuthorizationWindow.xaml
    /// </summary>
    public partial class AdministratorAuthorizationWindow : Window
    {
        private const string ADMIN_CODE = "0000";
        public AdministratorAuthorizationWindow()
        {
            InitializeComponent();
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            var passwordCode = password.Password;

            if (passwordCode == ADMIN_CODE)
            {
                DialogResult = true;
            } else
            {
                DialogResult = false;
            }
        }
    }
}

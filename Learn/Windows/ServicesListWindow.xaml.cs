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
    /// Логика взаимодействия для ServicesListWindow.xaml
    /// </summary>

    public partial class ServicesListWindow : Window
    {
        private readonly LearnEntities _db;
        private readonly int _totalCount;
        private Session _session = Session.Get();

        public ServicesListWindow()
        {
            
            InitializeComponent();

            _db = new LearnEntities();

            _totalCount = _db.Services.Count();

            totalCount.Text = _totalCount.ToString();

            LoadServices();

            if (!_session.IsAdmin)
            {
                recentServiceClients.Visibility = Visibility.Collapsed;
            }
        }

        private void TextBlock_Initialized(object sender, EventArgs e)
        {
            var current = sender as TextBlock;
            var service = current.DataContext as Services;

            if (service.CurrentDiscount != null && service.CurrentDiscount > 0)
            {
                current.Visibility = Visibility.Visible;
                var discountPercent = (double)service.CurrentDiscount / 100d;
                var price = service.Cost - (service.Cost * discountPercent);
                current.Text = price.ToString();
            }
        }

        private void StackPanel_Initialized(object sender, EventArgs e)
        {
            var current = sender as StackPanel;
            var service = current.DataContext as Services;

            if (service.CurrentDiscount == 0)
            {
                current.Visibility= Visibility.Collapsed;
            }
        }

        private void cost_Initialized(object sender, EventArgs e)
        {
            var current = sender as TextBlock;
            var service = current.DataContext as Services;

            if (service.CurrentDiscount != null && service.CurrentDiscount > 0)
            {
                current.TextDecorations = TextDecorations.Strikethrough;
            }
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            var current = sender as Button;
            var service = current.DataContext as Services;

            var window = new ServiceWindow(service.Id);
            var result = window.ShowDialog();

            if (result == true)
            {
                LoadServices();
                
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            var current = sender as Button;
            var service = current.DataContext as Services;

            if (service.ServiceClients.Any())
            {
                MessageBox.Show("Удаление запрещено!");
                return;
            }
        }

        private void LoadServices()
        {
            if (_db == null)
            {
                return;
            }

            var s = _db.Services.AsQueryable();

            if (searchText.Text.Any())
            {
                s = s.Where(x => x.ServiceName.Contains(searchText.Text));
            }
            switch (filter.SelectedIndex)
            {
                case 1: // 0 - 5 
                    s = s.Where(x => 0 <= x.CurrentDiscount && x.CurrentDiscount <= 5);
                    break;
                case 2: // 5 - 15
                    s = s.Where(x => 5 <= x.CurrentDiscount && x.CurrentDiscount <= 15);
                    break;
                case 3: // 15 - 30
                    s = s.Where(x => 15 <= x.CurrentDiscount && x.CurrentDiscount <= 30);
                    break;
                case 4: // 30 - 70
                    s = s.Where(x => 30 <= x.CurrentDiscount && x.CurrentDiscount <= 70);
                    break;
                case 5: // 70 - 100 
                    s = s.Where(x => 70 <= x.CurrentDiscount && x.CurrentDiscount <= 100);
                    break;
            };

            switch(costSort.SelectedIndex)
            {
                case 0:
                    s = s.OrderBy(x => x.Cost);
                    break;
                case 1:
                    s = s.OrderByDescending(x => x.Cost);
                    break;
            }

            services.ItemsSource = s.ToList();
            currentCount.Text = s.Count().ToString();

        }

        private void searchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadServices();
        }

        private void costSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadServices();
        }

        private void filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadServices();
        }

        private void delete_Initialized(object sender, EventArgs e)
        {
            var current = sender as Button;

            if (!_session.IsAdmin)
            {
                current.Visibility = Visibility.Collapsed;
            }
        }

        private void edit_Initialized(object sender, EventArgs e)
        {
            var current = sender as Button;

            if (!_session.IsAdmin)
            {
                current.Content = "Открыть";
            }
        }

        private void recentServiceClients_Click(object sender, RoutedEventArgs e)
        {
            var window = new ServiceClientsListWindow();
            window.Show();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            _session.IsAdmin = false;
            var window = new GreetingWindow();
            window.Show();
            Close();
        }
    }
}

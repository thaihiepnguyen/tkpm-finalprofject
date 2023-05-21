using MyShop.BUS;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyShop.UI.MainPage.Pages
{
    /// <summary>
    /// Interaction logic for Customer.xaml
    /// </summary>
    public partial class Customer : Page
    {
        Frame _pageNavigation;
        CustomerBUS _customerBUS;
        private ObservableCollection<CustomerDTO>? _customers = null;
        private string _currentKey = "";
        private int _currentPage = 1;
        private int _rowsPerPage = 8;
        private int _totalItems = 0;
        private int _totalPages = 0;
        public Customer(Frame pageNavigation)
        {
            _pageNavigation = pageNavigation;
            _customerBUS = new CustomerBUS();
            _customers = _customerBUS.getAll();
            InitializeComponent();
        }

        class CustomerResources
        {
            public string FirstIcon { get; set; }
            public string LastIcon { get; set; }
            public string NextIcon { get; set; }
            public string PrevIcon { get; set; }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            updateDataSource();
            updatePagingInfo();
            this.DataContext = new CustomerResources()
            {
                FirstIcon = "Assets/Images/ic-first.png",
                LastIcon = "Assets/Images/ic-last.png",
                NextIcon = "Assets/Images/ic-next.png",
                PrevIcon = "Assets/Images/ic-prev.png"
            };
        }

        private void updateDataSource(int page = 1, string keyword = "")
        {
            _currentPage = page;

            (_customers, _totalItems) = _customerBUS.findCustomersBySearch(_currentPage, _rowsPerPage, keyword);

            // search xong thì gán lại rỗng
            if (keyword.Length != 0)
            {
                SearchTermTextBox.Text = keyword;
            }

            customersListView.ItemsSource = _customers;

            infoTextBlock.Text = $"Đang hiển thị {_customers.Count} trên tổng số {_totalItems} khách hàng";
        }

        private void updatePagingInfo()
        {
            _totalPages = _totalItems / _rowsPerPage +
                   (_totalItems % _rowsPerPage == 0 ? 0 : 1);

            pageInfoTextBlock.Text = $"{_currentPage}/{_totalPages}";
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.Navigate(new AddCustomer(_pageNavigation));
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int i = customersListView.SelectedIndex;

            var customer = _customers[i];
            if (customer != null)
            {
                _pageNavigation.NavigationService.Navigate(new CustomerDetail(customer, _pageNavigation));
            }
        }

        private void FirstButton_Click(object sender, RoutedEventArgs e)
        {
            updateDataSource(1, _currentKey);
            updatePagingInfo();
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                updateDataSource(_currentPage, _currentKey);
                updatePagingInfo();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                updateDataSource(_currentPage, _currentKey);
            }
        }

        private void LastButton_Click(object sender, RoutedEventArgs e)
        {
            updateDataSource(_totalPages, _currentKey);
            updatePagingInfo();
        }

        private void SearchTermTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string keyword = SearchTermTextBox.Text;
                _currentKey = keyword;

                updateDataSource(1, keyword);
                updatePagingInfo();
            }
        }
    }
}

using MyShop.BUS;
using MyShop.DTO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyShop.UI.MainPage.Pages
{
    /// <summary>
    /// Interaction logic for UpdateCustomer.xaml
    /// </summary>
    public partial class UpdateCustomer : Page
    {
        Frame _pageNavigation;
        CustomerDTO _currentCustomer;
        CustomerBUS _customerBUS;
        public UpdateCustomer(CustomerDTO currentCustomer, Frame pageNavigation)
        {
            _pageNavigation = pageNavigation;
            InitializeComponent();

            _currentCustomer= currentCustomer;
            _customerBUS = new CustomerBUS();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.GoBack();
        }

        private void SaveCustomer_Click(object sender, RoutedEventArgs e)
        {
            _currentCustomer.CusName = NameTermTextBox.Text;
            _currentCustomer.Tel = PhoneTermTextBox.Text;
            _currentCustomer.Address = AddressTermTextBox.Text.Trim();

            _customerBUS.patchCustomer(_currentCustomer);

            MessageBox.Show("Khách hàng đã cập nhật thành công", "Thông báo", MessageBoxButton.OK);
            _pageNavigation.NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = _currentCustomer;
        }
    }
}

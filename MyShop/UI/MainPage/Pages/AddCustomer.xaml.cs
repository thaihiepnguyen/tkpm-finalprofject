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
    /// Interaction logic for AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Page
    {
        Frame _pageNavigation;
        CustomerBUS _customerBUS;
        public AddCustomer(Frame pageNavigation)
        {
            _pageNavigation = pageNavigation;
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.GoBack();
        }

        private void SaveCustomer_Click(object sender, RoutedEventArgs e)
        {
            CustomerDTO customerDTO = new CustomerDTO();

            customerDTO.CusName = NameTermTextBox.Text;
            customerDTO.Tel = PhoneTermTextBox.Text;
            customerDTO.Address = AddressTermTextBox.Text.Trim();

            _customerBUS.addCustomer(customerDTO);

            MessageBox.Show("Khách hàng đã thêm thành công", "Thông báo", MessageBoxButton.OK);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _customerBUS = new CustomerBUS();
        }
    }
}

using MyShop.BUS;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            if (!isDataValid())
            {
                MessageBox.Show("Vui lòng nhập giá trị hợp lệ!!", "Thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CustomerDTO customerDTO = new CustomerDTO();

            customerDTO.CusName = NameTermTextBox.Text;
            customerDTO.Tel = PhoneTermTextBox.Text;
            customerDTO.Address = AddressTermTextBox.Text.Trim();

            _customerBUS.addCustomer(customerDTO);

            MessageBox.Show("Khách hàng đã thêm thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private Boolean isNameValid()
        {
            if (NameTermTextBox.Text != "")
            {
                NameTermBorder.BorderBrush = System.Windows.Media.Brushes.Orange;
                NameTermBorder.BorderThickness = new Thickness(0.5);
                return true;
            }
            else
            {
                NameTermBorder.BorderBrush = System.Windows.Media.Brushes.Red;
                NameTermBorder.BorderThickness = new Thickness(2);
                return false;

            }
        }
        private Boolean isPhoneValid()
        {
            if (PhoneTermTextBox.Text != "" &&
               Regex.IsMatch(PhoneTermTextBox.Text, @"^0\d{9}$"))
            {
                PhoneTermBorder.BorderBrush = System.Windows.Media.Brushes.Orange;
                PhoneTermBorder.BorderThickness = new Thickness(0.5);
                return true;
            }
            else
            {
                PhoneTermBorder.BorderBrush = System.Windows.Media.Brushes.Red;
                PhoneTermBorder.BorderThickness = new Thickness(2);
                return false;

            }
        }
        private Boolean isAddressValid()
        {
            if (AddressTermTextBox.Text != "")
            {
                AddressTermBorder.BorderBrush = System.Windows.Media.Brushes.Orange;
                AddressTermBorder.BorderThickness = new Thickness(0.5);
                return true;
            }
            else
            {
                AddressTermBorder.BorderBrush = System.Windows.Media.Brushes.Red;
                AddressTermBorder.BorderThickness = new Thickness(2);
                return false;

            }
        }
        private Boolean isDataValid()
        {
            Boolean cusNameValidFlag = isNameValid();
            Boolean phoneValidFlag = isPhoneValid();
            Boolean addressValidFlag = isAddressValid();
            

            Boolean isValid = cusNameValidFlag && phoneValidFlag && addressValidFlag;

            if (isValid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _customerBUS = new CustomerBUS();
        }
    }
}

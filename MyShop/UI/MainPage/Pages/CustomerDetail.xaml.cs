using DocumentFormat.OpenXml.Drawing.Charts;
using MyShop.BUS;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for CustomerDetail.xaml
    /// </summary>
    public partial class CustomerDetail : Page
    {
        private CustomerDTO _currentCustomer;
        private Frame _pageNavigation;
        private List<ProductDTO> _products;
        private List<PurchaseDTO> _purchases;
        private OrderBUS _orderBUS;
        private CustomerBUS _customerBUS;
        public CustomerDetail(CustomerDTO currentCustomer, Frame pageNavigation)
        {
            _pageNavigation= pageNavigation;
            InitializeComponent();

            _currentCustomer = currentCustomer;

            var productBUS = new ProductBUS();
            var orderBUS = new OrderBUS();
            _orderBUS = orderBUS;
            var customerBUS = new CustomerBUS();
            _customerBUS = customerBUS;

            // khởi tạo dữ liệu ok
            _products = new List<ProductDTO>();

            _purchases = orderBUS.findPurchasesByCusID(currentCustomer.CusID);


            ObservableCollection<Data> dataList = new ObservableCollection<Data>();
            foreach (var purchase in _purchases)
            {
                var product = productBUS.findProductById(purchase.ProID);
                _products.Add(product);
                Data item = new Data();
                item.Product = product;
                item.Purchase = purchase;
                dataList.Add(item);
            }

            productsListView.ItemsSource = dataList;
            this.DataContext = currentCustomer;
        }

        public class Data : INotifyPropertyChanged
        {
            public ProductDTO Product { get; set; }
            public PurchaseDTO Purchase { get; set; }

            public event PropertyChangedEventHandler? PropertyChanged;
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.GoBack();
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void UpdateCustomer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DelCustomer_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult choice = MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông Báo", MessageBoxButton.OKCancel);

            if (choice == MessageBoxResult.OK)
            {
                // lưu lại trạng thái trước đó
                _customerBUS.delCustomerById(_currentCustomer.CusID);
                _pageNavigation.NavigationService.GoBack();
            }
            else
            {

            }
        }
    }
}

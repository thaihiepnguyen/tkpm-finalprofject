using MyShop.BUS;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Printing;

namespace MyShop.UI.MainPage.Pages
{
    /// <summary>
    /// Interaction logic for AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Page
    {
        Frame _pageNavigation;
        private ProductBUS _productBUS;
        private CustomerBUS _customerBUS;
        private OrderBUS _orderBUS;
        private ObservableCollection<CustomerDTO> _customers;
        private bool _verifyOrder = false;
        private int _orderID = 0;
        private ObservableCollection<Data> _data;
        private int _currentCustomerID;
        private ProductDTO _currentProduct;
        private ObservableCollection<ProductDTO> _products;
        private Decimal _currentTotalPrice = 0;
        private Decimal _totalRealPrice = 0;
        private ShopOrderDTO _shopOrderDTO;
        private int _currentQuantity;
        private ObservableCollection<OrderDetail.Data> _list;
        private List<PurchaseDTO> _purchaseBuffer;
        private decimal _realPrice;
        
        public class Data
        {
            public string ProName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public decimal TotalPrice { get; set; }
        }



        public AddOrder(Frame pageNavigation, ObservableCollection<OrderDetail.Data> list)
        {
            _pageNavigation = pageNavigation;
            _productBUS = new ProductBUS();
            _customerBUS = new CustomerBUS();
            _orderBUS = new OrderBUS();
            _data = new ObservableCollection<Data>();
            _purchaseBuffer = new List<PurchaseDTO>();
            _list = list;
            InitializeComponent();
        }

        private void SaveOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_purchaseBuffer.Count == 0)
            {
                MessageBox.Show("Chưa có sản phẩm nào ở đơn hàng này", "Thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // lúc này mới lưu vào database 
            foreach (var purchase in _purchaseBuffer)
            {
                // lúc mà thêm purchase thì ở DAO đã xóa số lượng bên product luôn rồi !
                _orderBUS.addPurchase(purchase);
            }

            _orderBUS.patchShopOrder(_shopOrderDTO);
            _list.Add(new OrderDetail.Data(_shopOrderDTO, _customerBUS));

            MessageBox.Show("Đã lưu đơn hàng thành công", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
            _pageNavigation.NavigationService.GoBack();
            _verifyOrder = false;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            if (_verifyOrder == true)
            {
                result = MessageBox.Show("Đơn hàng chưa được lưu. Bạn có muốn tiếp tục không?", "Thông Báo", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    return;
                }
            }

            _pageNavigation.NavigationService.GoBack();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var products = await _productBUS.getAll();
            _products = products;
            var customers = _customerBUS.getAll();
            _customers = customers;
            ProductCombobox.ItemsSource = products;
            _currentProduct = products[0];
            ProductCombobox.SelectedIndex = 0;

            CustomerCombobox.ItemsSource = customers;
            CustomerCombobox.SelectedIndex = 0;
            FinalPrice.Text = string.Format("{0:N0} đ", _currentTotalPrice);

            this.DataContext = _currentProduct;
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!isQuantityValid())
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ!!", "Thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var purchareDTO = new PurchaseDTO();
            

            var customerDTO = (CustomerDTO)CustomerCombobox.SelectedValue;

            var productDTO = (ProductDTO)ProductCombobox.SelectedValue;
            int quantity = int.Parse(QuantityTermTextBox.Text);

            _currentQuantity = quantity;
            if ( _currentQuantity > _currentProduct.Quantity)
            {
                MessageBox.Show("Số lượng sản phẩm đặt mua vượt quá số lượng tồn kho", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // lúc này chưa tạo đơn hàng
            if (!_verifyOrder)
            {
                var shopOrderDTO = new ShopOrderDTO();
                shopOrderDTO.CusID = customerDTO.CusID;
                shopOrderDTO.CreateAt = DateTime.Now.Date;
                _orderID = _orderBUS.addShopOrder(shopOrderDTO);
                shopOrderDTO.OrderID = _orderID;
                _shopOrderDTO = shopOrderDTO;
            }

            // đã khởi tạo một đơn hàng
            // giá này là đã có khuyến mãi rồi 
            decimal priceOfProduct = (int)productDTO.PromotionPrice;
            // quá trình tạo đơn hàng
            purchareDTO.OrderID = _orderID;
            purchareDTO.ProID = productDTO.ProId;
            purchareDTO.Quantity = quantity;
            purchareDTO.TotalPrice = _orderBUS.calProductProfit(priceOfProduct) * quantity;

            // giá real :))) 
            _realPrice = priceOfProduct * quantity;

            _purchaseBuffer.Add(purchareDTO);

            MessageBox.Show("Sản phẩm đã thêm thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            var data = new Data
            {
                Quantity = quantity,
                Price = priceOfProduct,
                ProName = productDTO.ProName,
                TotalPrice = _orderBUS.calProductProfit(priceOfProduct) * quantity
            };
            _currentProduct.Quantity -= quantity;
            _currentTotalPrice += data.TotalPrice;

            _data.Add(data);

            ordersListView.ItemsSource = _data;

            _verifyOrder = true;
            _currentCustomerID = customerDTO.CusID;

            FinalPrice.Text = string.Format("{0:N0} đ", _currentTotalPrice);
            
            _shopOrderDTO.FinalTotal = _currentTotalPrice;
            _totalRealPrice += _realPrice;

            // tính lợi nhuận
            _shopOrderDTO.ProfitTotal = _currentTotalPrice - _totalRealPrice;
            CustomerCombobox.IsEnabled = false;
        }

        private Boolean isQuantityValid()
        {
            int parsed;
            bool success = int.TryParse(QuantityTermTextBox.Text,
                out parsed);
            if (QuantityTermTextBox.Text != "" && success && parsed > 0)
            {
                QuantityTermBorder.BorderBrush = System.Windows.Media.Brushes.Orange;
                QuantityTermBorder.BorderThickness = new Thickness(0.5);
                return true;
            }
            else
            {
                QuantityTermBorder.BorderBrush = System.Windows.Media.Brushes.Red;
                QuantityTermBorder.BorderThickness = new Thickness(2);
                return false;

            }
        }

        /*
        private void SaveCustomer_Click(object sender, RoutedEventArgs e)
        {
            var customerDTO = new CustomerDTO();

            int CusID;


            customerDTO.CusName = CustomerTermTextBox.Text;
            CusID = _customerBUS.addCustomer(customerDTO);
            customerDTO.CusID = CusID;

            _customers.Add(customerDTO);

            MessageBox.Show("Khách hàng đã thêm thành công", "Thông báo", MessageBoxButton.OK);
        }*/

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var result = MessageBox.Show("Bạn có muốn xóa không?", "Thông Báo", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                int index = ordersListView.SelectedIndex; if (index == -1) return;
                _data.RemoveAt(index);
            }
            _verifyOrder = true;
        }

        private void ProductCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ProductCombobox.SelectedIndex;

            if (index != -1)
                _currentProduct.copy(_products[index]);
        }
    }
}
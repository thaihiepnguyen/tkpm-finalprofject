using DocumentFormat.OpenXml.Office2010.ExcelAc;
using MyShop.BUS;
using MyShop.DAO;
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
    /// Interaction logic for UpdateOrder.xaml
    /// Mã nguồn ở đây rất tạm bợ :<
    /// </summary>
    
    public partial class UpdateOrder : Page
    {
        private Frame _pageNavigation;
        private SuperOrderDetail.OrderInfo? _orderInfo;
        private ObservableCollection<SuperOrderDetail.Data>? _dataList;
        private ProductBUS _productBUS;
        private CustomerBUS _customerBUS;
        private ProductDTO _currentProduct;
        private OrderBUS _orderBUS;
        private ObservableCollection<CustomerDTO> _customers;
        private Decimal _currentTotalPrice = 0;
        private ObservableCollection<ProductDTO> _products;

        // Xác minh rằng đơn hàng có sự thay đổi
        private bool _verifyOrder = false;
        private int _orderID = 0;
        private ObservableCollection<Data> _data;
        private int _currentCustomerID;
        private Decimal _totalRealPrice = 0;
        private ShopOrderDTO _shopOrderDTO;
        private int _currentQuantity;
        private ObservableCollection<OrderDetail.Data> _list;
        private List<PurchaseDTO> _purchaseBuffer;
        private List<ProductDTO> _productBuffer;
        private CustomerDTO _customer;
        private decimal _realPrice;
        public class Data
        {
            public int ID { get; set; }
            public string ProName { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public decimal RealPrice { get; set; }
            public decimal TotalPrice { get; set; }

            public ProductDTO CurrentProduct { get; set; }
            public PurchaseDTO Purchase { get; set; }
        }


        public UpdateOrder(Frame pageNavigation, SuperOrderDetail.OrderInfo? orderInfo, ObservableCollection<SuperOrderDetail.Data>? dataList)
        {
            InitializeComponent();
            this._pageNavigation = pageNavigation;
            this._orderInfo = orderInfo;
            this._dataList = dataList;
            _productBUS = new ProductBUS();
            _customerBUS = new CustomerBUS();
            _orderBUS = new OrderBUS();
            _data = new ObservableCollection<Data>();
            _purchaseBuffer = new List<PurchaseDTO>();
            _productBuffer = new List<ProductDTO>();
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

        private void ProductCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ProductCombobox.SelectedIndex;

            if (index != -1)
                _currentProduct.copy(_products[index]);
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (QuantityTermTextBox.Text == "")
            {
                MessageBox.Show("Vui lòng nhập số lượng", "Thông báo", MessageBoxButton.OK);
                return;
            }
            // tạo ra purchase mới OK
            var purchareDTO = new PurchaseDTO();
            // lấy sản phẩm ở combobox OK
            var productDTO = (ProductDTO)ProductCombobox.SelectedValue;
            // lấy số lượng ở EditText OK  
            int quantity = int.Parse(QuantityTermTextBox.Text);

            // verify quantity OK
            _currentQuantity = quantity;
            if (quantity <= 0 || _currentQuantity > _currentProduct.Quantity)
            {
                MessageBox.Show("Có lỗi xảy ra", "Thông báo", MessageBoxButton.OK);
                return;
            }
            // quá trình thêm đơn hàng
            // set orderID OK 
            purchareDTO.OrderID = _orderID;

            // 
            purchareDTO.ProID = productDTO.ProId;
            purchareDTO.Quantity = quantity;
            purchareDTO.TotalPrice = _orderBUS.calProductProfit(productDTO.Price) * quantity;

            // giá real :))) 
            _realPrice = productDTO.Price * quantity;


            // thêm vào buffer OK
            _purchaseBuffer.Add(purchareDTO);
            _productBuffer.Add(productDTO);

            MessageBox.Show("Sản phẩm đã thêm thành công", "Thông báo", MessageBoxButton.OK);

            // truyền dữ liệu vào listView
            _currentProduct.Quantity -= quantity;
            var data = new Data
            {
                ID = _orderID,
                Purchase = purchareDTO,
                Quantity = quantity,
                Price = productDTO.Price,
                ProName = productDTO.ProName,
                RealPrice = productDTO.Price * quantity,
                CurrentProduct = _currentProduct,
                TotalPrice = _orderBUS.calProductProfit(productDTO.Price) * quantity
            };
            
            _currentTotalPrice += data.TotalPrice;
            _totalRealPrice += _realPrice;

            _data.Add(data);

            _verifyOrder = true;
            _currentCustomerID = _shopOrderDTO.CusID;

            FinalPrice.Text = string.Format("{0:N0} đ", _currentTotalPrice);

            _shopOrderDTO.FinalTotal = _currentTotalPrice;
            // tính lợi nhuận
            _shopOrderDTO.ProfitTotal = _currentTotalPrice - _totalRealPrice;
            CustomerCombobox.IsEnabled = false;
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var result = MessageBox.Show("Bạn có muốn xóa không?", "Thông Báo", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                var item = ordersListView.SelectedItem as Data;
                _currentTotalPrice -= item.TotalPrice;
                
                _totalRealPrice -= item.RealPrice;
                // tính lợi nhuận = tổng doanh thu - tổng thực tế

                // cập nhật lại order
                _shopOrderDTO.FinalTotal = _currentTotalPrice;
                _shopOrderDTO.ProfitTotal = _currentTotalPrice - _totalRealPrice;

                // xóa purchase đi OK 
                _purchaseBuffer.Remove(item.Purchase);

                // xóa product đi OK 
                _productBuffer.Remove(item.CurrentProduct);


                // trả quantity về lại cho this product 
                item.CurrentProduct.Quantity += item.Quantity;

                // remove ở UI
                _data.Remove(item);
                FinalPrice.Text = string.Format("{0:N0} đ", _currentTotalPrice);

            }
            _verifyOrder = true;
        }

        private void SaveOrder_Click(object sender, RoutedEventArgs e)
        {
            var orderBUS = new OrderBUS();

            var customerDTO = (CustomerDTO)CustomerCombobox.SelectedValue;
            _customer = customerDTO;
            _shopOrderDTO.CusID = customerDTO.CusID;

            // gán lại thông tin mới cho UI
            _orderInfo.Order = _shopOrderDTO;

            orderBUS.patchShopOrder(_shopOrderDTO);
            _orderInfo.Customer = _customer;

            foreach (var item in _dataList)
            {
                orderBUS.delPurchaseById(item.Purchase.PurchaseID);
            }

            _dataList.Clear();

            for (int i = 0; i < _purchaseBuffer.Count; i++)
            {
                SuperOrderDetail.Data item = new();
                item.Product = _productBuffer[i];
                item.Purchase = _purchaseBuffer[i];
                orderBUS.addPurchase(item.Purchase);
                _dataList.Add(item);
            }
            MessageBox.Show("Đã lưu đơn hàng thành công", "Thông Báo");
            _pageNavigation.NavigationService.GoBack();
            _verifyOrder = false;
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

            _currentCustomerID = _orderInfo.Order.CusID;
            _customer = _orderInfo.Customer;

            for (int i = 0; i < customers.Count; i++)
            {
                if (customers[i].CusID == _currentCustomerID)
                {
                    CustomerCombobox.SelectedIndex = i;
                    break;
                }
            }

            _currentTotalPrice = (decimal)_orderInfo.Order.FinalTotal;
            _totalRealPrice = _currentTotalPrice - (decimal)_orderInfo.Order.ProfitTotal;
           
            _orderID = _orderInfo.Order.OrderID;
            foreach (var item in _dataList)
            {
                var data = new Data
                {
                    ID = _orderID,
                    Purchase = item.Purchase,
                    CurrentProduct = item.Product,
                    Quantity = item.Purchase.Quantity,
                    Price = item.Product.Price,
                    ProName = item.Product.ProName,
                    RealPrice = item.Purchase.Quantity * item.Product.Price,
                    TotalPrice = item.Purchase.TotalPrice
                };

                _purchaseBuffer.Add(item.Purchase);
                _productBuffer.Add(item.Product);
                _data.Add(data);
            }
            _shopOrderDTO = _orderInfo.Order;
            this.DataContext = _currentProduct;
            ordersListView.ItemsSource = _data;
            FinalPrice.Text = string.Format("{0:N0} đ", _currentTotalPrice);
        }
    }
}

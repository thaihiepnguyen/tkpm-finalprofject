﻿using MyShop.BUS;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;

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
        private ShopOrderDTO _shopOrderDTO;
        private int _currentQuantity;
        private ObservableCollection<OrderDetail.Data> _list;
        private List<PurchaseDTO> _purchaseBuffer;


        public class Data {
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
            foreach (var purchase in _purchaseBuffer)
            {
                _orderBUS.addPurchase(purchase);
            }

            _orderBUS.patchShopOrder(_shopOrderDTO);
            _list.Add(new OrderDetail.Data(_shopOrderDTO, _customerBUS));

            MessageBox.Show("Đã lưu đơn hàng thành công", "Thông Báo");
            _pageNavigation.NavigationService.GoBack();
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var products = _productBUS.getAll();
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
            if (QuantityTermTextBox.Text == "")
            {
                MessageBox.Show("Vui lòng nhập số lượng");
                return;
            }

            var purchareDTO = new PurchaseDTO();
            var orderDetailDTO = new ShopOrderDTO();

            var customerDTO = (CustomerDTO)CustomerCombobox.SelectedValue;
            
            var productDTO = (ProductDTO)ProductCombobox.SelectedValue;
            int quantity = int.Parse(QuantityTermTextBox.Text);

            _currentQuantity = quantity;
            if (quantity <= 0 || _currentQuantity > _currentProduct.Quantity)
            {
                MessageBox.Show("Có lỗi xảy ra", "Thông báo", MessageBoxButton.OK);
                return;
            }

            orderDetailDTO.CusID = customerDTO.CusID;
            orderDetailDTO.CreateAt = DateTime.Now.Date;

            if (!_verifyOrder)
            {
                _orderID = _orderBUS.addShopOrder(orderDetailDTO);
            }

            orderDetailDTO.OrderID = _orderID;
            purchareDTO.OrderID = _orderID;
            purchareDTO.ProID = productDTO.ProId;
            purchareDTO.Quantity = quantity;
            purchareDTO.TotalPrice = productDTO.Price * quantity;

            _purchaseBuffer.Add(purchareDTO);

            MessageBox.Show("Sản phẩm đã thêm thành công", "Thông báo", MessageBoxButton.OK);

            var data = new Data
            {
                Quantity = quantity,
                Price = productDTO.Price,
                ProName = productDTO.ProName,
                TotalPrice = productDTO.Price * quantity
            };
            _currentProduct.Quantity -= quantity;
            _currentTotalPrice += data.TotalPrice;

            _data.Add(data);

            ordersListView.ItemsSource = _data;

            _verifyOrder = true;
            _currentCustomerID = customerDTO.CusID;

            FinalPrice.Text = string.Format("{0:N0} đ", _currentTotalPrice);
            _shopOrderDTO = orderDetailDTO;
            _shopOrderDTO.FinalTotal = _currentTotalPrice;
            CustomerCombobox.IsEnabled = false;
        }

        private void SaveCustomer_Click(object sender, RoutedEventArgs e)
        {
            var customerDTO = new CustomerDTO();

            int CusID;


            customerDTO.CusName = CustomerTermTextBox.Text;
            CusID = _customerBUS.addCustomer(customerDTO);
            customerDTO.CusID = CusID;

            _customers.Add(customerDTO);

            MessageBox.Show("Khách hàng đã thêm thành công", "Thông báo", MessageBoxButton.OK);
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void ProductCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ProductCombobox.SelectedIndex;

            if (index != -1)
                _currentProduct.copy(_products[index]);
        }
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using MyShop.BUS;
using MyShop.DTO;
using System.Diagnostics;
using MyShop.UI.MainPage.Pages.NewFolder;
using Azure;
using System.Windows.Input;

namespace MyShop.UI.MainPage.Pages
{
    public partial class Order : Page
    {

      
        public Order()
        {
            InitializeComponent();
        }
        class Data
        {
            public string? ProName { get; set; }
            public string? ProImage { get; set; }
            public decimal? Price { get; set; }
            public int OrderID { get; set; }
            public int ProID { get; set; }

            public Data(OrderDTO orderDTO)
            {
                ProName = orderDTO.ProName;
                ProImage = orderDTO.ImagePath;
                Price = orderDTO.Price;
                OrderID = orderDTO.OrderID;
                ProID = orderDTO.ProID;
            }
        }

        private List<OrderDTO>? _orders = null;

        int _currentPage = 1;
        int _rowsPerPage = 6;
        int _totalItems = 0;
        int _totalPages = 0;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _updateDataSource(1);
            _updatePagingInfo();
            

            currentPageComboBox.SelectedIndex = _currentPage - 1;
        }
   


        private void _updateDataSource( int page = 1, string keyword = "", DateTime? startDate = null, DateTime? endDate = null)
        {
            int userID = 2;
            List<Data> list = new List<Data>();
            _currentPage = page;
            OrderBUS orderBUS = new OrderBUS();
            (_orders, _totalItems) = orderBUS.findOderBySearch(userID,_currentPage, _rowsPerPage, keyword, startDate, endDate);
            foreach (var order in _orders)
            {
                list.Add(new Data(order));
            }

     /*       if (list.Count == 0)
            {
                MessageText.Text = "Opps! Không tìm thấy bất kì sản phẩm nào.";
            }
*/
            dataListView.ItemsSource = list; 
            infoTextBlock.Text = $"Đang hiển thị {_orders.Count} trên tổng số {_totalItems} sản phẩm";
        }



        private void _updatePagingInfo()
        {
            _totalPages = _totalItems / _rowsPerPage +
                   (_totalItems % _rowsPerPage == 0 ? 0 : 1);

            // Cập nhật ComboBox
            var lines = new List<Tuple<int, int>>();
            for (int i = 1; i <= _totalPages; i++)
            {
                lines.Add(new Tuple<int, int>(i, _totalPages));
            }
            currentPageComboBox.ItemsSource = lines;


            currentPageComboBox.ItemsSource = lines;
            currentPageComboBox.SelectedIndex = _currentPage - 1;

            // Cập nhật nút Previous
            previousButton.IsEnabled = _currentPage == 1;

            // Cập nhật nút Next
            nextButton.IsEnabled = _currentPage < _totalPages;
        }

        private void currentPageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currentPageComboBox.SelectedIndex >= 0)
            {
                _currentPage = currentPageComboBox.SelectedIndex + 1;

                _updateDataSource(_currentPage);
            }


        }

        /*      private void sortOptionRadioButton_Checked(object sender, RoutedEventArgs e)
              {
                  _updateDataSource(_currentPage);
              }*/



        private void searchButton_Click(object sender, RoutedEventArgs e)
        {/*
            string keyword = keywordTextBox.Text;

            _updateDataSource(1, keyword);
            _updatePagingInfo();

            currentPageComboBox.SelectedIndex = _currentPage - 1;*/

            /*   DateTime dateStart = dateStartPicker.SelectedDate.Value.Date;
               DateTime dateEnd = dateEndPicker.SelectedDate.Value.Date;
               Trace.WriteLine(dateEnd);
               Trace.WriteLine(dateStart);*/
            DateTime? startDate = dateStartPicker.SelectedDate;
            DateTime? endDate = dateEndPicker.SelectedDate;

            _updateDataSource(1, "", startDate, endDate);
            _updatePagingInfo();

            currentPageComboBox.SelectedIndex = _currentPage - 1;
        }
        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _updateDataSource(_currentPage - 1);
                currentPageComboBox.SelectedIndex = _currentPage - 1;
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _updateDataSource(_currentPage + 1);
                currentPageComboBox.SelectedIndex = _currentPage - 1;
            }
        }

        private void previousButton_Click_1(object sender, RoutedEventArgs e)
        {



        }

        private void dataListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        
        }

        private void Button_Click_Edit(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button.DataContext == null)
            {
                return;
            }

            var item = button.DataContext as Data;
            var screen = new DetailOrder(item.OrderID);

            screen.Closed += (s, args) =>
            {
                _updateDataSource();
            };

            screen.ShowDialog();


        }
    }
}

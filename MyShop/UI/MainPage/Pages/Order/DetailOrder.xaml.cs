using MyShop.BUS;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace MyShop.UI.MainPage.Pages.NewFolder
{
    /// <summary>
    /// Interaction logic for DetailOrder.xaml
    /// </summary>
    /// 
  
    public partial class DetailOrder : Window
    {


        private OrderDTO orderDTO { get; set; }
        private OrderBUS orderBUS = new OrderBUS();
        public int idImage;
        public DetailOrder(int data)
        {
            InitializeComponent();
            idImage = data;
          
            orderDTO = orderBUS.GetOne(data);
        }
/*        GetOne(int orderId)*/

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = orderDTO;

        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            string address = addressTextBox.Text;
            if(orderBUS.UpdateOne(orderDTO.OrderID, address))

            DialogResult = true;
        }
        

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
           

           if(orderBUS.DeleteOne(orderDTO.OrderID))
            DialogResult = true;
        }
    }
}

using MyShop.BUS;
using MyShop.DAO;
using MyShop.UI.LoginPage;
using MyShop.UI.MainPage;
using MyShop.UI.MainPage.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DocumentFormat.OpenXml.CustomProperties;
using System.Diagnostics;

namespace MyShop.UI.MainPage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int DashBoard = 0;
        const int Home = 1;
        const int Category = 2;
        const int Promotion = 3;
        const int Order = 4;
        const int Customer = 5;
        const int Statistical = 6;
        private int _currentPage = 0;

        class Resoures
        {
            public string MainBgPath { get; set; }
            public string CloseIconPath { get; set; }
            public string MinimizeIconPath { get; set; }
            public string Logo { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        ObservableCollection<Item> Items = null;
        

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // init db

            //new DatabaseUtilitites(
            //    "sqlexpress",
            //    "MyShopDB_9",
            //    "admin",
            //    "admin"
            //    );

            // Load Nav Background
            this.DataContext = new Resoures()
            {
                MainBgPath = "Assets/Images/main-bg.png",
                CloseIconPath = "Assets/Images/close-icon.png",
                MinimizeIconPath = "Assets/Images/minimize-icon.png",
                Logo = "Assets/Images/logo.png"
            };

            Items = new ObservableCollection<Item>()
            {
                new Item()
                {
                    FontIcon = "Dashboard",
                    ItemName = "Dashboard"
                },
                new Item()
                {
                    FontIcon = "Home",
                    ItemName = "Home"
                },
                new Item()
                {
                    FontIcon = "Gears",
                    ItemName = "Categories"
                },
                new Item()
                {
                    FontIcon = "Tags",
                    ItemName = "Promotions"
                },
                new Item()
                {
                    FontIcon = "ShoppingCart",
                    ItemName = "Orders",
                },
                new Item()
                {
                    FontIcon = "User",
                    ItemName = "Customers",
                },
                new Item()
                {
                    FontIcon = "Briefcase",
                    ItemName = "Report"
                }
            };

            ListOfItems.ItemsSource = Items;


            var currentpage = Properties.Settings.Default.CurrentPage;
            _currentPage = currentpage;

            ListOfItems.SelectedIndex = currentpage;
        }

        private void ListOfItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedIndex = ListOfItems.SelectedIndex;

            Properties.Settings.Default.CurrentPage = selectedIndex;
            Properties.Settings.Default.Save();

            changePage(selectedIndex, e);
        }

        private void changePage(int selectedIndex, SelectionChangedEventArgs e)
        {
            if (selectedIndex == DashBoard)
            {
                pageNavigation.NavigationService.Navigate(new DashBoard(loadingProgressBar));

            }
            if (selectedIndex == Home)
            {
                pageNavigation.NavigationService.Navigate(new Home(pageNavigation, loadingProgressBar));
            }
            if (selectedIndex == Category)
            {
                pageNavigation.NavigationService.Navigate(new ModifyCategory(pageNavigation, loadingProgressBar));
            }
            if (selectedIndex == Promotion)
            {
                pageNavigation.NavigationService.Navigate(new ModifyPromotion(pageNavigation, loadingProgressBar));
            }
            if (selectedIndex == Order)
            {
                pageNavigation.NavigationService.Navigate(new OrderDetail(pageNavigation, loadingProgressBar));
            }
            if (selectedIndex == Customer)
            {
                pageNavigation.NavigationService.Navigate(new Customer(pageNavigation));
            }
            if (selectedIndex == Statistical)
            {
                pageNavigation.NavigationService.Navigate(new Statistical(pageNavigation, loadingProgressBar));
            }
            // reset border trước khi click
            resetBorder();


            var addedContainer = ListOfItems.ItemContainerGenerator.ContainerFromItem(e.AddedItems[0]) as ListViewItem;
            if (addedContainer != null)
            {
                var border = FindVisualChild<Border>(addedContainer);
                if (border != null)
                {
                    border.Background = new SolidColorBrush(Colors.WhiteSmoke);
                    border.BorderBrush = new SolidColorBrush(Colors.DarkOrange);
                    border.CornerRadius = new CornerRadius(20);
                    border.BorderThickness = new Thickness(1);
                    border.Width = 140;
                }
            }


            if (e.RemovedItems.Count > 0)
            {
                var removedContainer = ListOfItems.ItemContainerGenerator.ContainerFromItem(e.RemovedItems[0]) as ListViewItem;
                if (removedContainer != null)
                {
                    var border = FindVisualChild<Border>(removedContainer);
                    if (border != null)
                    {
                        border.Background = new SolidColorBrush(Colors.Transparent);
                        border.BorderThickness = new Thickness(0);
                    }
                }
            }
        }

        // Tìm trong parent. Nếu có kiểu dữ liệu T thì trả ra T còn không thì thôi.
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T result)
                {
                    return result;
                }
                else
                {
                    T childResult = FindVisualChild<T>(child);
                    if (childResult != null)
                        return childResult;
                }
            }
            return null;
        }

        async private void ListOfItems_Loaded(object sender, RoutedEventArgs e)
        {
            if (ListOfItems.Items.Count > 0)
            {
                // Phải chờ cho nó load xong thì mới gán border được haha
                await Task.Delay(50);
                var container = ListOfItems.ItemContainerGenerator.ContainerFromIndex(_currentPage) as ListViewItem;
                if (container != null)
                {
                    var border = FindVisualChild<Border>(container);
                    if (border != null)
                    {
                        border.Background = new SolidColorBrush(Colors.WhiteSmoke);
                        border.BorderBrush = new SolidColorBrush(Colors.DarkOrange);
                        border.CornerRadius = new CornerRadius(20);
                        border.BorderThickness = new Thickness(1);
                        border.Width = 140;
                    }
                }
            }
        }

        private void resetBorder()
        {
            foreach(var item in ListOfItems.Items)
            {
                var container = ListOfItems.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                if (container != null)
                {
                    var border = FindVisualChild<Border>(container);
                    if (border != null)
                    {
                        border.Background = new SolidColorBrush(Colors.Transparent);
                        border.BorderThickness = new Thickness(0);
                    }
                }
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.UsernameRemember = false;
            Properties.Settings.Default.Save();
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}

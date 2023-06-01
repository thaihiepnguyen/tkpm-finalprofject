using Azure;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using MyShop.BUS;
using MyShop.DAO;
using MyShop.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Policy;
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
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Spreadsheet;
using Border = System.Windows.Controls.Border;

namespace MyShop.UI.MainPage.Pages
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    
    public partial class AddProduct : System.Windows.Controls.Page
    {
        private bool _isImageChanged = false;
        private FileInfo? _selectedImage = null;
        private ProductBUS _productBUS;
        private CategoryBUS _categoryBUS;
        private Frame _pageNavigation;
        private ProgressBar _loadingProgressBar;
        class AddProductResources
        {
            public string ProImage { get; set; }
        }

        public AddProduct(Frame pageNavigation, ProgressBar loadingProgressBar)
        {
            InitializeComponent();

            _productBUS = new ProductBUS();
            _categoryBUS = new CategoryBUS();
            _loadingProgressBar = loadingProgressBar;

            var categories = _categoryBUS.getAll();


            CategoryCombobox.ItemsSource = categories;
            CategoryCombobox.SelectedIndex = 0;

            DataContext = new AddProductResources()
            {
                ProImage = "Assets/Images/add_image.png"
            };

            _pageNavigation = pageNavigation;
        }

        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();
            screen.Filter = "Files|*.png; *.jpg; *.jpeg;";
            if (screen.ShowDialog() == true)
            {
                _isImageChanged = true;
                _selectedImage = new FileInfo(screen.FileName);

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(screen.FileName, UriKind.Absolute);
                bitmap.EndInit();

                AddedImage.Source = bitmap;
            }
        }

        private void SaveProduct_Click(object sender, RoutedEventArgs e)
        {

            if (!isDataValid())
            {
                MessageBox.Show("Vui lòng nhập giá trị hợp lệ!!", "Thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (_selectedImage == null)
            {
                MessageBox.Show("Vui lòng nhập ảnh đại diện");
                return;
            }
            var categoryDTO = (CategoryDTO)CategoryCombobox.SelectedValue;

            var productDTO = new ProductDTO();

            productDTO.ProName = NameTermTextBox.Text;
            productDTO.Ram = Double.Parse(RamTermTextBox.Text);
            productDTO.Rom = int.Parse(RomTermTextBox.Text);
            productDTO.ScreenSize = Double.Parse(ScreenSizeTermTextBox.Text);
            productDTO.TinyDes = DesTermTextBox.Text;
            productDTO.Price = Decimal.Parse(PriceTermTextBox.Text);
            productDTO.Trademark = TradeMarkTermTextBox.Text;
            productDTO.BatteryCapacity = int.Parse(PinTermTextBox.Text);
            productDTO.CatID = categoryDTO.CatID;
            productDTO.Quantity = int.Parse(QuantityTermTextBox.Text);
            productDTO.Block = 0;

        

            int id = _productBUS.saveProduct(productDTO);

            string key = Guid.NewGuid().ToString();

            _productBUS.uploadImage(_selectedImage, id, key);

            MessageBox.Show("Sản phẩm đã thêm thành công", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.Navigate(new ModifyCategory(_pageNavigation, _loadingProgressBar));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.GoBack();
        }
        private Boolean isProNameValid()
        {
            if (NameTermTextBox.Text != "" )
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
        private Boolean isTinyDesValid()
        {
            if (DesTermTextBox.Text != "")
            {
                DesTermBorder.BorderBrush = System.Windows.Media.Brushes.Orange;
                DesTermBorder.BorderThickness = new Thickness(0.5);
                return true;
            }
            else
            {
                DesTermBorder.BorderBrush = System.Windows.Media.Brushes.Red;
                DesTermBorder.BorderThickness = new Thickness(2);
                return false;

            }
        }
        private Boolean isTradeMarkValid()
        {
            if (TradeMarkTermTextBox.Text != "")
            {
                TradeMarkTermBorder.BorderBrush = System.Windows.Media.Brushes.Orange;
                TradeMarkTermBorder.BorderThickness = new Thickness(0.5);
                return true;
            }
            else
            {
                TradeMarkTermBorder.BorderBrush = System.Windows.Media.Brushes.Red;
                TradeMarkTermBorder.BorderThickness = new Thickness(2);
                return false;

            }
        }
        
        private Boolean isPriceValid()
        {
            Decimal parsed;
            bool success = Decimal.TryParse(PriceTermTextBox.Text,
                out parsed);
            if (PriceTermTextBox.Text != "" && success)
            {
                PriceTermBorder.BorderBrush = System.Windows.Media.Brushes.Orange;
                PriceTermBorder.BorderThickness = new Thickness(0.5);
                return true;
            }
            else
            {
                PriceTermBorder.BorderBrush = System.Windows.Media.Brushes.Red;
                PriceTermBorder.BorderThickness = new Thickness(2);
                return false;
                
            }
        }
        private Boolean isRamValid()
        {
            Double parsed;
            bool success = Double.TryParse(RamTermTextBox.Text,
                out parsed);
            if (RamTermTextBox.Text != "" && success)
            {
                RamTermBorder.BorderBrush = System.Windows.Media.Brushes.Orange;
                RamTermBorder.BorderThickness = new Thickness(0.5);
                return true;
            }
            else
            {
                RamTermBorder.BorderBrush = System.Windows.Media.Brushes.Red;
                RamTermBorder.BorderThickness = new Thickness(2);
                return false;

            }
        }
        private Boolean isRomValid()
        {
            int parsed;
            bool success = int.TryParse(RomTermTextBox.Text,
                out parsed);
            if (RomTermTextBox.Text != "" && success)
            {
                RomTermBorder.BorderBrush = System.Windows.Media.Brushes.Orange;
                RomTermBorder.BorderThickness = new Thickness(0.5);
                return true;
            }
            else
            {
                RomTermBorder.BorderBrush = System.Windows.Media.Brushes.Red;
                RomTermBorder.BorderThickness = new Thickness(2);
                return false;

            }
        }
        private Boolean isScreenSizeValid()
        {
            Double parsed;
            bool success = Double.TryParse(ScreenSizeTermTextBox.Text,
                out parsed);
            if (ScreenSizeTermTextBox.Text != "" && success)
            {
                ScreenSizeTermBorder.BorderBrush = System.Windows.Media.Brushes.Orange;
                ScreenSizeTermBorder.BorderThickness = new Thickness(0.5);
                return true;
            }
            else
            {
                ScreenSizeTermBorder.BorderBrush = System.Windows.Media.Brushes.Red;
                ScreenSizeTermBorder.BorderThickness = new Thickness(2);
                return false;

            }
        }
        private Boolean isBatteryCapacityValid()
        {
            int parsed;
            bool success = int.TryParse(PinTermTextBox.Text,
                out parsed);
            if (PinTermTextBox.Text != "" && success)
            {
                PinTermBorder.BorderBrush = System.Windows.Media.Brushes.Orange;
                PinTermBorder.BorderThickness = new Thickness(0.5);
                return true;
            }
            else
            {
                PinTermBorder.BorderBrush = System.Windows.Media.Brushes.Red;
                PinTermBorder.BorderThickness = new Thickness(2);
                return false;

            }
        }
        private Boolean isQuantityValid()
        {
            int parsed;
            bool success = int.TryParse(QuantityTermTextBox.Text,
                out parsed);
            if (QuantityTermTextBox.Text != "" && success)
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
        //private Boolean isPriceValid()
        //{
        //    if (PriceTermTextBox.Text != "" &&
        //       Regex.IsMatch(PriceTermTextBox.Text, @"^[0-9]*\.?[0-9]+$"))
        //    {
        //        PriceTermBorder.BorderBrush = System.Windows.Media.Brushes.Orange;
        //        PriceTermBorder.BorderThickness = new Thickness(0.5);
        //        return true;
        //    }
        //    else
        //    {
        //        PriceTermBorder.BorderBrush = System.Windows.Media.Brushes.Red;
        //        PriceTermBorder.BorderThickness = new Thickness(2);
        //        return false;

        //    }
        //}
        private Boolean isDataValid()
        {
            Boolean proNameValidFlag = isProNameValid();
            Boolean tinyDesValidFlag = isTinyDesValid();
            Boolean tradeMarkValidFlag = isTradeMarkValid();
            Boolean priceValidFlag = isPriceValid();
            Boolean ramValidFlag = isRamValid();
            Boolean romValidFlag = isRomValid();
            Boolean screenSizeValidFlag  = isScreenSizeValid();
            Boolean batteryCapacityValidFlag = isBatteryCapacityValid();
            Boolean quantityValidFlag = isQuantityValid();

            Boolean isValid = proNameValidFlag && tinyDesValidFlag && tradeMarkValidFlag && priceValidFlag && ramValidFlag && romValidFlag && screenSizeValidFlag && batteryCapacityValidFlag && quantityValidFlag;

            if (isValid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

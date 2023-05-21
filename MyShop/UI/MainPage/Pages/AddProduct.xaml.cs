using Azure;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using MyShop.BUS;
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

namespace MyShop.UI.MainPage.Pages
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Page
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

            MessageBox.Show("Sản phẩm đã thêm thành công", "Thông báo", MessageBoxButton.OK);
        }

        private void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.Navigate(new ModifyCategory(_pageNavigation, _loadingProgressBar));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.GoBack();
        }
    }
}

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using MyShop.BUS;
using MyShop.DTO;

namespace MyShop.UI.MainPage.Pages
{
    /// <summary>
    /// Interaction logic for ProductDetail.xaml
    /// </summary>
    public partial class ProductDetail : Page
    {
        private ProductDTO _product;
        private Frame _pageNavigation;
        private ProductBUS _productBUS;
        private CategoryBUS _categoryBUS;
        private PromotionBUS _promotionBUS;
        private CategoryDTO _category;
        protected PromotionDTO _promotion;
        private Home _home;
        private Data _currentData;
        private ProgressBar _loadingProgressBar;




        // Mục đích là đổ dữ liệu của Class này lên UI
        class Data : INotifyPropertyChanged
        {
            public ProductDTO Product { get; set; }
            public CategoryDTO Category { get; set; }
            public PromotionDTO? Promotion { get; set; }

            public string CatIcon { get; set; }


            public Data(ProductDTO productDTO, CategoryDTO categoryDTO, PromotionDTO? promotion)
            {
                this.Product = productDTO;
                this.Category = categoryDTO;
                if (promotion != null) {
                    this.Promotion = promotion;
                } else
                {
                    this.Promotion = new();
                    this.Promotion.DiscountPercent = 0;
                }
                
            }

            public event PropertyChangedEventHandler? PropertyChanged;
        }

        public ProductDetail(Home home, ProductDTO product, Frame pageNavigation, ProgressBar loadingProgressBar)
        {
            InitializeComponent();
            _home = home;
            _product = product;
            _pageNavigation = pageNavigation;
            _productBUS = new ProductBUS();
            _categoryBUS = new CategoryBUS();
            _promotionBUS = new PromotionBUS();
            _loadingProgressBar = loadingProgressBar;

            CategoryDTO category = _categoryBUS.getCategoryById(_product.CatID);
            PromotionDTO promotion = null;
            var promotions = _promotionBUS.getAll();
            promotions.Insert(0, new PromotionDTO()
            {
                PromoCode = "Không áp dụng"
            });

            PromotionsCombobox.ItemsSource = promotions;

            // nếu PromoID == null; nghĩa là sản phẩm chưa được áp dụng khuyến mãi 
            if (_product.PromoID != null)
            {
                promotion = _promotionBUS.getPromotionById((int)_product.PromoID);
                _promotion = promotion;

                PromotionsCombobox.SelectedValue = (PromotionDTO)promotions.Where(item => item.IdPromo == _product.PromoID).ToList()[0];
            } else // chưa áp dụng khuyến mãi
            {
                PromotionsCombobox.SelectedIndex = 0;
                _promotion = null;
            }
            
            _category = category;
            Data data = new(_product, category, _promotion);
            _currentData = data;

            DataContext = data;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Lưu lại trạng thái trước đó 
            var (key, page, currency, startPrice, endPrice) = _home.getCurrentState();

            _pageNavigation.NavigationService.Navigate(new Home(_pageNavigation, _loadingProgressBar, page, currency, key, startPrice, endPrice));
        }

        private void DelProduct_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult choice = MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông Báo", MessageBoxButton.OKCancel);

            if (choice == MessageBoxResult.OK)
            {
                // lưu lại trạng thái trước đó
                var (key, page, currency, startPrice, endPrice) = _home.getCurrentState();
                _productBUS.delProduct(_product.ProId);
                _pageNavigation.NavigationService.Navigate(new Home(_pageNavigation, _loadingProgressBar, page, currency, key, startPrice, endPrice));
            } else
            {

            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var clonedProduct = (ProductDTO)_product.Clone();

            _pageNavigation.NavigationService.Navigate(new UpdateProduct(_product, _category, _pageNavigation));
        }

        private void AddOrder_Click(object sender, RoutedEventArgs e)
        {
        }

        int flag = 0; // bỏ qua lần đầu
        private void PromotionsCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var promotion = (PromotionDTO)PromotionsCombobox.SelectedValue;

            if (promotion != null && flag != 0)
            {
                _currentData.Promotion.copy(promotion);

                _product.PromoID = _currentData.Promotion.IdPromo;
                double percent = 1 - promotion.DiscountPercent * 1.0 / 100;
                _product.PromotionPrice = (decimal?)((double)_product.Price * percent);
                _productBUS.patchProduct(_product);
                _promotion = promotion;
                MessageBox.Show("Đã áp dụng mã khuyến mãi thành công", "Thông Báo");
            }
            flag = 1;
        }

        private void PromotionBorder_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }
    }
}

using MyShop.BUS;
using MyShop.DTO;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for UpdatePromotion.xaml
    /// </summary>
    public partial class UpdatePromotion : Page
    {

        Frame _pageNavigation;
        PromotionDTO _promotion;
        PromotionBUS _promotionBUS;
        public UpdatePromotion(Frame pageNavigation, PromotionDTO promotion)
        {
                 _pageNavigation = pageNavigation; 
               _promotion = promotion;
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.GoBack();
        }

        private void SavePromotion_Click(object sender, RoutedEventArgs e)
        {
           

            _promotion.PromoCode = NameCodeTextBox.Text;
            _promotion.DiscountPercent = int.Parse(NameDiscountTextBox.Text);

            _promotionBUS.updatePromotion(_promotion);

            MessageBox.Show("Chỉnh sửa thể loại thành công", "Thông báo", MessageBoxButton.OK);
            _pageNavigation.NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _promotion;
            _promotionBUS = new PromotionBUS();
        }
    }
}

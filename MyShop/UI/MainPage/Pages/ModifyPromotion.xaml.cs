using MyShop.BUS;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyShop.UI.MainPage.Pages
{
    /// <summary>
    /// Interaction logic for ModifyPromotion.xaml
    /// </summary>
    public partial class ModifyPromotion : Page
    {
        private PromotionBUS _promotionBUS;
        private Frame _pageNavigation;
        private ObservableCollection< PromotionDTO> _promotion;
        private ProgressBar _loadingProgressBar;
        public ModifyPromotion(Frame pageNavigation, ProgressBar loadingProgressBar)
        {
            _pageNavigation = pageNavigation;
            _loadingProgressBar = loadingProgressBar;
            InitializeComponent();
        }
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int i = promotionListViews.SelectedIndex;

            PromotionDTO promotion = _promotion[i];
            if (promotion != null)
            {
                _pageNavigation.NavigationService.Navigate(new UpdatePromotion(_pageNavigation, promotion));
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _promotionBUS = new PromotionBUS();
            _promotion = _promotionBUS.getAll();
            promotionListViews.ItemsSource = _promotion;
        }


        private void DelPromotion_Click(object sender, RoutedEventArgs e)
        {
            int i = promotionListViews.SelectedIndex;

            if (i == -1)
            {
                MessageBox.Show("Vui lòng chọn mã khuyến mãi trước khi xóa", "Thông báo", MessageBoxButton.OK);
            }
            else
            {
                MessageBoxResult choice = MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông Báo", MessageBoxButton.OKCancel);

                if (choice == MessageBoxResult.OK)
                {
                    bool isSuccess; string message;
                    var IdPromo = _promotion[i].IdPromo;

                    (isSuccess, message) = _promotionBUS.delPromotionById((int)IdPromo);
                    if (!isSuccess)
                    {
                        MessageBox.Show(message, "Thông báo");
                    }else
                    {
                        _promotion.RemoveAt(i);
                    }
                    
                }
            }
        }

        private void SavePromotion_Click(object sender, RoutedEventArgs e)
        {
            var Apromotion = new PromotionDTO(); 
            Apromotion.PromoCode = NameCodeTextBox.Text;
            Apromotion.DiscountPercent = int.Parse(NameDiscountTextBox.Text) ; 
            int id = _promotionBUS.addPromotion(Apromotion); 
            Apromotion.IdPromo = id;
            _promotion.Add(Apromotion);

            MessageBox.Show("Thể loại đã thêm thành công", "Thông báo", MessageBoxButton.OK);
        }
    }
}

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
    /// Interaction logic for UpdateCategory.xaml
    /// </summary>
    public partial class UpdateCategory : Page
    {
        Frame _pageNavigation;
        CategoryDTO _category;
        CategoryBUS _categoryBUS;
        List<IconCategoryDTO> _icons;
        public UpdateCategory(Frame pageNavigation, CategoryDTO category, List<IconCategoryDTO> icons)
        {
            _pageNavigation = pageNavigation;
            _icons = icons;
            _category = category;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _category;
            _categoryBUS = new CategoryBUS();

            CategoryCombobox.ItemsSource = _icons;

            CategoryCombobox.SelectedIndex = 0;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _pageNavigation.NavigationService.GoBack();
        }

        private void SaveCategory_Click(object sender, RoutedEventArgs e)
        {
            _category.CatName = NameTermTextBox.Text;
            _category.CatDescription = DesTermTextBox.Text;
            _category.CatIcon = _icons[CategoryCombobox.SelectedIndex].CatIcon;

            _categoryBUS.updateCategory(_category);

            MessageBox.Show("Chỉnh sửa thể loại thành công", "Thông báo", MessageBoxButton.OK);
            _pageNavigation.NavigationService.GoBack();
        }
    }
}

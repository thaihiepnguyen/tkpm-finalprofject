using MyShop.BUS;
using MyShop.DAO;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyShop.UI.MainPage.Pages
{
    /// <summary>
    /// Interaction logic for ModifyCategory.xaml
    /// </summary>
    public partial class ModifyCategory : Page
    {
        private CategoryBUS _categoryBUS;
        private Frame _pageNavigation;
        private ObservableCollection<CategoryDTO> _categories;
        private ProgressBar _loadingProgressBar;
        List<IconCategoryDTO> _icons = new List<IconCategoryDTO>() {
                new IconCategoryDTO("Android"),
                new IconCategoryDTO("Apple"),
                new IconCategoryDTO("Windows"),
                new IconCategoryDTO("MobilePhone"),
            };
        public ModifyCategory(Frame pageNavigation, ProgressBar loadingProgressBar)
        {
            _pageNavigation = pageNavigation;
            _loadingProgressBar = loadingProgressBar;
            InitializeComponent();
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int i = categoriesListView.SelectedIndex;

            var category = _categories[i];
            if (category != null)
            {
                _pageNavigation.NavigationService.Navigate(new UpdateCategory(_pageNavigation, category, _icons));
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _categoryBUS= new CategoryBUS();

            _categories = _categoryBUS.getAll();


            categoriesListView.ItemsSource = _categories;
            CategoryCombobox.ItemsSource = _icons;

            CategoryCombobox.SelectedIndex = 0;
        }

        private void SaveCategory_Click(object sender, RoutedEventArgs e)
        {
            var category = new CategoryDTO();

            category.CatName = NameTermTextBox.Text;
            category.CatDescription = DesTermTextBox.Text;
            category.CatIcon = _icons[CategoryCombobox.SelectedIndex].CatIcon;
            

            int id = _categoryBUS.addCategory(category);

            category.CatID = id;
            _categories.Add(category);

            MessageBox.Show("Thể loại đã thêm thành công", "Thông báo", MessageBoxButton.OK);
        }

        private void DelCategory_Click(object sender, RoutedEventArgs e)
        {
            int i = categoriesListView.SelectedIndex;

            if (i == -1)
            {
                MessageBox.Show("Vui lòng chọn thể loại trước khi xóa", "Thông báo", MessageBoxButton.OK);
            }
            else
            {
                MessageBoxResult choice = MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông Báo", MessageBoxButton.OKCancel);

                if (choice == MessageBoxResult.OK)
                {

                    var CatID = _categories[i].CatID;
                    bool isSuccess; string message;

                    (isSuccess, message) = _categoryBUS.delCategoryById(CatID);
                    if (!isSuccess)
                    {
                        MessageBox.Show(message, "Thông Báo");
                    } else
                    {
                        _categories.RemoveAt(i);
                    }
                }
                else
                {

                }
            }
        }

    }
}

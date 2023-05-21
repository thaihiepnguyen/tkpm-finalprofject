using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyShop.UI.MainPage
{
    public class RelativeToAbsoluteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //"Images/avatar01.jpg"
            string relative = (string)value;
            // C:\Users\dev\source\repos\DemoBinding\DemoBinding\bin\Debug\net7.0-windows Chu y co san dau \
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            // C:\Users\dev\source\repos\DemoBinding\DemoBinding\bin\Debug\net7.0-windows/avatar01.jpg
            string absolute = $"{folder}{relative}";
            return absolute;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

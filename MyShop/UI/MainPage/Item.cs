using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.UI.MainPage
{
    // Item của navigation
    class Item : INotifyPropertyChanged
    {
        public string FontIcon { get; set; }
        public string ItemName { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DTO
{
    public class CustomerDTO : INotifyPropertyChanged
    {
        public int CusID { get; set; }
        public string CusName { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

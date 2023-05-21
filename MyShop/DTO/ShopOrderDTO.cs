using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DTO
{
    public class ShopOrderDTO : INotifyPropertyChanged
    {
        public int OrderID { get; set; }
        public int? CusID { get; set; }
        public DateTime CreateAt { get; set; }
        public Decimal? FinalTotal { get; set; }
        public Decimal? ProfitTotal { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

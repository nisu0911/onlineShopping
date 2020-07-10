using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication12.Models.ViewModel
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> SellingPrice { get; set; }
        public string IsSpecial { get; set; }
        public string Photo { get; set; }
    }
}
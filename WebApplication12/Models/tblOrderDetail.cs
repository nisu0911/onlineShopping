//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication12.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblOrderDetail
    {
        public int OrderDetailID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<int> UnitPrice { get; set; }
    
        public virtual tblOrder tblOrder { get; set; }
        public virtual tblProduct tblProduct { get; set; }
    }
}

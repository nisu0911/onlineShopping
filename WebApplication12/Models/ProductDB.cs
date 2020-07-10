using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication12.Models
{
    public class ProductDB
    {
        public static List<tblProduct> GetAllRecentItem()
        {
            using (var context = new eMDBEntities())
            {
                return context.tblProducts.Where(s => s.IsSpecial == "Special").Take(4).ToList();
            }
        }
        public static List<tblProduct> GetAllFoodRecentItem()
        {
            using (var context = new eMDBEntities())
            {
                return context.tblProducts.Take(8).ToList();
            }
        }
    }
}
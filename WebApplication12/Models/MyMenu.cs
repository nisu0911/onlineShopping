using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication12.Models
{
    public class MyMenu
    {
        public static List<tblCategory> GetMenus()
        {
            using (var context = new eMDBEntities())
            {
                return context.tblCategories.ToList();
            }
        }
        public static List<tblCategory> GetSubMenus(int menuid)
        {
            using (var context = new eMDBEntities())
            {
                return context.tblCategories.Where(u => u.CategoryID == menuid).ToList();
            }
        }
    }
}
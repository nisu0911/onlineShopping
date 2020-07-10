using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication12.Models;
using WebApplication12.Models.ViewModel;

namespace WebApplication12.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult ManageCategory()
        {
            return View();
        }
        public JsonResult getData()
        {
            using (eMDBEntities db = new eMDBEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                List<CategoryViewModel> lst = new List<CategoryViewModel>();
                var catList = db.tblCategories.ToList();
                foreach (var item in catList)
                {
                    lst.Add(new CategoryViewModel() { CategoryID = item.CategoryID, CategoryName = item.CategoryName });
                }
                return Json(new { data = lst }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                using (eMDBEntities db = new eMDBEntities())
                {
                    ViewBag.Action = "New Category";
                    return View(new CategoryViewModel());
                }
            }
            else
            {
                using (eMDBEntities db = new eMDBEntities())
                {
                    CategoryViewModel cvm = new CategoryViewModel();
                    var category = db.tblCategories.Where(x => x.CategoryID == id).FirstOrDefault();
                    cvm.CategoryID = category.CategoryID;
                    cvm.CategoryName = category.CategoryName;
                    ViewBag.Action="Edit Category";
                    return View(cvm);
                }
            }
        }
        [HttpPost]
        public ActionResult AddOrEdit(CategoryViewModel cvm)
        {
            using(eMDBEntities db=new eMDBEntities())
            {
                if (cvm.CategoryID == 0)
                {
                    tblCategory tb = new tblCategory();
                    tb.CategoryName = cvm.CategoryName;
                    db.tblCategories.Add(tb);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    tblCategory tbl = db.tblCategories.Where(x => x.CategoryID == cvm.CategoryID).FirstOrDefault();
                    tbl.CategoryName = cvm.CategoryName;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);

                }
            }
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using(eMDBEntities db=new eMDBEntities())
            {
                tblCategory tbd = db.tblCategories.Where(x => x.CategoryID == id).FirstOrDefault();
                db.tblCategories.Remove(tbd);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
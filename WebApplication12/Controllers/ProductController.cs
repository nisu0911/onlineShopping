using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication12.Models;
using WebApplication12.Models.ViewModel;

namespace WebApplication12.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult ManageProduct()
        {
            return View();
        }
        public JsonResult GetData()
        {
            using (eMDBEntities db = new eMDBEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                List<ProductViewModel> lstitem = new List<ProductViewModel>();
                var lst = db.tblProducts.Include("tblCategory").ToList();
                foreach (var item in lst)
                {
                    lstitem.Add(new ProductViewModel() { ProductID = item.ProductID, CategoryName = item.tblCategory.CategoryName, ProductName = item.ProductName, UnitPrice = item.UnitPrice, SellingPrice = item.SellingPrice, Photo = item.Photo });
                }
                return Json(new { data = lstitem }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                using (eMDBEntities db = new eMDBEntities())
                {
                    ViewBag.Categories = db.tblCategories.ToList();

                    return View(new ProductViewModel());
                }
            }
            else
            {
                using (eMDBEntities db = new eMDBEntities())
                {

                    ViewBag.Categories = db.tblCategories.ToList();
                    tblProduct item = db.tblProducts.Where(i => i.ProductID == id).FirstOrDefault();
                    ProductViewModel itemvm = new ProductViewModel();
                    itemvm.ProductID = item.ProductID;
                    itemvm.CategoryID = Convert.ToInt32(item.CategoryID);
                    itemvm.ProductName = item.ProductName;
                    itemvm.UnitPrice = item.UnitPrice;
                    itemvm.SellingPrice = item.SellingPrice;
                    itemvm.Description = item.Description;
                    itemvm.Photo = item.Photo;

                    return View(itemvm);
                }
            }
        }

        [HttpPost]

        public ActionResult AddOrEdit(ProductViewModel ivm)
        {
            using (eMDBEntities db = new eMDBEntities())
            {
                if (ivm.ProductID == 0)
                {
                        tblProduct itm = new tblProduct();

                        itm.CategoryID = Convert.ToInt32(ivm.CategoryID);
                        itm.ProductName = ivm.ProductName;
                        itm.UnitPrice = ivm.UnitPrice;
                        itm.SellingPrice = ivm.SellingPrice;
                        itm.Description = ivm.Description;
                        HttpPostedFileBase fup = Request.Files["Photo"];
                        if (fup != null)
                        {
                            if (fup.FileName != "")
                            {
                                fup.SaveAs(Server.MapPath("~/ProductImages/" + fup.FileName));
                                itm.Photo = fup.FileName;
                            }
                        }


                        db.tblProducts.Add(itm);
                        db.SaveChanges();
                        ViewBag.Message = "Created Successfully";                  
                }
                else
                {
                    tblProduct itm = db.tblProducts.Where(i => i.ProductID == ivm.ProductID).FirstOrDefault();
                    itm.CategoryID = Convert.ToInt32(ivm.CategoryID);
                    itm.ProductName = ivm.ProductName;
                    itm.UnitPrice = ivm.UnitPrice;
                    itm.SellingPrice = ivm.SellingPrice;
                    itm.Description = ivm.Description;
                    HttpPostedFileBase fup = Request.Files["Photo"];
                    if (fup != null)
                    {
                        if (fup.FileName != "")
                        {
                            fup.SaveAs(Server.MapPath("~/ProductImages/" + fup.FileName));
                            itm.Photo = fup.FileName;
                        }
                    }



                    db.SaveChanges();
                    ViewBag.Message = "Updated Successfully";

                }
                ViewBag.Categories = db.tblCategories.ToList();
                return View(new ProductViewModel());

            }


        }

        [HttpPost]

        public ActionResult Delete(int id)
        {
            using (eMDBEntities db = new eMDBEntities())
            {
                tblProduct sm = db.tblProducts.Where(x => x.ProductID == id).FirstOrDefault();
                db.tblProducts.Remove(sm);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
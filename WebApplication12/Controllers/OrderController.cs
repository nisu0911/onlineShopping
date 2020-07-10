using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication12.Models;
using WebApplication12.Models.ViewModel;

namespace WebApplication12.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult ManageOrder()
        {
            return View();
        }
        public JsonResult GetData()
        {
            using (eMDBEntities db= new eMDBEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                List<OrderViewModel> lstorder = new List<OrderViewModel>();
                var empList = db.tblOrders.Select(x => new { OrderId = x.OrderID, Firstname = x.FirstName, Lastname = x.LastName, Address = x.Address, Phone = x.Phone, Total = x.Total, OrderDate = x.OrderDate, DeliveredStatus = x.DeliveredStatus }).ToList();
                foreach (var item in empList)
                {
                    lstorder.Add(new OrderViewModel() { OrderID = item.OrderId, Firstname = item.Firstname, Lastname = item.Lastname, Address = item.Address, Phone = item.Phone, Total = item.Total.ToString(), OrderDate = item.OrderDate.ToString(), DeliveredStatus = item.DeliveredStatus });
                }
                return Json(new { data = lstorder }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]

        public ActionResult ViewOrder(int id)
        {
            using (eMDBEntities db = new eMDBEntities())
            {

                List<OrderDetailsViewModel> lstod = new List<OrderDetailsViewModel>();
                var empList = db.tblOrderDetails.Where(x => x.OrderID == id).ToList();
                foreach (tblOrderDetail item in empList)
                {
                    lstod.Add(new OrderDetailsViewModel() { ProductID = Convert.ToInt32(item.ProductID), ProductName = item.tblProduct.ProductName, Quantity = Convert.ToInt32(item.Quantity), UnitPrice = Convert.ToDecimal(item.UnitPrice) });
                }
                Session.Add("itemlist", lstod);
                Session.Add("orderid", id);
                return View(lstod);
            }
        }
        [HttpPost, ActionName("ViewOrder")]
        public ActionResult ViewOrder_Post(int id)
        {
            using (eMDBEntities db = new eMDBEntities())
            {

                tblOrder sm = db.tblOrders.Where(x => x.OrderID == id).FirstOrDefault();
                sm.DeliveredStatus = "Confirmed";





                db.SaveChanges();
                return RedirectToAction("ManageOrder", "Order");
            }

        }
        eMDBEntities db = new eMDBEntities();
        public ActionResult PrintBill()
        {
            List<OrderDetailsViewModel> lst = null;
            if (Session["itemlist"] != null)
            {
                lst = (List<OrderDetailsViewModel>)Session["itemlist"];
                ViewBag.orderlst = lst;
                if (Session["orderid"] != null)
                {
                    int oid = Convert.ToInt32(Session["orderid"].ToString());
                    BillViewModel blv = new BillViewModel();
                    tblOrder tbo = db.tblOrders.Where(o => o.OrderID == oid).FirstOrDefault();
                    ViewBag.Fullname = tbo.FirstName + " " + tbo.LastName;
                    ViewBag.Phone = tbo.Phone;
                    ViewBag.Address = tbo.Address;
                    ViewBag.OrderDate = tbo.OrderDate;

                }

            }
            return View(lst);


        }


        [HttpGet]

        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                using (eMDBEntities db = new eMDBEntities())
                {
                    // ViewBag.Menus = db.tblMenus.ToList();
                    return View(new tblCategory());
                }
            }
            else
            {
                using (eMDBEntities db = new eMDBEntities())
                {
                    // ViewBag.Menus = db.tblMenus.ToList();
                    return View(db.tblCategories.Where(x => x.CategoryID == id).FirstOrDefault());
                }
            }
        }

        [HttpPost]

        public ActionResult AddOrEdit(tblCategory sm)
        {
            using (eMDBEntities db = new eMDBEntities())
            {
                if (sm.CategoryID == 0)
                {
                    db.tblCategories.Add(sm);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.Entry(sm).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }


        }

        [HttpPost]

        public ActionResult Delete(int id)
        {
            using (eMDBEntities db = new eMDBEntities())
            {
                tblOrder sm = db.tblOrders.Where(x => x.OrderID == id).FirstOrDefault();
                db.tblOrders.Remove(sm);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
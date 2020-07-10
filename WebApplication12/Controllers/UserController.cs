using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication12.Models;
using WebApplication12.Models.ViewModel;

namespace WebApplication12.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult ManageUser()
        {
            return View();
        }

        public JsonResult GetData()
        {
            using (eMDBEntities db = new eMDBEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;
                List<UserViewModel> lst = new List<UserViewModel>();
                var userList = db.tblUsers.ToList();
                foreach (var item in userList)
                {
                    lst.Add(new UserViewModel() { UserId = item.UserID, Username = item.Username, Password = item.Password, Fullname = item.Fullname, Email = item.Email });
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
                    ViewBag.Action = "New User";
                    return View(new UserViewModel());
                }
            }
            else
            {
                using (eMDBEntities db = new eMDBEntities())
                {
                    UserViewModel sub = new UserViewModel();
                    var menu = db.tblUsers.Where(x => x.UserID == id).FirstOrDefault();
                    sub.UserId = menu.UserID;
                    sub.Username = menu.Username;
                    sub.Password = menu.Password;
                    sub.Fullname = menu.Fullname;
                    sub.Email = menu.Email;
                    ViewBag.Action = "Edit User";
                    return View(sub);
                }
            }
        }

        [HttpPost]
        public ActionResult AddOrEdit(UserViewModel sm)
        {
            using (eMDBEntities db = new eMDBEntities())
            {
                if (sm.UserId == 0)
                {
                    tblUser tb = new tblUser();
                    tb.Username = sm.Username;
                    tb.Password = sm.Password;
                    tb.Fullname = sm.Fullname;
                    tb.Email = sm.Email;
                    db.tblUsers.Add(tb);
                    db.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    tblUser tbm = db.tblUsers.Where(m => m.UserID == sm.UserId).FirstOrDefault();
                    tbm.Username = sm.Username;
                    tbm.Password = sm.Password;
                    tbm.Fullname = sm.Fullname;
                    tbm.Email = sm.Email;
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
                tblUser sm = db.tblUsers.Where(x => x.UserID == id).FirstOrDefault();
                db.tblUsers.Remove(sm);
                db.SaveChanges();
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
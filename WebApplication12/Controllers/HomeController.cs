using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebApplication12.Models;
using WebApplication12.Models.ViewModel;

namespace WebApplication12.Controllers
{
    public class HomeController : Controller
    {
        eMDBEntities _db = new eMDBEntities();
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult ViewItem(int id)
        {
            return View("ViewItem", _db.tblProducts.Find(id));
        }
        public ActionResult ProductList(string search, int? page, int id = 0)
        {
            if (id != 0)
            {
                return View(_db.tblProducts.Where(p => p.ProductID == id).ToList().ToPagedList(page ?? 1, 12));
            }
            else
            {
                if (search != "")
                {
                    return View(_db.tblProducts.Where(s => s.Description.Contains(search) || s.ProductName.Contains(search) || search == null).ToList().ToPagedList(page ?? 1, 12));
                }
                else
                {
                    return View(_db.tblProducts.ToList().ToPagedList(page ?? 1, 12));
                }
            }
        }
        public ActionResult ForgetPassword()
        {
            return View();

            //return RedirectToAction("Index", "Home");
        }
        //[ValidateOnlyIncomingValuesAttribute]
        [HttpPost]

        public ActionResult ForgetPassword(UserViewModel uv)
        {

            if (ModelState.IsValid)
            {
                //https://www.google.com/settings/security/lesssecureapps
                //Make Access for less secure apps=true

                string from = "swarihalwa@gmail.com";
                using (MailMessage mail = new MailMessage(from, uv.Username))
                {
                    try
                    {
                        tblUser tb = _db.tblUsers.Where(u => u.Username == uv.Username).FirstOrDefault();
                        if (tb != null)
                        {
                            mail.Subject = "Password Recovery";
                            mail.Body = "Your Password is:" + tb.Password;

                            mail.IsBodyHtml = false;
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = "smtp.gmail.com";
                            smtp.EnableSsl = true;
                            NetworkCredential networkCredential = new NetworkCredential(from, "password");
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = networkCredential;
                            smtp.Port = 587;
                            smtp.Send(mail);
                            ViewBag.Message = "Your Password Is Sent to your email";
                        }
                        else
                        {
                            ViewBag.Message = "email Doesnot Exist in Database";
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {

                    }

                }

            }
            return View();


            //return RedirectToAction("Index", "Home");
        }
    }
}

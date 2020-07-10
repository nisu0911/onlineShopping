using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication12.Models;
using WebApplication12.Models.ViewModel;

namespace WebApplication12.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel l, string ReturnUrl = "")
        {
            using (eMDBEntities db = new eMDBEntities())
            {
                var users = db.tblUsers.Where(a => a.Username == l.Username && a.Password == l.Password).FirstOrDefault();
                if (users != null)
                {
                    Session.Add("username", users.Username);
                    FormsAuthentication.SetAuthCookie(l.Username, true);
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        Session.Add("userid", users.UserID);
                        if (users.UserRoles.Where(r => r.RoleID == 1).FirstOrDefault() != null)
                        {
                            return RedirectToAction("DashBoard", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                else
                {

                    ModelState.AddModelError("", "Invalid User");

                }
            }
            return View();
        }
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(UserViewModel uv)
        {
            using (eMDBEntities _db = new eMDBEntities())
            {
                tblUser tbl = _db.tblUsers.Where(u => u.Username == uv.Username).FirstOrDefault();
                if (tbl != null)
                {
                    return Json(new { success = false, message = "User Already Register" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    tblUser tb = new tblUser();
                    tb.Email = uv.Email;
                    tb.Username = uv.Username;
                    tb.Password = uv.Password;
                    _db.tblUsers.Add(tb);
                    _db.SaveChanges();

                    UserRole ud = new UserRole();
                    ud.UserID = tb.UserID;
                    ud.UserRoleID = 2;
                    _db.UserRoles.Add(ud);
                    _db.SaveChanges();
                    return Json(new { success = true, message = "User Register Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]

        public ActionResult ChangePassword(ChangePasswordViewModel ch)
        {
            int UserID = Convert.ToInt32(Session["userid"].ToString());
            using (eMDBEntities db = new eMDBEntities())
            {
                tblUser us = db.tblUsers.Where(u => u.UserID == UserID && u.Password == ch.OldPassword).FirstOrDefault();
                if (us != null)
                {
                    us.Password = ch.NewPassword;
                    db.SaveChanges();

                }
                else
                {
                    ViewBag.Message = "Wrong Old Password";
                }
                return Json(new { success = true, message = "Password Changed Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(ForgotPasswordViewModel uv)
        {
            using (MailMessage mm = new MailMessage("swarihalwa@gmail.com", uv.Email))
            {
                using (eMDBEntities _db = new eMDBEntities())
                {
                    tblUser tb = _db.tblUsers.Where(e => e.Email == uv.Email).FirstOrDefault();
                    if (tb != null)
                    {
                        mm.Subject = "Password Recovery";
                        mm.Body = "Your Password is: " + tb.Password;

                        mm.IsBodyHtml = false;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential("swarihalwa.com", "password");
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);
                        ViewBag.Message = "Password Sent Please Check your email";
                    }
                    else
                    {
                        ViewBag.Message = "Email doesnot exist in our database";
                    }
                }
            }
            return View();
        }

    }
}
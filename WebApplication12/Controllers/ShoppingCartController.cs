﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication12.Models;
using WebApplication12.Models.ViewModel;

namespace WebApplication12.Controllers
{
    public class ShoppingCartController : Controller
    {
        eMDBEntities db = new eMDBEntities();
        // GET: ShoppingCart
        [Authorize]
        public ActionResult ShoppingCartList()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            // Return the view 
            return View(viewModel);
        }
        // 
        // GET: /Store/AddToCart/5 
        [Authorize]
        public ActionResult AddToCart(int id)
        {
            // Retrieve the album from the database 
            var addedItem = db.tblProducts
                .Single(item => item.ProductID == id);

            // Add it to the shopping cart 
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedItem);

            // Go back to the main store page for more shopping 
            return RedirectToAction("ShoppingCartList");
        }
        // AJAX: /ShoppingCart/UpdateCartCount/5 
        [HttpPost]
        public ActionResult UpdateCartCount(int id, int cartCount)
        {
            ShoppingCartRemoveViewModel results = null;
            try
            {
                // Get the cart 
                var cart = ShoppingCart.GetCart(this.HttpContext);

                // Get the name of the album to display confirmation 
                string itemName = db.tblCarts
                    .Single(item => item.RecordID == id).tblProduct.ProductName;

                // Update the cart count 
                int itemCount = cart.UpdateCartCount(id, cartCount);

                //Prepare messages
                string msg = "The quantity of " + Server.HtmlEncode(itemName) +
                        " has been refreshed in your shopping cart.";
                if (itemCount == 0) msg = Server.HtmlEncode(itemName) +
                        " has been removed from your shopping cart.";
                //
                // Display the confirmation message 
                results = new ShoppingCartRemoveViewModel
                {
                    Message = msg,
                    CartTotal = cart.GetTotal(),
                    CartCount = cart.GetCount(),
                    ItemCount = itemCount,
                    DeleteId = id
                };
            }
            catch
            {
                results = new ShoppingCartRemoveViewModel
                {
                    Message = "Error occurred or invalid input...",
                    CartTotal = -1,
                    CartCount = -1,
                    ItemCount = -1,
                    DeleteId = id
                };
            }
            return Json(results);
        }
        // 
        // AJAX: /ShoppingCart/RemoveFromCart/5 
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart 
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the album to display confirmation 
            string albumName = db.tblCarts.Single(item => item.RecordID == id).tblProduct.ProductName;

            // Remove from cart 
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message 
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(albumName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }
        // 
        // GET: /ShoppingCart/CartSummary 
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
        [Authorize]
        public ActionResult AddressAndPayment()
        {
            //var cart = ShoppingCart.GetCart(this.HttpContext);

            //List<tblCart> lst = cart.GetCartItems();
            var cart = ShoppingCart.GetCart(this.HttpContext);
            OrderViewModel ordv = new OrderViewModel();
            ordv.Total = cart.GetTotal().ToString();


            return View(ordv);


        }
        [HttpPost]

        public ActionResult AddressAndPayment(OrderViewModel ovm)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            tblOrder tb = new tblOrder();
            tb.Username = Session["username"].ToString();
            tb.FirstName = ovm.Firstname;
            tb.LastName = ovm.Lastname;
            tb.Address = ovm.Address;
            tb.Phone = ovm.Phone;
            tb.Total = cart.GetTotal();
            tb.OrderDate = Convert.ToDateTime(DateTime.Today.ToShortDateString());
            tb.DeliveredStatus = "Pending";

            db.tblOrders.Add(tb);
            db.SaveChanges();





            List<tblCart> lst = cart.GetCartItems();
            foreach (var item in lst)
            {
                tblOrderDetail tbord = new tblOrderDetail();
                tbord.OrderID = tb.OrderID;
                tbord.ProductID = item.ProductID;
                tbord.Quantity = item.Count;
                tbord.UnitPrice = Convert.ToInt32( item.tblProduct.SellingPrice);
                db.tblOrderDetails.Add(tbord);
                db.SaveChanges();
            }


            ViewBag.Message = "Your Ordered Done Successfully, It Takes 2/3 Hours In Our working Days";
            return View();


        }
    }
}
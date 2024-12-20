using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Moodeng.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        private moodeng_Entities db = new moodeng_Entities();

        private List<Cart> cartItems;
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();


            cartItems = db.Carts
                .Where(c => c.UserId == userId)
                .ToList();

            return View(cartItems);
        }
        public ActionResult AddToOrder()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                cartItems = db.Carts
               .Where(c => c.UserId == userId)
               .ToList();
                Order order = new Order
                {
                    OrderDate = DateTime.Now,
                    CustomerId = userId,
                    OrderStatus = "pending",
                    RetailStoreId = 4,
                    Payment = "on delivery",
                    PaymentStatus = 0,
                };

                var res = db.Orders.Add(order);

                foreach (Cart item in cartItems)
                {
                    OrderDetail orderDetail = new OrderDetail
                    {
                        OrderId = res.OrderId,
                        ProductName = item.Product.ProductName,
                        Quantity = item.Quantity,
                        UnitPrice = item.Product.Price,
                        TotalPrice = item.Product.Price * item.Quantity
                     
                    };
                    db.OrderDetails.Add(orderDetail);
                }
                db.Carts.RemoveRange(cartItems);
                db.SaveChanges();
                return Json(new { success = true, message = "Product added to cart successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }

        }
    }
}
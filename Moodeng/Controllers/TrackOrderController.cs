using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Moodeng.Controllers
{
    public class TrackOrderController : Controller
    {
        private moodeng_Entities db = new moodeng_Entities();

        // GET: TrackOrder
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var orders = db.Orders
                .Where(o => o.CustomerId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        // GET: OrderDetails
        public ActionResult OrderDetails(int id)
        {
            var userId = User.Identity.GetUserId();
            var order = db.Orders
                .Include("OrderDetails")
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null || order.CustomerId != userId)
            {
                return HttpNotFound(); 
            }

            return View(order);
        }

        public ActionResult TrackOrder()
        {
            var userId = User.Identity.GetUserId();
            var orders = db.Orders
                .Where(o => o.CustomerId == userId).Include(o => o.OrderDetails).ToList();
            return View(orders);
        }

    }
}

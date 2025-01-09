using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Moodeng.Models;
using System.Net;

namespace Moodeng.Controllers
{
    public class OrderRetailController : Controller
    {
        private moodeng_Entities db = new moodeng_Entities();

        // GET: OrderRetail
        public ActionResult Index()
        {
            var orders = db.Orders
                .AsNoTracking() // Improve query performance if data is read-only
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }


        // GET: OrderRetail/Details/5
        public ActionResult OrderDetails(int id)
        {
            var userId = User.Identity.GetUserId();
            var order = db.Orders
                .Include("OrderDetails")
                .FirstOrDefault(o => o.OrderId == id);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
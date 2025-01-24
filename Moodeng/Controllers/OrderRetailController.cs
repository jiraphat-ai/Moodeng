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

        // GET: OrderRetail/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            // Pass the current status to the view
            return View(order);
        }

        // POST: OrderRetail/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string status)
        {
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            if (!string.IsNullOrEmpty(status))
            {
                order.OrderStatus = status; // Update the status
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmOrder(int id)
        {
            var order = db.Orders.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }

            // เปลี่ยนสถานะเป็น "Completed"
            order.OrderStatus = "Completed";

            // บันทึกการเปลี่ยนแปลงในฐานข้อมูล
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges(); // บันทึกการเปลี่ยนแปลง

            // ส่งกลับไปยังหน้าแสดงรายละเอียดของคำสั่งซื้อ
            return RedirectToAction("OrderDetails", new { id = id });
        }





    }
}
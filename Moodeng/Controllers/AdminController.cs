using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Moodeng.Controllers
{
    public class AdminController : Controller
    {
        private readonly moodeng_Entities _context;

        public AdminController()
        {
            _context = new moodeng_Entities();
        }

        // GET: Admin
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var userRole = User.IsInRole("admin");
            var TotalUser = _context.AspNetUsers.Count();
            var totalRetail = _context.RetailStores.Count();
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var totalOrder = _context.Orders.Count(u => u.OrderDate >= today && u.OrderDate < tomorrow);
            ViewBag.TotalUser = TotalUser;
            ViewBag.TotalRetail = totalRetail;
            ViewBag.TotalOrder = totalOrder;
            var monthlyData = _context.Orders
     .Where(o => o.OrderDate.HasValue && o.OrderStatus == "Completed")
     .GroupBy(o => new { Year = o.OrderDate.Value.Year, Month = o.OrderDate.Value.Month }) // Use .Value
     .Select(g => new
     {
         Month = g.Key.Month,
         Year = g.Key.Year,
         Total = g.Sum(o => o.TotalAmount)
     })
     .OrderBy(g => g.Year)
     .ThenBy(g => g.Month)
     .ToList();

            var labels = monthlyData.Select(m =>
            {
                switch (m.Month)
                {
                    case 1: return "Jan";
                    case 2: return "Feb";
                    case 3: return "Mar";
                    case 4: return "Apr";
                    case 5: return "May";
                    case 6: return "Jun";
                    case 7: return "Jul";
                    case 8: return "Aug";
                    case 9: return "Sep";
                    case 10: return "Oct";
                    case 11: return "Nov";
                    case 12: return "Dec";
                    default: return "Unknown";
                }
            }).ToArray();

            var data = monthlyData.Select(m => m.Total).ToArray();


            ViewBag.G1_label = labels;
            ViewBag.G1_data = data;

            var TopData = _context.OrderDetails
     .GroupBy(o => new {name = o.ProductName }) // Use .Value
     .Select(g => new
     {
         Name = g.Key.name,
         Total = g.Count()
     }).OrderByDescending(o => o.Total).Take(5)
     .ToList();
            ViewBag.G2_label = TopData.Select(m => m.Name).ToArray();
            ViewBag.G2_data = TopData.Select(m => m.Total).ToArray();


            var LessData = _context.OrderDetails
    .GroupBy(o => new { name = o.ProductName }) // Use .Value
    .Select(g => new
    {
        Name = g.Key.name,
        Total = g.Count()
    }).OrderBy(o => o.Total).Take(5)
    .ToList();
            ViewBag.G3_label = LessData.Select(m => m.Name).ToArray();
            ViewBag.G3_data = LessData.Select(m => m.Total).ToArray();

            // Pending Orders

            var pendingOrdersCount = _context.Orders.Sum(o => (decimal?)o.TotalAmount) ?? 0;
            return View();
        }
        public ActionResult ManageUser()
        {
            return View();
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

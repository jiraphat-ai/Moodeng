using System;
using System.Linq;
using System.Web.Mvc;
using Moodeng.Models;

namespace Moodeng.Controllers
{
    public class AdminDashboardController : Controller
    {
        private readonly moodeng_Entities _context;

        public AdminDashboardController()
        {
            _context = new moodeng_Entities();
        }

        // GET: Dashboard
        public ActionResult Index()
        {
            // Total Sales
            var totalSales = _context.Orders.Sum(o => (decimal?)o.TotalAmount) ?? 0;
            // Pending Orders

            var pendingOrdersCount = 10;
            var completedOrdersCount = 6;


            // Best-Selling Products
            var bestSellingProducts = _context.OrderDetails
                .GroupBy(od => od.ProductName)
                .Select(g => new
                {
                    ProductName = g.Key,
                    TotalSold = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(p => p.TotalSold)
                .Take(5)
                .ToList();

            // Least-Selling Products
            var leastSellingProducts = _context.OrderDetails
                .GroupBy(od => od.ProductName)
                .Select(g => new
                {
                    ProductName = g.Key,
                    TotalSold = g.Sum(od => od.Quantity)
                })
                .OrderBy(p => p.TotalSold)
                .Take(5)
                .ToList();

            // Trending Products (e.g., sold in the last 30 days)
            var thirtyDaysAgo = DateTime.Now.AddDays(-30);
            var trendingProducts = _context.OrderDetails
                .Where(od => od.Order.OrderDate >= thirtyDaysAgo)
                .GroupBy(od => od.ProductName)
                .Select(g => new
                {
                    ProductName = g.Key,
                    TotalSold = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(p => p.TotalSold)
                .Take(5)
                .ToList();

            // Pass data to the view
            ViewBag.TotalSales = totalSales;
            ViewBag.BestSellingProducts = bestSellingProducts;
            ViewBag.LeastSellingProducts = leastSellingProducts;
            ViewBag.TrendingProducts = trendingProducts;
            ViewBag.PendingOrdersCount = pendingOrdersCount;
            ViewBag.CompletedOrdersCount = completedOrdersCount;

            return View();
        }


        }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moodeng.Models;
using System.Net;
using Microsoft.AspNet.Identity;

namespace Moodeng.Controllers
{
    public class HomeController : Controller
    {
        private moodeng_Entities db = new moodeng_Entities();

        public ActionResult Index(string searchCategory)
        {
            var products = GetProducts();

            if (!String.IsNullOrEmpty(searchCategory))
            {
                products = products.Where(p => p.Category.CategoryName.Equals(searchCategory, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(products);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        private List<Product> GetProducts()
        {
            return db.Products.ToList();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        private bool AddProductToWishlist(int productId)
        {
            var userId = User.Identity.GetUserId();
            var wishlistItem = new Wishlist
            {
                UserId = userId,
                ProductId = productId,
                AddedDate = DateTime.Now
            };

            db.Wishlists.Add(wishlistItem);
            int status = db.SaveChanges();
            // Simulate successful addition
            return status > 0;
        }
        [HttpPost]
        public JsonResult AddToWishlist(int productId)
        {
            try
            {
                // Logic to add the product to the wishlist
                // Example: Save productId to the database or session
                bool isAdded = AddProductToWishlist(productId); // Replace with your logic

                if (isAdded)
                {
                    return Json(new { success = true, message = "Product added to wishlist successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to add product to wishlist." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }
    }
}

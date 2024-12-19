using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Moodeng.Models;
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
                products = products
                    .Where(p => p.Category.CategoryName.Equals(searchCategory, StringComparison.OrdinalIgnoreCase))
                    .ToList();
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

            var product = db.Products.SingleOrDefault(x => x.ProductId == id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        private bool ToggleWishlist(int productId)
        {
            var userId = User.Identity.GetUserId();

            // ตรวจสอบว่า Product มีอยู่ใน Wishlist หรือไม่
            var existingWishlistItem = db.Wishlists
                .FirstOrDefault(w => w.UserId == userId && w.ProductId == productId);

            if (existingWishlistItem != null)
            {
                // หากมีอยู่แล้วให้ลบออก
                db.Wishlists.Remove(existingWishlistItem);
                db.SaveChanges();
                return false; // สถานะเป็นการลบออก
            }

            // หากไม่มี ให้เพิ่มเข้า Wishlist
            var wishlistItem = new Wishlist
            {
                UserId = userId,
                ProductId = productId,
                AddedDate = DateTime.Now
            };

            db.Wishlists.Add(wishlistItem);
            db.SaveChanges();
            return true; // สถานะเป็นการเพิ่ม
        }

        [HttpPost]
        public JsonResult AddToWishlist(int productId)
        {
            try
            {
                var isAdded = ToggleWishlist(productId);

                if (isAdded)
                {
                    return Json(new { success = true, message = "Product added to wishlist successfully!" });
                }
                else
                {
                    return Json(new { success = true, message = "Product removed from wishlist successfully!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }

        public ActionResult AddToCart(int productId, int quantity = 1)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                // ตรวจสอบว่าสินค้ามีอยู่ใน Cart หรือไม่
                var existingCartItem = db.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

                if (existingCartItem != null)
                {
                    // หากมีแล้วให้เพิ่มจำนวนสินค้า
                    existingCartItem.Quantity += quantity;
                }
                else
                {
                    // หากไม่มี ให้เพิ่มสินค้าใหม่ลงใน Cart
                    var cartItem = new Cart
                    {
                        UserId = userId,
                        ProductId = productId,
                        Quantity = quantity,
                        AddedDate = DateTime.Now
                    };

                    db.Carts.Add(cartItem);
                }

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

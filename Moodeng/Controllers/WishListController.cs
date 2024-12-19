using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Moodeng.Controllers
{
    public class WishListController : Controller
    {
        private moodeng_Entities db = new moodeng_Entities();

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var wishlistProducts = db.Wishlists
                .Where(w => w.UserId == userId).ToList();

            return View(wishlistProducts);
        }

        // Method to delete a product from the wishlist
        [HttpPost]
        public JsonResult RemoveFromWishlist(int? wishlistId)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                var wishlistItem = db.Wishlists
                    .FirstOrDefault(w => w.WishlistId == wishlistId && w.UserId == userId);

                if (wishlistItem == null)
                {
                    return Json(new { success = false, message = "Wishlist item not found." });
                }

                db.Wishlists.Remove(wishlistItem);
                db.SaveChanges();

                return Json(new { success = true, message = "Product removed from wishlist successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred: " + ex.Message });
            }
        }
        // Method to add a product to the cart
        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity = 1)
        {
            try
            {
                var product = db.Products.Find(productId);
                if (product == null)
                {
                    return Json(new { success = false, message = "Product not found." });
                }

                var userId = User.Identity.GetUserId();

                var existingCartItem = db.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += quantity;
                }
                else
                {
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
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

        // Method to display wishlist
      
    }
}
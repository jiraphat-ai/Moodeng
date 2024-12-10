using Microsoft.AspNet.Identity;
using Moodeng.Models;
using System.Linq;
using System.Web.Mvc;

namespace Moodeng.Controllers
{
    public class CartController : Controller
    {
        private moodeng_Entities db = new moodeng_Entities();

        // GET: Cart
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var userRole = User.IsInRole("customer");
            if (userRole)
            {

            }
           
            var cartItems = db.Carts
                .Where(c => c.UserId == userId)
                .ToList();

            return View(cartItems);
        }

        [HttpPost]
        public JsonResult RemoveFromCart(int productId)
        {
            var userId = User.Identity.GetUserId();
            var cartItem = db.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

            if (cartItem != null)
            {
                db.Carts.Remove(cartItem);
                db.SaveChanges();
                return Json(new { success = true, message = "Item removed from cart." });
            }
            return Json(new { success = false, message = "Item not found in cart." });
        }

        [HttpPost]
        public JsonResult UpdateQuantity(int productId, int quantity)
        {
            var userId = User.Identity.GetUserId();
            var cartItem = db.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                db.SaveChanges();
                return Json(new { success = true, message = "Cart updated successfully." });
            }
            return Json(new { success = false, message = "Item not found in cart." });
        }

        [HttpPost]
        public ActionResult ProceedToPayment()
        {
            var userId = User.Identity.GetUserId();
            var cartItems = db.Carts.Where(c => c.UserId == userId).ToList();

            Session["Cart"] = cartItems;

            return Json(new { success = true, message = "Proceed to Checkout Successful" });
        }

    }
}

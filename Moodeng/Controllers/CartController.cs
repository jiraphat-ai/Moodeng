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
                // สามารถเพิ่มเงื่อนไขเพิ่มเติมได้ หากต้องการ
            }

            // ดึงสินค้าที่อยู่ใน Cart ของผู้ใช้
            var cartItems = db.Carts
                .Where(c => c.UserId == userId)
                .ToList();

            // ดึง Categories ของสินค้าที่อยู่ใน Cart และกรองข้อมูลที่ไม่ซ้ำ
            var categories = cartItems
                .Select(c => c.Product.Category)
                .Distinct()
                .ToList();

            // ส่ง Categories ไปยัง View ผ่าน ViewBag
            ViewBag.Categories = categories;

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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Moodeng;

namespace Moodeng.Controllers
{
    public class WishlistsController : ApiController
    {
        private moodeng_Entities db = new moodeng_Entities();

        // GET: api/Wishlists
        public IHttpActionResult GetWishlists()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return Json(db.Wishlists.ToList());
        }

        // GET: api/Wishlists/5
        [ResponseType(typeof(Wishlist))]
        public async Task<IHttpActionResult> GetWishlist(int id)
        {
            Wishlist wishlist = await db.Wishlists.FindAsync(id);
            if (wishlist == null)
            {
                return NotFound();
            }

            return Ok(wishlist);
        }

        // PUT: api/Wishlists/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutWishlist(int id, Wishlist wishlist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != wishlist.WishlistId)
            {
                return BadRequest();
            }

            db.Entry(wishlist).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WishlistExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Wishlists
        [ResponseType(typeof(Wishlist))]
        public async Task<IHttpActionResult> PostWishlist(Wishlist wishlist)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Wishlists.Add(wishlist);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = wishlist.WishlistId }, wishlist);
        }

        // DELETE: api/Wishlists/5
        [ResponseType(typeof(Wishlist))]
        public async Task<IHttpActionResult> DeleteWishlist(int id)
        {
            Wishlist wishlist = await db.Wishlists.FindAsync(id);
            if (wishlist == null)
            {
                return NotFound();
            }

            db.Wishlists.Remove(wishlist);
            await db.SaveChangesAsync();

            return Ok(wishlist);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WishlistExists(int id)
        {
            return db.Wishlists.Count(e => e.WishlistId == id) > 0;
        }
    }
}
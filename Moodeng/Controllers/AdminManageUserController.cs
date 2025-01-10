using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Moodeng;

namespace Moodeng.Controllers
{
    public class AdminManageUserController : Controller
    {
        private moodeng_Entities db = new moodeng_Entities();

        // GET: AdminManageUser
        public ActionResult Index(string searchTerm, string role, string status)
        {
            var users = db.AspNetUsers.AsQueryable();

            // ค้นหาจากชื่อหรืออีเมล
            if (!string.IsNullOrEmpty(searchTerm))
            {
                users = users.Where(u => u.UserName.Contains(searchTerm) 
                                     || u.Email.Contains(searchTerm));
            }

            // กรองตาม Role
            if (!string.IsNullOrEmpty(role) && role != "All")
            {
                users = users.Where(u => u.Role == role);
            }

            // กรองตามสถานะ
            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                bool isActive = status == "Active";
                users = users.Where(u => u.LockoutEnabled == isActive);
            }

            // ส่งค่าที่ใช้กรองไปแสดงใน View
            ViewBag.CurrentSearch = searchTerm;
            ViewBag.CurrentRole = role;
            ViewBag.CurrentStatus = status;

            return View(users.ToList());
        }

        // GET: AdminManageUser/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // GET: AdminManageUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminManageUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,UserName,PhoneNumber,PasswordHash,Role")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                // สร้าง ID ใหม่
                aspNetUser.Id = Guid.NewGuid().ToString();
                
                // ตั้งค่าเริ่มต้น
                aspNetUser.EmailConfirmed = false;
                aspNetUser.PhoneNumberConfirmed = false;
                aspNetUser.TwoFactorEnabled = false;
                aspNetUser.LockoutEnabled = true;
                aspNetUser.AccessFailedCount = 0;
                aspNetUser.SecurityStamp = Guid.NewGuid().ToString();

                try 
                {
                    db.AspNetUsers.Add(aspNetUser);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error creating user. " + ex.Message);
                }
            }

            return View(aspNetUser);
        }

        // GET: AdminManageUser/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: AdminManageUser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,Role")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetUser);
        }

        // GET: AdminManageUser/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: AdminManageUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

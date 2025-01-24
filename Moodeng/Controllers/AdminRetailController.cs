using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Web.Security;
using Moodeng.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;


namespace Moodeng.Controllers
{
    public class AdminRetailController : Controller
    {
        private moodeng_Entities db = new moodeng_Entities();
        [Authorize]
        public ActionResult Create()
        {
            var categories = db.Categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            });
            var model = new ProductCreateViewModel
            {
                Categories = categories
            };
            ViewBag.Categories = categories;
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCreateViewModel model, HttpPostedFileBase picture)
        {

            if (ModelState.IsValid)

            {

                var product = new Product

                {

                    CategoryId = model.CategoryId,

                    ProductName = model.ProductName,

                    Price = model.Price,

                    Description = model.Description,

                    RetailId = User.Identity.GetUserId()

                };



                if (picture != null)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var relativePath = "/Content/images/" + fileName;
                        var absolutePath = Server.MapPath(relativePath);
                        file.SaveAs(absolutePath);
                        product.Picture = relativePath; // Save the relative path in the database
                    }
                }

                db.Products.Add(product);

                db.SaveChanges();

                return RedirectToAction("Index", "AdminRetail");

            }

            return View(model);

        }



        // GET: ProductsRetail/Details/5
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


        [HttpPost]

        [Authorize]

        [ValidateAntiForgeryToken]

        public ActionResult Delete(int id)

        {

            var product = db.Products.Find(id);

            if (product == null)

            {

                return HttpNotFound();

            }



            if (product.RetailId != User.Identity.GetUserId())

            {

                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            }



            db.Products.Remove(product);

            db.SaveChanges();

            return RedirectToAction("Index", "AdminRetail");

        }



         // GET: ProductsRetail/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // POST: ProductsRetail/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,CategoryId,ProductName,Price,Picture,Description,RetailId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        
        }



        [Authorize]

        public ActionResult Index()

        {

            string userId = User.Identity.GetUserId();

            ViewBag.Categories = db.Categories

                .Select(c => new SelectListItem

                {

                    Value = c.CategoryId.ToString(),

                    Text = c.CategoryName

                })

                .ToList();



            var products = db.Products

                .Include(p => p.Category)

                .Where(p => p.RetailId == userId)

                .ToList();



            return View(products);

        }

        public static string GetImagePath(string picture)
        {
            if (string.IsNullOrEmpty(picture))
            {
                return "/images/default.jpg"; 
            }

            if (Uri.IsWellFormedUriString(picture, UriKind.Absolute))
            {
                return picture; 
            }

            return $"/uploads/{picture}";
        }


    }
}
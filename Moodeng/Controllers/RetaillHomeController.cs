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

    public class RetaillHome : Controller

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

                    using (var ms = new MemoryStream())

                    {

                        picture.InputStream.CopyTo(ms);

                        string base64 = Convert.ToBase64String(ms.ToArray());

                        product.Picture = "data:" + picture.ContentType + ";base64," + base64;

                    }

                }



                db.Products.Add(product);

                db.SaveChanges();

                return RedirectToAction("Details", new { id = product.ProductId });

            }

            return View(model);

        }



        public ActionResult Details(int id)

        {

            var product = db.Products.Find(id);

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

            return RedirectToAction("Index", "Home");

        }



        [Authorize]

        public ActionResult Edit(int id)

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



            var categories = db.Categories.Select(c => new SelectListItem

            {

                Value = c.CategoryId.ToString(),

                Text = c.CategoryName

            });



            var model = new ProductCreateViewModel

            {

                ProductId = product.ProductId,

                CategoryId = product.CategoryId,

                ProductName = product.ProductName,

                Price = product.Price,

                Description = product.Description,

                Picture = product.Picture,

                Categories = categories

            };



            return View(model);

        }



        [HttpPost]

        [Authorize]

        [ValidateAntiForgeryToken]

        public ActionResult Edit(ProductCreateViewModel model, HttpPostedFileBase picture)

        {

            if (ModelState.IsValid)

            {

                var product = db.Products.Find(model.ProductId);

                if (product == null)

                {

                    return HttpNotFound();

                }



                if (product.RetailId != User.Identity.GetUserId())

                {

                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

                }



                product.CategoryId = model.CategoryId;

                product.ProductName = model.ProductName;

                product.Price = model.Price;

                product.Description = model.Description;



                if (picture != null)

                {

                    using (var ms = new MemoryStream())

                    {

                        picture.InputStream.CopyTo(ms);

                        string base64 = Convert.ToBase64String(ms.ToArray());

                        product.Picture = "data:" + picture.ContentType + ";base64," + base64;

                    }

                }



                db.SaveChanges();

                return RedirectToAction("Details", new { id = product.ProductId });

            }

            return View(model);

        }



        [Authorize]

        public ActionResult Index()

        {

            string userId = User.Identity.GetUserId();

            var products = db.Products

                .Include("Category")

                .Where(p => p.RetailId == userId)

                .ToList();

            return View(products);

        }

    }

}

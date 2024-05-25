using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;
using QLLaCoffee.App_Start;
using QLLaCoffee.Models;

namespace QLLaCoffee.Areas.Admin.Controllers
{
    public class DrinksController : Controller
    {
        private LaCoffeeDBContext db = new LaCoffeeDBContext();

        [RoleUser(FunctionID = "DU_Xem")]
        public ActionResult Index()
        {
            var drinks = db.Drinks.Include(d => d.DrinkCategories);
            return View(drinks.ToList());
        }

        [HttpPost]
        [RoleUser(FunctionID = "DU_Xem")]
        public ActionResult Index(string drinkName, string drinkCategoryName, decimal? minPrice, decimal? maxPrice)
        {
            ViewBag.DrinkName = drinkName;
            ViewBag.DrinkCategoryName = drinkCategoryName;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            var result = (from item in db.Drinks
                          where (item.DrinkName.ToLower().Contains(drinkName.ToLower()) || string.IsNullOrEmpty(drinkName)) &&
                                 (item.DrinkCategories.DrinkCategoryName.ToLower().Contains(drinkCategoryName.ToLower()) || string.IsNullOrEmpty(drinkCategoryName)) &&
                                 (item.DrinkPrice >= minPrice || minPrice == null) &&
                                 (item.DrinkPrice <= maxPrice || maxPrice == null)
                          select item).ToList();
            return View(result);
        }

        [RoleUser(FunctionID = "DU_Them")]
        public ActionResult Create()
        {
            ViewBag.DrinkCategoryID = new SelectList(db.DrinkCategories, "DrinkCategoryID", "DrinkCategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Drinks drinks)
        {
            if (ModelState.IsValid)
            {
                db.Drinks.Add(drinks);
                var img = Request.Files["img"];
                if (img != null && img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))
                    {
                        string imgName = drinks.DrinkName + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        drinks.DrinkImage = imgName;
                        string PathDir = "~/Public/images/drinks/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DrinkCategoryID = new SelectList(db.DrinkCategories, "DrinkCategoryID", "DrinkCategoryName", drinks.DrinkCategoryID);
            return View(drinks);
        }

        [RoleUser(FunctionID = "DU_Sua")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Drinks drinks = db.Drinks.Find(id);
            if (drinks == null)
            {
                return HttpNotFound();
            }
            ViewBag.DrinkCategoryID = new SelectList(db.DrinkCategories, "DrinkCategoryID", "DrinkCategoryName", drinks.DrinkCategoryID);
            return View(drinks);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Drinks drinks)
        {
            if (ModelState.IsValid)
            {
                var img = Request.Files["img"];
                if (img != null && img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))
                    {
                        string imgName = drinks.DrinkName + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        drinks.DrinkImage = imgName;
                        string PathDir = "~/Public/images/drinks/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }
                db.Entry(drinks).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DrinkCategoryID = new SelectList(db.DrinkCategories, "DrinkCategoryID", "DrinkCategoryName", drinks.DrinkCategoryID);
            return View(drinks);
        }

        [HttpPost]
        [RoleUser(FunctionID = "DU_Xoa")]
        public ActionResult Delete(int id)
        {
            Drinks drinks = db.Drinks.Find(id);
            string PathDir = "~/Public/images/drinks/";
            if (drinks.DrinkImage != null)
            {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(drinks.DrinkImage);
                string[] DelPath = Directory.GetFiles(Server.MapPath(PathDir), fileName + ".*");
                foreach (string dp in DelPath)
                {
                    System.IO.File.Delete(dp);
                }
            }
            db.Drinks.Remove(drinks);
            db.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        [RoleUser(FunctionID = "DU_Xoa")]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var o = db.Drinks.Find(Convert.ToInt32(item));
                        string PathDir = "~/Public/images/drinks/";
                        if (o.DrinkImage != null)
                        {
                            string fileName = System.IO.Path.GetFileNameWithoutExtension(o.DrinkImage);
                            string[] DelPath = Directory.GetFiles(Server.MapPath(PathDir), fileName + ".*");
                            foreach (string dp in DelPath)
                            {
                                System.IO.File.Delete(dp);
                            }
                        }
                        db.Drinks.Remove(o);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}

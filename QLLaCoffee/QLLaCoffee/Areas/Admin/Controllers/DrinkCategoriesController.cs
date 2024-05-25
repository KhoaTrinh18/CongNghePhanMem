using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLLaCoffee.App_Start;
using QLLaCoffee.Models;

namespace QLLaCoffee.Areas.Admin.Controllers
{
    [RoleUser(FunctionID = "LDU_QuanLy")]
    public class DrinkCategoriesController : Controller
    {
        private LaCoffeeDBContext db = new LaCoffeeDBContext();

        public ActionResult Index()
        {
            return View(db.DrinkCategories.ToList());
        }

        [HttpPost]
        public ActionResult Index(string drinkCategoryName)
        {
            ViewBag.DrinkCategoryName = drinkCategoryName;
            var result = (from item in db.DrinkCategories
                          where (item.DrinkCategoryName.ToLower().Contains(drinkCategoryName.ToLower()) || string.IsNullOrEmpty(drinkCategoryName))
                          select item).ToList();
            return View(result);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DrinkCategories drinkCategories)
        {
            if (ModelState.IsValid)
            {
                db.DrinkCategories.Add(drinkCategories);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(drinkCategories);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DrinkCategories drinkCategories = db.DrinkCategories.Find(id);
            if (drinkCategories == null)
            {
                return HttpNotFound();
            }
            return View(drinkCategories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DrinkCategories drinkCategories)
        {
            if (ModelState.IsValid)
            {
                db.Entry(drinkCategories).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(drinkCategories);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            DrinkCategories drinkCategory = db.DrinkCategories.Find(id);
            db.DrinkCategories.Remove(drinkCategory);
            db.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var o = db.DrinkCategories.Find(Convert.ToInt32(item));
                        db.DrinkCategories.Remove(o);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}

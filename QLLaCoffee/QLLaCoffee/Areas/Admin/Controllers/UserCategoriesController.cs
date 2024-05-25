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
    [RoleUser(FunctionID = "LND_QuanLy")]
    public class UserCategoriesController : Controller
    {
        private LaCoffeeDBContext db = new LaCoffeeDBContext();

        public ActionResult Index()
        {
            return View(db.UserCategories.ToList());
        }

        [HttpPost]
        public ActionResult Index(string userCategoryName)
        {
            ViewBag.UserCategoryName = userCategoryName;
            var result = (from item in db.UserCategories
                          where (item.UserCategoryName.ToLower().Contains(userCategoryName.ToLower()) || string.IsNullOrEmpty(userCategoryName))
                          select item).ToList();
            return View(result);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserCategories userCategories)
        {
            if (ModelState.IsValid)
            {
                db.UserCategories.Add(userCategories);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userCategories);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserCategories userCategories = db.UserCategories.Find(id);
            if (userCategories == null)
            {
                return HttpNotFound();
            }
            return View(userCategories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserCategories userCategories)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userCategories).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userCategories);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            UserCategories userCategory = db.UserCategories.Find(id);
            db.UserCategories.Remove(userCategory);
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
                        var o = db.UserCategories.Find(Convert.ToInt32(item));
                        db.UserCategories.Remove(o);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}

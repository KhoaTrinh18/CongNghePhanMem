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
    [RoleUser(FunctionID = "LMH_QuanLy")]
    public class GoodsCategoriesController : Controller
    {
        private LaCoffeeDBContext db = new LaCoffeeDBContext();

        public ActionResult Index()
        {
            return View(db.GoodsCategories.ToList());
        }

        [HttpPost]
        public ActionResult Index(string goodsCategoryName)
        {
            ViewBag.GoodsCategoryName = goodsCategoryName;
            var result = (from item in db.GoodsCategories
                          where (item.GoodsCategoryName.ToLower().Contains(goodsCategoryName.ToLower()) || string.IsNullOrEmpty(goodsCategoryName))                
                          select item).ToList();
            return View(result);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GoodsCategories goodsCategories)
        {
            if (ModelState.IsValid)
            {
                db.GoodsCategories.Add(goodsCategories);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(goodsCategories);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GoodsCategories goodsCategories = db.GoodsCategories.Find(id);
            if (goodsCategories == null)
            {
                return HttpNotFound();
            }
            return View(goodsCategories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GoodsCategories goodsCategories)
        {
            if (ModelState.IsValid)
            {
                db.Entry(goodsCategories).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(goodsCategories);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            GoodsCategories goodsCategory = db.GoodsCategories.Find(id);
            db.GoodsCategories.Remove(goodsCategory);
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
                        var o = db.GoodsCategories.Find(Convert.ToInt32(item));
                        db.GoodsCategories.Remove(o);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}

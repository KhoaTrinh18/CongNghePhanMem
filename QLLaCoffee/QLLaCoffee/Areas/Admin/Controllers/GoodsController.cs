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
    public class GoodsController : Controller
    {
        private LaCoffeeDBContext db = new LaCoffeeDBContext();

        [RoleUser(FunctionID = "MH_Xem")]
        public ActionResult Index()
        {
            var goods = db.Goods.Include(g => g.GoodsCategories);
            ViewBag.GoodsID = new SelectList(db.Goods, "GoodsID", "GoodsName");
            return View(goods.ToList());
        }

        [HttpPost]
        public ActionResult Index(string goodsName, string goodsCategoryName, string goodsUnit, decimal? minPrice, decimal? maxPrice, int? minCount, int? maxCount)
        { 
            ViewBag.GoodsName = goodsName;
            ViewBag.GoodsCategoryName = goodsCategoryName;
            ViewBag.GoodsUnit = goodsUnit;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.MinCount = minCount;
            ViewBag.MaxCount = maxCount;
            var result = (from item in db.Goods
                          where (item.GoodsName.ToLower().Contains(goodsName.ToLower()) || string.IsNullOrEmpty(goodsName)) &&
                                 (item.GoodsCategories.GoodsCategoryName.ToLower().Contains(goodsCategoryName.ToLower()) || string.IsNullOrEmpty(goodsCategoryName)) &&
                                 (item.GoodsUnit.ToLower().Contains(goodsUnit.ToLower()) || string.IsNullOrEmpty(goodsUnit)) &&
                                 (item.GoodsCount >= minCount || minCount == null) &&
                                 (item.GoodsCount <= maxCount || maxCount == null) &&
                                 (item.GoodsPrice >= minPrice || minPrice == null) &&
                                 (item.GoodsPrice <= maxPrice || maxPrice == null)
                          select item).ToList();
            return View(result);
        }

        [RoleUser(FunctionID = "MH_Them")]
        public ActionResult Create()
        {
            ViewBag.GoodsCategoryID = new SelectList(db.GoodsCategories, "GoodsCategoryID", "GoodsCategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Goods goods)
        {
            if (ModelState.IsValid)
            {
                db.Goods.Add(goods);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GoodsCategoryID = new SelectList(db.GoodsCategories, "GoodsCategoryID", "GoodsCategoryName", goods.GoodsCategoryID);
            return View(goods);
        }

        [RoleUser(FunctionID = "MH_Sua")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goods goods = db.Goods.Find(id);
            if (goods == null)
            {
                return HttpNotFound();
            }
            ViewBag.GoodsCategoryID = new SelectList(db.GoodsCategories, "GoodsCategoryID", "GoodsCategoryName", goods.GoodsCategoryID);
            return View(goods);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Goods goods)
        {
            if (ModelState.IsValid)
            {
                db.Entry(goods).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GoodsCategoryID = new SelectList(db.GoodsCategories, "GoodsCategoryID", "GoodsCategoryName", goods.GoodsCategoryID);
            return View(goods);
        }

        [HttpPost]
        [RoleUser(FunctionID = "MH_Xoa")]
        public ActionResult Delete(int id)
        {
            Goods goods = db.Goods.Find(id);
            db.Goods.Remove(goods);
            db.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        [RoleUser(FunctionID = "MH_Xoa")]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var o = db.Goods.Find(Convert.ToInt32(item));
                        db.Goods.Remove(o);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}

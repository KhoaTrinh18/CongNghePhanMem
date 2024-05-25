using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLLaCoffee.App_Start;
using QLLaCoffee.Models;

namespace QLLaCoffee.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private LaCoffeeDBContext db = new LaCoffeeDBContext();

        [RoleUser(FunctionID = "ND_Xem")]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        [HttpPost]
        public ActionResult Index(string fullName, string gender, string userCategoryName)
        {
            ViewBag.FullName = fullName;
            ViewBag.Gender = gender;
            ViewBag.UserCategoryName = userCategoryName;
            var result = (from item in db.Users
                          where (item.FullName.ToLower().Contains(fullName.ToLower()) || string.IsNullOrEmpty(fullName)) &&
                                (item.Gender.ToLower().Contains(gender.ToLower()) || string.IsNullOrEmpty(gender)) &&
                                (item.UserCategories.UserCategoryName.ToLower().Contains(userCategoryName.ToLower()) || string.IsNullOrEmpty(userCategoryName)) 
                          select item).ToList();
            return View(result);
        }

        [RoleUser(FunctionID = "ND_Them")]
        public ActionResult Create()
        {
            ViewBag.UserCategoryID = new SelectList(db.UserCategories, "UserCategoryID", "UserCategoryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var img = Request.Files["img"];
                if (img != null && img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))
                    {
                        string imgName = user.UserImage + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        user.UserImage = imgName;
                        string PathDir = "~/Public/images/users/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }
                if (db.Users.Count(u => u.AccountName == user.AccountName) > 0)
                {
                    ViewBag.Error = "Tên đăng nhập đã tồn tại";
                    ViewBag.UserCategoryID = new SelectList(db.UserCategories, "UserCategoryID", "UserCategoryName", user.UserCategoryID);
                    return View();
                }
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserCategoryID = new SelectList(db.UserCategories, "UserCategoryID", "UserCategoryName", user.UserCategoryID);
            return View(user);
        }

        [RoleUser(FunctionID = "ND_Sua")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserCategoryID = new SelectList(db.UserCategories, "UserCategoryID", "UserCategoryName", user.UserCategoryID);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var img = Request.Files["img"];
                if (img != null && img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))
                    {
                        string imgName = user.UserImage + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        user.UserImage = imgName;
                        string PathDir = "~/Public/images/users/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserCategoryID = new SelectList(db.UserCategories, "UserCategoryID", "UserCategoryName", user.UserCategoryID);
            return View(user);
        }


        [HttpPost]
        [RoleUser(FunctionID = "ND_Xoa")]
        public ActionResult Delete(int id)
        {
            User user = db.Users.Find(id);
            string PathDir = "~/Public/images/users/";
            if (user.UserImage != null)
            {
                string fileName = System.IO.Path.GetFileNameWithoutExtension(user.UserImage);
                string[] DelPath = Directory.GetFiles(Server.MapPath(PathDir), fileName + ".*");
                foreach (string dp in DelPath)
                {
                    System.IO.File.Delete(dp);
                }
            }
            db.Users.Remove(user);
            db.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        [RoleUser(FunctionID = "ND_Xoa")]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var o = db.Users.Find(Convert.ToInt32(item));
                        string PathDir = "~/Public/images/users/";
                        if (o.UserImage != null)
                        {
                            string fileName = System.IO.Path.GetFileNameWithoutExtension(o.UserImage);
                            string[] DelPath = Directory.GetFiles(Server.MapPath(PathDir), fileName + ".*");
                            foreach (string dp in DelPath)
                            {
                                System.IO.File.Delete(dp);
                            }
                        }
                        db.Users.Remove(o);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}

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
    public class AuthorizationsController : Controller
    {
        private LaCoffeeDBContext db = new LaCoffeeDBContext();

        [RoleUser(FunctionID = "PQ")]
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

        [RoleUser(FunctionID = "PQ")]
        public ActionResult Authorize(int UserCategoryID)
        {
            ViewBag.ListFunctions = db.Functions.ToList();
            UserCategories userCategories = db.UserCategories.Find(UserCategoryID);
            return View(userCategories);
        }

        [HttpPost]
        public ActionResult Authorize(int UserCategoryID, List<string> functionIDs)
        {
            // Xóa các quyền hiện tại
            var existingAuthorizations = db.Authorizations.Where(a => a.UserCategoryID == UserCategoryID).ToList();
            db.Authorizations.RemoveRange(existingAuthorizations);
            db.SaveChanges();

            // Thêm các quyền mới
            if (functionIDs != null && functionIDs.Count > 0)
            {
                foreach (var functionID in functionIDs)
                {
                    var authorization = new Authorizations
                    {
                        UserCategoryID = UserCategoryID,
                        FunctionID = functionID
                    };
                    db.Authorizations.Add(authorization);
                }
                db.SaveChanges();
            }
            return RedirectToAction("Authorize", new { UserCategoryID  = UserCategoryID});
        }
        public ActionResult ErrorAuthorization()
        {
            return View();
        }
    }
}

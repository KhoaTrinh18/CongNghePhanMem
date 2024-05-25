using QLLaCoffee.App_Start;
using QLLaCoffee.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLLaCoffee.Controllers
{
    public class LoginController : Controller
    {
        private LaCoffeeDBContext db = new LaCoffeeDBContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string accountName, string password)
        {
            foreach (var user in db.Users)
            {
                if (user.AccountName == accountName && user.Password == password)
                {
                    SessionConfig.SetUser(user);
                    if (user.UserCategories.UserCategoryName != "Quản lý kho")
                    {
                        return RedirectToAction("Index", "Sell", new { area = "Admin", showAmountModal = true });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Goods", new { area = "Admin" });
                    }
                }
            }
            ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng";
            return View();
        }

        public ActionResult Logout()
        {
            SessionConfig.SetUser(null);
            SessionConfig.SetShiftReport(null);
            return RedirectToAction("Login");
        }
    }
}

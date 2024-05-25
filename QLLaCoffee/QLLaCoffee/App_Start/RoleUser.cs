using QLLaCoffee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QLLaCoffee.App_Start
{
    public class RoleUser : AuthorizeAttribute
    {
        public String FunctionID { get; set; }

        LaCoffeeDBContext db = new LaCoffeeDBContext();
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = SessionConfig.GetUser();
            if (user == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        controller = "Login",
                        action = "Login",
                        area = ""
                    }));
                return;
            }

            if (!string.IsNullOrEmpty(FunctionID))
            {
                var count = db.Authorizations.Count(m => m.UserCategoryID == user.UserCategoryID && m.FunctionID == FunctionID);
                if (count <= 0)
                {
                    filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        controller = "Authorizations",
                        action = "ErrorAuthorization",
                        area = "Admin"
                    }));
                    return;
                }
            }
            return;
        }
    }
}
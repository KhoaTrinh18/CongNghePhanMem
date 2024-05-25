using QLLaCoffee.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLLaCoffee.App_Start
{
    public static class SessionConfig
    {
        public static void SetUser(User user)
        {
            HttpContext.Current.Session["user"] = user;
        }

        public static User GetUser()
        {
            return (User)HttpContext.Current.Session["user"];
        }

        public static void SetShiftReport(ShiftReports shiftReports)
        {
            HttpContext.Current.Session["shiftReports"] = shiftReports;
        }

        public static ShiftReports GetShiftReport()
        {
            return (ShiftReports)HttpContext.Current.Session["shiftReports"];
        }
    }
}
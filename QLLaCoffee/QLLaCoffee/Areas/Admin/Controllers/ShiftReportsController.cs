using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using QLLaCoffee.App_Start;
using QLLaCoffee.Models;
using SelectPdf;

namespace QLLaCoffee.Areas.Admin.Controllers
{
    [RoleUser(FunctionID = "BCKC")]
    public class ShiftReportsController : BaseController
    {
        private LaCoffeeDBContext db = new LaCoffeeDBContext();

        // GET: Admin/ShiftReports
        public ActionResult Index()
        {
            ViewBag.Check = 0;
            var shiftReports = SessionConfig.GetShiftReport();
            shiftReports.BillCount = db.Bills.Where(b => b.CreateDate >= shiftReports.FirstTime && b.TableID == null).Count();
            if (db.Bills.Where(b => b.CreateDate >= shiftReports.FirstTime && b.TableID == null).Count() > 0)
            {
                shiftReports.Revenue = db.Bills.Where(b => b.CreateDate >= shiftReports.FirstTime && b.TableID == null).Sum(b => b.TotalAmount);
            }
            if (db.Bills.Where(b => b.Tables.Status == true).Count() > 0)
            {
                shiftReports.UncollectedAmount = db.Bills.Where(b => b.Tables.Status == true).Sum(b => b.TotalAmount);
            }
            db.Entry(shiftReports).State = EntityState.Modified;
            db.SaveChanges();
            return View(shiftReports);
        }

        [HttpPost]
        public ActionResult Index(decimal? lastAmount)
        {
            var shiftReports = SessionConfig.GetShiftReport();
            if (lastAmount == null)
            {
                ViewBag.Check = 1;
                ViewBag.Error = "Số tiền kết ca không được để trống";
                return View(shiftReports);

            }
            else
            {
                shiftReports.LastAmount = (decimal)lastAmount;
                shiftReports.LastTime = DateTime.Now;
                db.Entry(shiftReports).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("ExportPDF");
        }

        public ActionResult ExportPDF()
        {
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A6;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;
            converter.Options.MarginTop = 20;
            converter.Options.MarginBottom = 20;
            var shiftReports = SessionConfig.GetShiftReport();
            var htmlPDF = base.RenderPartialToString("~/Areas/Admin/Views/ShiftReports/BaoCaoKetCa.cshtml", shiftReports);
            PdfDocument doc = converter.ConvertHtmlString(htmlPDF);
            string fileName = string.Format("{0}.pdf", shiftReports.ShiftReportID);
            string pathFile = Path.Combine(Server.MapPath("~/Public/PDF"), fileName);
            doc.Save(pathFile);
            doc.Close();
            return RedirectToAction("Logout", "Login", new { area = ""});
        }
    }
}

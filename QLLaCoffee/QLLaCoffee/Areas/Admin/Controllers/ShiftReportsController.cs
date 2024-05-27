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
    public class ShiftReportsController : BaseController
    {
        private LaCoffeeDBContext db = new LaCoffeeDBContext();

        [RoleUser(FunctionID = "KC")]
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

        [RoleUser(FunctionID = "KC")]
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

        [RoleUser(FunctionID = "KC")]
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
        [RoleUser(FunctionID = "BCKC_Xem")]
        public ActionResult DanhSach()
        {
            return View(db.ShiftReports.ToList());
        }

        [HttpPost]
        [RoleUser(FunctionID = "BCKC_Xem")]
        public ActionResult DanhSach(DateTime? createDate)
        {
            if (createDate != null)
            {
                var result = (from item in db.ShiftReports
                              where (item.LastTime.Day == createDate.Value.Day) &&
                                    (item.LastTime.Month == createDate.Value.Month) &&
                                    (item.LastTime.Year == createDate.Value.Year) 
                              select item).ToList();
                return View(result);
            }
            return RedirectToAction("DanhSach");
        }

        [HttpPost]
        [RoleUser(FunctionID = "BCKC_Xoa")]
        public ActionResult Delete(String id)
        {
            ShiftReports shiftReports = db.ShiftReports.Find(id);
            db.ShiftReports.Remove(shiftReports);
            db.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        [RoleUser(FunctionID = "BCKC_Xoa")]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var o = db.ShiftReports.Find(item.ToString());
                        db.ShiftReports.Remove(o);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [RoleUser(FunctionID = "BCKC_XemCT")]
        public ActionResult Details(String id)
        {
            TempData["ShiftReportID"] = id;
            ShiftReports shiftReports = db.ShiftReports.Find(id);
            return View(shiftReports);
        }

        [RoleUser(FunctionID = "BCKC_XemCT")]
        public ActionResult ExportPDFCT()
        {
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A6;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;
            converter.Options.MarginTop = 20;
            converter.Options.MarginBottom = 20;
            String shiftReportID = TempData["ShiftReportID"] as String;
            ShiftReports shiftReports = db.ShiftReports.Find(shiftReportID);
            var htmlPDF = base.RenderPartialToString("~/Areas/Admin/Views/ShiftReports/BaoCaoKetCa.cshtml", shiftReports);
            PdfDocument doc = converter.ConvertHtmlString(htmlPDF);
            string fileName = string.Format("{0}.pdf", shiftReports.ShiftReportID);
            string pathFile = Path.Combine(Server.MapPath("~/Public/PDF"), fileName);
            doc.Save(pathFile);
            doc.Close();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}

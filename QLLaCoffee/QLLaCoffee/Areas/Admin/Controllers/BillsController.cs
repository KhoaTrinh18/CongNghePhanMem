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
using SelectPdf;

namespace QLLaCoffee.Areas.Admin.Controllers
{
    
    public class BillsController : BaseController
    {
        private LaCoffeeDBContext db = new LaCoffeeDBContext();

        [RoleUser(FunctionID = "HD_Xem")]
        public ActionResult Index()
        {
            return View(db.Bills.Where(b => b.TableID == null).ToList());
        }

        [HttpPost]
        [RoleUser(FunctionID = "HD_Xem")]
        public ActionResult Index(DateTime? createDate)
        {
            if (createDate != null)
            {
                var result = (from item in db.Bills
                              where (item.CreateDate.Day == createDate.Value.Day) &&
                                    (item.CreateDate.Month == createDate.Value.Month) &&
                                    (item.CreateDate.Year == createDate.Value.Year) &&
                                    (item.TableID == null)
                              select item).ToList();
                return View(result);
            }      
            return RedirectToAction("Index");
        }

        [HttpPost]
        [RoleUser(FunctionID = "HD_Xoa")]
        public ActionResult Delete(String id)
        {
            Bills bill = db.Bills.Find(id);
            db.Bills.Remove(bill);
            db.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        [RoleUser(FunctionID = "HD_Xoa")]
        public ActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var o = db.Bills.Find(item.ToString());
                        db.Bills.Remove(o);
                        db.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [RoleUser(FunctionID = "HD_XemCT")]
        public ActionResult Details(String id)
        {
            TempData["BillID"] = id;
            Bills bills = db.Bills.Find(id);
            return View(bills.BillInfos.ToList());  
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
            String billID = TempData["BillID"] as String;
            Bills bills = db.Bills.Find(billID);
            var htmlPDF = base.RenderPartialToString("~/Areas/Admin/Views/Bills/HoaDon.cshtml", bills.BillInfos.ToList());
            PdfDocument doc = converter.ConvertHtmlString(htmlPDF);
            string fileName = string.Format("{0}.pdf", bills.BillID);
            string pathFile = Path.Combine(Server.MapPath("~/Public/PDF"), fileName);
            doc.Save(pathFile);
            doc.Close();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}

using QLLaCoffee.App_Start;
using QLLaCoffee.Models;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLLaCoffee.Areas.Admin.Controllers
{
    [RoleUser(FunctionID = "ThongKe")]
    public class ThongKeController : BaseController
    {
        LaCoffeeDBContext db = new LaCoffeeDBContext();
        // GET: Admin/ThongKe
        public ActionResult Index()
        {
            var result = db.Goods.Where(g => g.GoodsCount <= 5).ToList();
            return View(result);
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
            var result = db.Goods.Where(g => g.GoodsCount <= 5).ToList();
            var htmlPDF = base.RenderPartialToString("~/Areas/Admin/Views/ThongKe/BaoCao.cshtml", result);
            PdfDocument doc = converter.ConvertHtmlString(htmlPDF);
            string fileName = string.Format("{0}.pdf", "BC" + DateTime.Now.Month + DateTime.Now.Day);
            string pathFile = Path.Combine(Server.MapPath("~/Public/PDF"), fileName);
            doc.Save(pathFile);
            doc.Close();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}
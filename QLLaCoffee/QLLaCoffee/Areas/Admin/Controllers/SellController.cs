using Antlr.Runtime.Misc;
using QLLaCoffee.App_Start;
using QLLaCoffee.Models;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace QLLaCoffee.Areas.Admin.Controllers
{
    [RoleUser(FunctionID = "BH_QuanLy")]
    public class SellController : BaseController
    {
        private LaCoffeeDBContext db = new LaCoffeeDBContext();

        [RoleUser]
        public ActionResult Index()
        {
            return View(db.Tables.ToList());
        }

        [RoleUser]
        public ActionResult Order(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tables table = db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            ViewBag.TableName = table.TableName;
            ViewBag.ListDrinks = db.Drinks.ToList();
            ViewBag.ListDrinkCategories = db.Drinks.Select(p => p.DrinkCategories).Distinct().ToList();
            TempData["TableID"] = table.TableID;
            Bills bills = db.Bills.Where(b => b.TableID == id).FirstOrDefault();
            if (bills != null)
            {
                ViewBag.check = true;
                return View(bills.BillInfos.ToList());
            }
            else
            {
                ViewBag.check = false;
                table.Status = false;
                db.Entry(table).State = EntityState.Modified;
                db.SaveChanges();
                return View(db.BillInfos.ToList());
            }
        }

        [RoleUser]
        public ActionResult SelectDrinkByCategory(int id)
        {
            var drinks = from d in db.Drinks where d.DrinkCategoryID == id select d;
            return View("Order", drinks);
        }

        [RoleUser]
        public ActionResult AddDrinkToBill(int id)
        {
            int? tableID = TempData["TableID"] as int?;
            Drinks drink = db.Drinks.Find(id);
            BillInfos billInfo = new BillInfos();
            Tables table = db.Tables.Find(tableID);
            billInfo.DrinkID = id;
            billInfo.DrinkCount = 1;
            billInfo.DrinkPrice = drink.DrinkPrice * billInfo.DrinkCount;
            if (table.Status == false && !table.Bills.Any())
            {
                Bills bills = new Bills();
                bills.BillID = "HD" + DateTime.Now.Month + DateTime.Now.Day + Guid.NewGuid().ToString().Substring(0, 8);
                Session["Bills"] = bills.BillID;
                bills.CreateDate = DateTime.Now;
                bills.TotalAmount = billInfo.DrinkPrice;
                bills.TableID = (int)tableID;
                bills.UserID = SessionConfig.GetUser().UserID;
                billInfo.BillID = bills.BillID;
                db.Bills.Add(bills);
                db.BillInfos.Add(billInfo);
                table.Status = true;
                table.Bills.Add(bills);
                db.Entry(table).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                Bills bills = db.Bills.Where(b => b.TableID == tableID).FirstOrDefault();
                BillInfos existingBillInfo = db.BillInfos.FirstOrDefault(bi => bi.BillID == bills.BillID && bi.DrinkID == id);
                if (existingBillInfo != null)
                {
                    existingBillInfo.DrinkCount += 1;
                    existingBillInfo.DrinkPrice = existingBillInfo.DrinkCount * drink.DrinkPrice;
                    db.Entry(existingBillInfo).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    billInfo.BillID = bills.BillID;
                    db.BillInfos.Add(billInfo);
                    db.SaveChanges();
                }

                bills.TotalAmount = db.BillInfos
                    .Where(bi => bi.BillID == bills.BillID)
                    .Sum(bi => bi.DrinkPrice);
                db.Entry(bills).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Order", new { id = tableID });
        }

        [RoleUser]
        public ActionResult ExportPDF()
        {
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A6;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;
            converter.Options.MarginTop = 20;
            converter.Options.MarginBottom = 20;
            int? tableID = TempData["TableID"] as int?;
            Bills bills = db.Bills.Where(b => b.TableID == tableID).FirstOrDefault();
            bills.CreateDate = DateTime.Now;
            db.Entry(bills).State = EntityState.Modified;
            db.SaveChanges();
            var list = bills.BillInfos.ToList();
            var htmlPDF = base.RenderPartialToString("~/Areas/Admin/Views/Sell/HoaDonTamTinh.cshtml", list);
            PdfDocument doc = converter.ConvertHtmlString(htmlPDF);
            string fileName = string.Format("{0}.pdf", bills.BillID + "temp");
            string pathFile = Path.Combine(Server.MapPath("~/Public/PDF"), fileName);
            doc.Save(pathFile);
            doc.Close();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [RoleUser]
        public ActionResult ThanhToan()
        {
            int? tableID = TempData["TableID"] as int?;
            Tables table = db.Tables.Find(tableID);
            table.Status = false;
            Bills bills = db.Bills.Where(b => b.TableID == tableID).FirstOrDefault();
            bills.TableID = null;
            bills.CreateDate = DateTime.Now;
            db.Entry(bills).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [RoleUser]
        public ActionResult Delete(int idDrink, String idBill)
        {
            BillInfos billInfos = db.BillInfos.Find(idBill, idDrink);
            Bills bills = db.Bills.Find(idBill);
            db.BillInfos.Remove(billInfos);
            db.SaveChanges();
            if (!db.BillInfos.Any(b => b.BillID.Equals(idBill)))
            {

                db.Bills.Remove(bills);
                db.SaveChanges();
            }
            else
            {
                bills.TotalAmount = db.BillInfos
                   .Where(bi => bi.BillID == bills.BillID)
                   .Sum(bi => bi.DrinkPrice);
                db.Entry(bills).State = EntityState.Modified;
                db.SaveChanges();
            }
           ;
            return Json(new { success = true });
        }

        [RoleUser]
        public ActionResult GiamSoLuong(int idDrink, String idBill)
        {
            int? tableID = TempData["TableID"] as int?;
            BillInfos billInfos = db.BillInfos.Find(idBill, idDrink);
            Drinks drinks = db.Drinks.Find(idDrink);
            Bills bills = db.Bills.Find(idBill);
            billInfos.DrinkCount--;
            billInfos.DrinkPrice = drinks.DrinkPrice * billInfos.DrinkCount;
            db.Entry(billInfos).State = EntityState.Modified;
            db.SaveChanges();
            bills.TotalAmount = db.BillInfos
                 .Where(bi => bi.BillID == bills.BillID)
                 .Sum(bi => bi.DrinkPrice);
            db.Entry(bills).State = EntityState.Modified;
            db.SaveChanges();
            if (billInfos.DrinkCount == 0)
            {
                db.BillInfos.Remove(billInfos);
                db.SaveChanges();
                if (!db.BillInfos.Any(b => b.BillID.Equals(idBill)))
                {
                    db.Bills.Remove(bills);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Order", new { id = tableID });
        }

        [RoleUser]
        public ActionResult TangSoLuong(int idDrink, String idBill)
        {
            int? tableID = TempData["TableID"] as int?;
            BillInfos billInfos = db.BillInfos.Find(idBill, idDrink);
            Drinks drinks = db.Drinks.Find(idDrink);
            Bills bills = db.Bills.Find(idBill);
            billInfos.DrinkCount++;
            billInfos.DrinkPrice = drinks.DrinkPrice * billInfos.DrinkCount;
            db.Entry(billInfos).State = EntityState.Modified;
            db.SaveChanges();
            bills.TotalAmount = db.BillInfos
                   .Where(bi => bi.BillID == bills.BillID)
                   .Sum(bi => bi.DrinkPrice);
            db.Entry(bills).State = EntityState.Modified;
            db.SaveChanges();
            if (billInfos.DrinkCount == 0)
            {
                db.BillInfos.Remove(billInfos);
                db.SaveChanges();
                if (!db.BillInfos.Any(b => b.BillID.Equals(idBill)))
                {
                    db.Bills.Remove(bills);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Order", new { id = tableID });
        }

        [HttpPost]
        [RoleUser(FunctionID = "B_ThayDoi")]
        public ActionResult ChangeTableCount(int? tableCount)
        {
            if (tableCount != null)
            {
                db.Tables.RemoveRange(db.Tables.ToList());
                for (int i = 1; i <= tableCount; i++)
                {
                    db.Tables.Add(new Tables("Bàn " + i, false));
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [RoleUser(FunctionID = "KC")]
        public ActionResult StartShift(decimal firstAmount)
        {
            ShiftReports shiftReports = new ShiftReports();
            shiftReports.ShiftReportID = "KC" + DateTime.Now.Month + DateTime.Now.Day + Guid.NewGuid().ToString().Substring(0, 8);
            shiftReports.FirstAmount = firstAmount;
            shiftReports.LastAmount = 0;
            shiftReports.BillCount = 0;
            shiftReports.Revenue = 0;
            shiftReports.UserID = SessionConfig.GetUser().UserID;
            if (db.Bills.Where(b => b.Tables.Status == true).Count() > 0)
            {
                shiftReports.UncollectedAmount = db.Bills.Where(b => b.Tables.Status == true).Sum(b => b.TotalAmount);
            }
            else
            {
                shiftReports.UncollectedAmount = 0;
            }
            shiftReports.FirstTime = DateTime.Now;
            shiftReports.LastTime = DateTime.Now;
            db.ShiftReports.Add(shiftReports);
            db.SaveChanges();
            SessionConfig.SetShiftReport(shiftReports);
            return RedirectToAction("Index");
        }
    }
}
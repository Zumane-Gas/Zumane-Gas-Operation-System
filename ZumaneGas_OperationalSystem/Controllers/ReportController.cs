using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZumaneGas_OperationalSystem.Models;

namespace ZumaneGas_OperationalSystem.Controllers
{
    public class ReportController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DailyReport(Daily_Report report)
        {
            Daily_Report daily = new Daily_Report();


            //Steps. Return all sales from current Date, take those sales and add them up to come with a final total
            List<Sale> sales = db.Sales.Where(x => x.SaleD.Day.Equals(DateTime.Now.Day) && x.SaleD.Month.Equals(DateTime.Now.Month)
                                && x.SaleD.Year.Equals(DateTime.Now.Year)).ToList();
            //current sum/total
            var Eft = sales.Where(x => x.SaleType == SaleType.EFT).Sum(x => x.finalPrice);
            ViewBag.eft = Eft;

            var cash = sales.Where(x => x.SaleType == SaleType.Cash).Sum(x => x.finalPrice);
            ViewBag.Cash = cash;

            var sum = sales.Sum(x => x.finalPrice);
            var tot = sum;
           

            int deposits = sales.Where(x => x.DepositItem != null).Sum(x => x.DepositQuantity);
            ViewBag.deposits = deposits;
        
            //
            int r1 = sales.Where(x => x.item1 == "2" || x.item1 == "3" || x.item1 == "4" || x.item1 == "5" || x.item1 == "6" || x.item1 == "7" || x.ServiceType == ServiceType.Refill).Sum(x => x.Qty1);
            int r2 = sales.Where(x => x.item2 == "2" || x.item2 == "3" || x.item2 == "4" || x.item2 == "5" || x.item2 == "6" || x.item2 == "7" || x.ServiceType == ServiceType.Refill).Sum(x => x.Qty2);
            int totR = r1 + r2;
            ViewBag.Refills = totR;


            //Exchanges Thorough Details
            int Nine1 = sales.Where(x => x.item1 == "9" && x.ServiceType != ServiceType.Refill).Sum(x => x.Qty1);
            int Nine2 = sales.Where(x => x.item2 == "9" && x.ServiceType != ServiceType.Refill).Sum(x => x.Qty2);
            int NineTot = Nine1 + Nine2;
            ViewBag.Nine = NineTot;
          
            int Fourteen1 = sales.Where(x => x.item1 == "14").Sum(x => x.Qty1);
            int Fourteen2 = sales.Where(x => x.item2 == "14").Sum(x => x.Qty2);
            int Fourteen = Fourteen1 + Fourteen2;
            ViewBag.Fourteen = Fourteen;

            int Nineteen1 = sales.Where(x => x.item1 == "19").Sum(x => x.Qty1);
            int Nineteen2 = sales.Where(x => x.item2 == "19").Sum(x => x.Qty2);
            int Nineteen = Nineteen1 + Nineteen2;
            ViewBag.Nineteen = Nineteen;


            //products 
            int CookerTop = sales.Where(x => x.Product_Item1 == "Cooker Top").Sum(x => x.Product_Qty1);
            ViewBag.CookerTop = CookerTop;

            int Regulator = sales.Where(x => x.Product_Item1 == "Regulator").Sum(x => x.Product_Qty1);
            ViewBag.Regulator = Regulator;

            int Key = sales.Where(x => x.Product_Item1 == "Cylinder Key").Sum(x => x.Product_Qty1);
            ViewBag.Key = Key;

            //48 kg
            var used = sales.Where(x => x.New == true).Count();
            int Fourty1 = sales.Where(x => x.item1 == "48").Sum(x => x.Qty1);
            int Fourty2 = sales.Where(x => x.item2 == "48").Sum(x => x.Qty2);
            int Fourty = Fourty1 + Fourty2 + used;
            ViewBag.Fourty = Fourty;

            ViewBag.CurrTotSales = (NineTot + Fourteen + Nineteen + Fourty + deposits) + (CookerTop + Regulator + Key) + totR;
            ViewBag.Exchanges = NineTot + Fourteen + Nineteen + Fourty;

            //stock
            //Closing Stock 
            int Curr_9kg = db.Stockes.Where(x => x.ProductName == "Gas 9kg").Sum(x => x.Quantity);
            ViewBag.Curr9kg = Curr_9kg;

            int Curr_14kg = db.Stockes.Where(x => x.ProductName == "Gas 14kg").Sum(x => x.Quantity);
            ViewBag.Curr14kg = Curr_14kg;

            int Curr_19kg = db.Stockes.Where(x => x.ProductName == "Gas 19kg").Sum(x => x.Quantity);
            ViewBag.Curr19kg = Curr_19kg;

            int Curr_48kg = db.Stockes.Where(x => x.ProductName == "Gas 48kg").Sum(x => x.Quantity);
            ViewBag.Curr48kg = Curr_48kg;

            int Curr_CT = db.Stockes.Where(x => x.ProductName == "Cooker Top").Sum(x => x.Quantity);
            ViewBag.CurrCT = Curr_CT;

            int Curr_Reg = db.Stockes.Where(x => x.ProductName == "Regulator").Sum(x => x.Quantity);
            ViewBag.Curr_Reg = Curr_Reg;

            int Curr_Key = db.Stockes.Where(x => x.ProductName == "Cylinder Key").Sum(x => x.Quantity);
            ViewBag.Curr_Key = Curr_Key;

            //Opening Stock 
            int Opening9kg = Curr_9kg + NineTot;
            ViewBag.Opening9kg = Opening9kg;

            int Opening14kg = Curr_14kg + Fourteen;
            ViewBag.Opening14kg = Opening14kg;

            int Opening19kg = Curr_19kg + Nineteen;
            ViewBag.Opening19kg = Opening19kg;

            int Opening48kg = Curr_48kg + Fourty;
            ViewBag.Opening48kg = Opening48kg;

            int OpeningCT = Curr_CT + CookerTop;
            ViewBag.OpeningCT = OpeningCT;

            int OpeningReg = Curr_Reg + Regulator;
            ViewBag.OpeningReg = OpeningReg;

            int OpeningKey = Curr_Key + Key;
            ViewBag.OpeningKey = OpeningKey;

            //Payouts
            List<DailyPayout> payouts = db.payouts.ToList();
            int Total_Payouts = payouts.Where(x => x.Payout_Date.Day.Equals(DateTime.Now.Day) && x.Payout_Date.Month.Equals(DateTime.Now.Month)
                                && x.Payout_Date.Year.Equals(DateTime.Now.Year)).Sum(x => x.Payout_Amount);
            ViewBag.Payouts = Total_Payouts;
            ViewBag.c = tot - Total_Payouts - Eft;


            List<DailyPayout> D_Payouts = db.payouts.Where(x => x.Payout_Date.Day.Equals(DateTime.Now.Day) && x.Payout_Date.Month.Equals(DateTime.Now.Month)
                                && x.Payout_Date.Year.Equals(DateTime.Now.Year)).ToList();

            //List<string> des = new List<string>();

            //foreach(var I in D_Payouts)
            //{
            //    des.Add(I.Payout_Descriptiom);
            //    daily.PayoutsDesc = des;
            //}
            //des = D_Payouts.Select(x => x.Payout_Descriptiom).ToList();
            //ViewBag.d = des;

            return View();
        }
        [HttpPost]
        public ActionResult New_Report(Daily_Report report)
        {
            Daily_Report d = new Daily_Report();

            d.CashSales = report.CashSales;
            d.Closing_Amount = report.Closing_Amount;
            d.Cooker_Tops = report.Cooker_Tops;
            d.CylinderKeys = report.CylinderKeys;
            d.Deposits = report.Deposits;
            d.EFTSales = report.EFTSales;
            d.Regulators = report.Regulators;
            d.Exchanges_14 = report.Exchanges_14;
            d.Exchanges_19 = report.Exchanges_19;
            d.Exchanges_48 = report.Exchanges_48;
            d.Exchanges_9 = report.Exchanges_9;
            d.Opening_14kg = report.Opening_14kg;
            d.Opening_19kg = report.Opening_19kg;
            d.Opening_48kg = report.Opening_48kg;
            d.Opening_9kg = report.Opening_9kg;
            d.Opening_CT = report.Opening_CT;
            d.Opening_Key = report.Opening_Key;
            d.Opening_Reg = report.Opening_Reg;
            d.Current_14kg = report.Current_14kg;
            d.Current_19kg = report.Current_19kg;
            d.Current_48kg = report.Current_48kg;
            d.Current_9kg = report.Current_9kg;
            d.Current_CT = report.Current_CT;
            d.Current_Key = report.Current_Key;
            d.Current_Reg = report.Current_Reg;
            d.Payout_Amount = report.Payout_Amount;
            d.Refills = report.Refills;
            d.Total_Revenue = report.Total_Revenue;
            d.Shift = report.Shift;
            d.Search_Shift = d.Shift.ToString();
            d.Report_Date = DateTime.Now;
            d.Search_Date = " " + d.Report_Date.ToShortDateString();
            //d.RecordedPayouts = db.payouts.ToList();
            //d.ClosingStock = db.Stockes.ToList();
           
            //Adding
            db.reports.Add(d);
            db.SaveChanges();
            

            return View("DailyReport");

        }

        public JsonResult getLatestReport()
        {
            Daily_Report r = db.reports.ToList().Last();
            if (r != null)
            {
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult ViewReport(int? ReportID)
        {
            Daily_Report report = db.reports.Find(ReportID);
            if (ReportID != null)
            {
                return View(report);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }

        public ActionResult Report_Filter()
        {
            return View();
        }


        [HttpGet]
        public ActionResult RetrieveReport(string ReportDate)
        {
            Daily_Report d = new Daily_Report();
            Daily_Report report = db.reports.Where(x => x.Search_Date == ReportDate /*&& x.Search_Shift == Shift*/).FirstOrDefault();
           
            if (report != null)
            {
                Daily_Report daily = db.reports.FirstOrDefault(report.Shift.Equals(d.Shift));
                return View(daily);

            }
            else
            {
                return Json("Error Retrieving Report", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
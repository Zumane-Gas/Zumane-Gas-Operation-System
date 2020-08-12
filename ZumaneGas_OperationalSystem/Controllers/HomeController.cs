using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZumaneGas_OperationalSystem.Models;

namespace ZumaneGas_OperationalSystem.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            //Steps. Return all sales from current Date, take those sales and add them up to come with a final total
            List<Sale> sales = db.Sales.Where(x => x.SaleD.Day.Equals(DateTime.Now.Day) && x.SaleD.Month.Equals(DateTime.Now.Month) 
                                && x.SaleD.Year.Equals(DateTime.Now.Year)).ToList();
            //current sum/total
            var sum = sales.Sum(x => x.finalPrice);
            ViewBag.c = sum;

            var Eft = sales.Where(x => x.SaleType == SaleType.EFT).Sum(x => x.finalPrice);


            int CurrTotSales = sales.Count();
            ViewBag.CurrTotSales = CurrTotSales;

            return View(db.Sales.ToList());
        }
        //Reports
        public void Reports()
        {
            //Average Daily Sale

            //Average Monthly Sales


        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
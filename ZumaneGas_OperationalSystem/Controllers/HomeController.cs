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

          
            var order = db.orders.Where(x => x.Order_Date.Value.Day.Equals(DateTime.Now.Day) && x.Order_Date.Value.Month.Equals(DateTime.Now.Month) && x.Order_Date.Value.Year.Equals(DateTime.Now.Year) && x.OrderStatus == OrderStatus.Processing && x.DeliveryStatus == DeliveryStatus.NotDelivered && x.isViewed == false).Count();
            int OrderCount = order;
            if (OrderCount > 0)
            {
                ViewBag.Orders = OrderCount;
            }

            //getOverdue similar logic
            var o = db.orders.Where(x => x.Order_Date != null && x.OrderStatus == OrderStatus.Processing && x.DeliveryStatus == DeliveryStatus.NotDelivered).ToList();
            int result;
            int overdue_Count = 0;
            List<Order> overdue = new List<Order>();

            foreach (var item in o)
            {
                result = DateTime.Compare(item.Order_Date.Value, DateTime.Today);
                if (result < 0)
                {
                    overdue.Add(item);
                    overdue_Count = overdue.Count();
                }
            }

            if (overdue_Count > 0)
            {
                ViewBag.Overdue = overdue_Count;
            }

            //Notification badge
            var totalCount = OrderCount + overdue_Count;
            if (totalCount > 0)
            {
                ViewBag.total = totalCount;
            }

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
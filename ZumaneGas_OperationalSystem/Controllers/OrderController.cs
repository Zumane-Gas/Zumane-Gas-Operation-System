using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZumaneGas_OperationalSystem.Models;

namespace ZumaneGas_OperationalSystem.Controllers
{
    public class OrderController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Capture_CustomerDetails()
        {
            return View();
        }
        [HttpPost]
        public void Capture_CustomerDetails(Order Order)
        {
            Order O = new Order();
            O.Address_Line1 = Order.Address_Line1;
            O.ContactNumber = Order.ContactNumber;
            O.CustomerFullName = Order.CustomerFullName;
            O.StreetName_Number = Order.StreetName_Number;
            O.AltrenateContactNum = Order.AltrenateContactNum;

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //O.Order_Date = DateTime.Now;
                db.orders.Add(O);
                db.SaveChanges();
            }
        }
        //Get recently added order row
        public JsonResult getLatestCapture()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            Order o = db.orders.ToList().Last();
            if (o != null)
            {
                return Json(o, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult NextPage_Order(int? id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Order order = db.orders.Find(id);
            if (order != null)
            {
                return View(order);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        public ActionResult Capture_OrderDetails()
        {
            return View();
        }

        //update sale information to last added order row
        [HttpPost]
        public ActionResult Capture_OrderDetails(Order Order)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            try
            {
                //Order o = new Order(Order.Order_ID, Order.Order_Qty1, Order.Order_price1, Order.Order_Date);

                var record = db.orders.Where(x => x.Order_ID == Order.Order_ID).FirstOrDefault();

                if (record != null)
                {
                    record.Order_Qty1 = Order.Order_Qty1;
                    record.Order_price1 = Order.Order_price1;
                    record.Order_item1 = Order.Order_item1;

                    record.Order_Qty2 = Order.Order_Qty2;
                    record.Order_price2 = Order.Order_price2;
                    record.Order_item2 = Order.Order_item2;

                    record.Order_Date = Order.Order_Date;
     
                    double Vat = 0.15;
                    record.Vat = Vat;
                    record.UnitPrice = (Order.Order_price1 + Order.Order_price2) * Vat;
                    record.finalPrice = Order.Order_price1 + Order.Order_price2;

                    db.Entry(record).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            return View("NextPage_Order");
        }
        public JsonResult getOrder(int OrderId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Order o = db.orders.Where(x => x.Order_ID == OrderId).FirstOrDefault();

            if (o != null)
            {
                return Json(o, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult OrderCalculator()
        {
            return View();
        }
        //Methods to do
        //NewOrder
        //Get Orders
    }
}
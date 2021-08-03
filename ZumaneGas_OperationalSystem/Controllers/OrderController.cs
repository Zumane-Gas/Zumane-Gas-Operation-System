using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZumaneGas_OperationalSystem.Models;

namespace ZumaneGas_OperationalSystem.Controllers
{
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Capture_CustomerDetails()
        {
            ApplicationDbContext db = new ApplicationDbContext();
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
            O.Order_Date = null;

            using (db)
            {
                db.orders.Add(O);
                db.SaveChanges();
            }
        }
        //Get recently added order row
        public JsonResult getLatestCapture()
        {
            //Order o = db.orders.Last();
            //order by descending (step 1)
            //return the first record (step 2)
            var v = db.orders.OrderByDescending(x => x.Order_ID).First();
            Order o = db.orders.Where(x => x.Order_ID == v.Order_ID).FirstOrDefault();

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
            Order orders = db.orders.Find(id);
            if (orders != null)
            {
                return View(orders);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        public ActionResult Capture_PaymentDetails(int? id)
        {
            Order order = db.orders.Find(id);
            if(order != null)
            {
                return View(order);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }
        [HttpPost]
        public void FinalisePayment(Order Order)
        {
            var record = db.orders.Where(x => x.Order_ID == Order.Order_ID).FirstOrDefault();

            if(record != null)
            {
                record.PaymentStatus = Order.PaymentStatus;
                record.PaymentType = Order.PaymentType;
                //record.ProcessingDate = Order.ProcessingDate;
                record.Order_change = Order.Order_change;
                record.Order_cashReceived = Order.Order_cashReceived;
                record.isToday = Order.isToday;
                record.OrderStatus = Order.OrderStatus;
                record.DeliveryStatus = DeliveryStatus.NotDelivered;

                //Sale details - add new sale details - Sale will be recorded and transaction will go through, the items ordered will not be deducted until order is processed
                if(record.isToday == true)
                {
                    Sale sale = new Sale();
                    record.ProcessingDate = DateTime.Now;
                    sale.cashReceived = record.Order_cashReceived;
                    sale.change = record.Order_change;
                    sale.finalPrice = record.finalPrice;
                    sale.item1 = record.Order_item1;
                    sale.item2 = record.Order_item2;
                    sale.price1 = record.Order_price1;
                    sale.price2 = record.Order_price2;
                    sale.Qty1 = record.Order_Qty1;
                    sale.Qty2 = record.Order_Qty2;
                    sale.SaleD = DateTime.Now;
                    sale.Alias_SaleD = sale.SaleD;
                    if(record.PaymentType == PaymentType.Cash)
                    {
                        sale.SaleType = SaleType.Cash;
                    }
                    else if(record.PaymentType == PaymentType.EFT)
                    {
                        sale.SaleType = SaleType.EFT;
                    }
                    sale.SaleStatus = SaleStatus.Order;
                    //Sale is linked with Order_Number -> This can be used as reference to order 
                    sale.Order_Number = record.Order_Number;
                    sale.Order_ID = record.Order_ID;
                    sale.InvoiceId = db.Sales.Count().ToString();
                    db.Sales.Add(sale); //<- New Sale_ID will be added -> the Sale_ID needs to be linked with Order -> Vice Versa
                }
                else if(record.isToday == false)
                {
                    record.ProcessingDate = Order.ProcessingDate;
                }
                //feed.OrderDate = record.SaleDate;
                int count = db.orders.Count();
                record.Order_Counter = count;
                db.Entry(record).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public void Complete_Order_Paid(Order order) {

            var record = db.orders.Where(x => x.Order_ID == order.Order_ID).First();
            var sale = db.Sales.Where(x => x.Order_ID == order.Order_ID).First();

            var getEmpty1 = db.Empties.Where(x => x.Empty_Size == record.Order_item1).FirstOrDefault();
            var getEmpty2 = db.Empties.Where(x => x.Empty_Size == record.Order_item2).FirstOrDefault();

            if (getEmpty1 != null)
            {
                getEmpty1.Empty_Size_Quantity = getEmpty1.Empty_Size_Quantity + record.Order_Qty1;
                db.Entry(getEmpty1).State = EntityState.Modified;
            }

            if (getEmpty2 != null)
            {
                getEmpty2.Empty_Size_Quantity = getEmpty2.Empty_Size_Quantity + record.Order_Qty2;
                db.Entry(getEmpty2).State = EntityState.Modified;
            }


            //Minus stock
            var GasSize1 = db.Stockes.Where(x => x.C_Size == record.Order_item1).FirstOrDefault();
            var GasSize2 = db.Stockes.Where(x => x.C_Size == record.Order_item2).FirstOrDefault();
            //size1
            if (GasSize1 != null)
            {
                GasSize1.Quantity = GasSize1.Quantity - record.Order_Qty1;
                db.Entry(GasSize1).State = EntityState.Modified;
            }

            //size2 
            if (GasSize2 != null)
            {
                GasSize2.Quantity = GasSize2.Quantity - record.Order_Qty2;
                db.Entry(GasSize2).State = EntityState.Modified;
            }
            //sale.SaleD = DateTime.Now;
            //sale.Alias_SaleD = sale.SaleD;
            record.DeliveryStatus = DeliveryStatus.Delivered;
            record.isViewed = true;
            record.OrderStatus = OrderStatus.Processed;
            sale.Delivery = true;
            //sale.SaleStatus = SaleStatus.Order;
            
            record.ProcessingDate = DateTime.Now;
            record.Order_InvoiceNumber = "ZUM" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + record.Order_ID;
            db.Entry(sale).State = EntityState.Modified;
            db.Entry(record).State = EntityState.Modified;
            db.SaveChanges();
        }

        //Complete_Order -> Next Page
        public ActionResult Outstanding_Order(int? id)
        {
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
        public void Complete_Order_Outstanding(Order order)
        {
            var record = db.orders.Where(x => x.Order_ID == order.Order_ID).First();

            Sale sale = new Sale();
            sale.finalPrice = record.finalPrice;
            sale.item1 = record.Order_item1;
            sale.item2 = record.Order_item2;
            sale.price1 = record.Order_price1;
            sale.price2 = record.Order_price2;
            sale.Qty1 = record.Order_Qty1;
            sale.Qty2 = record.Order_Qty2;
            sale.SaleD = DateTime.Now;
            sale.Alias_SaleD = sale.SaleD;

            if (record.PaymentType == PaymentType.Cash)
            {
                sale.SaleType = SaleType.Cash;
                sale.cashReceived = record.Order_cashReceived;
                sale.change = record.Order_change;
            }
            else if (record.PaymentType == PaymentType.EFT)
            {
                sale.SaleType = SaleType.EFT;
                sale.cashReceived = 0;
                sale.change = 0;
            }

            var getEmpty1 = db.Empties.Where(x => x.Empty_Size == record.Order_item1).First();


            if (getEmpty1 != null)
            {
                getEmpty1.Empty_Size_Quantity = getEmpty1.Empty_Size_Quantity + record.Order_Qty1;
                db.Entry(getEmpty1).State = EntityState.Modified;
            }

            if (order.Order_item2 != null)
            {
                var getEmpty2 = db.Empties.Where(x => x.Empty_Size == record.Order_item2).First();
                if (getEmpty2 != null)
                {
                    getEmpty2.Empty_Size_Quantity = getEmpty2.Empty_Size_Quantity + record.Order_Qty2;
                    db.Entry(getEmpty2).State = EntityState.Modified;
                }


            }
            //Minus stock
            var GasSize1 = db.Stockes.Where(x => x.C_Size == record.Order_item1).FirstOrDefault();
            var GasSize2 = db.Stockes.Where(x => x.C_Size == record.Order_item2).FirstOrDefault();
            //size1
            if (GasSize1 != null)
            {
                GasSize1.Quantity = GasSize1.Quantity - record.Order_Qty1;
                db.Entry(GasSize1).State = EntityState.Modified;
            }

            //size2 
            if (GasSize2 != null)
            {
                GasSize2.Quantity = GasSize2.Quantity - record.Order_Qty2;
                db.Entry(GasSize2).State = EntityState.Modified;
            }

            record.DeliveryStatus = DeliveryStatus.Delivered;
            record.isViewed = true;
            record.OrderStatus = OrderStatus.Processed;
            sale.SaleStatus = SaleStatus.Order;
            sale.Order_Number = record.Order_Number;
            sale.Order_ID = record.Order_ID;
            sale.Delivery = true;
            record.ProcessingDate = DateTime.Now;
            var month = DateTime.Now.Month.ToString();
            //if(month < 10)
            //{
            //    month = "0" + month.ToString();
            //}
            //else
            //{
            //    month = month;
            //}

            record.Order_InvoiceNumber = "ZUM" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + record.Order_ID;
            sale.InvoiceId = db.Sales.Count().ToString();
            db.Sales.Add(sale);
            db.Entry(record).State = EntityState.Modified;
            db.SaveChanges();
        }
        public ActionResult A4_Order(int? id)
        {
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
        public ActionResult OrderReview(int? id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
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

            Order orders = db.orders.Find(id);
            if (orders != null)
            {
                return View(orders);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }

        //update sale information to last added order row
        [HttpPost]
        public void Capture_OrderDetails(Order Order)
        {

            var record = db.orders.Where(x => x.Order_ID == Order.Order_ID).FirstOrDefault();
       
            if (record != null)
            {
                record.Order_Qty1 = Order.Order_Qty1;
                record.Order_price1 = Order.Order_price1;
                record.Order_item1 = Order.Order_item1;

                record.Order_Qty2 = Order.Order_Qty2;
                record.Order_price2 = Order.Order_price2;
                record.Order_item2 = Order.Order_item2;

                record.SaleDate = Order.SaleDate;
                string alias_saledate = Order.SaleDate;
                //Use for conversion
                DateTime O_Date;
                //= DateTime.Parse(alias_saledate);
                //DateTime.TryParse(alias_saledate, out O_Date);
                //DateTime.TryParseExact(alias_saledate, "yyyy-dd-MM hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out O_Date);
                var datetime = DateTime.ParseExact(alias_saledate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                O_Date = datetime;
                record.Order_Date = O_Date;
                record.Alias_Order_Date = record.Order_Date.Value;

                double Vat = 0.15;
                record.Vat = Vat;
                //record.UnitPrice = (Order.Order_price1 + Order.Order_price2) * Vat;
                record.finalPrice = (Order.Order_price1 * Order.Order_Qty1) + (Order.Order_price2 * Order.Order_Qty2);

                record.Order_Number = "#Zum" + record.Order_ID;
                db.Entry(record).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public JsonResult getOrder(int OrderId)
        {
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
        [HttpPost]   
        public void addNote(Order order)
        {
            var record = db.orders.Where(x => x.Order_ID == order.Order_ID).First();
            string[] comm;
            if(record != null)
            {
                comm = order.notes;
                record.notes = comm;
                db.Entry(record).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public ActionResult OrderCalculator()
        {
            return View();
        }
        public JsonResult getItemInformation(int? Size)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var Info = db.Goods.Where(x => x.Cylinder_Size == Size).FirstOrDefault();

            if(Info != null)
            {
                return Json(Info, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("No Information", JsonRequestBehavior.AllowGet);
            }
        }
        //Get Orders
        public JsonResult ProcessingOrders()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            //Orders that are due
            var order = db.orders.Where(x => x.Order_Date.Value.Day.Equals(DateTime.Now.Day) && x.Order_Date.Value.Month.Equals(DateTime.Now.Month) && x.Order_Date.Value.Year.Equals(DateTime.Now.Year) && x.OrderStatus == OrderStatus.Processing && x.DeliveryStatus == DeliveryStatus.NotDelivered).Count();
            int OrderCount = order;
            if (OrderCount > 0)
            {
                return Json(OrderCount, JsonRequestBehavior.AllowGet);
                //ViewBag.orders = OrderCount;
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        //Will use to return information in card (OrderInformation)
        public JsonResult getViewStatus()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var due_Orders = db.orders.Where(x => x.Order_Date.Value.Day.Equals(DateTime.Now.Day) && x.Order_Date.Value.Month.Equals(DateTime.Now.Month) && x.Order_Date.Value.Year.Equals(DateTime.Now.Year) && x.OrderStatus == OrderStatus.Processing && x.DeliveryStatus == DeliveryStatus.NotDelivered).ToList();
            if(due_Orders != null)
            {
                return Json(due_Orders, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("No Orders", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getOverdue()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var o = db.orders.ToList();
            int result;
            List<Order> overdue = new List<Order>();

            foreach (var item in o.Where(x => x.Order_Date != null && x.OrderStatus == OrderStatus.Processing && x.DeliveryStatus == DeliveryStatus.NotDelivered))
            {
                result = DateTime.Compare(item.Order_Date.Value, DateTime.Today);
                if(result < 0)
                {
                    overdue.Add(item);
                }
            }
            return Json(overdue,JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAllOrders()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var getAll = db.orders.Where(x => x.OrderStatus == OrderStatus.Processing && x.DeliveryStatus == DeliveryStatus.NotDelivered).ToList();
            if(getAll != null)
            {
                return Json(getAll, JsonRequestBehavior.AllowGet);
            }
            else
            {
               return Json("No Orders", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult getOrders_Due()
        {
            ApplicationDbContext db = new ApplicationDbContext();
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

            foreach(var item in o)
            {
                result = DateTime.Compare(item.Order_Date.Value, DateTime.Today);
                if(result < 0)
                {
                    overdue.Add(item);
                    overdue_Count = overdue.Count();
                }
            }

            if(overdue_Count > 0)
            {
                ViewBag.Overdue = overdue_Count;
            }

            //Notification badge
            var totalCount = OrderCount + overdue_Count;
            if(totalCount > 0)
            {
                ViewBag.total = totalCount;
            }


            return View();
        }

        public JsonResult getDelivered()
        {
            var orders = db.orders.Where(x => x.ProcessingDate.Value.Day == DateTime.Now.Day && x.ProcessingDate.Value.Month == DateTime.Now.Month 
            && x.ProcessingDate.Value.Year == DateTime.Now.Year && x.DeliveryStatus == DeliveryStatus.Delivered && x.PaymentStatus == PaymentStatus.Paid).ToList();

            if(orders != null)
            {
                return Json(orders, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("No Deliveries Made", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
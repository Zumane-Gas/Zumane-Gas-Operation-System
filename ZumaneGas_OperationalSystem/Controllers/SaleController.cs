using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using SelectPdf;
using ZumaneGas_OperationalSystem.Models;

namespace ZumaneGas_OperationalSystem.Controllers
{
    public class SaleController : Controller
    {
        // GET: Sale
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NewSale()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult NewSale(Sale sale)
        //{
        //    //Search For Gas Details and populate on View 
        //    string Inv = sale.InvId;
        //    int InventoryId = 0;
        //    var Inventory = db.Goods.ToList().Where(x => x.Gas_Size == Inv).ToList();

        //    if(Inventory.Count > 0)
        //    {
        //        foreach(var I in Inventory)
        //        {
        //            InventoryId = I.Invetory_Id;
        //        }
        //    }

        //    String a = InventoryId.ToString();
        //    sale.Invetory_Id = InventoryId;

        //    //Actual Sale Procedure

        //    //db.Sales.Add(sale);
        //    //db.SaveChanges();
        //    return View();

        //}
       
        [HttpPost]
        public void RecordSale(Sale s)
        {
           // var v = db.Goods.ToList().Where(x => x.Size_Code == code).FirstOrDefault();
           // v = v.Invetory_Id;

            Sale sale = new Sale();

            //items
            sale.item1 = s.item1;

            if(s.item2 == "Select Size")
            {
                sale.item2 = null;
            }
            else
            {
                sale.item2 = s.item2;
            }
            
            sale.item3 = s.item3;

            //prices or unit price
            sale.price1 = s.price1;
            sale.price2 = s.price2;
            sale.price3 = s.price3;

            //Quantity for each item
            sale.Qty1 = s.Qty1;
            sale.Qty2 = s.Qty2;
            sale.Qty3 = s.Qty3;

            //Gas Appliaces Items
            sale.Product_Item1 = s.Product_Item1;
            sale.Product_Item2 = s.Product_Item2;

            //prices for appliances
            sale.Product_Price1 = s.Product_Price1;
            sale.Product_Price2 = s.Product_Price2;

            //qty for appliances
            sale.Product_Qty1 = s.Product_Qty1;
            sale.Product_Qty2 = s.Product_Qty2;
            sale.SaleDate = s.SaleDate;

            //Deposit
            sale.DepositItem = s.DepositItem;
            sale.DepositPrice = s.DepositPrice;
            sale.DepositQuantity = s.DepositQuantity;

            //new 
            sale.New = s.New;
            sale.SaleType = s.SaleType;
            //calculations
           

            sale.Delivery = s.Delivery;
            sale.ServiceType = s.ServiceType;
            sale.Vat = 0.15;
            var VAT = sale.Vat;
            var Excl1 = sale.price1 - (sale.price1 * VAT);
            var Tot = ((sale.price1 * sale.Qty1) + (sale.price2 * sale.Qty2) + (sale.price3 * sale.Qty3) +
                (sale.Product_Price1 * sale.Product_Qty1) + (sale.Product_Price2 * sale.Product_Qty2) + (sale.DepositPrice * sale.DepositQuantity));       
                
            if (sale.ServiceType == ServiceType.Refill)
            {
                //need to make vakue dynamic
                Tot = Tot + 20;
            }
            else if(sale.ServiceType == ServiceType.Exchange)
            {
                Tot = Tot;
               
            }
            sale.finalPrice = Tot;
            //Total Items
            sale.TotalItems = s.TotalItems;
            var Items = sale.TotalItems;
            Items = sale.Qty1 + sale.Qty2 + sale.Qty3 + sale.Product_Qty1 + sale.Product_Qty2 + sale.DepositQuantity;

            //Cash Received and change
            sale.cashReceived = s.cashReceived;
            var rec = sale.cashReceived;

            Stock stock = new Stock();
            string P_Name = "Gas 48kg";
            //productName and cylinderSize
            //for productName use ProductItem1 and productItem2
            //for cylinderSize use item1 and item2

            var Product1 = db.Stockes.Where(x => x.ProductName == sale.Product_Item1).FirstOrDefault();
            var Product2 = db.Stockes.Where(x => x.ProductName == sale.Product_Item2).FirstOrDefault();
            var GasSize1 = db.Stockes.Where(x => x.C_Size == sale.item1).FirstOrDefault();
            var GasSize2 = db.Stockes.Where(x => x.C_Size == sale.item2).FirstOrDefault();
            var GasS = db.Stockes.Where(x => x.ProductName == P_Name).FirstOrDefault();

            var getEmpty1 = db.Empties.Where(x => x.Empty_Size == sale.item1).First();

            if (getEmpty1 != null)
            {
                getEmpty1.Empty_Size_Quantity = getEmpty1.Empty_Size_Quantity + sale.Qty1;
                db.Entry(getEmpty1).State = EntityState.Modified;
            }

            if (sale.item2 != null)
            {
                var getEmpty2 = db.Empties.Where(x => x.Empty_Size == sale.item2).First();
                if(getEmpty2 != null)
                {
                    getEmpty2.Empty_Size_Quantity = getEmpty2.Empty_Size_Quantity + sale.Qty2;
                    db.Entry(getEmpty2).State = EntityState.Modified;
                }
               
            }
            


            //product1
            if (Product1 != null)
            {
                Product1.Quantity = Product1.Quantity - sale.Product_Qty1;
                db.Entry(Product1).State = EntityState.Modified;

            }
            
            //new
            if (GasS != null)
            {
                if(sale.New == true)
                {
                    GasS.Quantity = GasS.Quantity - 1;
                    db.Entry(GasS).State = EntityState.Modified;
                }
            }
            //size1
            if (GasSize1 != null && sale.ServiceType != ServiceType.Refill)
            {
                GasSize1.Quantity = GasSize1.Quantity - sale.Qty1;
                db.Entry(GasSize1).State = EntityState.Modified;
            }

            //size2 
            if (GasSize2 != null && sale.ServiceType != ServiceType.Refill)
            {
                GasSize2.Quantity = GasSize2.Quantity - sale.Qty2;
                db.Entry(GasSize2).State = EntityState.Modified;
            }

            if(sale.SaleType == SaleType.EFT || sale.Delivery == true)
            {
                sale.finalPrice = Tot;
                rec = 0;
                sale.change = 0;
                sale.InvoiceId = db.Sales.Count().ToString();
                sale.SaleD = DateTime.Now;
                sale.Alias_SaleD = sale.SaleD;
                db.Sales.Add(sale);
                db.SaveChanges();
            }
            else if(sale.SaleType == SaleType.Cash){
                if (rec >= sale.finalPrice)
                {
                    sale.change = (rec - Tot);
                    sale.InvoiceId = db.Sales.Count().ToString();
                    sale.SaleD = DateTime.Now;
                    sale.Alias_SaleD = sale.SaleD;
                    db.Sales.Add(sale);
                    db.SaveChanges();

                }
            }
        }
         public ActionResult allSales()
        {
            return View(db.Sales.ToList());
        }
        public ActionResult GenerateInvoice(int? id)
        {
            Sale sale = db.Sales.Find(id);
            //Sale sale = db.Sales.Where(x => x.InvoiceId == id).FirstOrDefault();
            if(sale != null)
            {
                return View(sale); 
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }  
        }
        public ActionResult GenerateInvoice_Reprint(int? id)
        {
            Sale sale = db.Sales.Find(id);
            //Sale sale = db.Sales.Where(x => x.InvoiceId == id).FirstOrDefault();
            if (sale != null)
            {
                return View(sale);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult printMyPage(int id)
        {
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A3;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

            PdfDocument doc = converter.ConvertUrl("http://localhost:1267/Sale/GenerateInvoice/" + id);
            doc.Save("C://images//printedDoc" + id + ".pdf");
            doc.Close();
            return View();
        }

        public JsonResult getLatestSale()
        {
            Sale s = db.Sales.ToList().Last();
            if(s != null)
            {
                return Json(s, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult getSale(int saleId)
        {
            Sale s = db.Sales.Where(x => x.Sale_Id == saleId).FirstOrDefault();

            if(s != null)
            {
                return Json(s, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult S_Sale()
        {
            
            return View();
        }

        [HttpGet]
        public JsonResult getSaleDetails(string InvoiceId)
        {
            Sale s = db.Sales.FirstOrDefault(x => x.InvoiceId == InvoiceId);

            if (s != null)
            {
                return Json(s, JsonRequestBehavior.AllowGet);
            }
            else
            {
                
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult searchSale(string InvoiceId)
        {
            Sale sale = db.Sales.Where(x => x.InvoiceId == InvoiceId).FirstOrDefault();
            var SaleId = sale.Sale_Id;
            ViewBag.InvoiceId = SaleId;
            if (InvoiceId != null && sale != null)
            {
                return View(sale);
            }
            else
            {
                ViewBag.Error = "No Sale found";
                return View("NoRecord");
            }
        }
       
        public ActionResult NoRecord()
        {
            return View();
        }
        public JsonResult getGAS(double? Size)
        {
            Invetory v = db.Goods.ToList().Where(x => Convert.ToDouble(x.Cylinder_Size) == Size).FirstOrDefault();

            if (v != null)
            {
               
                return Json(v, JsonRequestBehavior.AllowGet);
            }

            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }

        }
        
        public JsonResult getProduct(string Code)
        {
            Invetory v = db.Goods.Where(x => x.Size_Code == Code).FirstOrDefault();

            if(v != null)
            {
                return Json(v, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }


    }
}
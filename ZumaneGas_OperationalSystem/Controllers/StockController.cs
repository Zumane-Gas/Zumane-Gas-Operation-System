using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZumaneGas_OperationalSystem.Models;

namespace ZumaneGas_OperationalSystem.Controllers
{
    public class StockController : Controller
    {
        // GET: Stock
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NewStock(Stock stock)
        {
            ViewBag.Inv = new SelectList(db.Goods.Where(x => x.ProductName != null && x.Cylinder_Size != 2 && x.Cylinder_Size != 3 && x.Cylinder_Size != 4 && x.Cylinder_Size != 5 && x.Cylinder_Size != 6 && x.Cylinder_Size != 7),
                "Invetory_Id", "ProductName", stock.Invetory_Id);
            //ViewBag.Product = new SelectList(db.Goods, "Inventory_Id", "ProductName", stock.Invetory_Id);
            return View();
        }
        
        //Working
        public void AddStock(Stock s)
        {
           // int? StockId = null;

            Stock stock = new Stock();
           // StockId = stock.Stock_Id;
           // stock = db.Stockes.Find(StockId);

            stock.Inventory = s.Inventory;
            stock.Invetory_Id = s.Invetory_Id;
            stock.Quantity = s.Quantity;
            stock.Report = s.Report;
            stock.Cylinder_Size = s.Cylinder_Size;
            stock.ProductName = s.ProductName;
            stock.C_Size = s.C_Size;

            //db.Entry(stock).State = EntityState.Modified;
            db.Stockes.Add(stock);
            db.SaveChanges();

        }       
        public ActionResult ViewStock()
        {
            Stock stock = new Stock();
            ViewBag.Inv = new SelectList(db.Goods.Where(x => x.ProductName != null && x.Cylinder_Size != 2 && x.Cylinder_Size != 3 && x.Cylinder_Size != 4 && x.Cylinder_Size != 5 && x.Cylinder_Size != 6 && x.Cylinder_Size != 7),
                      "Invetory_Id", "ProductName", stock.Invetory_Id);
            return View();
        }

        [HttpGet]
        public ActionResult StockEdit(int? StockID)
        {
            Stock stock = new Stock();
             try
                {
                    stock = db.Stockes.Find(StockID);
                    if(stock!= null)
                    {
                        
                        return Json(stock, JsonRequestBehavior.AllowGet);
                    }
                   // return Json(null, JsonRequestBehavior.AllowGet);
                    return RedirectToAction("StockEdit", new { stock = Equals(stock)});
                }
                catch(Exception ex)
                {
                    return null;
                }
        }

        [HttpPost]
        public ActionResult StockEdit([Bind(Include = "Stock_Id,Invetory_Id,Quantity,NewStock,C_Size,Cylinder_Size,ProductName")]Stock stock)
        {

            if (ModelState.IsValid)
            {
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
               
            }
            return RedirectToAction("AvailableStock");
        }
        public JsonResult getInventoryDetails(int InvId)
        {
            Stock s = new Stock();

            List<Invetory> v = db.Goods.Where(x => x.Invetory_Id == InvId).ToList();

            if(v != null)
            {
                return Json(v, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult getStockDetails(int InvID)
        {
            var st = db.Stockes.Where(x => x.Invetory_Id == InvID).FirstOrDefault();

            if(st != null)
            {
                return Json(st, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult AvailableStock()
        {

            List<Stock> stock = db.Stockes.ToList();

            foreach(var item in stock)
            {
                if(item.Quantity < 1)
                {
                    item.Status = Status.OutOfStock;
                }
                else
                {
                    item.Status = Status.Available;
                }
            }
            return View(stock);
        }

        
        public ActionResult EditStock(int? StockID)
        {
            if (StockID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Stock stock = db.Stockes.Find(StockID);
            if (stock == null)
            {
                return HttpNotFound();
            }

            ViewBag.Inv = new SelectList(db.Stockes.Where(x => x.ProductName != null && x.Cylinder_Size != 2 && x.Cylinder_Size != 3 && x.Cylinder_Size != 4 && x.Cylinder_Size != 5 && x.Cylinder_Size != 6 && x.Cylinder_Size != 7),
              "Invetory_Id", "ProductName");

            return View();
        }
        public void RecordStock(Stock s)
        {

            Stock stock = new Stock();
            stock.Invetory_Id = s.Invetory_Id;
            stock.Quantity = s.Quantity;

            var NewTot = stock.Quantity + stock.Quantity;
            stock.Quantity = NewTot;

            if (ModelState.IsValid)
            {
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
            }
        }


        //public JsonResult getProductName(string ProductName)
        //{
        //    var p = db.Goods.Where(x => x.ProductName == ProductName).ToList();

        //    if(p != null)
        //    {
        //        return Json(p, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json("Error", JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public JsonResult getCylinderSize(int CylinderSize)
        //{
        //    var s = db.Goods.Where(x => x.Cylinder_Size == CylinderSize);

        //    if(s != null)
        //    {
        //        return Json(s, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json("Error", JsonRequestBehavior.AllowGet);
        //    }
        //}


        //public ActionResult NewStock(Stock s)
        //{
        //    Stock stock = new Stock();
        //   

        //    ViewBag.Inv = new SelectList(db.Goods, "Invetory_Id", "Cylinder_Size", stock.Invetory_Id);
        //    ViewBag.Product = new SelectList(db.Goods, "Inventory_Id", "ProductName",stock.Invetory_Id);
        //    return View();

        //}

    }
}
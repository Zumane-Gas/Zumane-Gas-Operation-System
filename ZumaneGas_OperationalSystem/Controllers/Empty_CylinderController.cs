using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using ZumaneGas_OperationalSystem.Models;

namespace ZumaneGas_OperationalSystem.Controllers
{
    public class Empty_CylinderController : Controller
    {
        // GET: Empty_Cylinder
        public ActionResult Index()
        {
            return View();
        }

        // New set of Empties
        // I have inventory now all I need to do is add the inventory to size
        [HttpGet] 
        public ActionResult NewSet()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Stock stock = new Stock();
            //Call Inventory
            //ViewBag.Inv = new SelectList(db.Goods.Where(x => x.Cylinder_Size == 9 && x.Cylinder_Size == 14 && x.Cylinder_Size == 19 && x.Cylinder_Size == 48),
            //   "Invetory_Id", "Cylinder_Size", stock.Invetory_Id);
            ViewBag.Inv = new SelectList(db.Goods.Where(x => x.ProductName != null && x.Cylinder_Size != 2 && x.Cylinder_Size != 3 && x.Cylinder_Size != 4 && x.Cylinder_Size != 5 && x.Cylinder_Size != 6 && x.Cylinder_Size != 7 && x.Cylinder_Size != null),
                      "Invetory_Id", "Cylinder_Size", stock.Invetory_Id);
            return View();
        }

        [HttpPost]
        public void Add_Empties(Empty_Cylinder Empty)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Empty_Cylinder e = new Empty_Cylinder();

            e.Empty_Size = Empty.Empty_Size;
            e.Empty_Size_Quantity = Empty.Empty_Size_Quantity;
            e.Total_Empties_Small = Empty.Total_Empties_Small;
            e.Total_Empties_Large = Empty.Total_Empties_Large;

            var GetSingle = db.Empties.Where(x => x.EmptiesID == Empty.EmptiesID).First();

            if (ModelState.IsValid)
            {
                if(GetSingle != null)
                    {
                        //Need to add a new parameter to keep track of old and new (add them up)
                        db.Entry(Empty).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Empties.Add(Empty);
                        db.SaveChanges();
                    }
                }
            
        }

        public JsonResult GetCurrent_Quantity(string Size)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            Empty_Cylinder E = db.Empties.Where(x => x.Empty_Size == Size).FirstOrDefault();

            if(E != null)
            {
                return Json(E, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }

        }

    }
}
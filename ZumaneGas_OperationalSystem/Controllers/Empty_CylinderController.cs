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
        private ApplicationDbContext db = new ApplicationDbContext();
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
            var count = db.Empties.ToList().Count();

            e.Empty_Size = Empty.Empty_Size;
            var currrent_size = e.Empty_Size_Quantity;
            e.Empty_Size_Quantity = Empty.Empty_Size_Quantity + currrent_size;
            e.Total_Empties_Small = Empty.Total_Empties_Small;
            e.Total_Empties_Large = Empty.Total_Empties_Large;

            if(count > 0 && Empty.EmptiesID != 0)
            {
                var GetSingle = db.Empties.Where(x => x.EmptiesID == Empty.EmptiesID).First();
                if (GetSingle != null)
                {
                    GetSingle.Empty_Size_Quantity = GetSingle.Empty_Size_Quantity + Empty.Empty_Size_Quantity;
                    //Need to add a new parameter to keep track of old and new (add them up)
                    db.Entry(GetSingle).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                db.Empties.Add(e);
                db.SaveChanges();
            }
        }
        //Minus empties on order request
        public ActionResult Decrement_Empties(Empty_Cylinder empties)
        {
            Empty_Cylinder empty = new Empty_Cylinder();
            ViewBag.Inv = new SelectList(db.Empties.Where(x => x.Empty_Size_Quantity != 0),"EmptiesID", "Empty_Size", empty.EmptiesID);

            string[] items = { };
            int[] qty = { };

            //Nested for loop

            /** if(Required_size >= Total_Available empties){
             * foreach(var i in )
             * } **/

            /*****
             On client side:
             Document total empties - e.g 50 -> 40 9kg, 5 19kg, 5 14kg
             Store empties in an array -> when required empties have been captured, then button to complete will be activated. 
             ******/

            return View();
        }

        public ActionResult getAllEmpties()
        {
            Empty_Cylinder empty = new Empty_Cylinder();
            ViewBag.Empties = new SelectList(db.Empties.ToList(), "EmptiesID", "Empty_Size", empty.EmptiesID);
 
            var empties_Count = db.Empties.ToList().Sum(x => x.Empty_Size_Quantity);
            ViewBag.Avail_Count = empties_Count;

            return View();

        }
        
        public JsonResult GetCurrent_Quantity(string Size)
        {
           

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
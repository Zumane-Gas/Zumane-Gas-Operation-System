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


        // Add new empty details [e.g. new size to consider when counting empties]
        [HttpPost]
        public void Add_Empties(Empty_Cylinder Empty)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Empty_Cylinder e = new Empty_Cylinder();


            //Important check -- Check if gas size already exist,  if yes { increment quantity } else { Add gas Size and details}
            //var getAll_ID = db.Empties.Select(x => new { x.EmptiesID }).ToList();
            var getID = Empty.EmptiesID;
            var getSingleRecord = db.Empties.Where(x => x.EmptiesID == getID).FirstOrDefault();

            if (getSingleRecord != null)
            {
                var new_size = Empty.Empty_Size_Quantity;
                getSingleRecord.Empty_Size_Quantity = getSingleRecord.Empty_Size_Quantity + new_size;
                db.Entry(getSingleRecord).State = EntityState.Modified;
            }
            else
            {
                e.Empty_Size = Empty.Empty_Size;
                //var currrent_size = e.Empty_Size_Quantity;
                e.Empty_Size_Quantity = Empty.Empty_Size_Quantity;
                db.Empties.Add(e);
            }
            db.SaveChanges();
        }
        [HttpPost]
        public void Add_Empties_Array(Empty_Cylinder Empty)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Empty_Cylinder e = new Empty_Cylinder();
            var count = db.Empties.ToList().Count();
            //need to add to db (use as temp array)
            //string[] items = Empty.items;
            //int[] quanties = Empty.quanties;

            //int i = 0;
            //while(i <= items.Length && i <= quanties.Length)
            //{
            //    for(i; i <= items.Length;)
            //    {
            //        for(i; i <= quanties.Length;)
            //        {
            //            e.Empty_Size = items[i];
            //            var currrent_size = e.Empty_Size_Quantity;
            //            e.Empty_Size_Quantity = quanties[i] + currrent_size;
            //            i = i + 1;
            //        }
            //    }
            //}

            e.Empty_Size = Empty.Empty_Size;
            var currrent_size = e.Empty_Size_Quantity;
            e.Empty_Size_Quantity = Empty.Empty_Size_Quantity + currrent_size;

            if (count > 0 && Empty.EmptiesID != 0)
            {
                var GetSingle = db.Empties.Where(x => x.EmptiesID == Empty.EmptiesID).First();
                if (GetSingle != null)
                {
                    GetSingle.Empty_Size_Quantity = GetSingle.Empty_Size_Quantity + Empty.Empty_Size_Quantity;
                    //Need to add a new parameter to keep track of old and new (add them up)
                    db.Entry(GetSingle).State = EntityState.Modified;
                    //db.SaveChanges();
                }
            }
            else
            {
                db.Empties.Add(e);

            }
            db.SaveChanges();
        }


        [HttpPost]
        public void Add_Empties_Order(Empty_Cylinder Empty)
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
                    //db.SaveChanges();
                }
            }
            else
            {
                db.Empties.Add(e);
               
            }
            db.SaveChanges();
        }
        //Minus empties on order request
        public ActionResult Decrement_Empties()
        {
            Empty_Cylinder empty = new Empty_Cylinder();
            ViewBag.Inv = new SelectList(db.Empties.Where(x => x.Empty_Size_Quantity != 0),"EmptiesID", "Empty_Size", empty.EmptiesID);
            return View();
        }

        public void Order_DecrementEmpies(Empty_Cylinder Empty_ClientSide)
        {   
            /******
             -- Information required from client side 
             1. empty size 
             2. quanity to decrement

            -- The process 
            1. Store data in arrays (sizes and quantity) 
            2. iterate through arrays to getSize and getQuantity
            3. Update empties table
            4. SaveChanges

            --- Decrement empties on sale 
            1. get values from sale - 
                1.1. if(array_items and array_qty length is not 0)
                1.2. pass values to Order_DecrementEmpties 
                1.3 logic already caters for values passed from client side 
            2. This eliminate duplicating logic - reusability
             *******/
            
            string[] getSizes = Empty_ClientSide.getSize;
            int[] getQuantity = Empty_ClientSide.getQuantity;

            for(int S = 0; S < getSizes.Length; S++)
            {
                    var tempSize = getSizes[S];
                    var getSize_Info = db.Empties.Where(x => x.Empty_Size == tempSize).FirstOrDefault();
                    var currentSize = getSize_Info.Empty_Size_Quantity;
                    getSize_Info.Empty_Size_Quantity = currentSize - getQuantity[S];
                    db.Entry(getSize_Info).State = EntityState.Modified;           
            }

            //db.SaveChanges();     
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
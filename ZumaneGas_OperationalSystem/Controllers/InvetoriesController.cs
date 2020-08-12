using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZumaneGas_OperationalSystem.Models;

namespace ZumaneGas_OperationalSystem.Controllers
{
    public class InvetoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Invetories
        public ActionResult Index()
        {
            return View(db.Goods.ToList());
        }

        // GET: Invetories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invetory invetory = db.Goods.Find(id);
            if (invetory == null)
            {
                return HttpNotFound();
            }
            return View(invetory);
        }

        // GET: Invetories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Invetories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Invetory_Id,Cylinder_Size,Cylinder_Price,Size_Code,ProductName,Product_Price,Deposit,DepositFee")] Invetory invetory)
        {
            if (ModelState.IsValid)
            {
                db.Goods.Add(invetory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(invetory);
        }

        // GET: Invetories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invetory invetory = db.Goods.Find(id);
            if (invetory == null)
            {
                return HttpNotFound();
            }
            return View(invetory);
        }

        // POST: Invetories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Invetory_Id,Cylinder_Size,Cylinder_Price,Size_Code,ProductName,Product_Price,Deposit,DepositFee")] Invetory invetory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invetory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invetory);
        }

        // GET: Invetories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invetory invetory = db.Goods.Find(id);
            if (invetory == null)
            {
                return HttpNotFound();
            }
            return View(invetory);
        }

        // POST: Invetories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invetory invetory = db.Goods.Find(id);
            db.Goods.Remove(invetory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

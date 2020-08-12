using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZumaneGas_OperationalSystem.Models;

namespace ZumaneGas_OperationalSystem.Controllers
{
    public class PayoutController : Controller
    {
        // GET: Payout
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NewPayout()
        {
            return View();
        }
        public ActionResult RecordPayout(DailyPayout Payout)
        {
               ApplicationDbContext db = new ApplicationDbContext();
          
                DailyPayout p = new DailyPayout();
                p.Payout_Descriptiom = Payout.Payout_Descriptiom;
                p.Payout_Amount = Payout.Payout_Amount;
                p.Payout_Date = Payout.Payout_Date;

                Payout.Payout_Date = DateTime.Now;
                db.payouts.Add(Payout);
                db.SaveChanges();
                
            return RedirectToAction("Index","Home");
        }
    }
}
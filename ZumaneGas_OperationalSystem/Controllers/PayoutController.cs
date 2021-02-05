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

            Company_Feed feed = new Company_Feed();
            feed.DateTime = DateTime.Now;
            feed.Title = "New Payout";
            feed.SubTitle = "New Payout Recorded at " + feed.DateTime;
            feed.Details = Payout.Payout_Descriptiom;
            feed.updated_Cost = Payout.Payout_Amount.ToString();
            //feed.Details = "Payout Details: " + 


            DailyPayout p = new DailyPayout();
            p.Payout_Descriptiom = Payout.Payout_Descriptiom;
            p.Payout_Amount = Payout.Payout_Amount;
            p.Payout_Date = Payout.Payout_Date;

            if (ModelState.IsValid)
            {
                Payout.Payout_Date = DateTime.Now;
                db.Feeds.Add(feed);
                db.payouts.Add(Payout);
                db.SaveChanges();
            }
            
                
            return RedirectToAction("Index","Home");
        }
    }
}
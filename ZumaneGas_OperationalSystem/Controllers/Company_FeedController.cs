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
    public class Company_FeedController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Company_Feed
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LatestFeeds()
        {
            return View();
        }

        public ActionResult getStockFeeds()
        {
            var Stock_Feeds = from S in db.Feeds
                              where S.Title.Contains("Stock Details Updated") && S.DateTime.Value.Day.Equals(DateTime.Now.Day) && S.DateTime.Value.Month.Equals(DateTime.Now.Month) && S.DateTime.Value.Year.Equals(DateTime.Now.Year)
                              select S;
            if(Stock_Feeds != null)
            {
                return Json(Stock_Feeds, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return ViewBag.NoFeedsMessage = "No stock added";
            }
        }

        public ActionResult getPayoutFeeds()
        {
            var Stock_Feeds = from S in db.Feeds
                              where S.Title.Contains("New Payout") && S.DateTime.Value.Day.Equals(DateTime.Now.Day) && S.DateTime.Value.Month.Equals(DateTime.Now.Month) && S.DateTime.Value.Year.Equals(DateTime.Now.Year)
                              select S;

            if (Stock_Feeds != null)
            {
                return Json(Stock_Feeds, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return ViewBag.NoFeedsMessage = "No Payouts Recorded";
            }
        }

        //public JsonResult getCurrentFeeds()
        //{
        //    List<Company_Feed> feeds = db.Feeds.OrderByDescending(x => x.DateTime).Where(x => x.DateTime.Day.Equals(DateTime.Now.Day) && x.DateTime.Month.Equals(DateTime.Now.Month)
        //                       && x.DateTime.Year.Equals(DateTime.Now.Year)).ToList();
            
        //    if (feeds != null)
        //    {
        //        return Json(feeds, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json("Error", JsonRequestBehavior.AllowGet);
        //    }
        //}



    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CozaStore.Models;
using CozaStore.Areas.Dashboard.Models;
namespace CozaStore.Areas.Dashboard.Controllers
{
    public class AdminController : Controller
    {
        CozaStoreEntities db = new CozaStoreEntities();
        // GET: Dashboard/Admin
        public ActionResult Index()
        {
            var sum = db.Order.Sum(n => n.TotalPrice);
            var cntorder = db.Order.ToList();
            var cntuser = db.User.ToList();
            ViewBag.sum = sum;
            ViewBag.cntorder = cntorder.Count();
            ViewBag.cntuser = cntuser.Count();
            return View();
        }
       [ChildActionOnly]
       public ActionResult ViewTopProduct()
        {
            var lstproduct = (from c in db.Product
                             select c).ToList().Take(6);

            List<Rankproduct> list = new List<Rankproduct>();
            foreach (var item in lstproduct)
            {
                int count = 0;
                var lstdetail = from c in db.DetailsOrder
                                where c.Productid == item.Productid
                                select c;
                if(lstdetail != null)
                count = lstdetail.Count();
                Rankproduct tmp = new Rankproduct { product = item, numproduct = count };
                list.Add(tmp);
            }
             
            return View(list);
        }
        [ChildActionOnly]
        public ActionResult chart()
        {
            List<decimal?> totalprice = new List<decimal?>();
            for(int i =1; i <= 12; i++)
            {
                var item = db.Order.Where(n => n.OrderDay.Value.Month == i).Sum(c => c.TotalPrice);
                if (item != null)
                    totalprice.Add(item);
                else
                    totalprice.Add(0);
            }
                
            return View(totalprice);
        }
        [ChildActionOnly]
        public ActionResult ViewRankUser()
        {
            var rankUser = db.User.ToList();
            List<RankUser> lstrank = new List<RankUser>();
            decimal? sum = 0;
            foreach(var item in rankUser)
            {
                decimal? sumitem = db.Order.Where(n => n.Userid == item.Userid).Sum(n => n.TotalPrice);
                lstrank.Add(new RankUser
                {
                    user = item,
                    total = sumitem
                });
                sum += sumitem;
            }
            return View(lstrank);
        }
        [ChildActionOnly]
        public ActionResult Order()
        {
            var lstorder = from c in db.Order
                           join p in db.User
                           on c.Userid equals p.Userid
                           select new UserOrder
                           {
                               user = p,
                               order = c
                           };
            return View(lstorder.ToList());
        }
    }
}
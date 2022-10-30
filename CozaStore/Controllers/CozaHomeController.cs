using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CozaStore.Models;
namespace CozaStore.Controllers
{
    public class CozaHomeController : Controller
    {
        CozaStoreEntities db = new CozaStoreEntities();
        // GET: CozaHome
        public ActionResult Index()
        {

            return View(db.Product.ToList());
        }
        [ChildActionOnly]
        public ActionResult TypeShose()
        {
            return PartialView(db.Category.ToList());
        }
        [ChildActionOnly]
        public ActionResult Banner()
        {
            return PartialView();
        }

        public ActionResult ProductDetails(int? productid)
        {
            Product product = db.Product.SingleOrDefault(n => n.Productid == productid);
            if(product == null)
            {
                return RedirectToAction("Index");
            }
            return View(product);
        }
    }
}
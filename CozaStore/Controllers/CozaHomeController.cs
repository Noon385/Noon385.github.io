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
            var banner = db.Banner.Join(db.Product,
                e => e.Productid,
                p => p.Productid,
                (e, p) => new product_banner {
                    product = p,
                    banner = e
                });
            return PartialView(banner.ToList());
        }
        List<Size> FullSize()
        {
            List<Size> Size = db.Size.ToList();
            return Size;
        }
        List<Color> FullColor()
        {
            List<Color> Color = db.Color.ToList();
            return Color;
        }

        public ActionResult ProductDetails(int? productid)
        {
            Product product = db.Product.SingleOrDefault(n => n.Productid == productid);
            if(product == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Size = FullSize();
            ViewBag.Color = FullColor();
            return View(product);
        }
        public ActionResult about()
        {
            return View();
        }
        public ActionResult contact()
        {
            return View();
        }
        
    }
}
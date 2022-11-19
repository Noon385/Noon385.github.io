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
            var comment = from c in db.Comment
                          join p in db.User
                          on c.Userid equals p.Userid
                          where c.Productid == productid
                          select new Usercomment {
                              comment = c,
                              user = p
                          };
            if(product == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Size = FullSize();
            ViewBag.Color = FullColor();
            ViewBag.comment = comment.ToList();
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
        public ActionResult AddComment(int productid, FormCollection f)
        {
            if(Session["User"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            Comment comment = new Comment();
            User user = Session["User"] as User;
            comment.Productid = productid;
            comment.Userid = user.Userid;
            comment.Message = f["comment"].ToString();
            comment.Time = DateTime.Now;
            db.Comment.Add(comment);
            db.SaveChanges();
            return RedirectToAction("ProductDetails", new { productid = productid });
        }
    }
}
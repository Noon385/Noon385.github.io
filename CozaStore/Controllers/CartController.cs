using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CozaStore.Models;
namespace CozaStore.Controllers
{
    public class CartController : Controller
    {
        CozaStoreEntities db = new CozaStoreEntities();
        public List<Cart> GetCart()
        {
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if(lstCart == null)
            {
                lstCart = new List<Cart>();
                Session["Cart"] = lstCart;
            }
            return lstCart;
        }

        public ActionResult AddCart(int productid, string url , FormCollection f)
        {
            List<Cart> lstCart = GetCart();
            Cart product = lstCart.Find(n => n.Productid == productid);
            int sizeid = int.Parse(f["Type-Size"].ToString());
            int colorid = int.Parse(f["Type-Color"].ToString());
            int numproduct = int.Parse(f["num-product"]);
            if(product == null)
            {
                product = new Cart(productid,sizeid,colorid,numproduct);
                lstCart.Add(product);
            }
            else
            {
                product.ProductNumber += numproduct;
            }
            return Redirect(url);
        }

        private int TotalNumberProduct()
        {
            int totalNumberProduct = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if(lstCart != null)
            {
                totalNumberProduct = lstCart.Sum(n => n.ProductNumber);
            }
            return totalNumberProduct;
        }
        // GET: Cart
        private decimal TotalPrice()
        {
            decimal TotalPrice = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if(lstCart != null)
            {
                TotalPrice = lstCart.Sum(n => n.TotalProductPrice);
            }
            return TotalPrice;
        }
        List<Color> FullColor()
        {
            return db.Color.ToList();
        }
         List<Size> FullSize()
        {
            return db.Size.ToList();
        }

        public ActionResult Cart()
        {
            List<Cart> lstCart = GetCart();
            if(lstCart.Count == 0)
            {
                return RedirectToAction("Index", "CozaHome");
            }
            ViewBag.Number = TotalNumberProduct();
            ViewBag.TotalPrice = TotalPrice();
            ViewBag.Size = FullSize();
            ViewBag.Color = FullColor();
            return View(lstCart);
        }

        public ActionResult DeleteProduct(int productid)
        {
            List<Cart> lstCart = GetCart();
            Cart product = lstCart.SingleOrDefault(n => n.Productid == productid);
            if(product != null)
            {
                lstCart.RemoveAll(n => n.Productid == productid);
                if(lstCart.Count == 0)
                {
                    return RedirectToAction("Index", "CozaHome");
                }
            }
            return RedirectToAction("Cart");
        }

        public ActionResult UpdateCart(int productid, FormCollection f)
        {
            List<Cart> lstcart = GetCart();
            if(lstcart == null)
            {
                return RedirectToAction("Cart", "Cart");
            }
            Cart product = lstcart.SingleOrDefault(n => n.Productid == productid);
            if(product == null)
            {
                return RedirectToAction("Cart", "Cart");
            }
            int num = int.Parse(f["num-product"]);
            product.ProductNumber = num;
            return RedirectToAction("Cart", "Cart");
        }
        public ActionResult ClearCart()
        {
            List<Cart> lstCart = GetCart();
            lstCart.Clear();
            return RedirectToAction("Index", "CozaHome");

        }
    }
}
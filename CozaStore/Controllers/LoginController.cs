using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CozaStore.Models;
namespace CozaStore.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        CozaStoreEntities db = new CozaStoreEntities();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            string Email = f["Email"].ToString();
            string Password = f["Password"].ToString();
            User user = db.User.SingleOrDefault(n => n.Gmail == Email && n.Password == Password);
            if(user == null)
            {
                TempData["err1"] = "Email or Password is incorrect !!!";
                TempData["Email"] = Email;
                TempData["Password"] = Password;
                
                return Redirect("Login");
            }
            Session["User"] = user;
            return RedirectToAction("Index", "CozaHome");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(FormCollection f)
        {
            string FullName = f["FullName"]; 
            string Email = f["Email"]; 
            string Password = f["Password"]; 
            string RePassword = f["RePassword"];
            DateTime BirthDay = DateTime.Parse(f["BirthDay"].ToString());
            bool Gender = bool.Parse(f["Gender"]);
            string PhoneNumber = f["PhoneNumber"];
            string Address = f["Address"];

            if(Password != RePassword)
            {
                TempData["err1"] = "Erro";
                return RedirectToAction("Register");
            }
            User usertmp = db.User.SingleOrDefault(n => n.Gmail == Email);
            if(usertmp != null)
            {
                TempData["err1"] = "Email erro";
                return RedirectToAction("Register");
            }
            User user = new User();
            user.FullName = FullName;
            user.Gmail = Email;
            user.Password = Password;
            user.BirthDay = BirthDay;
            user.Gender = Gender;
            user.Phone = PhoneNumber;
            user.Address = Address;
            db.User.Add(user);
            db.SaveChanges();
            return RedirectToAction("Login");
        }
    }
}
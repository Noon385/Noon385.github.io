using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CozaStore.Areas.Dashboard.Controllers
{
    public class AdminController : Controller
    {
        // GET: Dashboard/Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}
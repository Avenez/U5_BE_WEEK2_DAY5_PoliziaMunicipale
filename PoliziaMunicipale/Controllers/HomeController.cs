using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace PoliziaMunicipale.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["inserimento"] == null) 
            {
                Session["inserimento"] = false;
            }

            bool ins = (bool)Session["inserimento"];

            if (ins == false)
            {
                TempData["Inserimento"] = false;
            }
            else 
            {
                TempData["Inserimento"] = true;
                Session["inserimento"] = false;
            }
            

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult RecuperoDati() 
        {
            return View();
        }
    }
}
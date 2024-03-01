using PoliziaMunicipale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoliziaMunicipale.Controllers
{
    public class AnagraficaController : Controller
    {
        // GET: Anagrafica
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult CreateAnagrafica() {
        return View();
        }

        [HttpPost]
        public ActionResult CreateAnagrafica(Anagrafica A)
        {
            try 
            {
                Anagrafica.InserisciNuovaAnagrafica(A);
                Session["Inserimento"] = true;
                Session["Messaggio"] = "Inserimento Anagrafe avvenuto con Successo";
                return RedirectToAction("Index", "Home");
            }
            catch 
            {
                return View();
            }
            
            
        }
    }
}
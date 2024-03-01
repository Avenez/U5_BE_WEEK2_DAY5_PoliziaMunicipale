using PoliziaMunicipale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoliziaMunicipale.Controllers
{
    public class TipoDiViolazioneController : Controller
    {
        // GET: TipoDiViolazione
        public ActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public ActionResult CreateViolazione() 
        {
            try
            {
                List<TipoViolazione> listaViolazioniDb = TipoViolazione.GetTipoViolazioni();
                
                ViewBag.listViolazioni = listaViolazioniDb;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }


        //Richiamo il metodo e inserisco una nuova Violazione grazie al metodo inserito nel model
        //Uso le Session per passare valori sia ad HomeController che alla View Index di Home
        [HttpPost]
        public ActionResult CreateViolazione(TipoViolazione Vi)
        {
            try
            {
                TipoViolazione.InserisciNuovoTipoViolazione(Vi);
                Session["Inserimento"] = true;
                Session["Messaggio"] = "Inserimento Violazione avvenuto con Successo";
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }

            
        }
    }
}
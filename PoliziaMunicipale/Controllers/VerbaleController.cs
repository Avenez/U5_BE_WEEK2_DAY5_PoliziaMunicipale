using PoliziaMunicipale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PoliziaMunicipale.Controllers
{
    public class VerbaleController : Controller
    {
        // GET: Verbale
        public ActionResult Index()
        {
            return View();
        }

        //Creo delle SelectList da usare nella View CreateVerbale per ottenere delle select che permettono la selezione dei trasgressori ("colpevoli") 
        //e il tipo di trasgressione commessa.
        //Queste Vengono create grazie ai metodi interni al model "Verbale"
        //Queste vengono passate attraverso ViewBag
        [HttpGet]
        public ActionResult CreateVerbale()
        {
            try 
            {
                List<Anagrafica> listaAnagraficaDb = Anagrafica.GetAllAnagrafiche();
                SelectList dropListAnagrafica = new SelectList(listaAnagraficaDb, "IdAnagrafica", "fullIdAngrafica");
                ViewBag.dropListAnagrafica = dropListAnagrafica;
            } 
            catch (Exception ex)
            {
            Console.WriteLine(ex.Message);
            }

            try
            {
                List<TipoViolazione> listaViolazioniDb = TipoViolazione.GetTipoViolazioni();
                SelectList dropListViolazioni = new SelectList(listaViolazioniDb, "IdViolazione", "Descrizione");
                ViewBag.dropListViolazioni = dropListViolazioni;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return View();
        }


        //Richiamo il metodo e inserisco un nuovo Verbale grazie al metodo inserito nel model
        //Ruchiamo anche il metodo per la sottrazione dei punti dal totale della persona selezionata come "colpevole" dell'infrazione (metodo nel model Anagrafica)
        //Uso le Session per passare valori sia ad HomeController che alla View Index di Home
        [HttpPost]
        public ActionResult CreateVerbale(Verbale V)
        {
            try
            {
               Verbale.InserisciNuovoVerbale(V);
               Anagrafica.SottraiPunti(V.IdAnagrafica, V.DecurtamentoPunti);
               Session["Inserimento"] = true;
               Session["Messaggio"] = "Inserimento Verbale avvenuto con Successo";
                
               return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }


        }


        //Action che crea una lista di violazioni grazie al metodo contenuto nel Model e che passa alla View tramite ViewBag
        [HttpGet]
        public ActionResult GetVerbaliAnagrafica(int order) 
        {

                try
                {
                    List<SommarioViolazioniAnagrafe> listaViolazioniAnagrafica = SommarioViolazioniAnagrafe.GetSommarioViolazioniAnagrafe(order);
                    ViewBag.ListViolazioniAnagrafe = listaViolazioniAnagrafica;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                ViewBag.Order = order;

                return View();

        }


        //Action che crea una lista di violazioni grazie al metodo contenuto nel Model e che passa alla View tramite ViewBag
        [HttpGet]
        public ActionResult GetPuntiAnagrafica(int order2)
        {

                try
                {
                    List<SommarioViolazioniAnagrafe> listaPuntiAnagrafica = SommarioViolazioniAnagrafe.GetSommarioPuntiAnagrafe(order2);
                    ViewBag.ListPuntiAnagrafe = listaPuntiAnagrafica;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                ViewBag.Order2 = order2;

                return View();

        }

        //Action che crea una lista di violazioni grazie al metodo contenuto nel Model e che passa alla View tramite ViewBag
        [HttpGet]
        public ActionResult GetSommarioAnagrafe(int type, int punti)
        {

                try
                {
                    List<SommarioViolazioniAnagrafe> listaAnagrafica = SommarioViolazioniAnagrafe.GetSommarioAnagrafe(type, punti);
                    ViewBag.ListAnagrafe = listaAnagrafica;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }



                return View();
            



        }

        //Action che crea una lista di violazioni grazie al metodo contenuto nel Model e che passa alla View tramite ViewBag
        [HttpGet]
        public ActionResult GetVerbali(int importo)
        {

                try
                {
                    List<Verbale> listaVerbali = Verbale.GetVerbaliConImportoSuperiore(importo);
                    ViewBag.ListVerbali = listaVerbali;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return View();


        }

    }

    
}
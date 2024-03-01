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

        [HttpGet]
        public ActionResult GetVerbaliAnagrafica(int order) 
        {
            if (order != null)
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
            else 
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        public ActionResult GetPuntiAnagrafica(int order2)
        {
            if (order2 != null)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        public ActionResult GetSommarioAnagrafe(int type, int punti)
        {
            if (type != null)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }

            //GetVerbaliConImportoSuperiore(int importoMinimo)


        }

        [HttpGet]

        public ActionResult GetVerbali(int importo)
        {
            if (importo != null)
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
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

    }

    
}
using MachineCafeIHM.ApiWrapper;
using MachineCafeIHM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MachineCafeIHM.Controllers
{
    public class MachineCafeController : Controller
    {
        // GET: ChoixBoisson
        public ActionResult ChoixBoisson()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChoixBoisson(string badgecode)
        {
            return View();
        }
        [HttpPost]
        public ActionResult DemanderBoisson(ChoixBoissonApiPoco choix)
        {

            MachineCafeApiWrapper machineCafeApiWrapper = new MachineCafeApiWrapper("http://localhost:60351/api/MachineCafe/", "", "");
            string message = string.Empty;
            machineCafeApiWrapper.DemanderBoisson(choix, out message);

            if (string.IsNullOrEmpty(message))
                message = "Votre boisson est servi.";

            ViewBag.message = message;
           
            return View("ChoixBoisson");
        }
        public ActionResult ChoixBoissonList(string badgecode)
        {

            MachineCafeApiWrapper machineCafeApiWrapper = new MachineCafeApiWrapper("http://localhost:60351/api/MachineCafe/", "", "");
            string message = string.Empty;
            ChoixBoissonListApiPoco choixBoissonList = machineCafeApiWrapper.DemandeBoissonList(badgecode, out message);
            ViewBag.choixList = choixBoissonList;
            return View("ChoixBoisson");
        }
    }
}
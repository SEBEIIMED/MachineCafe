using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Generics.Security;
using MachineCafe.Core;
using MachineCafe.Core.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MachineCafe.Service.Controllers
{
   
    [Route("api/[controller]")]
    public class MachineCafeController : Controller
    {
        private readonly IMachineCafeChoixRepository _MachineCafeChoixRepository;

        #region constructors-------------------------------------------------
        public MachineCafeController(IMachineCafeChoixRepository machineCafeChoixRepository) 
        {
            _MachineCafeChoixRepository = machineCafeChoixRepository;
        }
        #endregion
        #region Methods------------------------------------------------------
        [HttpPost("DemanderBoisson")]
        public IActionResult DemanderBoisson([FromBody]MachineCafeChoix choix)
        {
            try
            {
                MachineCafeChoix retour = _MachineCafeChoixRepository.DemanderBoisson(choix);
                return Ok(retour);
            }
            catch (Exception ex)
            {
                // on trace l'erreur ici ci possible
                return new BadRequestResult();
            }
        }
        [HttpPost("ChoixBoissonList")]
        public ActionResult ChoixBoissonList([FromBody]string badgecode)
        {
            try
            {
                IQueryable<MachineCafeChoix> machineCafechoixquery = _MachineCafeChoixRepository.ChoixBoissonList(badgecode);
                return Ok(machineCafechoixquery.ToList());
            }
            catch(Exception ex)
            {
                // on trace l'erreur ici ci possible
                return new BadRequestResult();
            }
        }

       
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Generics.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MachineCafeService.Controllers
{
   
    [Route("api/Services")]
    public class ServicesController : Controller
    {
        // POST: api/Services
        [HttpPost]
        public IActionResult GenerateToken(string app_key, string app_secret)
        {
            try
            {

                // Pour test seulement.
                if (app_key != "machinecafe_demo" || app_secret != "machinecafe_demo")
                    throw new Exception("Appid not found");


                TokenManager tokenmanager = new TokenManager();
                var tokendata = tokenmanager.PrepareTokenData(app_key);
                string access_token = tokenmanager.GenerateToken(tokendata, app_secret);
                var retour = new
                {
                    expires_in = 120.ToString(),
                    token_type = "BEARER",
                    access_token = access_token
                };

                return Ok(retour);
            }
            catch(Exception ex)
            {
                // on trace l'erreur ici ci possible
                return new BadRequestResult();
            }

        }
    }
}

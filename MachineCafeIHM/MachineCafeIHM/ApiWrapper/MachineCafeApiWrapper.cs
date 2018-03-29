using MachineCafeIHM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MachineCafeIHM.ApiWrapper
{
    public class MachineCafeApiWrapper : BaseApiWrapper
    {
        public MachineCafeApiWrapper(string baseUrl, string appID, string appSecret)
          : base(baseUrl, appID, appSecret)
        {
        }
        public ChoixBoissonApiPoco DemanderBoisson(ChoixBoissonApiPoco choix, out string errorMessage)
        {
            string path = "DemanderBoisson";
            ChoixBoissonApiPoco retour = base.PostFromApi<ChoixBoissonApiPoco>(path, choix, out errorMessage);

            return retour;
        }
        public ChoixBoissonListApiPoco DemandeBoissonList(string codebadge, out string errorMessage)
        {
            string path = "ChoixBoissonList";
            ChoixBoissonListApiPoco retour = base.PostFromApi<string, ChoixBoissonListApiPoco>(path, codebadge, out errorMessage);

            return retour;
        }
    }
}
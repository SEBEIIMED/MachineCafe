using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MachineCafe.Core.Data;

namespace MachineCafe.Core.Repository
{
    public class MachineCafeChoixRepository : GenericRepository<MachineCafeChoix>, IMachineCafeChoixRepository
    {
        public MachineCafeChoixRepository(DataContext context) : base(context)
        { }   
        public IQueryable<MachineCafeChoix> ChoixBoissonList(string codebadge)
        {
            return this.GetAll().Where(s => s.CodeBadge == codebadge);
        }
        public MachineCafeChoix DemanderBoisson(MachineCafeChoix choix)
        {
           // s.BoissonType == choix.BoissonType && s.CodeBadge == choix.CodeBadge
            var query = this.GetAll().Where(s => (s.CustomMug == choix.CustomMug && s.SucreQuantite == choix.SucreQuantite
                                                  && s.BoissonType == choix.BoissonType && s.CodeBadge == choix.CodeBadge));
            // on ajoute pas le choix si deja ajouter dans l'historique ou l'utilise n'a pas utilisé la badge.
            if (query.Count() == 0)
               return this.Add(choix);

            return choix;
            
        }
    }
}

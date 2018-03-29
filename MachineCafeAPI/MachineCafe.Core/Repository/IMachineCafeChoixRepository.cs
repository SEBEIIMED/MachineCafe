using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MachineCafe.Core.Repository
{
    
    public interface IMachineCafeChoixRepository :IGenericRepository<MachineCafeChoix>
    {
        MachineCafeChoix DemanderBoisson(MachineCafeChoix choix);
        IQueryable<MachineCafeChoix> ChoixBoissonList(string codebadge);
    }
}

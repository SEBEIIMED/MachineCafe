
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MachineCafeIHM.Models
{
    public class ChoixBoissonApiPoco
    {
        public string BoissonType { get; set; }
        public int SucreQuantite { get; set; }
        public bool CustomMug { get; set; }
        public string CodeBadge { get; set; }
    }
    public class ChoixBoissonListApiPoco : List<ChoixBoissonApiPoco>
    {

    }
}
using MachineCafe.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MachineCafe.Core
{
    public class MachineCafeChoix : IAuditable
    {
        [Key]
        public int ChoixKey { get; set; }
        public BoissonTypeEnum BoissonType { get; set; }
        public int SucreQuantite { get; set; }
        public bool CustomMug { get; set; }
        public string CodeBadge { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string CreatedBy { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Classes
{
    [Table("Contrat_Types")]
    public class Contract_Type
    {
        [Key,Required]
        [Display(Name = "Contract Type")]
        public String Title { get; set; }
        public Boolean Duration { get; set; }

        public virtual ICollection<Contract> Contrats { get; set; }
    }
}

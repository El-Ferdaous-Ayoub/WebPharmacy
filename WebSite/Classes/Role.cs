using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classes
{
    [Table("Roles")]
    public class Role
    {
        [Required, Key]
        [Display(Name = "Department")]
        public String Department_Title { get; set; }
        [Display(Name = "Pharmacy Info")]
        public Boolean Pharmacy_Info { get; set; }
        public Boolean Categories { get; set; }
        public Boolean Producers { get; set; }
        public Boolean Products { get; set; }
        public Boolean Orders { get; set; }
        public Boolean Departments { get; set; }
        public Boolean ContractTypes { get; set; }
        public Boolean Employees { get; set; }
    }
}

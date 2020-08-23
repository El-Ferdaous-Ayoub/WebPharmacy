using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classes
{
    [Table("Pharmacy")]
    public class Pharmacy
    {
        [Required, Key]
        [Display(Name = "Pharmacy")]
        public String Name { get; set; }
        public String Address { get; set; }
        public String Phone { get; set; }
        public String Email { get; set; }
    }
}

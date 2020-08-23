using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Classes
{
    [Table("Contracts")]
    public class Contract
    {
        [Key,Required]
        [Display(Name = "Employee NIC")]
        public String Employee_NIC { get; set; }
        [Display(Name = "Contract Type")]
        public  String Type { get; set; }
        [Display(Name = "Start")]
        public  DateTime Start { get; set; }
        [Display(Name = "End")]
        public  DateTime End { get; set; }
        public  String Document { get; set; }
        

        public virtual Contract_Type Contrat_Type { get; set; }
        public virtual Employee Employee { get; set; }

    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Classes
{
    [Table("Contracts")]
    public class Contract
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Employee NIC")]
        public int Employee_ID { get; set; }
        [Display(Name = "Contract Type")]
        public  String Type { get; set; }
        public  DateTime Start { get; set; }
        public  String Document { get; set; }

        [ForeignKey("Employee_ID")]
        public virtual Employee Employee { get; set; }

    }
}

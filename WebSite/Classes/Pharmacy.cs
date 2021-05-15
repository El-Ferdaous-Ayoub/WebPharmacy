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
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$",
            ErrorMessage = "Not a valid phone number")]
        public String Phone { get; set; }
        [EmailAddress]
        public String Email { get; set; }
    }
}

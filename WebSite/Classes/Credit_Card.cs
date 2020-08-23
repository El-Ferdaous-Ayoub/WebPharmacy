using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Classes
{
    [Table("Credit_Cards")]
    public class Credit_Card
    {
        [Required,Key]
        [Display(Name = "Card Number")]
        public String CardNumber { get; set; }
        public int Mounth { get; set; }
        public int Year { get; set; }
        public int CVC { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Classes.Models
{
    public class UserName
    {
        [Display(Name = "Username")]
        public String Username { get; set; }
        [Display(Name = "New Username")]
        [MinLength(3)]
        public String NewUsername { get; set; }
    }
}

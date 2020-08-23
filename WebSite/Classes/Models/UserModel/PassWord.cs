using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Classes.Models
{
    public class PassWord
    {
        [Display(Name = "Current Password")]
        [PasswordPropertyText(true)]
        public String Password { get; set; }
        [Display(Name = "New Password")]
        [PasswordPropertyText(true)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$",
           ErrorMessage = "Password is not sufficiently complex")]
        public String NewPassword { get; set; }
        [Display(Name = "Re-type New Password")]
        [PasswordPropertyText(true)]
        public String RetypeNewPassword { get; set; }
    }
}

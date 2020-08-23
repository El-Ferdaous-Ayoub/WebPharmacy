using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Classes.Models
{
    public class Contact
    {
        public String Phone { get; set; }

        [Display(Name = "Phone 2")]
        public String Phone2 { get; set; }
        [EmailAddress]
        public String Email { get; set; }
        public String Address { get; set; }
        public String Country { get; set; }
        public String City { get; set; }
        public String District { get; set; }

        public void SetInfo(UserModel user)
        {
            if (user != null)
            {
                Phone = user.Phone;
                Phone2 = user.Phone2;
                Email = user.Email;
                Address = user.Address;
                Country = user.Country;
                City = user.City;
                District = user.District;
            }
        }
    }
}

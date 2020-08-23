using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Classes.Models
{
    public class UserInfo
    {
        public String Photo { get; set; }
        public String NIC { get; set; }
        [Display(Name = "First Name")]
        public String FirstName { get; set; }
        [Display(Name = "Last Name")]
        public String LastName { get; set; }
        public String Gender { get; set; }

        public void SetInfo(UserModel user)
        {
            if (user != null)
            {
                Photo = user.Photo;
                NIC = user.NIC;
                FirstName = user.FirstName;
                LastName = user.LastName;
                Gender = user.Gender;
            }
        }
    }
}

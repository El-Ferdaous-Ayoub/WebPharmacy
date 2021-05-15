using Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class Contact
    {
        public int ID { get; set; }
        public String Phone { get; set; }
        public String Phone2 { get; set; }
        public String Email { get; set; }
        public String Address { get; set; }

        public void SetInfo(Employee user)
        {
            throw new NotImplementedException();
        }

        public void SetToEmployee(Employee user)
        {
            user.Phone = this.Phone;
            user.Phone2 = this.Phone2;
            user.Email = this.Email;
            user.Address = this.Address;
        }
    }
}
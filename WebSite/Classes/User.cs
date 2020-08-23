using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Classes.Models;

namespace Classes
{
    [Table("Users")]
    public class User
    {
        [Key]
        public String UserName { get; set; }
        public String Password { get; set; }
        public String NIC { get; set; }
        public String Photo { get; set; }
        public String Rank { get; set; } // Employee OR Customer
        [Display(Name = "First Name")]
        public String FirstName { get; set; }
        [Display(Name = "Last Name")]
        public String LastName { get; set; }
        public String Gender { get; set; }
        public String Phone { get; set; }
        [Display(Name = "Phone 2")]
        public String Phone2 { get; set; }
        [EmailAddress]
        public String Email { get; set; }
        public String Address { get; set; }
        public String Country { get; set; }
        public String City { get; set; }
        public String District { get; set; }
        [CreditCard]
        [Display(Name = "Card Number")]
        public String CreditCardN { get; set; }

        public virtual Credit_Card Credit_Card { get; set; }

        public User GetUser()
        {
            return new User()
            {
                UserName = UserName,
                Password = Password,
                NIC = NIC,
                Photo = Photo,
                FirstName = FirstName,
                LastName = LastName,
                Gender = Gender,
                Phone = Phone,
                Address = Address,
                Country = Country,
                City = City,
                District = District,
                Email = Email,
                Rank = Rank,
                CreditCardN = CreditCardN
            };
        }

        public void SetInfo(UserInfo Info)
        {
            if (Info != null)
            {
                Phone = Info.Photo;
                NIC = Info.NIC;
                FirstName = Info.FirstName;
                LastName = Info.LastName;
                Gender = Info.Gender;
            }
        }

        public void SetContact(Contact contact)
        {
            if (contact != null)
            {
                Phone = contact.Phone;
                Phone2 = contact.Phone2;
                Email = contact.Email;
                Address = contact.Address;
                Country = contact.Country;
                City = contact.City;
                District = contact.District;
            }
        }
    }
}

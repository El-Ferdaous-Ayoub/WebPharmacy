using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Classes.Models
{
    public class UserModel
    {
        [Required,Key]
        public String UserName { get; set; }
        public String Password { get; set; }
        public String NIC { get; set; }
        public String Photo { get; set; }
        public String Rank { get; set; } 
        [Display(Name = "First Name")]
        public String FirstName { get; set; }
        [Display(Name = "Last Name")]
        public String LastName { get; set; }
        public String Gender { get; set; }

        [Display(Name = "Phone")]
        public String Phone { get; set; }
        [EmailAddress]
        public String Email { get; set; }
        public String Address { get; set; }
        public String Country { get; set; }
        public String City { get; set; }
        public String District { get; set; }
        [Display(Name = "Card Number")]
        public String CreditCardN { get; set; }
        [Display(Name = "Departement")]
        public String Departement_Title { get; set; }
        [Display(Name = "Phone 2")]
        public String Phone2 { get; set; }
        [Display(Name = "Health Coverage")]
        public String Health_Coverage { get; set; }
        [Display(Name = "Bank Account")]
        public String BankAccountN { get; set;}

        public void SetUser(User user)
        {
            if (user != null)
            {
                UserName = user.UserName;
                Password = user.Password;
                Photo = user.Photo;
                Rank = user.Rank;
                NIC = user.NIC;
                FirstName = user.FirstName;
                LastName = user.LastName;
                Gender = user.Gender;
                Phone = user.Phone;
                Phone2 = user.Phone2;
                Email = user.Email;
                Address = user.Address;
                City = user.City;
                District = user.District;
                Country = user.Country;

            }
        }

        public void SetEmployee(Employee employee)
        {
            if (employee != null)
            {
                Departement_Title = employee.Department_Title;
                Health_Coverage = employee.Health_Coverage;
                BankAccountN = employee.BankAccountN;
            }
        }

        public User GetUser()
        {
            return new User() {
                UserName = UserName,
                Password = Password,
                Photo = Photo,
                NIC = NIC,
                Rank = Rank,
                FirstName = FirstName,
                LastName = LastName,
                Gender = Gender,
                Phone = Phone,
                Phone2 = Phone2,
                Email = Email,
                Address = Address,
                City = City,
                District = District,
                Country = Country,
            };
        }

        public Employee GetEmployee()
        {
            return new Employee() {
                ID = NIC,
                Department_Title = Departement_Title,
                Health_Coverage = Health_Coverage,
                UserName = UserName,
                BankAccountN = BankAccountN
            };
        }
    }

   
}
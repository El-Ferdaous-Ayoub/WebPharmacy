using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Classes.Models;

namespace Classes
{
    [Table("Users")]
    public class Employee
    {
        [Key]
        public int ID { get; set; }
        public String NIC { get; set; }
        public String Password { get; set; }
        public String Picture { get; set; }
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
        [Display(Name = "Department")]
        public int Department_ID { get; set; }
        [Display(Name = "Bank Account")]
        public String BankAccountN { get; set; }

        [ForeignKey("Department_ID")]
        public virtual Department Department { get; set; }

    }
}

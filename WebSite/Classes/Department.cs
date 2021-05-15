using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Classes
{
    [Table("Departements")]
    public class Department
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Department")]
        public String Title { get; set; }
        [Display(Name = "Time Work")]
        public String Time_Work { get; set; }
        //Roles
        [Display(Name = "Pharmacy Info")]
        public Boolean Pharmacy_Info { get; set; }
        public Boolean Categories { get; set; }
        public Boolean Producers { get; set; }
        public Boolean Products { get; set; }
        public Boolean Orders { get; set; }
        public Boolean Departments { get; set; }
        public Boolean Contracts { get; set; }
        public Boolean Employees { get; set; }

        public virtual ICollection<Employee> Emps { get; set; }
    }
}
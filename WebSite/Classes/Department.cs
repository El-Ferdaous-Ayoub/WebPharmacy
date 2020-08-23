using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Classes
{
    [Table("Departements")]
    public class Department
    {
        [Key,Required]
        [Display(Name = "Department")]
        public String Title { get; set; }
        [Display(Name = "Time Work")]
        public String Time_Work { get; set; }
        public float Salary { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
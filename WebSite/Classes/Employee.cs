using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Classes
{
    [Table("Employees")]
    public class Employee
    {
        [Key, Required]
        public String ID { get; set; }
        public String UserName { get; set; }
        [Display(Name = "Department")]
        public String Department_Title { get; set; }
        [Display(Name = "Health Coverage")]
        public String Health_Coverage { get; set; }
        [Display(Name = "Bank Account")]
        public String BankAccountN { get; set; }

        public virtual Department Department { get; set; }
        public virtual User User { get; set; }

        public void SetEmployee(Employee employee)
        {
            if (employee != null)
            {
                ID = employee.ID;
                UserName = employee.UserName;
                Department_Title = employee.Department_Title;
                Health_Coverage = employee.Health_Coverage;
                BankAccountN = employee.BankAccountN;
            }
        }

        public Employee GetEmployee()
        {
            return new Employee()
            {
                ID = ID,
                UserName = UserName,
                Department_Title = Department_Title,
                Health_Coverage = Health_Coverage,
                BankAccountN = BankAccountN
            };
        }
    }
}

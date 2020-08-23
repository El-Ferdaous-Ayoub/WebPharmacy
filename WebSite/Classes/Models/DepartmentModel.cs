using Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Classes.Models
{
    public class DepartmentModel
    {
        [Key, Required]
        [Display(Name = "Department")]
        public String Title { get; set; }
        [Display(Name = "Time Work")]
        public String Time_Work { get; set; }
        public float Salary { get; set; }
        [Display(Name = "Pharmacy Info")]
        public Boolean Pharmacy_Info { get; set; }
        public Boolean Categories { get; set; } = false;
        public Boolean Producers { get; set; } = false;
        public Boolean Products { get; set; } = false;
        public Boolean Orders { get; set; } = false;
        public Boolean Departments { get; set; } = false;
        public Boolean ContractTypes { get; set; } = false;
        public Boolean Employees { get; set; } = false;


        public Department GetDepartment()
        {
            return new Department() { Title = Title, Time_Work = Time_Work, Salary = Salary };
        }

        public Role GetRole()
        {
            return new Role()
            {
                Department_Title = Title,
                Pharmacy_Info = Pharmacy_Info,
                Categories = Categories,
                Producers = Producers,
                Products = Products,
                Orders = Orders,
                Departments = Departments,
                ContractTypes = ContractTypes,
                Employees = Employees,
            };
        }

        public void SetDepartment(Department department)
        {
            Title = department.Title;
            Time_Work = department.Time_Work;
            Salary = department.Salary;
        }

        public void SetRole(Role role)
        {
            if (role != null)
            {
                Pharmacy_Info = role.Pharmacy_Info;
                Categories = role.Categories;
                Producers = role.Producers;
                Products = role.Products;
                Orders = role.Orders;
                Departments = role.Departments;
                ContractTypes = role.ContractTypes;
                Employees = role.Employees;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BAL;
using Classes;
using Classes.Models;

namespace WebSite.Models
{
    public class Config :Controller
    {
        public static Pharmacy Pharmacy;
        public static int DataExist { get; set; } = 2;

        public static void ConfigTables()
        {
            if (new MCategories().GetUNCategory() == null)
                new MCategories().Add(new Category() { Name = "Unknown" });
            if (new MProducers().GetUNProducer() == null)
                new MProducers().Add(new Producer() { Name = "Unknown" });
            if (new MDepartments().GetUNDepartment() == null)
                new MDepartments().Add(new Department() { Title = "Unknown" });
        }

        public static void CheckData()
        {
            Pharmacy = new Pharmacy_Info().Get();
            if (Pharmacy == null)
                DataExist = 2;
            else if (new MEmployees().Get_All().Count == 0)
            {
                Department department = new Department() { Title = "ADMIN" };
                if (new MDepartments().Get(title: department.Title) == null)
                {
                    foreach (System.Reflection.PropertyInfo prop in typeof(Department).GetProperties())
                        if (prop.PropertyType == typeof(Boolean))
                            prop.SetValue(department, true);
                    new MDepartments().Add(department);
                }
                DataExist = 1;
            }
            else DataExist = 0;
        }

        public static void LogOut()
        {
            Account.User = null;
            Account.Department = null;
        }   

        public static void LogIn(Employee user,Department department)
        {
            if (user != null && department != null)
            {
                Account.User = user;
                Account.Department = department;
            }
        }
    }
}
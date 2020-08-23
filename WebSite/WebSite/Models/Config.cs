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
            if (new MCategories().Get("Unknown") == null)
                new MCategories().Add(new Category() { Name = "Unknown" });
            if (new MProducers().Get("Unknown") == null)
                new MProducers().Add(new Producer() { Name = "Unknown" });
            if (new MDepartments().Get("Unknown") == null)
                new MDepartments().Add(new Department() { Title = "Unknown" });
            if (new MContract_Types().Get("Unknown") == null)
                new MContract_Types().Add(new Contract_Type() { Title = "Unknown" });
        }

        public static void CheckData()
        {
            Pharmacy = new Pharmacy_Info().Get();
            if (Pharmacy == null)
                DataExist = 2;
            else if (new MUsers().GetUEmployees().Count == 0) {
                Department department = new Department() { Title = "ADMIN" };
                if(new MDepartments().Get(department.Title) == null)
                new MDepartments().Add(department);
                    Role role = new Role() { Department_Title = "ADMIN" };
                foreach (System.Reflection.PropertyInfo prop in typeof(Role).GetProperties())
                    if (prop.PropertyType == typeof(Boolean))
                        prop.SetValue(role, true);
                if (new MDepartments().GetRole(department.Title) == null)
                    
                new MDepartments().AddRole(role);
                DataExist = 1;
            }
            else DataExist = 0;
        }

        public static void LogOut()
        {
            Account.User = null;
            Account.Department = null;
        }   

        public static void LogIn(UserModel user,DepartmentModel department)
        {
            if (user != null && department != null)
            {
                Account.User = user;
                   Account.Department = department;
            }
        }
    }
}
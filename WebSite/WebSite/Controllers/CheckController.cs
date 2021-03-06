﻿using BAL;
using Classes;
using Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class CheckController : Controller
    {
        public  ActionResult CheckStatus(String Access = "")
        {
            Config.CheckData();
            if (Config.DataExist == 2)
                return RedirectToRoute(new { controller = "Pharmacy", action = "Create",t = 1 });
            else if (Config.DataExist == 1)
                return RedirectToRoute(new { controller = "Employees", action = "Create",t=1 });
            else if (Account.User == null || Account.Department == null)
                return RedirectToRoute(new { controller = "Account", action = "Login" });
            else
            {
                try
                {
                    Employee user = new MAccounts().Login(Account.User.NIC, Account.User.Password);
                    if (user == null || String.IsNullOrEmpty(user.NIC)) throw new Exception();
                    Employee employee = new MEmployees().Get(user.ID);
                    if (employee == null) throw new Exception();
                    Department department = new MDepartments().Get(employee.Department_ID);
                    if (department == null) throw new Exception();
                    Config.LogIn(employee, department);
                }
                catch
                {
                    return RedirectToRoute(new { controller = "Account", action = "Logout" });
                }   
                foreach (System.Reflection.PropertyInfo prop in typeof(Department).GetProperties())
                if (prop.PropertyType == typeof(Boolean) && (Boolean)prop.GetValue(Account.Department))
                    return null;
            }
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
    }
}

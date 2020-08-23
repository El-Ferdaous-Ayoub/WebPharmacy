using BAL;
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
                    User user = new MUsers().Login(Account.User.UserName, Account.User.Password);
                    if (user == null || String.IsNullOrEmpty(user.NIC)) throw new Exception();
                    Employee employee = new MEmployees().Get(user.NIC);
                    if (employee == null) throw new Exception();
                    if (String.IsNullOrEmpty(employee.Department_Title)) throw new Exception();
                    Department department = new MDepartments().Get(employee.Department_Title);
                    if (department == null) throw new Exception();
                    Role role = new MDepartments().GetRole(employee.Department_Title);
                    if (role == null) throw new Exception();
                    UserModel userModel = new UserModel();
                    userModel.SetUser(user);
                    userModel.SetEmployee(employee);
                    DepartmentModel departmentModel = new DepartmentModel();
                    departmentModel.SetDepartment(department);
                    departmentModel.SetRole(role);
                    Config.LogIn(userModel, departmentModel);
                }
                catch
                {
                    return RedirectToRoute(new { controller = "Account", action = "Logout" });
                }   
                foreach (System.Reflection.PropertyInfo prop in typeof(DepartmentModel).GetProperties())
                if (prop.PropertyType == typeof(Boolean) && prop.Name.CompareTo(Access) == 0 && (Boolean)prop.GetValue(Account.Department))
                    return null;
            }
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
    }
}

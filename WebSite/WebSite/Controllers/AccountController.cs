using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Classes;
using BAL;
using Classes.Models;
using WebSite.Models;
using System.Text.RegularExpressions;
using System.Data.Entity.Validation;

namespace WebSite.Controllers
{
    public class AccountController : Controller
    {
        private MEmployees edb = new MEmployees();
        private MUsers udb = new MUsers();

        public ActionResult Login()
        {
            if (Account.User != null)
                return RedirectToRoute(new {controller = "Home",action="Index" });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel login)
        {
            if (Account.User != null)
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            try
            {
                User user = new MUsers().Login(login.UserName, login.Password);
                if (user == null) throw new Exception("UserName Or Password Is Incorrect");
                if (String.IsNullOrEmpty(user.NIC)) throw new Exception("NIC of User does not exist");
                Employee employee = new MEmployees().Get(user.NIC);
                if (employee == null) throw new Exception("This User does not have the access");
                if (String.IsNullOrEmpty(employee.Department_Title)) throw new Exception("This User does not have an access");
                Department department = new MDepartments().Get(employee.Department_Title);
                if (department == null) throw new Exception("This User does not have the access");
                Role role = new MDepartments().GetRole(employee.Department_Title);
                if (role == null) throw new Exception("This User does not have the access");
                UserModel userModel = new UserModel();
                userModel.SetUser(user);
                userModel.SetEmployee(employee);
                DepartmentModel departmentModel = new DepartmentModel();
                departmentModel.SetDepartment(department);
                departmentModel.SetRole(role);
                Config.LogIn(userModel,departmentModel);
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(login);
        }

        public ActionResult Logout()
        {
            Config.LogOut();
            return RedirectToAction("Login");
        }

        public ActionResult UserProfile()
        {
            if (Account.User == null)
                return RedirectToAction("Login");
            return View(Account.User);
        }

        public ActionResult EditUserName()
        {
            if (Account.User == null)
                return RedirectToAction("Login");
            UserName username = new UserName() {Username = Account.User.UserName };
            return View(username);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserName(String NewUsername)
        {
            if (Account.User == null)
                return RedirectToAction("Logout");
            try
            { 

                User user = udb.Get(Account.User.UserName);
                if (user == null)
                    return RedirectToAction("Logout");
                if (ModelState.IsValid)
                {
                    User newuser = user.GetUser();
                    newuser.UserName = NewUsername;
                    udb.Add(newuser);
                    udb.NewUserName(user.UserName, NewUsername);
                    user = udb.Get(NewUsername);
                    Account.User.SetUser(user);
                    Employee employee = edb.Get(user.NIC);
                    if(employee != null)
                    Account.User.SetEmployee(employee);
                    return RedirectToAction("UserProfile");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
            }
            return RedirectToAction("EditUserName");
        }

        public ActionResult ChangePassword()
        {
            if (Account.User == null)
                return RedirectToAction("Login");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(String Password,String NewPassword)
        {
            if (Account.User == null)
                return RedirectToAction("Login");
            try
            {
                User user = udb.Get(Account.User.UserName);
                if (user == null)
                    return RedirectToAction("Login");
                if (Password.CompareTo(user.Password) != 0)
                    throw new Exception("Password Is Incorrect");
                Regex regex = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");
                if(!regex.IsMatch(NewPassword))
                    throw new Exception("Password is not sufficiently complex");
                user.Password = NewPassword;
                    udb.Update(user);
                Account.User.SetUser(udb.Get(user.UserName));
                return RedirectToAction("UserProfile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        public ActionResult EditPersonalInfo()
        {
            if (Account.User == null)
                return RedirectToAction("Login");
            UserInfo info = new UserInfo();
            info.SetInfo(Account.User);
            return View(info);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPersonalInfo(UserInfo info, HttpPostedFileBase ImageUrl)
        {
            if (Account.User == null)
                return RedirectToAction("Login");
            try
            {
                User user = udb.Get(Account.User.UserName);
                if (user == null)
                    return RedirectToAction("Login");

                if (Account.Department != null && user.NIC.CompareTo(info.NIC) != 0)
                {
                    Employee employee = edb.Get(user.NIC);
                    if (employee != null)
                    {
                        Employee newnic = employee.GetEmployee();
                        newnic.ID = info.NIC;
                        edb.NewNIC(user.NIC, newnic);
                    }
                }
                User up_user = user.GetUser();
                up_user.SetInfo(info);
                if (ImageUrl != null)
                {
                    string ImageName = System.IO.Path.GetFileName(ImageUrl.FileName);
                    string physicalPath = Server.MapPath("~/Content/Images/Users/" + user.UserName + ImageName);
                    ImageUrl.SaveAs(physicalPath);
                    up_user.Photo = user.UserName + ImageName;
                }
                udb.Update(up_user);
                Account.User.SetUser(udb.Get(up_user.UserName));
                return RedirectToAction("UserProfile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(info);
        }

        public ActionResult EditContact()
        {
            if (Account.User == null)
                return RedirectToAction("Login");
            Contact contact = new Contact();
            contact.SetInfo(Account.User);
            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditContact(Contact contact)
        {
            if (Account.User == null)
                return RedirectToAction("Login");
            try
            {
                User user = udb.Get(Account.User.UserName);
                if (user == null)
                    return RedirectToAction("Login");
                
                    user.SetContact(contact);
                    udb.Update(user);
                    Account.User.SetUser(udb.Get(user.UserName));
                    return RedirectToAction("UserProfile");
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(contact);
        }
    }
}

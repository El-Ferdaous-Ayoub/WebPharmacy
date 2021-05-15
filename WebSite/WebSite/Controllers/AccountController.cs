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
using System.IO;

namespace WebSite.Controllers
{
    public class AccountController : Controller
    {
        private MEmployees db = new MEmployees();

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
                Employee user = new MAccounts().Login(login.UserName, login.Password);
                if (user == null) throw new Exception("Username Or Password Is Incorrect");
                Department department = new MDepartments().Get(user.Department_ID);
                if (department == null) throw new Exception("You doesn't have the access");
                Config.LogIn(user,department);
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
            Employee em = db.GetByNIC(Account.User.NIC);
            em.Department = new MDepartments().Get(em.Department_ID) ;
            return View(em);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfile(Employee user, HttpPostedFileBase ImageUrl)
        {
            if (Account.User == null)
                return RedirectToAction("Login");
            try
            {
                
                if (db.GetByNIC(Account.User.NIC) == null)
                    return RedirectToAction("Login");

                if (ImageUrl != null)
                {
                    if (System.IO.File.Exists($"~/Content/Images/Users/{user.Picture}"))
                        System.IO.File.Delete($"~/Content/Images/Users/{user.Picture}");

                    var str = "";
                    string ImageName = Path.GetFileName(ImageUrl.FileName);
                    do
                    {
                        str = GetName();
                    } while (System.IO.File.Exists($"~/Content/Images/Users/{str}{ImageName}"));
                    string physicalPath = Server.MapPath($"~/Content/Images/Users/{str}{ImageName}");
                    ImageUrl.SaveAs(physicalPath);
                    user.Picture = $"{str}{ImageName}";
                }
                db.Update(user);
                Account.User = new Employee();
                Account.User = db.GetByNIC(user.NIC);
                return RedirectToAction("UserProfile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(user);
        }

        public ActionResult ChangePassword()
        {
            if (Account.User == null)
                return RedirectToAction("Login");
            return View(new PassWord());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(PassWord passWord)
        {
            if (Account.User == null)
                return RedirectToAction("Login");
            try
            {
                Employee user = db.Get(Account.User.ID);
                if (user == null)
                    return RedirectToAction("Login");
                if (Account.User.Password.CompareTo(passWord.Password) != 0)
                    throw new Exception("Password Is Incorrect");
                Regex regex = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");
                if(!regex.IsMatch(passWord.NewPassword))
                    throw new Exception("Password is not sufficiently complex");
                user.Password = passWord.NewPassword;
                    db.Update(user);
                Account.User.Password = passWord.NewPassword;
                return RedirectToAction("UserProfile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        public String GetName()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 20)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

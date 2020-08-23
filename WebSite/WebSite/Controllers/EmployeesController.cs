using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Classes;
using Classes.Models;
using BAL;
using System.Net;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class EmployeesController : Controller
    {
        private MEmployees db = new MEmployees();
        private MUsers udb = new MUsers();

        // GET: Employees
        public ActionResult Index(String Error = "")
        {
            var action = new CheckController().CheckStatus("Employees");
            if (action != null) return action;
            ViewBag.Error = Error;
            return View(udb.GetUEmployees());
        }

        // GET: Employees/Details/5
        public ActionResult More(String id)
        {
            var action = new CheckController().CheckStatus("Employees");
            if (action != null) return action;
            new CheckController().CheckStatus("Employees");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Get(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            UserModel userModel = new UserModel();
            userModel.SetEmployee(employee);
            userModel.SetUser(udb.GetByNIC(id));
            return View(userModel);
        }

        // GET: Employees/Create
        public ActionResult Create(int t = 0)
        {
            if (t == 0)
            {
                var action = new CheckController().CheckStatus("Employees");
                if (action != null) return action;
            }
            ViewBag.Departement_Title = new SelectList(new MDepartments().Get_All()
                .Where(i => i.Title.CompareTo("Unknown") != 0).ToList(), "Title", "Title");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserModel userModel, HttpPostedFileBase ImageUrl)
        {
            if (udb.GetUEmployees().Count != 0)
            {
                var action = new CheckController().CheckStatus("Employees");
                if (action != null) return action;
            }
            Config.CheckData();
            try
            {
                if (String.IsNullOrEmpty(userModel.NIC) ||
                 String.IsNullOrWhiteSpace(userModel.NIC))
                    throw new Exception($"Cannot Create Employee By Empty NIC");

                if (String.IsNullOrEmpty(userModel.Departement_Title) ||
                    String.IsNullOrWhiteSpace(userModel.Departement_Title) ||
                    userModel.Departement_Title.CompareTo("Unknown") == 0)
                    throw new Exception($"Cannot Create Employee By Empty Or '{userModel.Departement_Title}' Department");

                userModel.NIC = userModel.NIC.ToUpper();
                Employee employee = userModel.GetEmployee();
                employee.UserName = userModel.NIC;
                User user = userModel.GetUser();
                user.UserName = userModel.NIC;
                user.Password = userModel.NIC;
                user.Rank = "Employee";

                if (ImageUrl != null)
                {
                    string ImageName = System.IO.Path.GetFileName(ImageUrl.FileName);
                    string physicalPath = Server.MapPath("~/Content/Images/Users/" + employee.ID + ImageName);
                    ImageUrl.SaveAs(physicalPath);
                    user.Photo = employee.ID + ImageName;
                }
                udb.Add(user);
                db.Add(employee);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            ViewBag.Departement_Title = new SelectList(new MDepartments().Get_All()
                .Where(i => i.Title.CompareTo("Unknown") != 0).ToList(), "Title", "Title", userModel.Departement_Title);
            return View(userModel);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(String id)
        {
            var action = new CheckController().CheckStatus("Employees");
            if (action != null) return action;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Get(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.Departement_Title = new SelectList(new MDepartments().Get_All()
                .Where(i => i.Title.CompareTo("Unknown") != 0).ToList(), "Title", "Title", employee.Department_Title);
            UserModel userModel = new UserModel();
            userModel.SetEmployee(employee);
            userModel.SetUser(udb.GetByNIC(id));
            return View(userModel);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserModel userModel, HttpPostedFileBase ImageUrl, String NewNIC)
        {
            var action = new CheckController().CheckStatus("Employees");
            if (action != null) return action;
            try
            {
                if (ModelState.IsValid)
                {
                    if (String.IsNullOrEmpty(userModel.Departement_Title) ||
                        String.IsNullOrWhiteSpace(userModel.Departement_Title)||
                        userModel.Departement_Title.CompareTo("Unknown") == 0)
                        throw new Exception($"Cannot Update Department's Employee To Empty Or '{userModel.Departement_Title}' Department");
                   
                    Employee employee = userModel.GetEmployee();
                    User user = userModel.GetUser();

                    String orgNIC = employee.ID;

                    if (!String.IsNullOrEmpty(userModel.NIC) && userModel.NIC.CompareTo(NewNIC) != 0)
                    {
                        if (String.IsNullOrWhiteSpace(userModel.NIC))
                        throw new Exception($"Cannot Create Employee By Empty NIC");

                        employee.ID = NewNIC.ToUpper();
                        user.NIC = employee.ID;
                        db.NewNIC(orgNIC, employee);
                        udb.Update(user);
                    }
                    else
                        db.Update(employee);

                    if (ImageUrl != null)
                    {
                        string ImageName = System.IO.Path.GetFileName(ImageUrl.FileName);
                        string physicalPath = Server.MapPath("~/Content/Images/Users/" + employee.ID + ImageName);
                        ImageUrl.SaveAs(physicalPath);
                        user.Photo = employee.ID + ImageName;
                    }

                    udb.Update(user);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            ViewBag.Departement_Title = new SelectList(new MDepartments().Get_All()
                .Where(i => i.Title.CompareTo("Unknown") != 0).ToList(), "Title", "Title", userModel.Departement_Title);
            return View(userModel);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(String ID)
        {
            var action = new CheckController().CheckStatus("Employees");
            if (action != null) return action;
            try
            {
                db.Remove(ID);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToRoute(new {action = "Index", Error = ex.Message });
            }
        }
    }
}

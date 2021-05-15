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
using System.IO;

namespace WebSite.Controllers
{
    public class EmployeesController : Controller
    {
        private MEmployees db = new MEmployees();
        private MDepartments ddb = new MDepartments();
        // GET: Employees
        public ActionResult Index(String Error = "")
        {
            var action = new CheckController().CheckStatus("Employees");
            if (action != null) return action;
            ViewBag.Error = Error;
            var list = db.Get_All();
            foreach (var item in list)
            item.Department = new MDepartments().Get(item.Department_ID);
            return View(list);
        }

        // GET: Employees/Details/5
        public ActionResult More(int id)
        {
            var action = new CheckController().CheckStatus("Employees");
            if (action != null) return action;
            new CheckController().CheckStatus("Employees");
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Employee employee = db.Get(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            employee.Department = new MDepartments().Get(employee.Department_ID);
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create(int t = 0)
        {
            if (t == 0)
            {
                var action = new CheckController().CheckStatus("Employees");
                if (action != null) return action;
            }
            ViewBag.Department_ID = new SelectList(new MDepartments().Get_All()
                .Where(i => i.Title.CompareTo("Unknown") != 0).ToList(), "ID", "Title");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee, HttpPostedFileBase ImageUrl)
        {
            if (db.Get_All().Count != 0)
            {
                var action = new CheckController().CheckStatus("Employees");
                if (action != null) return action;
            }
            Config.CheckData();
            try
            {
                if (String.IsNullOrEmpty(employee.NIC) ||
                 String.IsNullOrWhiteSpace(employee.NIC))
                    throw new Exception($"Cannot Create Employee By Empty NIC");

                if (ddb.GetUNDepartment().ID == employee.Department_ID )
                    throw new Exception($"Cannot Create Employee By Empty Or 'Uknown' Department");

                employee.NIC.ToUpper();

                if (ImageUrl != null)
                {
                    if (System.IO.File.Exists($"~/Content/Images/Users/{employee.Picture}"))
                        System.IO.File.Delete($"~/Content/Images/Users/{employee.Picture}");

                    var str = "";
                    string ImageName = Path.GetFileName(ImageUrl.FileName);
                    do
                    {
                        str = GetName();
                    } while (System.IO.File.Exists($"~/Content/Images/Users/{str}{ImageName}"));
                    string physicalPath = Server.MapPath($"~/Content/Images/Users/{str}{ImageName}");
                    ImageUrl.SaveAs(physicalPath);
                    employee.Picture = $"{str}{ImageName}";
                }
                employee.Password = employee.NIC;
                db.Add(employee);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            ViewBag.Department_ID = new SelectList(new MDepartments().Get_All()
                .Where(i => i.Title.CompareTo("Unknown") != 0).ToList(), "ID", "Title", employee.Department_ID);
            return View(employee);
        }

        public String GetName()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 20)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int id)
        {
            var action = new CheckController().CheckStatus("Employees");
            if (action != null) return action;
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Employee employee = db.Get(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.Department_ID = new SelectList(new MDepartments().Get_All()
                .Where(i => i.Title.CompareTo("Unknown") != 0).ToList(), "ID", "Title", employee.Department_ID);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee, HttpPostedFileBase ImageUrl)
        {
            var action = new CheckController().CheckStatus("Employees");
            if (action != null) return action;
            try
            {
                if (ModelState.IsValid)
                {
                    if (ddb.GetUNDepartment().ID == employee.Department_ID)
                        throw new Exception($"Cannot Update Department's Employee To Empty Or 'Uknown' Department");

                    if (ImageUrl != null)
                    {
                        if (System.IO.File.Exists($"~/Content/Images/Users/{employee.Picture}"))
                            System.IO.File.Delete($"~/Content/Images/Users/{employee.Picture}");

                        var str = "";
                        string ImageName = Path.GetFileName(ImageUrl.FileName);
                        do
                        {
                            str = GetName();
                        } while (System.IO.File.Exists($"~/Content/Images/Users/{str}{ImageName}"));
                        string physicalPath = Server.MapPath($"~/Content/Images/Users/{str}{ImageName}");
                        ImageUrl.SaveAs(physicalPath);
                        employee.Picture = $"{str}{ImageName}";
                    }
                    db.Update(employee);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            ViewBag.Department_ID = new SelectList(new MDepartments().Get_All()
                .Where(i => i.Title.CompareTo("Unknown") != 0).ToList(), "ID", "Title", employee.Department_ID);
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int ID)
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

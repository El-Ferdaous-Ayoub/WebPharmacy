using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Classes;
using BAL;
using System.Net;
using System.Reflection;
using Classes.Models;

namespace WebSite.Controllers
{
    public class DepartmentsController : Controller
    {
        private MDepartments db = new MDepartments();

        // GET: Departements
        public ActionResult Index(String Error = "")
        {
            var action = new CheckController().CheckStatus("Departments");
            if (action != null) return action;
            ViewBag.Error = Error;
            return View(db.Get_All());
        }

        // GET: Departements/Create
        public ActionResult Create()
        {
            var action = new CheckController().CheckStatus("Departments");
            if (action != null) return action;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department DM)
        {
            var action = new CheckController().CheckStatus("Departments");
            if (action != null) return action;
            try
            {
                if (ModelState.IsValid)
                {
                    if (String.IsNullOrEmpty(DM.Title) ||
                        String.IsNullOrWhiteSpace(DM.Title))
                        throw new Exception($"Cannot Create Department By Empty Title");
                    db.Add(DM);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(DM);
        }

        // GET: Departements/Edit/5
        public ActionResult Edit(int id)
        {
            var action = new CheckController().CheckStatus("Departments");
            if (action != null) return action;
            if (db.GetUNDepartment().ID == id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department departement = db.Get(id);
            if (departement == null)
            {
                return HttpNotFound();
            }
            return View(departement);
        }

        // POST: Departements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Department department)
        {
            var action = new CheckController().CheckStatus("Departments");
            if (action != null) return action;
            try
            {
                if (ModelState.IsValid)
                {
                    if (String.IsNullOrWhiteSpace(department.Title))
                        throw new Exception($"Cannot Update Department To Empty Title");
                    else db.Update(department);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(department);
        }

        // GET: Departements/Delete/5
        public ActionResult Delete(int id)
        {
            var action = new CheckController().CheckStatus("Departments");
            if (action != null) return action;
            if (db.GetUNDepartment().ID == id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department departement = db.Get(id);
            if (departement == null)
            {
                return HttpNotFound();
            }
            return View(departement);
        }

        // POST: Departements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var action = new CheckController().CheckStatus("Departments");
            if (action != null) return action;
            try
            {
                if (db.GetUNDepartment().ID == id) throw new Exception("Not authorized to delete");
                db.Remove(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToRoute(new { controller = "Departments", action = "Index", Error = ex.Message });
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BAL;
using Classes;

namespace WebSite.Controllers
{
    public class CategoriesController : Controller
    {
        MCategories db = new MCategories();

        // GET: Categories
        public ActionResult Index(String Error = "")
        {
            var action = new CheckController().CheckStatus("Categories");
            if (action != null) return action;
            ViewBag.Error = Error;
            return View(db.Get_All());
        }


        // GET: Categories/Create
        public ActionResult Create()
        {
            var action = new CheckController().CheckStatus("Categories");
            if (action != null) return action;
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            var action = new CheckController().CheckStatus("Categories");
            if (action != null) return action;
            try
            {
                if (ModelState.IsValid)
                {
                    if (String.IsNullOrEmpty(category.Name) ||
               String.IsNullOrWhiteSpace(category.Name))
                        throw new Exception($"Cannot Create Category By Empty Name");
                    db.Add(category);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(String id)
        {
            var action = new CheckController().CheckStatus("Categories");
            if (action != null) return action;
            if (id == null || id.CompareTo("Unknown") == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category categorie = db.Get(id);
            if (categorie == null)
            {
                return HttpNotFound();
            }
            return View(categorie);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name")] Category category,String NewName)
        {
            var action = new CheckController().CheckStatus("Categories");
            if (action != null) return action;
            try
            {
                if (ModelState.IsValid)
                {
                    if (String.IsNullOrEmpty(category.Name) ||
               String.IsNullOrWhiteSpace(category.Name))
                        throw new Exception($"Cannot Update Category To Empty Name");

                    db.NewName(category.Name, new Category() { Name = NewName });
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(String id)
        {
            var action = new CheckController().CheckStatus("Categories");
            if (action != null) return action;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category categorie = db.Get(id);
            if (categorie == null)
            {
                return HttpNotFound();
            }
            ViewBag.Products = new MProducts().Get_All().Where(item =>
                item.Category_Name.CompareTo(id) == 0).ToList();
            return View(categorie);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(String id)
        {
            var action = new CheckController().CheckStatus("Categories");
            if (action != null) return action;
            try
            {
                if (id.CompareTo("Unknown") == 0) throw new Exception("Not authorized to delete");
                db.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToRoute(new { controller = "Categories", action = "Index", Error = ex.Message });
            }
        }
    }
}
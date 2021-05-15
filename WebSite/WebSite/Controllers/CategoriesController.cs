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
        public ActionResult Edit(int id)
        {
            var action = new CheckController().CheckStatus("Categories");
            if (action != null) return action;
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
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
        public ActionResult Edit([Bind(Include = "ID,Name")] Category category)
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

                    db.Update(category);
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
        public ActionResult Delete(int id)
        {
            var action = new CheckController().CheckStatus("Categories");
            if (action != null) return action;
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Category categorie = db.Get(id);
            if (categorie == null)
            {
                return HttpNotFound();
            }
            var products = new MProducts().Get_All().Where(item =>
                item.Category_ID == id).ToList();

            foreach (var item in products)
                item.Producer = new MProducers().Get(item.Producer_ID);

            ViewBag.Products = products;
            return View(categorie);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var action = new CheckController().CheckStatus("Categories");
            if (action != null) return action;
            try
            {
                var uc = db.Get_All().Where(i => i.Name.CompareTo("Unknown") == 0).First().ID;
                if (id == uc) throw new Exception("This category cannot be deleted");
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
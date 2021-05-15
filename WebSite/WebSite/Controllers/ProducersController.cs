using BAL;
using Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Controllers
{
    public class ProducersController : Controller
    {
        private MProducers db = new MProducers();

        // GET: Producers
        public ActionResult Index(String Error = "")
        {
            var action = new CheckController().CheckStatus("Producers");
            if (action != null) return action;
            ViewBag.Error = Error;
            return View(db.Get_All());
        }

        // GET: Producers/Details/5
        public ActionResult Details(int id)
        {
            var action = new CheckController().CheckStatus("Producers");
            if (action != null) return action;
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Producer producer = db.Get(id);
            if (producer == null)
            {
                return HttpNotFound();
            }
            return View(producer);
        }

        // GET: Producers/Create
        public ActionResult Create()
        {
            var action = new CheckController().CheckStatus("Producers");
            if (action != null) return action;
            return View();
        }

        // POST: Producers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Address,Email,Phone")] Producer producer)
        {
            var action = new CheckController().CheckStatus("Producers");
            if (action != null) return action;
            try
            {
                if (ModelState.IsValid)
                {
                    if (String.IsNullOrEmpty(producer.Name) ||
                          String.IsNullOrWhiteSpace(producer.Name))
                        throw new Exception($"Cannot Create Producer By Empty Name");
                    db.Add(producer);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(producer);
        }

        // GET: Producers/Edit/5
        public ActionResult Edit(int id)
        {
            var action = new CheckController().CheckStatus("Producers");
            if (action != null) return action;
            if (id == db.GetUNProducer().ID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producer producer = db.Get(id);
            if (producer == null)
            {
                return HttpNotFound();
            }
            return View(producer);
        }

        // POST: Producers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Address,Email,Phone")] Producer producer)
        {
            if (db.GetUNProducer().ID == producer.ID)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var action = new CheckController().CheckStatus("Producers");
            if (action != null) return action;
            try
            {
                if (ModelState.IsValid)
                {
                    if (String.IsNullOrEmpty(producer.Name) ||
                          String.IsNullOrWhiteSpace(producer.Name))
                        throw new Exception($"Cannot Update Producer To Empty Name");
                    db.Update(producer);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return View(producer);
        }

        // GET: Producers/Delete/5
        public ActionResult Delete(int id)
        {
            var action = new CheckController().CheckStatus("Producers");
            if (action != null) return action;
            if (id == db.GetUNProducer().ID)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producer producer = db.Get(id);
            if (producer == null)
            {
                return HttpNotFound();
            }
            var products = new MProducts().Get_All().Where(item =>
                item.Producer_ID == id).ToList();

            foreach (var item in products)
                item.Category = new MCategories().Get(item.Category_ID);

            ViewBag.Products = products;
            return View(producer);
        }

        // POST: Producers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var action = new CheckController().CheckStatus("Producers");
            if (action != null) return action;
            try
            {
                var up = db.Get_All().Where(i => i.Name.CompareTo("Unknown") == 0).First().ID;
                if (id == up) throw new Exception("This producer cannot be deleted");
                db.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToRoute(new { controller = "Producers", action = "Index", Error = ex.Message });
            }
        }
    }
}

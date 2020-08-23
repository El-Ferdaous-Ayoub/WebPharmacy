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
        public ActionResult Details(string id)
        {
            var action = new CheckController().CheckStatus("Producers");
            if (action != null) return action;
            if (id == null)
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
        public ActionResult Create([Bind(Include = "Name,Address,Email,PhoneN")] Producer producer)
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
        public ActionResult Edit(string id)
        {
            var action = new CheckController().CheckStatus("Producers");
            if (action != null) return action;
            if (id == null || id.CompareTo("Unknown") == 0)
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
        public ActionResult Edit([Bind(Include = "Name,Address,Email,PhoneN")] Producer producer, String NewName)
        {
            var action = new CheckController().CheckStatus("Producers");
            if (action != null) return action;
            try
            {
                if (ModelState.IsValid)
                {
                    if (String.IsNullOrEmpty(NewName))
                        db.Update(producer);
                    else
                    {
                        if (String.IsNullOrWhiteSpace(NewName))
                            throw new Exception($"Cannot Update Producer To Empty Name");
                        Producer Nproducer = new Producer()
                        {
                            Name = NewName,
                            Address = producer.Address,
                            Email = producer.Email,
                            PhoneN = producer.PhoneN
                        };
                        db.NewName(producer.Name, Nproducer);
                    }
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
        public ActionResult Delete(string id)
        {
            var action = new CheckController().CheckStatus("Producers");
            if (action != null) return action;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producer producer = db.Get(id);
            if (producer == null)
            {
                return HttpNotFound();
            }
            ViewBag.Products = new MProducts().Get_All().Where(item =>
                item.Producer_Name.CompareTo(id) == 0).ToList();
            return View(producer);
        }

        // POST: Producers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var action = new CheckController().CheckStatus("Producers");
            if (action != null) return action;
            try
            {
                if (id.CompareTo("Unknown") == 0) throw new Exception("Not authorized to delete");
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

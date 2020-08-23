using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Classes;
using BAL;
using System.Net;

namespace WebSite.Controllers
{
    public class ContractTypesController : Controller
    {
        MContract_Types db = new MContract_Types();

        // GET: ContratTypes
        public ActionResult Index(String Error = "")
        {
            var action = new CheckController().CheckStatus("ContractTypes");
            if (action != null) return action;
            ViewBag.Error = Error;
            return View(db.Get_All());
        }

        // GET: ContratTypes/Create
        public ActionResult Create()
        {
            var action = new CheckController().CheckStatus("ContractTypes");
            if (action != null) return action;
            return View();
        }

        // POST: ContratTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Duration")] Contract_Type contract_Type)
        {
            var action = new CheckController().CheckStatus("ContractTypes");
            if (action != null) return action;
            try
            {
                if (ModelState.IsValid)
                {
                    if (String.IsNullOrEmpty(contract_Type.Title) ||
                        String.IsNullOrWhiteSpace(contract_Type.Title))
                        throw new Exception($"Cannot Create Contract Type By Empty Name");

                    db.Add(contract_Type);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(contract_Type);
        }

        // GET: ContratTypes/Edit/5
        public ActionResult Edit(String id)
        {
            var action = new CheckController().CheckStatus("ContractTypes");
            if (action != null) return action;
            if (id == null || id.CompareTo("Unknown") == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract_Type contract_Type = db.Get(id);
            if (contract_Type == null)
            {
                return HttpNotFound();
            }
            ViewBag.Contracts = new MContrats().Get_All().Where(item => !String.IsNullOrEmpty(item.Type) &&
                item.Type.CompareTo(id) == 0).ToList();
            return View(contract_Type);
        }

        // POST: ContratTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Title,Duration")] Contract_Type contract_Type,
            String NewTitle)
        {
            var action = new CheckController().CheckStatus("ContractTypes");
            if (action != null) return action;
            try
            {
                if (ModelState.IsValid)
                {
                   
                    if (!String.IsNullOrEmpty(NewTitle) && NewTitle.CompareTo(contract_Type.Title) != 0)
                    {
                        if (String.IsNullOrWhiteSpace(NewTitle))
                        throw new Exception($"Cannot Update Contract Type To Empty Name");
                        string org = contract_Type.Title;
                        contract_Type.Title = NewTitle;
                        db.NewTitle(org, contract_Type);
                    }
                    else
                        db.Update(contract_Type);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(contract_Type);
        }

        // GET: ContratTypes/Delete/5
        public ActionResult Delete(String id)
        {
            var action = new CheckController().CheckStatus("ContractTypes");
            if (action != null) return action;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract_Type contract_Type = db.Get(id);
            if (contract_Type == null)
            {
                return HttpNotFound();
            }
            ViewBag.Contracts = new MContrats().Get_All().Where(item =>
                item.Type.CompareTo(id) == 0).ToList();
            return View(contract_Type);
        }

        // POST: ContratTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(String id)
        {
            var action = new CheckController().CheckStatus("ContractTypes");
            if (action != null) return action;
            try
            {
                if (id.CompareTo("Unknown") == 0) throw new Exception("Not authorized to delete");
                db.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToRoute(new { controller = "Contracts", action = "Index", Error = ex.Message });
            }
        }
    }
}

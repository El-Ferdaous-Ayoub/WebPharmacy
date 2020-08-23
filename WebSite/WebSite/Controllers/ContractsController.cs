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
    public class ContractsController : Controller
    {
        MContrats db = new MContrats();

        public ActionResult Index(String Error = "")
        {
            var action = new CheckController().CheckStatus("ContractTypes");
            if (action != null) return action;
            ViewBag.Error = Error;
            return View(db.Get_All());
        }

        // GET: Contrats/Create
        public ActionResult Create()
        {
            var action = new CheckController().CheckStatus("ContractTypes");
            if (action != null) return action;
            ViewBag.Employee_NIC = new SelectList(new MEmployees().Get_All(), "ID", "ID");
            ViewBag.Type = new SelectList(new MContract_Types().Get_All()
                .Where(i => i.Title.CompareTo("Unknown") != 0), "Title", "Title");
            return View();
        }

        // POST: Contrats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Employee_NIC,Type,Start,End")] Contract contract
            , HttpPostedFileBase Document)
        {
            var action = new CheckController().CheckStatus("ContractTypes");
            if (action != null) return action;
            try
            {
               
                    if (String.IsNullOrEmpty(contract.Employee_NIC) ||
                        String.IsNullOrWhiteSpace(contract.Employee_NIC) )
                            throw new Exception($"Cannot Create Contract By Empty Employee NIC");

                    if(new MEmployees().Get(contract.Employee_NIC) == null)
                        throw new Exception($"Cannot Find Employee '{contract.Employee_NIC}' NIC");

                    if (String.IsNullOrEmpty(contract.Type) ||
                     String.IsNullOrWhiteSpace(contract.Type) ||
                     contract.Type.CompareTo("Unknown") == 0)
                        throw new Exception($"Cannot Create Contract By Empty Or '{contract.Type}' Type");
                if (!new MContract_Types().Get(contract.Type).Duration)
                    contract.End = DateTime.Now.AddYears(100);
                
                    if (Document != null)
                    {
                        string DocumentName = System.IO.Path.GetFileName(Document.FileName);
                        string physicalPath = Server.MapPath("~/Content/Documents/" + contract.Employee_NIC + DocumentName);
                        Document.SaveAs(physicalPath);
                        contract.Document = contract.Employee_NIC + DocumentName;
                    }
                    db.Add(contract);
                    return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            ViewBag.Employee_NIC = new SelectList(new MEmployees().Get_All(), "ID", "ID",contract.Employee_NIC);
            ViewBag.Type = new SelectList(new MContract_Types().Get_All()
                   .Where(i => i.Title.CompareTo("Unknown") == 0), "Title", "Title",contract.Type);
            return View(contract);
        }

        // GET: Contrats/Edit/5
        public ActionResult Edit(String id)
        {
            var action = new CheckController().CheckStatus("ContractTypes");
            if (action != null) return action;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Get(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            ViewBag.Employee_NIC = new SelectList(new MEmployees().Get_All(), "ID", "ID", id);
            ViewBag.Type = new SelectList(new MContract_Types().Get_All()
                .Where(i => i.Title.CompareTo("Unknown") != 0), "Title", "Title", contract.Type);
            return View(contract);
        }

        // POST: Contrats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Employee_NIC,Type,Start,End")] Contract contract
           , String oldNIC, HttpPostedFileBase Document)
        {
            var action = new CheckController().CheckStatus("ContractTypes");
            if (action != null) return action;
            try
            {
                    if (String.IsNullOrEmpty(contract.Employee_NIC) ||
                        String.IsNullOrWhiteSpace(contract.Employee_NIC))
                        throw new Exception($"Cannot Create Contract By Empty Employee NIC");

                    if (new MEmployees().Get(contract.Employee_NIC) == null)
                        throw new Exception($"Cannot Find Employee '{contract.Employee_NIC}' NIC");

                    if (String.IsNullOrEmpty(contract.Type) ||
                     String.IsNullOrWhiteSpace(contract.Type) ||
                     contract.Type.CompareTo("Unknown") == 0)
                        throw new Exception($"Cannot Create Contract By Empty Or '{contract.Type}' Category");

                if (!new MContract_Types().Get(contract.Type).Duration)
                    contract.End = DateTime.Now.AddYears(100);

                Contract orgContract = db.Get(oldNIC);
                    if (Document != null)
                    {
                        string DocumentName = System.IO.Path.GetFileName(Document.FileName);
                        string physicalPath = Server.MapPath("~/Content/Documents/" + contract.Employee_NIC + DocumentName);
                        Document.SaveAs(physicalPath);
                        contract.Document = contract.Employee_NIC + DocumentName;
                    }
                    else
                        contract.Document = orgContract.Document;
                    if (oldNIC.CompareTo(contract.Employee_NIC) != 0)
                        db.NewNIC(oldNIC, contract);
                    else
                        db.Update(contract);
                    return RedirectToAction("Index");
  
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            ViewBag.Type = new SelectList(new MContract_Types().Get_All(), "Title", "Title", contract.Type);
            return View(contract);
        }

        // GET: Contrats/Delete/5
        public ActionResult Delete(String id)
        {
            var action = new CheckController().CheckStatus("ContractTypes");
            if (action != null) return action;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Get(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // POST: Contrats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(String id)
        {
            var action = new CheckController().CheckStatus("ContractTypes");
            if (action != null) return action;
            try
            {
                db.Remove(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToRoute(new { controller = "Contracts", action = "Index", Error = ex.Message });
            }
        }
    }
}

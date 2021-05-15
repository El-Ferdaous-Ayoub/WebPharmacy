using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Classes;
using BAL;
using System.Net;
using System.IO;

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
            var list = db.Get_All();
            foreach (var item in list)
            {
                item.Employee = new MEmployees().Get(item.Employee_ID);
            }
            return View(list);
        }

        // GET: Contrats/Create
        public ActionResult Create()
        {
            var action = new CheckController().CheckStatus("ContractTypes");
            if (action != null) return action;
            ViewBag.Employee_ID = new SelectList(new MEmployees().Get_All(), "ID", "NIC");
            return View();
        }

        // POST: Contrats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Employee_ID,Type,Start")] Contract contract
            ,HttpPostedFileBase Document)
        {
            var action = new CheckController().CheckStatus("ContractTypes");
            if (action != null) return action;
            try
            {
               
                    if(new MEmployees().Get(contract.Employee_ID) == null)
                        throw new Exception($"Cannot Find The Employee");

                if (Document != null)
                    {
                    var str = "";
                    string fileName = Path.GetFileName(Document.FileName);
                    do
                    {
                        str = GetName();
                    } while (System.IO.File.Exists($"~/Content/Documents/{str}{fileName}"));

                    string physicalPath = Server.MapPath($"~/Content/Documents/{str}{fileName}");
                    Document.SaveAs(physicalPath);
                    contract.Document = $"{str}{fileName}";
                }
                    db.Add(contract);
                    return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            ViewBag.Employee_ID = new SelectList(new MEmployees().Get_All(), "ID", "NIC", contract.Employee_ID);
            return View(contract);
        }

        public String GetName()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 20)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // GET: Contrats/Edit/5
        public ActionResult Edit(int id)
        {
            var action = new CheckController().CheckStatus("ContractTypes");
            if (action != null) return action;
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Contract contract = db.Get(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            contract.Employee = new MEmployees().Get(contract.Employee_ID);
            return View(contract);
        }

        // POST: Contrats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Employee_ID,Type,Start,Document")] Contract contract
          , HttpPostedFileBase Document)
        {
            var action = new CheckController().CheckStatus("ContractTypes");
            if (action != null) return action;
            try
            {
                if (new MEmployees().Get(contract.Employee_ID) == null)
                    throw new Exception($"Cannot Find The Employee");

                if (Document != null)
                {
                    if (System.IO.File.Exists($"~/Content/Documents/{contract.Document}"))
                        System.IO.File.Delete($"~/Content/Documents/{contract.Document}");

                    var str = "";
                    string fileName = Path.GetFileName(Document.FileName);
                    do
                    {
                        str = GetName();
                    } while (System.IO.File.Exists($"~/Content/Documents/{str}{fileName}"));

                    string physicalPath = Server.MapPath($"~/Content/Documents/{str}{fileName}");
                    Document.SaveAs(physicalPath);
                    contract.Document = $"{str}{fileName}";
                }

                db.Update(contract);
                    return RedirectToAction("Index");
  
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            ViewBag.Employee_ID = new SelectList(new MEmployees().Get_All(), "ID", "NIC", contract.Employee_ID);
            return View(contract);
        }

        // GET: Contrats/Delete/5
        public ActionResult Delete(int id)
        {
            var action = new CheckController().CheckStatus("ContractTypes");
            if (action != null) return action;
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            Contract contract = db.Get(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            contract.Employee = new MEmployees().Get(contract.Employee_ID);
            return View(contract);
        }

        // POST: Contrats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
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

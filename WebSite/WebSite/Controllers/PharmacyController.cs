using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAL;
using Classes;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class PharmacyController : Controller
    {
        private Pharmacy_Info _Info = new Pharmacy_Info();

        // GET: Pharmacy/Create
        public ActionResult Create(int t = 0)
        {
            if (t == 0)
            {
                var action = new CheckController().CheckStatus("Pharmacy_Info");
                if (action != null) return action;
            }
            if (_Info.Get() != null)
                return RedirectToAction("Edit"); 
            return View();
        }

        // POST: Pharmacy/Create
        [HttpPost]
        public ActionResult Create(Pharmacy pharmacy)
        {
            try
            {
                if (String.IsNullOrEmpty(pharmacy.Name) ||
               String.IsNullOrWhiteSpace(pharmacy.Name))
                    throw new Exception($"Cannot Create Pharmacy By Empty Name");
                if (_Info.Get() == null)
                {
                    _Info.Add(pharmacy);
                    return RedirectToAction("Edit");
                }
                var action = new CheckController().CheckStatus("Pharmacy_Info");
                if (action != null) return action;
                throw new Exception();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
                return View(pharmacy);
            }
        }  


        // GET: Pharmacy/Edit/5
        public ActionResult Edit()
        {
            var action = new CheckController().CheckStatus("Pharmacy_Info");
            if (action != null) return action;
            return View(_Info.Get());
        }

        // POST: Pharmacy/Edit/5
        [HttpPost]
        public ActionResult Edit(Pharmacy pharmacy, String NewName)
        {
            var action = new CheckController().CheckStatus("Pharmacy_Info");
            if (action != null) return action;
            if (_Info.Get() != null)
            {
                if (!String.IsNullOrEmpty(NewName))
                {
                    Pharmacy ph = new Pharmacy() { Name = NewName, Email = pharmacy.Email, Address = pharmacy.Address, Phone = pharmacy.Phone };
                    _Info.NewName(ph);
                }
                else _Info.Update(pharmacy);
                Config.CheckData();
            }
            return RedirectToAction("Index","Home");
        }
    }
}

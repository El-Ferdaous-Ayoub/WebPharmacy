using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BAL;
using Classes;
using System.IO;

namespace WebSite.Controllers
{
    public class ProductsController : Controller
    {
        private MProducts db = new MProducts();
        private MCategories MC = new MCategories();
        private MProducers MP = new MProducers();

        // GET: Products
        public ActionResult Index(String Error = "")
        {
            var action = new CheckController().CheckStatus("Products");
            if (action != null) return action;
            ViewBag.Error = Error;
            return View(GetList(""));
        }

        [HttpPost]
        public ActionResult Index(String ID,int? i)
        {
            var action = new CheckController().CheckStatus("Products");
            if (action != null) return action;
            return View(GetList(ID));
        }

        private List<Product> GetList(String ID)
        {
            var list = db.Get_All().Where(item => item.ID.Contains(ID)).ToList();
            foreach (var item in list)
            {
                item.Category = MC.Get(item.Category_ID);
                item.Producer = MP.Get(item.Producer_ID);
            }
            return list;
        }

        public ActionResult Create()
        {
            var action = new CheckController().CheckStatus("Products");
            if (action != null) return action;
            ViewBag.Category_ID = new SelectList(MC.Get_All().Where(i => i.Name.CompareTo("Unknown") != 0).ToList(),
                "ID", "Name");
            ViewBag.Producer_ID = new SelectList(MP.Get_All().Where(i => i.Name.CompareTo("Unknown") != 0).ToList(),
                "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Product_Name,Producer_ID,Price,Category_ID")] Product product, HttpPostedFileBase ImageUrl)
        {
            var action = new CheckController().CheckStatus("Products");
            if (action != null) return action;
            try
            {
                if (ModelState.IsValid)
                {
                    if (String.IsNullOrEmpty(product.ID) ||
                        String.IsNullOrWhiteSpace(product.ID))
                        throw new Exception($"Cannot Create Product By Empty ID");
                   
                    if (ImageUrl != null)
                    {
                        var str = "";
                        string ImageName = Path.GetFileName(ImageUrl.FileName);
                        do
                        {
                            str = GetName();
                        } while (System.IO.File.Exists($"~/Content/Images/Products/{str}{ImageName}"));
                        string physicalPath = Server.MapPath($"~/Content/Images/Products/{str}{ImageName}");
                        ImageUrl.SaveAs(physicalPath);
                        product.Picture = $"{str}{ImageName}";
                    }
                    db.Add(product);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            ViewBag.Category_ID = new SelectList(MC.Get_All().Where(i => i.Name.CompareTo("Unknown") != 0).ToList(),
                "ID", "Name", product.Category_ID);
            ViewBag.Producer_ID = new SelectList(MP.Get_All().Where(i => i.Name.CompareTo("Unknown") != 0).ToList(),
                "ID", "Name", product.Producer_ID);
            return View(product);
        }

        public String GetName()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 20)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        // GET: Products/Edit/5
        public ActionResult Edit(string id)
        {
            var action = new CheckController().CheckStatus("Products");
            if (action != null) return action;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Get(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_ID = new SelectList(MC.Get_All().Where(i=>i.Name.CompareTo("Unknown") !=0).ToList(),
                "ID", "Name", product.Category_ID);
            ViewBag.Producer_ID = new SelectList(MP.Get_All().Where(i => i.Name.CompareTo("Unknown") != 0).ToList()
                , "ID", "Name", product.Producer_ID);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Product_Name,Producer_ID,Price,Quantity,Category_ID")] Product product, HttpPostedFileBase ImageUrl)
        {
            var action = new CheckController().CheckStatus("Products");
            if (action != null) return action;
            try
            {
                if (ModelState.IsValid)
                {
                    Product org = db.Get(product.ID);
                    if (org != null)
                        product.Picture = org.Picture;

                    if (ImageUrl != null)
                    {
                        if (System.IO.File.Exists($"~/Content/Images/Products/{org.Picture}"))
                            System.IO.File.Delete($"~/Content/Images/Products/{org.Picture}");
                        
                        var str = "";
                        string ImageName = Path.GetFileName(ImageUrl.FileName);
                        do
                        {
                            str = GetName();
                        } while (System.IO.File.Exists($"~/Content/Images/Products/{str}{ImageName}"));
                        string physicalPath = Server.MapPath($"~/Content/Images/Products/{str}{ImageName}");
                        ImageUrl.SaveAs(physicalPath);
                        product.Picture = $"{str}{ImageName}";
                    }
                    db.Update(product);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            ViewBag.Category_ID = new SelectList(MC.Get_All().Where(i => i.Name.CompareTo("Unknown") != 0).ToList(),
                    "ID", "Name", product.Category_ID);
            ViewBag.Producer_ID = new SelectList(MP.Get_All().Where(i => i.Name.CompareTo("Unknown") != 0).ToList(),
                "ID", "Name", product.Producer_ID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(string id)
        {
            var action = new CheckController().CheckStatus("Products");
            if (action != null) return action;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Get(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            product.Category = MC.Get(product.Category_ID);
            product.Producer = MP.Get(product.Producer_ID);
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var action = new CheckController().CheckStatus("Products");
            if (action != null) return action;
            try
            {
                db.Delete(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToRoute(new { controller = "Products", action = "Index", Error = ex.Message });
            }
        }
    }
}

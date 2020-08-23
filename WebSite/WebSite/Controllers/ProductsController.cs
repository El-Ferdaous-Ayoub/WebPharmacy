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
    public class ProductsController : Controller
    {
        private MProducts db = new MProducts();

        // GET: Products
        public ActionResult Index(String Error = "")
        {
            var action = new CheckController().CheckStatus("Products");
            if (action != null) return action;
            ViewBag.Error = Error;
            SetSelectedList();
            return View(db.Get_All());
        }

        private void SetSelectedList()
        {
            List<Category> lstcat = new MCategories().Get_All().ToList();
            List<Producer> lstpro = new MProducers().Get_All().ToList();
            lstcat.Add(new Category() { Name = "All" });
            lstpro.Add(new Producer() { Name = "All" });
            ViewBag.Category_Name = new SelectList(lstcat, "Name", "Name");
            ViewBag.Producer_Name = new SelectList(lstpro, "Name", "Name");
        }

        [HttpPost]
        public ActionResult Index(String ID, String Category_Name, String Producer_Name)
        {
            var action = new CheckController().CheckStatus("Products");
            if (action != null) return action;
            SetSelectedList();
            return View(GetSearched(ID, Category_Name, Producer_Name).ToList());
        }

        public IEnumerable<Product> GetSearched(String ID, String Category_Name, String Producer_Name)
        {
            if ((Producer_Name.CompareTo("All") == 0) && (Category_Name.CompareTo("All") == 0))
                return ((from e in db.Get_All() where e.ID.Contains(ID) || e.Product_Name.Contains(ID) select e));
            /*--------------------------------------------------------------------------------------------------*/
            if ((Producer_Name.CompareTo("All") != 0) && (Category_Name.CompareTo("All") == 0))
                return ((from e in db.Get_All()
                         where (e.ID.Contains(ID) || e.Product_Name.Contains(ID)) &&
                         (e.Producer_Name.CompareTo(Producer_Name) == 0)
                         select e));
            /*--------------------------------------------------------------------------------------------------*/
            if ((Producer_Name.CompareTo("All") == 0) && (Category_Name.CompareTo("All") != 0))
                return ((from e in db.Get_All()
                         where (e.ID.Contains(ID) || e.Product_Name.Contains(ID)) &&
                        (e.Category_Name.CompareTo(Category_Name) == 0)
                         select e));
            /*--------------------------------------------------------------------------------------------------*/
            return (from e in db.Get_All()
                    where (e.ID.Contains(ID) || e.Product_Name.Contains(ID)) && (
                    e.Category_Name.CompareTo(Category_Name) == 0 && e.Producer_Name.CompareTo(Producer_Name) == 0)
                    select e);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            var action = new CheckController().CheckStatus("Products");
            if (action != null) return action;
            ViewBag.Category_Name = new SelectList(new MCategories().Get_All().Where(i => i.Name.CompareTo("Unknown") != 0).ToList(),
                "Name", "Name");
            ViewBag.Producer_Name = new SelectList(new MProducers().Get_All().Where(i => i.Name.CompareTo("Unknown") != 0).ToList(),
                "Name", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Product_Name,Producer_Name,Price,Category_Name")] Product product, HttpPostedFileBase ImageUrl, float Discount)
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
                    if (String.IsNullOrEmpty(product.Category_Name) ||
                        String.IsNullOrWhiteSpace(product.Category_Name) ||
                        product.Category_Name.CompareTo("Unknown") == 0)
                        throw new Exception($"Cannot Create a Product By Empty Or '{product.Category_Name}' Category");
                    if (String.IsNullOrEmpty(product.Producer_Name) ||
                       String.IsNullOrWhiteSpace(product.Producer_Name) ||
                       product.Producer_Name.CompareTo("Unknown") == 0)
                        throw new Exception($"Cannot Create a Product By Empty Or '{product.Producer_Name}' Producer");

                    product.Discount = Discount / 100;
                    if (ImageUrl != null)
                    {
                        string ImageName = System.IO.Path.GetFileName(ImageUrl.FileName);
                        string physicalPath = Server.MapPath("~/Content/Images/Products/" + ImageName);
                        ImageUrl.SaveAs(physicalPath);
                        product.Picture = ImageName;
                    }
                    db.Add(product);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            ViewBag.Category_Name = new SelectList(new MCategories().Get_All().Where(i => i.Name.CompareTo("Unknown") != 0).ToList(),
                "Name", "Name", product.Category_Name);
            ViewBag.Producer_Name = new SelectList(new MProducers().Get_All().Where(i => i.Name.CompareTo("Unknown") != 0).ToList(),
                "Name", "Name", product.Producer_Name);
            return View(product);
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
            product.Discount *= 100;
            ViewBag.Category_Name = new SelectList(new MCategories().Get_All().Where(i=>i.Name.CompareTo("Unknown") !=0).ToList(),
                "Name", "Name", product.Category_Name);
            ViewBag.Producer_Name = new SelectList(new MProducers().Get_All().Where(i => i.Name.CompareTo("Unknown") != 0).ToList()
                , "Name", "Name", product.Producer_Name);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Product_Name,Producer_Name,Price,Quantity,Category_Name")] Product product, HttpPostedFileBase ImageUrl, float Discount)
        {
            var action = new CheckController().CheckStatus("Products");
            if (action != null) return action;
            try
            {
                if (ModelState.IsValid)
                {
                    if (String.IsNullOrEmpty(product.Category_Name) ||
               String.IsNullOrWhiteSpace(product.Category_Name) ||
               product.Category_Name.CompareTo("Unknown") == 0)
                        throw new Exception($"Cannot Update Product To Empty Or '{product.Category_Name}' Category");
                    if (String.IsNullOrEmpty(product.Producer_Name) ||
                       String.IsNullOrWhiteSpace(product.Producer_Name) ||
                       product.Producer_Name.CompareTo("Unknown") == 0)
                        throw new Exception($"Cannot Update Product To Empty Or '{product.Producer_Name}' Producer");
                    product.Discount = Discount / 100;
                    Product org = db.Get(product.ID);
                    if (org != null)
                        product.Picture = org.Picture;
                    

                    if (ImageUrl != null)
                    {
                        string ImageName = System.IO.Path.GetFileName(ImageUrl.FileName);
                        string physicalPath = Server.MapPath("~/Content/Images/Products/" + ImageName);
                        ImageUrl.SaveAs(physicalPath);
                        product.Picture = ImageName;
                    }
                    db.Update(product);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            ViewBag.Category_Name = new SelectList(new MCategories().Get_All(), "Name", "Name", product.Category_Name);
            ViewBag.Producer_Name = new SelectList(new MProducers().Get_All(), "Name", "Name", product.Producer_Name);
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

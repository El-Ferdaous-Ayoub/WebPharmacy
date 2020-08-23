using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BAL;
using Classes;
using Classes.Models;


namespace WebSite.Controllers
{
    public class OrdersController : Controller
    {
        MOrders db = new MOrders();
        MProducts pdb = new MProducts();
        private static List<ProductToOrder> Products;

        // GET: Orders
        public ActionResult Index(String Error = "")
        {
            var action = new CheckController().CheckStatus("Orders");
            if (action != null) return action;
            ViewBag.Error = Error;
            return View(db.GetPharmacyOrders());
        }

        [HttpPost]
        public JsonResult GetProducts()
        {
            return Json(Products);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            var action = new CheckController().CheckStatus("Orders");
            if (action != null) return action;
            FillProductsList();
            return View();
        }

        private void FillProductsList(String id = "")
        {
            var list = pdb.Get_All();
            Products = new List<ProductToOrder>();
                foreach (var item in list)
                {
                    Products.Add(new ProductToOrder(item));
                }
            if (!String.IsNullOrEmpty(id))
            {
                OrderDone done = db.IsDone(id);
                ViewBag.Done =(done != null)?done.ArrivedToStock:false;
                
                foreach (var item in db.GetOrders_Info(id))
                {
                    var oi = Products.Where(i => i.ID.CompareTo(item.Product_ID) == 0).FirstOrDefault();
                    if (oi != null)
                    {
                        oi.OrderQuantity = item.Quantity;
                        oi.PurchasePrice = item.Price;
                    }
                    else
                    {
                        Product product = new Product()
                        {
                            ID = item.Product_ID,
                            Picture = "",
                            Product_Name = "",
                            Quantity = 0,
                            Price = 0,
                            Category_Name = "Unknown",
                            Producer_Name = "Unknown"
                        };
                        pdb.Add(product);
                        Products.Add(new ProductToOrder(product, item.Price, item.Quantity));
                    }  
                }
            }
        }

        private void FillProductsForDelete(String id = "")
        {
            var list = pdb.Get_All();
            Products = new List<ProductToOrder>();
            OrderDone done = db.IsDone(id);
            ViewBag.Done = (done != null) ? done.ArrivedToStock : false;
            ViewBag.QuantityChanged = (done != null) ? done.QuantityChanged : false;
            foreach (var item in db.GetOrders_Info(id))
            {
                var pd = list.Where(i => i.ID.CompareTo(item.Product_ID) == 0).FirstOrDefault();
                if (pd == null)
                {
                    Product product = new Product()
                    {
                        ID = item.Product_ID,
                        Picture = "",
                        Product_Name = "",
                        Quantity = 0,
                        Price = 0,
                        Category_Name = "Unknown",
                        Producer_Name = "Unknown"
                    };
                    pdb.Add(product);
                }
                var oi = (new ProductToOrder(pd, item.Price, item.Quantity));
                Products.Add(oi);
            }
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order,Boolean Done = false,Boolean ChangeQuantity = false)
        {
            var action = new CheckController().CheckStatus("Orders");
            if (action != null) return action;
            try
            {
                if (!Done && ChangeQuantity) ChangeQuantity = false;
                order.Owner = "Pharmacy";
                List<OrderInfo> orderInfos = new List<OrderInfo>();
                float total = 0;
                foreach (var item in Products)
                {
                    if (item.OrderQuantity > 0)
                    {
                        orderInfos.Add(item.GetOrderInfo());
                        total += item.PurchasePrice * item.OrderQuantity;
                    }
                }
                order.TotalAmount = total;
                if (orderInfos.Count <= 0) return View(order);
                db.Add(order, orderInfos,Done,ChangeQuantity);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(order);
            }
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(String id)
        {
            var action = new CheckController().CheckStatus("Orders");
            if (action != null) return action;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Get(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            FillProductsList(id);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        public ActionResult Edit(Order order, Boolean Done, Boolean ChangeQuantity)
        {
            var action = new CheckController().CheckStatus("Orders");
            if (action != null) return action;
            try
            {
                if (!Done && ChangeQuantity) ChangeQuantity = false;
                List<OrderInfo> orderInfos = new List<OrderInfo>();
                float total = 0;
                foreach (var item in Products)
                {
                    if (item.OrderQuantity>0)
                    {
                        orderInfos.Add(item.GetOrderInfo());
                        total += item.PurchasePrice * item.OrderQuantity;
                    }
                }
                order.TotalAmount = total;
                if (orderInfos.Count <= 0) return View(order);
                db.Update(order, orderInfos, Done,ChangeQuantity);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            ViewBag.Done = Done;
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(String id)
        {
            var action = new CheckController().CheckStatus("Orders");
            if (action != null) return action;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Get(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            FillProductsForDelete(id);
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(String ID, Boolean SubtractQuantity = false)
        {
            var action = new CheckController().CheckStatus("Orders");
            if (action != null) return action;
            try
            {
                db.Delete(ID, SubtractQuantity);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToRoute(new { action = "Index", Error = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SetProducts(List<ProductToOrder> products)
        {
            foreach (var item in products)
            {
                var product = Products.Where(i => i.ID.CompareTo(item.ID) == 0).FirstOrDefault();
                if (product != null)
                {
                    product.PurchasePrice = item.PurchasePrice;
                    product.OrderQuantity = item.OrderQuantity;
                }
            }
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}

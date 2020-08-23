using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes;
using DAL;
using Microsoft.AspNetCore.Mvc;

namespace BAL
{
    public class MOrders
    {
        public Order Get(String ID)
        {
            return Get_Data.Get_Order(ID);
        }

        public List<Order> Get_All()
        {
            return Get_Data.Get_Orders().ToList();
        }

        public List<Order> GetPharmacyOrders()
        {
            return Get_Data.Get_Orders().ToList();
        }

        public List<Order> GetCustumorsOrders()
        {
            return Get_Data.Get_Orders().ToList();
        }

        public String GetNewID()
        {
            
            String ID = "";
            ID += DateTime.Now.Date.Year;
            ID += DateTime.Now.Date.Month;
            ID += DateTime.Now.Date.Day;
            ID += Get_All().Count.ToString() + new Random().Next(0,10);
            return ID;
        }

        public void Add(Order order,List<OrderInfo> OrderInfo,Boolean Done,Boolean ChangeQuantity)
        {
            do
            {
                order.ID = GetNewID();
            } while (Get(order.ID) != null);
            Management.Add(order);
            foreach (var item in OrderInfo)
            {
                Management.Add(new OrderInfo()
                {
                    Order_ID = order.ID,
                    Product_ID = item.Product_ID,
                    Price = item.Price,
                    Quantity = item.Quantity
                });

                if (Done && ChangeQuantity)
                {
                    Product product = Get_Data.Get_Product(item.Product_ID);
                    product.Quantity += item.Quantity;
                    new MProducts().Update(product);
                }
            }
            Management.Add(new OrderDone() { ID = order.ID, ArrivedToStock = Done ,QuantityChanged = ChangeQuantity});
        }

        public void Update(Order order, List<OrderInfo> OrderInfo,Boolean Done ,Boolean ChangeQuantity)
        {
            Order org = Get(order.ID);
            if (org == null) throw new Exception($"Order ({order.ID}) is Not Exist");
            var orgOIs = Get_Data.GetOIs_ByOrder(order.ID);
            var orderdone = Get_Data.GetOrderDone(org.ID);
            if (orderdone == null)
            {
                orderdone = new OrderDone()
                {
                    ID = order.ID,
                    ArrivedToStock = false,
                    QuantityChanged = false
                };
                Management.Add(orderdone);
            }
            foreach (var item in orgOIs)
            {
                var pi = OrderInfo.Where(i => i.Product_ID.CompareTo(item.Product_ID) == 0).FirstOrDefault();
                var product = Get_Data.Get_Product(item.Product_ID);

                if (product == null)
                {
                     product = new Product()
                    {
                        ID = item.Product_ID,
                        Picture = "",
                        Product_Name = "",
                        Quantity = 0,
                        Price = 0,
                        Category_Name = "Unknown",
                        Producer_Name = "Unknown"
                    };
                    new MProducts().Add(product);
                }

                if (orderdone.QuantityChanged)
                {
                    if (product.Quantity - item.Quantity >= 0)
                        product.Quantity -= item.Quantity;
                    else product.Quantity = 0;
                }

                if (pi != null && orderdone.ArrivedToStock && ChangeQuantity)
                {
                    product.Quantity += pi.Quantity;
                    pi.ID = item.ID;
                    pi.Order_ID = item.Order_ID;
                }

                new MProducts().Update(product);

                if (pi == null)
                {
                    Management.Remove(item);
                    continue;
                }
                Management.Detach(item);
                Management.Update(pi);
            }

            foreach (var item in OrderInfo)
            {
                if (orgOIs.Where(i => i.Product_ID.CompareTo(item.Product_ID) == 0).FirstOrDefault() != null)
                    continue;
                Management.Add(new OrderInfo()
                {
                    Order_ID = order.ID,
                    Product_ID = item.Product_ID,
                    Price = item.Price,
                    Quantity = item.Quantity
                });

                if (Done && ChangeQuantity)
                {
                    Product product = Get_Data.Get_Product(item.Product_ID);
                    product.Quantity += item.Quantity;
                    new MProducts().Update(product);
                }
            }

            if (Done)
            {
                orderdone.ArrivedToStock = Done;
                orderdone.QuantityChanged = ChangeQuantity;
                Management.Detach(Get_Data.GetOrderDone(org.ID));
                Management.Update(orderdone);
            }
            Management.Detach(org);
            Management.Update(order);
        }

        public void Delete(String ID,Boolean ChangeQuantity)
        {
            Order org = Get(ID);
            if (org == null) throw new Exception($"Order ({ID}) is Not Exist");
            var ordersinfo = Get_Data.GetOIs_ByOrder(ID).ToList();
            var orderdone = Get_Data.GetOrderDone(org.ID);
            foreach (var item in ordersinfo)
            {
                if (orderdone != null &&
                    orderdone.ArrivedToStock && orderdone.QuantityChanged)
                {
                    if (ChangeQuantity)
                    {
                        var product = Get_Data.Get_Product(item.Product_ID);
                        if (product != null)
                        {
                            if (product.Quantity - item.Quantity >= 0)
                                product.Quantity -= item.Quantity;
                            else product.Quantity = 0;
                            new MProducts().Update(product);
                        }
                    }
                }
                Management.Remove(item);
            }
            if (orderdone != null)
                Management.Remove(orderdone);
            Management.Remove(org);
        }

        public List<OrderInfo> GetOrders_Info(String OrderID)
        {
            return Get_Data.GetOIs_ByOrder(OrderID).ToList();
        }

        public OrderDone IsDone(String ID)
        {
            return Get_Data.GetOrderDone(ID);
        }
    }
}

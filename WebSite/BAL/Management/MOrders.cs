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

        public void Add(Order order,List<OrderInfo> OrderInfo)
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

                Product product = Get_Data.Get_Product(item.Product_ID);
                product.Quantity += item.Quantity;
                new MProducts().Update(product);
            }
        }

        public void Update(Order order, List<OrderInfo> OrderInfo)
        {
            Order org = Get(order.ID);
            if (org == null) throw new Exception($"Order ({order.ID}) is Not Exist");
            var orgOIs = Get_Data.GetOIs_ByOrder(order.ID);

            var uc = new MCategories().GetUNCategory().ID;
            var up = new MProducers().GetUNProducer().ID;

            foreach (var item in orgOIs)
            { 
                var ori = Get_Data.GetOrderInfo(item.Order_ID, item.Product_ID);
                var pi = OrderInfo.Where(i => i.Product_ID == item.Product_ID).FirstOrDefault();
                var product = Get_Data.Get_Product(item.Product_ID);

                if (pi != null)
                {
                    pi.ID = ori.ID;
                    pi.Order_ID = order.ID;
                    if (pi.Quantity != item.Quantity)
                    {
                        if (pi.Quantity > item.Quantity)
                            product.Quantity += (pi.Quantity - item.Quantity);
                        else if (pi.Quantity < item.Quantity)
                            product.Quantity -= (item.Quantity - pi.Quantity);
                        if (product.Quantity < 0)
                            product.Quantity = 0;

                        Management.Detach(ori);
                        Management.Update(pi);
                    }
                }
                else
                {
                    product.Quantity -= item.Quantity;
                    Management.Remove(item);
                }
                new MProducts().Update(product);
            }
            foreach (var item in OrderInfo.Where(i=> String.IsNullOrEmpty(i.Order_ID)).ToList())
            {
                Management.Add(new OrderInfo()
                {
                    Order_ID = order.ID,
                    Product_ID = item.Product_ID,
                    Price = item.Price,
                    Quantity = item.Quantity
                });

                Product product = Get_Data.Get_Product(item.Product_ID);
                product.Quantity += item.Quantity;
                new MProducts().Update(product);
            }
        }

        public void Delete(String ID,Boolean ChangeQuantity)
        {
            Order org = Get(ID);
            if (org == null) throw new Exception($"Order ({ID}) is Not Exist");
            var ordersinfo = Get_Data.GetOIs_ByOrder(ID).ToList();
            foreach (var item in ordersinfo)
            {
                if (ChangeQuantity)
                {
                    var product = Get_Data.Get_Product(item.Product_ID);
                    if (product != null)
                    {
                        product.Quantity -= item.Quantity;
                        if (product.Quantity < 0)
                            product.Quantity = 0;
                        new MProducts().Update(product);
                    }
                }
                Management.Remove(item);
            }
            Management.Remove(org);
        }

        public List<OrderInfo> GetOrders_Info(String OrderID)
        {
            return Get_Data.GetOIs_ByOrder(OrderID).ToList();
        }

        public List<OrderInfo> GetOIs_ByProduct(String PID)
        {
            return Get_Data.GetOIs_ByProduct(PID).ToList();
        }
    }
}

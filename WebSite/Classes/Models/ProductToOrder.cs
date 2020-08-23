using Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Classes.Models
{
    public class ProductToOrder
    {
        public ProductToOrder(Product product)
        {
            ID = product.ID;
            Image = product.Picture;
            Name = product.Product_Name;
            Category = product.Category_Name;
            Producer = product.Producer_Name;
            SellingPrice = product.Price;
            StockQuantity = product.Quantity;
            PurchasePrice = 0;
            OrderQuantity = 0;
        }

        public ProductToOrder(Product product, float Price, int Quantity)
        {
            ID = product.ID;
            Image = product.Picture;
            Name = product.Product_Name;
            Category = product.Category_Name;
            Producer = product.Producer_Name;
            SellingPrice = product.Price;
            StockQuantity = product.Quantity;
            PurchasePrice = 0;
            OrderQuantity = 0;
            PurchasePrice = Price;
            OrderQuantity = Quantity;
        }

        public ProductToOrder()
        {

        }

        public String ID { get; set; }
        public String Image { get; set; }
        [Display(Name ="Product Name")]
        public String Name { get; set; }
        [Display(Name = "Category")]
        public String Category { get; set; }
        [Display(Name = "Producer")]
        public String Producer { get; set; }
        [Display(Name = "Selling Price")]
        public float SellingPrice { get; set; }
        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }
        [Display(Name = "Purchase Price")]
        public float PurchasePrice { get; set; }
        [Display(Name = "Quantity")]
        public int OrderQuantity { get; set; }

        public OrderInfo GetOrderInfo()
        {
            return new OrderInfo() {
                Product_ID = ID,
                Quantity = OrderQuantity,
                Price = PurchasePrice
            };
        }
    }
}
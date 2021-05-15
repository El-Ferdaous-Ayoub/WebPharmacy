using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Classes
{
    [Table("OrderInfo")]
    public class OrderInfo
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Product ID")]
        public String Product_ID { get; set; }
        [Display(Name = "Order ID")]
        public String Order_ID { get; set; }
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
        [Range(0,float.MaxValue)]
        public float Price { get; set; }

        [ForeignKey("Product_ID")]
        public virtual Product Product { get; set; }
        [ForeignKey("Order_ID")]
        public virtual Order Order { get; set; }
    }
}

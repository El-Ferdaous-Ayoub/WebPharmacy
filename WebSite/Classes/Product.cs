using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    [Table("Products")]
    public class Product
    {
        [Key,Required]
        public String ID { get; set; }
        public String Picture { get; set; }
        [Display(Name = "Product Name")]
        public String Product_Name { get; set; }
        [Display(Name = "Producer")]
        public String Producer_Name { get; set; }
        [Display(Name = "Category")]
        public String Category_Name { get; set; }
        [Range(0,int.MaxValue)]
        public int Quantity { get; set; }
        [Range(0, float.MaxValue)]
        public float Price { get; set; }
        [Range(0, 1)]
        public float Discount { get; set; }

        public virtual Category Category { get; set; }
        public virtual Producer Producer { get; set; }
    }
}

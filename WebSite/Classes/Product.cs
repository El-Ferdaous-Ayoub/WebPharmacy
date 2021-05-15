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
        [Display(Name = "Product")]
        public String Product_Name { get; set; }
        public int Producer_ID { get; set; }
        public int Category_ID { get; set; }
        [Range(0,int.MaxValue)]
        public int Quantity { get; set; }
        [Range(0, float.MaxValue)]
        public float Price { get; set; }

        [ForeignKey("Category_ID")]
        public virtual Category Category { get; set; }
        [ForeignKey("Producer_ID")]
        public virtual Producer Producer { get; set; }
    }
}

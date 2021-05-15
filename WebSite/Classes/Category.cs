using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Category")]
        public String Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}

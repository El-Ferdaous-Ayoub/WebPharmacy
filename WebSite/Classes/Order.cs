using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    [Table("Orders")]
    public class Order
    {
        [Key, Required]
        public string ID { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        [Display(Name = "Paid Amount")]
        [Range(0,float.MaxValue)]
        public float PaidAmount { get; set; }
        [Display(Name = "Total Amount")]
        [Range(0, float.MaxValue)]
        public float TotalAmount { get; set; }

        public virtual ICollection<OrderInfo> Products { get; set; }
    }
}

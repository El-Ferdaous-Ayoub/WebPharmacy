using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Classes
{
    [Table("OrdersDone")]
    public class OrderDone
    {
        [Key,Required]
        public String ID { get; set; }
        public Boolean ArrivedToStock { get; set; }
        public Boolean QuantityChanged { get; set; }
    }
}

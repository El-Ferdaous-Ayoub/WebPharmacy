using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    [Table("Producers")]
    public class Producer
    {
        [Key,Required]
        [Display(Name = "Producer")]
        public String Name { get; set; }
        public String Address { get; set; }
        [EmailAddress]
        public String Email { get; set; }

     
        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$",
            ErrorMessage = "Not a valid phone number")]
        public String PhoneN { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}

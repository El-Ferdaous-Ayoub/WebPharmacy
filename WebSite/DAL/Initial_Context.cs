using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Initial_Context
    {
        public static DataContext db { get; set; } = new DataContext();
    }
}
        
    
   
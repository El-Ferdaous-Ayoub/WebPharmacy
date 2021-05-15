using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public static class InitialContext
    {
        public static DataContext db { get; set; } = new DataContext();
    }
}

using Classes;
using Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class Account
    {
        public static UserModel User { get; set; }
        public static DepartmentModel Department { get; set; }
    }
}
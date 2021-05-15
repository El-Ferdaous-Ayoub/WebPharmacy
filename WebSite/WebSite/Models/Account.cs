using BAL;
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
        public static Employee User { get; set; } 
        public static Department Department { get; set; } 
    }
}
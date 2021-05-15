using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using Classes;

namespace BAL
{
    public class MAccounts
    {
        public Employee Login(String UserName,String Password)
        {
            return Get_Data.Login(UserName,Password);
        }
    }
}

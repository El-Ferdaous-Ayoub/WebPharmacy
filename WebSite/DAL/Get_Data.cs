using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes;

namespace DAL
{
    public static class Get_Data
    {
        public static Category Get_Category(int ID)
        {
            return InitialContext.db.Categories.Where(i => i.ID == ID).FirstOrDefault();
        }

        public static Producer Get_Producer(int ID)
        {
            return InitialContext.db.Producers.Where(i => i.ID == ID).FirstOrDefault();
        }

        public static Product Get_Product(String ID)
        {
            return InitialContext.db.Products.Where(i => i.ID == ID).FirstOrDefault();
        }

        public static Order Get_Order(String ID)
        {
            return InitialContext.db.Orders.Where(i => i.ID == ID).FirstOrDefault();
        }

        public static Contract Get_Contract(int ID)
        {
            return InitialContext.db.Contracts.Where(i => i.ID == ID).FirstOrDefault();
        }

        public static Department Get_Department(int ID)
        {
            return InitialContext.db.Departments.Where(i => i.ID == ID ).FirstOrDefault();
        }

        public static Employee Get_Employee(int ID)
        {
            return InitialContext.db.Employees.Where(i => i.ID == ID).FirstOrDefault();
        }

        public static Employee GetEmployeeByNIC(String NIC)
        {
            return InitialContext.db.Employees.Where(i => i.NIC.CompareTo(NIC) == 0).FirstOrDefault();
        }

        public static List<Category> Get_Categories()
        {
            return InitialContext.db.Categories.ToList();
        }
        public static List<Producer> Get_Producers()
        {
            return InitialContext.db.Producers.ToList();
        }
        public static List<Product> Get_Products()
        {
            return InitialContext.db.Products.ToList();
        }
        public static List<Order> Get_Orders()
        {
            return InitialContext.db.Orders.ToList();
        }

        public static List<Contract> Get_Contracts()
        {
            return InitialContext.db.Contracts.ToList();
        }

        public static List<Employee> Get_Employees()
        {
            return InitialContext.db.Employees.ToList();
        }

        public static List<Department> Get_Departments()
        {
            return InitialContext.db.Departments.ToList();
        }

        public static Employee Login(String UserName,String Password)
        {
            return InitialContext.db.Employees.Where(item => (item.NIC.CompareTo(UserName) == 0 ||
            (!String.IsNullOrEmpty(item.Email) && item.Email.CompareTo(UserName) == 0))
            && item.Password.CompareTo(Password) == 0).FirstOrDefault();
        }

        public static OrderInfo GetOrderInfo(string oid,string pid)
        {
            return InitialContext.db.ProductsOrder.Where(item =>
            item.Order_ID.CompareTo(oid) == 0
            && item.Product_ID.CompareTo(pid) == 0).FirstOrDefault();
        }

        public static List<OrderInfo> GetOIs_ByOrder(String OrderID)
        {
            return InitialContext.db.ProductsOrder.Where(item => item.Order_ID.CompareTo(OrderID) == 0).ToList();
        }

        public static List<OrderInfo> GetOIs_ByProduct(String Product)
        {
            return InitialContext.db.ProductsOrder.Where(item => item.Product_ID.CompareTo(Product) == 0).ToList();
        }

        public static List<OrderInfo> GetAllOrdersInfo()
        {
            return InitialContext.db.ProductsOrder.ToList();
        }

        public static Pharmacy GetPharmacy()
        {
            return InitialContext.db.Pharmacy.FirstOrDefault();
        }
    }
}

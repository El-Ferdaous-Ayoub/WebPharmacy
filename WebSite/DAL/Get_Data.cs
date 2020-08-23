using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes;
namespace DAL
{
    public class Get_Data
    {
        public static Category Get_Category(String Name)
        {
            return Initial_Context.db.Categories.Where(i => i.Name.CompareTo(Name) == 0).FirstOrDefault();
        }

        public static Producer Get_Producer(String Name)
        {
            return Initial_Context.db.Producers.Where(i => i.Name.CompareTo(Name) == 0).FirstOrDefault();
        }

        public static Product Get_Product(String ID)
        {
            return Initial_Context.db.Products.Where(i => i.ID.CompareTo(ID) == 0).FirstOrDefault();
        }

        public static Order Get_Order(String ID)
        {
            return Initial_Context.db.Orders.Where(i => i.ID.CompareTo(ID) == 0).FirstOrDefault();
        }

        public static Contract Get_Contract(String NIC)
        {
            return Initial_Context.db.Contracts.Where(i => i.Employee_NIC.CompareTo(NIC) == 0).FirstOrDefault();
        }

        public static Contract_Type Get_Contract_Type(String Title)
        {
            return Initial_Context.db.Contract_Types.Where(i => i.Title.CompareTo(Title) == 0).FirstOrDefault();
        }

        public static Department Get_Department(String Title)
        {
            return Initial_Context.db.Departments.Where(i => i.Title.CompareTo(Title) == 0).FirstOrDefault();
        }

        public static Employee Get_Employee(String NIC)
        {
            return Initial_Context.db.Employees.Where(i => i.ID.CompareTo(NIC) == 0).FirstOrDefault();
        }

        public static User GetUserByNIC(String NIC)
        {
            return Initial_Context.db.Users.Where(i => i.NIC.CompareTo(NIC) == 0).FirstOrDefault();
        }

        public static User Get_User(String UserName)
        {
            return Initial_Context.db.Users.Where(i => i.UserName.CompareTo(UserName) == 0).FirstOrDefault();
        }

        public static List<Category> Get_Categories()
        {
            return Initial_Context.db.Categories.ToList();
        }
        public static List<Producer> Get_Producers()
        {
            return Initial_Context.db.Producers.ToList();
        }
        public static List<Product> Get_Products()
        {
            return Initial_Context.db.Products.ToList();
        }
        public static List<Order> Get_Orders()
        {
            return Initial_Context.db.Orders.ToList();
        }

        public static List<Contract> Get_Contracts()
        {
            return Initial_Context.db.Contracts.ToList();
        }

        public static List<Contract_Type> Get_Contract_Types()
        {
            return Initial_Context.db.Contract_Types.ToList();
        }

        public static List<Employee> Get_Employees()
        {
            return Initial_Context.db.Employees.ToList();
        }

        public static List<User> Get_Users()
        {
            return Initial_Context.db.Users.ToList();
        }

        public static List<Department> Get_Departments()
        {
            return Initial_Context.db.Departments.ToList();
        }

        public static User Login(String UserName,String Password)
        {
            return Initial_Context.db.Users.Where(item => (item.UserName.CompareTo(UserName) == 0 ||
            (!String.IsNullOrEmpty(item.Email) && item.Email.CompareTo(UserName) == 0))
            && item.Password.CompareTo(Password) == 0).FirstOrDefault();
        }

        public static OrderInfo GetOrderInfo(String Order,String Product)
        {
            return Initial_Context.db.ProductsOrder.Where(item =>
            item.Product_ID.CompareTo(Product) == 0 &&
            item.Order_ID.CompareTo(Order) == 0).FirstOrDefault();
        }

        public static List<OrderInfo> GetOIs_ByOrder(String OrderID)
        {
            return Initial_Context.db.ProductsOrder.Where(item => item.Order_ID.CompareTo(OrderID) == 0).ToList();
        }

        public static List<OrderInfo> GetOIs_ByProduct(String Product)
        {
            return Initial_Context.db.ProductsOrder.Where(item => item.Product_ID.CompareTo(Product) == 0).ToList();
        }

        public static List<OrderInfo> GetAllOrdersInfo()
        {
            return Initial_Context.db.ProductsOrder.ToList();
        }

        public static OrderDone GetOrderDone(String ID)
        {
            return Initial_Context.db.OrdersDone.Where(item => item.ID.CompareTo(ID) == 0).FirstOrDefault();
        }

        public static Role GetRole(String Department)
        {
            return Initial_Context.db.Roles.Where(item => item.Department_Title.CompareTo(Department) == 0).FirstOrDefault();
        }

        public static Pharmacy GetPharmacy()
        {
            return Initial_Context.db.Pharmacy.FirstOrDefault();
        }
    }
}

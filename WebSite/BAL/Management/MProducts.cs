using Classes;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class MProducts
    {
        public Product Get(String ID)
        {
            return Get_Data.Get_Product(ID);
        }

        public List<Product> Get_All()
        {
            return Get_Data.Get_Products().ToList();
        }

        public void Add(Product product)
        {
            if (Get(product.ID) != null) throw new Exception($"This Prodcut ({product.ID}) is Aready Exist");
            Management.Add(product);
        }

        public void Update(Product product)
        {
            Product org = Get(product.ID);
            if (org == null) throw new Exception($"Product ({product.ID}) is Not Exist");
            Management.Detach(org);
            Management.Update(product);
        }

        public void Delete(String ID)
        {
            Product org = Get(ID);
            if (org == null) throw new Exception($"Product ({ID}) is Not Exist");
            Management.Remove(org);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Classes;
using DAL;

namespace BAL
{
    public class MCategories
    {
        public Category Get(String Name)
        {
            return Get_Data.Get_Category(Name);
        }

        public List<Category> Get_All()
        {
            return Get_Data.Get_Categories().ToList();
        }

        public void Add(Category category)
        {
            if (Get(category.Name) != null) throw new Exception($"This Category ({category.Name}) is Aready Exist");
            Management.Add(category);
        }

        public void Update(Category category)
        {
            Category org = Get(category.Name);
            if (org == null) throw new Exception($"Category {category.Name} is Not Exist");
            Management.Detach(org);
            Management.Update(category);
        }

        public void Delete(String Name)
        {
            Category org = Get(Name);
            if (org == null) throw new Exception($"Category {Name} is Not Exist");
            var mp = new MProducts();
            foreach (var item in mp.Get_All())
            {
                if (item.Category_Name.CompareTo(Name) == 0)
                {
                    item.Category_Name = "Unknown";
                    mp.Update(item);
                }
            }
            Management.Remove(org);
        }

        public void NewName(String orgname,Category category)
        {
            Category org = Get(orgname);
            if (org == null) throw new Exception($"Category {orgname} is Not Exist");
            Add(category);
            var mp = new MProducts();
            foreach (var item in mp.Get_All())
            {
                if (item.Category_Name.CompareTo(orgname) == 0)
                {
                    item.Category_Name = category.Name;
                    mp.Update(item);
                }
            }
            Management.Remove(org);
        }
    }
}

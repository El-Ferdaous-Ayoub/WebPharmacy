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
        public Category Get(int ID)
        {
            return Get_Data.Get_Category(ID);
        }

        public Category Get(String Name)
        {
            return Get_Data.Get_Categories().Where(item => item.Name.CompareTo(Name) == 0).FirstOrDefault();
        }

        public List<Category> Get_All()
        {
            return Get_Data.Get_Categories().ToList();
        }

        public void Add(Category category)
        {
            if (Get(category.ID) != null) throw new Exception($"This Category ({category.Name}) is Aready Exist");
            Management.Add(category);
        }

        public void Update(Category category)
        {
            Category org = Get(category.ID);
            if (org == null) throw new Exception($"Category {category.Name} is Not Exist");
            var cat = Get(category.Name);
            if (cat != null && cat.ID != org.ID)
                throw new Exception($"Category {category.Name} is Aready Exist");
            Management.Detach(org);
            Management.Update(category);
        }

        public void Delete(int ID)
        {
            Category org = Get(ID);
            if (org == null) throw new Exception($"Category Not Exist");
            var mp = new MProducts();
            var uc = GetUNCategory().ID;
            foreach (var item in mp.Get_All())
            {
                if (item.Category_ID == ID)
                {
                    item.Category_ID = uc;
                    mp.Update(item);
                }
            }
            Management.Remove(org);
        }

        public Category GetUNCategory()
        {
            return Get_All().
                Where(i => i.Name.CompareTo("Unknown") == 0).FirstOrDefault();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Classes;
using DAL;

namespace BAL
{
   public class MProducers
    {
        public Producer Get(String Name)
        {
            return Get_Data.Get_Producer(Name);
        }

        public List<Producer> Get_All()
        {
            return Get_Data.Get_Producers().ToList();
        }

        public void Add(Producer producer)
        {
            if (Get(producer.Name) != null) throw new Exception($"This Producer ({producer.Name}) is Aready Exist");
            Management.Add(producer);
        }

        public void Update(Producer producer)
        {
            Producer org = Get(producer.Name);
            if (org == null) throw new Exception($"Producer ({producer.Name}) is Not Exist");
            Management.Detach(org);
            Management.Update(producer);
        }

        public void Delete(String Name)
        {
            Producer org = Get(Name);
            if (org == null) throw new Exception($"Producer ({Name}) is Not Exist");
            var mp = new MProducts();
            foreach (var item in mp.Get_All())
            {
                if (item.Producer_Name.CompareTo(Name) == 0)
                {
                    item.Producer_Name = "Unknown";
                    mp.Update(item);
                }
            }
            Management.Remove(Name);
        }

        public void NewName(String orgname,Producer producer)
        {
            Producer org = Get(orgname);
            if (org == null) throw new Exception($"Producer ({orgname}) is Not Exist");
            Add(producer);
            var mp = new MProducts();
            foreach (var item in mp.Get_All())
            {
                if (item.Producer_Name.CompareTo(orgname) == 0)
                {
                    item.Producer_Name = producer.Name;
                    mp.Update(item);
                }
            }
            Management.Remove(org);
        }
    }
}

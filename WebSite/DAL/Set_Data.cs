using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace DAL
{
    public class Management
    {
        public static void Add(Object O)
        {
            Initial_Context.db.Entry(O).State = EntityState.Added;
            Save();
        }

        public static void Remove(Object O)
        {
            Initial_Context.db.Entry(O).State = EntityState.Deleted;
            Save();
        }

        public static void Update(Object O)
        {
            Initial_Context.db.Entry(O).State = EntityState.Modified;
            Save();
        }

        public static void Detach(Object O)
        {
            Initial_Context.db.Entry(O).State = EntityState.Detached;
        }

        public static void Save()
        {
                Initial_Context.db.SaveChanges();
        }
    }
}

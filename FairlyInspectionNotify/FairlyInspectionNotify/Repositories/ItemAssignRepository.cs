using FairlyInspectionNotify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairlyInspectionNotify.Repositories
{
    public class ItemAssignRepository : GenericRepository<ItemAssign>
    {
        public ItemAssignRepository(InspectionEntities context) : base(context) { }
        private InspectionEntities db = new InspectionEntities();

        public IQueryable<ItemAssign> Query(int itemId = 0, string personID = "")
        {
            var query = GetAll();
            if (itemId != 0)
            {
                query = query.Where(q => q.ItemID == itemId);
            }
            if (!string.IsNullOrEmpty(personID))
            {
                query = query.Where(q => q.PersonID == personID);
            }

            return query;
        }

        public ItemAssign FindBy(int id)
        {
            return db.ItemAssign.Find(id);
        }

        //public void DeleteFile(int id)
        //{
        //    ItemAssign data = FindBy(id);

        //    db.SaveChanges();
        //}
    }
}

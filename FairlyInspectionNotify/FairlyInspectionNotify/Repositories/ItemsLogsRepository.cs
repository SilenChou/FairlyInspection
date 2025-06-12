using FairlyInspectionNotify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairlyInspectionNotify.Repositories
{
    public class ItemLogsRepository : GenericRepository<ItemLogs>
    {
        public ItemLogsRepository(InspectionEntities context) : base(context) { }
        private InspectionEntities db = new InspectionEntities();

        public IQueryable<ItemLogs> Query(int itemId = 0, int creater = 0)
        {
            var query = GetAll();
            if (itemId != 0)
            {
                query = query.Where(q => q.ItemID == itemId);
            }
            if (creater != 0)
            {
                query = query.Where(q => q.Creater == creater);
            }

            return query;
        }

        public ItemLogs FindBy(int id)
        {
            return db.ItemLogs.Find(id);
        }

        public void DeleteFile(int id)
        {
            ItemLogs data = FindBy(id);

            db.SaveChanges();
        }
    }
}

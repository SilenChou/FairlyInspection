using FairlyInspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Repositories
{
    public class ItemsLogsImageRepository : GenericRepository<ItemLogsImage>
    {
        public ItemsLogsImageRepository(InspectionEntities context) : base(context) { }
        private InspectionEntities db = new InspectionEntities();

        public IQueryable<ItemLogsImage> Query(int lid = 0)
        {
            var query = GetAll();
            if (lid != 0)
            {
                query = query.Where(q => q.LogID == lid);
            }

            return query;
        }

        public ItemLogsImage FindBy(int id)
        {
            return db.ItemLogsImage.Find(id);
        }

        public void DeleteFile(int id)
        {
            ItemLogsImage data = FindBy(id);

            db.SaveChanges();
        }
    }
}
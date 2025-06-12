using FairlyInspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Repositories
{
    public class CheckingItemLogsImageRepository : GenericRepository<CheckingItemLogsImage>
    {
        public CheckingItemLogsImageRepository(InspectionEntities context) : base(context) { }
        private InspectionEntities db = new InspectionEntities();

        public IQueryable<CheckingItemLogsImage> Query(int logId = 0)
        {
            var query = GetAll();

            if (logId != 0)
            {
                query = query.Where(q => q.LogID == logId);
            }

            return query;
        }

        public CheckingItemLogsImage FindBy(int id)
        {
            return db.CheckingItemLogsImage.Find(id);
        }
    }
}
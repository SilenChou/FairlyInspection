using FairlyInspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Repositories
{
    public class CheckingItemLogRepository : GenericRepository<CheckingItemLog>
    {
        public CheckingItemLogRepository(InspectionEntities context) : base(context) { }
        private InspectionEntities db = new InspectionEntities();

        public IQueryable<CheckingItemLog> Query(bool? isCompleted = null, int pathId = 0, int pointId = 0, int itemId = 0, int pathLogId = 0, int pointLogId = 0)
        {
            var query = GetAll();

            if (isCompleted.HasValue)
            {
                query = query.Where(q => q.IsCompleted == isCompleted);
            }
            if (pathId != 0)
            {
                query = query.Where(q => q.PathID == pathId);
            }
            if (pointId != 0)
            {
                query = query.Where(q => q.PointID == pointId);
            }
            if (itemId != 0)
            {
                query = query.Where(q => q.ItemID == itemId);
            }
            if (pathLogId != 0)
            {
                query = query.Where(q => q.PathLogID == pathLogId);
            }
            if (pointLogId != 0)
            {
                query = query.Where(q => q.PointLogID == pointLogId);
            }

            return query;
        }

        public CheckingItemLog FindBy(int id)
        {
            return db.CheckingItemLog.Find(id);
        }
    }
}
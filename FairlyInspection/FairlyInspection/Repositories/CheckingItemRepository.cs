using FairlyInspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Repositories
{
    public class CheckingItemRepository : GenericRepository<CheckingItem>
    {
        public CheckingItemRepository(InspectionEntities context) : base(context) { }
        private InspectionEntities db = new InspectionEntities();

        public IQueryable<CheckingItem> Query(bool? status = null, int pathId = 0, int pointId = 0, string title = "")
        {
            var query = GetAll();

            if (status.HasValue)
            {
                query = query.Where(q => q.IsOnline == status);
            }
            if (pathId != 0)
            {
                query = query.Where(q => q.PathID == pathId);
            }
            if (pointId != 0)
            {
                query = query.Where(q => q.PointID == pointId);
            }
            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(q => q.Title.Contains(title));
            }         

            return query;
        }

        public CheckingItem FindBy(int id)
        {
            return db.CheckingItem.Find(id);
        }
    }
}
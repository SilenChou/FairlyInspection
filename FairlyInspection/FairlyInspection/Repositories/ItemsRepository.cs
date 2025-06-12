using FairlyInspection.Models;
using FairlyInspection.Models.Join;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Repositories
{
    public class ItemsRepository : GenericRepository<Items>
    {
        public ItemsRepository(InspectionEntities context) : base(context) { }
        private InspectionEntities db = new InspectionEntities();

        public IQueryable<ItemsJoinLogs >ItemsJoinLogsQuery(int parendId = 0, int level = 0, bool getNearly = false, int id = 0)
        {

            var logsList = db.ItemLogs;

            var query = from l in logsList
                        join i in db.Items
                        on l.ItemID equals i.ID
                        where i.IsOnline
                        select new ItemsJoinLogs()
                        {
                            //ID = l.ID,
                            ItemID = i.ID,
                            ItemSystem = l.ItemSystem,
                            ItemTitle = i.Title,
                            Level = l.Level,
                            //CheckResult = l.CheckResult,
                            //MainPic = l.MainPic,
                            //Description = l.Description,
                            IsLatest = l.IsLatest,
                            CreateDate = l.CreateDate,
                            NextCheckDate = l.NextCheckDate,
                            Creater = l.ID,
                            ParentID = i.ParentID,
                            Interval = i.Interval,
                            IsOnline = i.IsOnline,                          
                        };
            if (parendId != 0)
            {
                query = query.Where(q => q.ParentID == parendId);
            }
            if (level != 0)
            {
                query = query.Where(q => q.Level == level);
            }
            var odpa = query.ToList();
            if (getNearly)
            {
                DateTime checkEnd = DateTime.Now.AddDays(2);
                query = query.Where(q => q.NextCheckDate <= checkEnd && q.IsLatest);
                odpa = query.ToList();
            }
            if (id != 0)
            {
                query = query.Where(q => q.ItemID == id);
            }
            return query;
        }

        public IQueryable<Items> Query(bool? status = null, string title = "", int level = 0, int parent = 0, int getUpperLevel = 0)
        {
            var query = GetAll();

            if (status.HasValue)
            {
                query = query.Where(q => q.IsOnline == status);
            }
            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(q => q.Title.Contains(title));
            }
            if (level != 0)
            {
                if (getUpperLevel != 0)
                {
                    int upperLevel = level - getUpperLevel;
                    query = query.Where(q => q.Level <= upperLevel);
                }
                else
                {
                    query = query.Where(q => q.Level == level);
                }
            }
            if (parent != 0)
            {
                query = query.Where(q => q.ParentID == parent);
            }

            return query;
        }

        public Items FindBy(int id)
        {
            return db.Items.Find(id);
        }

        //public void Delete(int id)
        //{
        //    Items data = FindBy(id);

        //    db.SaveChanges();
        //}
    }
}
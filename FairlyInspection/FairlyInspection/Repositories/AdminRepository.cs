using FairlyInspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Repositories
{
    public class AdminRepository : GenericRepository<Admin>
    {
        //private FairlyInspectionDBEntity db;

        //public AdminRepository() : this(null) { }

        //public AdminRepository(FairlyInspectionDBEntity context)
        //{
        //    db = context ?? new FairlyInspectionDBEntity();
        //}
        T8ERPEntities t8db = new T8ERPEntities();

        public AdminRepository(InspectionEntities context) : base(context) { }

        public IQueryable<Admin> Query(bool? status, int type = 0, string account = "")
        {
            var query = GetAll();
            if (status.HasValue)
            {
                query = query.Where(p => p.Status == status);
            }
            if (type != 0)
            {
                query = query.Where(q => q.Type == type);
            }
            if (!string.IsNullOrEmpty(account))
            {
                query = query.Where(q => q.Account.Contains(account));
            }
            return query;
        }

        public Admin Login(string account, string password)
        {
            Admin admin = GetAll().Where(p => p.Account == account && p.Password == password && p.Status).FirstOrDefault();
            if (admin == null && GetAll().Where(p => p.Account == account && p.Password == password).Count() <= 0)
            {
                if (!int.TryParse(password, out _))
                {
                    return null;
                }
                int intPassword = Convert.ToInt32(password);
                var t8GroupPerson = t8db.comGroupPerson.Where(q => q.PersonId == account && q.Birthday == intPassword).FirstOrDefault();
                if (t8GroupPerson != null)
                {
                    admin = new Admin
                    {
                        Name = t8GroupPerson.PersonName,
                        Account = t8GroupPerson.PersonId,
                        Password = t8GroupPerson.Birthday.ToString(),
                        Type = (int)AdminType.Inspector,
                        Department = 0,
                        Status = true,
                        UpdateDate = DateTime.Now,
                        Updater = 1,
                        CreateDate = DateTime.Now,
                        Creater = 1,
                    };
                    int Id = Insert(admin);
                }
            }
            return admin;
        }
    }
}
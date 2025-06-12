using FairlyInspection.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Repositories.T8Repositories
{
    public class GroupPersonRepository : GenericRepository<comGroupPerson>
    {
        public GroupPersonRepository(T8ERPEntities context) : base(context) { }
        private T8ERPEntities t8db = new T8ERPEntities();
        //private InspectionEntities db = new InspectionEntities();
        public IQueryable<comGroupPerson> Query()
        {
            var query = GetAll();

            return query;
        }

        public comGroupPerson FindByJobID(string jobId)
        {
            return t8db.comGroupPerson.Find(jobId);
        }
    }
}
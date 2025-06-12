using FairlyInspectionNotify.Models;
using FairlyInspectionNotify.Models.T8.Join;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairlyInspectionNotify.Repositories.T8Repositories
{
    public class PersonRepository : GenericRepository<comPerson>
    {
        public PersonRepository(T8ERPEntities context) : base(context) { }
        private T8ERPEntities t8db = new T8ERPEntities();
        private InspectionEntities db = new InspectionEntities();

        public IQueryable<GroupJoinPersonAndDepartment> GetPersonQuery(string name = "", string jobID = "", string deptId = "")
        {
            var query = from g in t8db.comGroupPerson
                        join p in t8db.comPerson
                        on g.PersonId equals p.PersonId
                        join d in t8db.comDepartment
                        on p.DeptId equals d.DeptId
                        where p.ServiceStatus != "40"
                        select new GroupJoinPersonAndDepartment()
                        {
                            JobID = g.PersonId,
                            Name = g.PersonName,
                            EngName = g.EngName,
                            DeptID = d.DeptId,
                            DepartmentStr = d.DeptName,
                            ParentDeptID = d.ParentDeptId,
                            InductionDate = p.InductionDate,
                            WorkAge = p.WorkAge,
                            WorkMonths = 0,
                            Birthday = g.Birthday,
                            Age = 0,
                            Email = g.EMail,
                            MSNNo = g.MSNNo,
                            Phone = g.Phone,
                            Gender = g.Sex == 0 ? "男" : "女",
                            EyeState = g.EyeState,
                            PositionId = p.PositionId
                        };
         
            if (!string.IsNullOrEmpty(deptId))
            {
                query = query.Where(q => q.DeptID == deptId);
            }
            if (!string.IsNullOrEmpty(jobID))
            {
                query = query.Where(q => q.JobID.Contains(jobID));
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(q => q.Name.Contains(name) || q.EngName.Contains(name));
            }

            return query;
        }

        public IQueryable<comPerson> Query()
        {
            var query = GetAll();

            return query;
        }

        public IQueryable<comGroupPerson> GroupPersonQuery()
        {
            var query = t8db.comGroupPerson;

            return query;
        }

        public comPerson FindBy(int id)
        {
            return t8db.comPerson.Find(id);
        }

        public GroupJoinPersonAndDepartment FindByJobID(string jid)
        {
            var temp = GetPersonQuery().ToList();
            return temp.Where(q => q.JobID == jid).FirstOrDefault();
        }

        public IQueryable<GroupJoinPersonAndDepartment> GetSelected(string selected)
        {
            var selectedList = selected.Split(',');
            return GetPersonQuery().Where(q => selectedList.Contains(q.JobID));
        }

        public int Age(DateTime bday)
        {
            DateTime now = DateTime.Today;
            int age = now.Year - bday.Year;
            if (now < bday.AddYears(age))
                age--;
            return age;
        }
    }
}

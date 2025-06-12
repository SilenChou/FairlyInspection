using FairlyInspection.Models;
using FairlyInspection.Models.T8.Join;
using FairlyInspection.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Repositories.T8Repositories
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
                            InductionDate = p.InductionDate,
                            WorkAge = p.WorkAge,
                            WorkMonths = 0,
                            Birthday = g.Birthday,
                            Age = 0,
                            Email = g.EMail,
                            MSNNo = g.MSNNo
                        };

            //var quertList = query.ToArray();
            //foreach (var item in quertList)
            //{
            //    var departmentInfo = db.Department.Where(q => q.Title == item.DepartmentStr).FirstOrDefault();
            //    if (departmentInfo == null)
            //    {
            //        DepartmentRepository departmentRepository = new DepartmentRepository(new InspectionEntities());
            //        departmentRepository.Insert(new Department()
            //        {
            //            Title = item.DepartmentStr,
            //            Status = 1,
            //            IsForeign = false,
            //        });
            //        departmentInfo = db.Department.Where(q => q.Title == item.DepartmentStr).FirstOrDefault();
            //    }
               
            //    item.DepartmentID = departmentInfo.ID;

            //    item.WorkAge = Convert.ToInt32(Math.Round(Convert.ToDouble(item.WorkAge / 12)));
            //    item.WorkMonths = item.WorkAge % 12;
            //    item.Age = Age(DateTime.ParseExact(item.Birthday.ToString(), "yyyyMMdd", null, System.Globalization.DateTimeStyles.AllowWhiteSpaces));
            //}

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

        public IQueryable<comDepartment> GetDepartments()
        {
            var query = t8db.comDepartment;

            return query;
        }
        public comDepartment FindDepartments(string deptId)
        {
            var query = t8db.comDepartment.Find(deptId);

            return query;
        }

        public IQueryable<comPerson> Query()
        {
            var query = GetAll();
        
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
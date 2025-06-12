using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Models.T8.Join
{
    public class GroupJoinPersonAndDepartment
    {
        //public string PersonId { get; set; }
        //public string PersonName { get; set; }
        //public string DeptName { get; set; }
        //public int InductionDate { get; set; }
        //public double WorkAge { get; set; }
        //public int WorkMonths { get; set; }
        //public int Birthday { get; set; }
        //public int Age { get; set; }
        //public string EMail { get; set; }
        //public string MSNNo { get; set; }

        public int ID { get; set; }
        public int DepartmentID { get; set; }
        public string DeptID { get; set; }
        //DeptName
        public string DepartmentStr { get; set; }
        //PersonId
        public string JobID { get; set; }
        //PersonName
        public string Name { get; set; }
        public string EngName { get; set; }
        //EMail
        public string Email { get; set; }
        public int InductionDate { get; set; }
        public int WorkAge { get; set; }
        public int WorkMonths { get; set; }
        public int Birthday { get; set; }
        public int Age { get; set; }
        public string MSNNo { get; set; }
    }
}
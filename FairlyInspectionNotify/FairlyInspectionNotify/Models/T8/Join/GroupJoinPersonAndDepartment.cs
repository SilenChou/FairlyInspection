using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairlyInspectionNotify.Models.T8.Join
{
    public class GroupJoinPersonAndDepartment
    {
        public int ID { get; set; }
        public int DepartmentID { get; set; }
        public string DeptID { get; set; }
        //DeptName
        public string DepartmentStr { get; set; }
        public string ParentDeptID { get; set; }
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
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string EyeState { get; set; }
        public string PositionId { get; set; }
    }
}

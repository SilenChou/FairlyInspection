//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FairlyInspection.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class comDepartment
    {
        public string DeptId { get; set; }
        public string DeptShortName { get; set; }
        public string DeptName { get; set; }
        public string DepartmentCategory { get; set; }
        public int ValidityFromDate { get; set; }
        public int ValidityToDate { get; set; }
        public string JobScheduleId { get; set; }
        public int Lv { get; set; }
        public string ParentDeptId { get; set; }
        public string RootDeptId { get; set; }
        public int DeptOrder { get; set; }
        public string CreatorId { get; set; }
        public long CreateTime { get; set; }
        public string LastOperatorId { get; set; }
        public long LastOperateTime { get; set; }
        public string Remark { get; set; }
        public short AttachementCount { get; set; }
        public long InternalId { get; set; }
        public bool HasPostil { get; set; }
        public short TimesPrinted { get; set; }
        public short TimesEmailSend { get; set; }
        public short TimesExported { get; set; }
        public string PermitterId { get; set; }
        public long PermitTime { get; set; }
        public byte PermitState { get; set; }
        public byte FlowState { get; set; }
        public byte VaryState { get; set; }
        public byte MergeOutState { get; set; }
        public bool IsInUsed { get; set; }
        public string AuthorizedId { get; set; }
        public byte CurrentState { get; set; }
        public string MnemonicId { get; set; }
        public string OrgId { get; set; }
        public string LeaderId { get; set; }
        public string AttCalendarId { get; set; }
        public string AttRangeId { get; set; }
        public string ViceLeaderId { get; set; }
        public int DesignQty { get; set; }
        public int DesignQtyFemale { get; set; }
        public bool ActiveSalSum { get; set; }
        public string PresetCostCenterId { get; set; }
        public byte UserRelDisplayType { get; set; }
        public string FeeAcctAttrId { get; set; }
        public string EMail { get; set; }
        public string LvId { get; set; }
        public bool IsVirtualDept { get; set; }
        public string DHrmOrgId { get; set; }
        public string AttScopeId { get; set; }
        public string SalScopeId { get; set; }
        public int FIValiFromDate { get; set; }
        public int FIValiToDate { get; set; }
        public string LeaderHrmOrgId { get; set; }
        public string ViceLeaderHrmOrgId { get; set; }
        public string UpdateKey { get; set; }
        public string SumDeptId { get; set; }
    }
}

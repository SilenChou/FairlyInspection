using FairlyInspection.Models.Others;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FairlyInspection.Areas.Admin.ViewModels.CheckingPathLog
{
    public class CheckingPathLogIndexView : PageQuery
    {
        public CheckingPathLogIndexView()
        {
            this.Sorting = "CreateDate";
            this.IsDescending = true;
        }

        public PageResult<CheckingPathLogView> PageResult { get; set; }

        [Display(Name = "路線編號")]
        public int PathID { get; set; }

        [Display(Name = "巡檢間隔單位")]
        public int IntervalType { get; set; }

        [Display(Name = "巡檢間隔")]
        public int Interval { get; set; }

        [Display(Name = "路線名稱")]
        public string Title { get; set; }

        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "工號")]
        public string PersonId { get; set; }

        [Display(Name = "完成狀態")]
        public bool? IsCompleted { get; set; }

        [Display(Name = "巡查時間")]
        public DateTime CreateDate { get; set; }
    }
}
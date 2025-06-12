using FairlyInspection.Models.Others;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FairlyInspection.Areas.Admin.ViewModels.CheckingPoint
{
    public class CheckingPointIndexView : PageQuery
    {
        public CheckingPointIndexView()
        {
            this.Sorting = "Title";
            this.IsDescending = false;
        }

        public PageResult<CheckingPointView> PageResult { get; set; }

        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "路線編號")]
        public int PathID { get; set; }

        [Display(Name = "項目名稱")]
        public string Title { get; set; }

        [Display(Name = "說明")]
        public string Description { get; set; }

        //[Display(Name = "巡檢間隔單位")]
        //public int IntervalType { get; set; }

        //[Display(Name = "巡檢間隔時間")]
        //public int Interval { get; set; }

        [Display(Name = "啟用狀態")]
        public bool? IsOnline { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreateDate { get; set; }
    }
}
using FairlyInspection.Areas.Admin.ViewModels.CheckingItem;
using FairlyInspection.Areas.Admin.ViewModels.CheckingPath;
using FairlyInspection.Areas.Admin.ViewModels.CheckingPathLog;
using FairlyInspection.Areas.Admin.ViewModels.CheckingPoint;
using FairlyInspection.Models.Others;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FairlyInspection.Areas.Admin.ViewModels.CheckingItemLog
{
    public class CheckingItemLogIndexView : PageQuery
    {
        public CheckingItemLogIndexView()
        {
            this.Sorting = "PathID";
            this.IsDescending = false;
        }

        public PageResult<CheckingItemLogView> PageResult { get; set; }

        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "路線編號")]
        public int PathID { get; set; }

        [Display(Name = "檢查點編號")]
        public int PointID { get; set; }

        [Display(Name = "細項編號")]
        public int ItemID { get; set; }

        [Display(Name = "備註")]
        public string Remark { get; set; }

        [Display(Name = "完成狀態")]
        public bool? IsCompleted { get; set; }

        [Display(Name = "巡查時間")]
        public DateTime CreateDate { get; set; }

        public CheckingPathView PathInfo{get; set;}
        public CheckingPointView PointInfo{get; set;}
        public CheckingItemView ItemInfo{get; set;}
    }
}
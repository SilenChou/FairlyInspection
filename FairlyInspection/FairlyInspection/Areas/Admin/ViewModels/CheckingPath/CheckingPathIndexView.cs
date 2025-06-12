using FairlyInspection.Models.Others;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FairlyInspection.Areas.Admin.ViewModels.CheckingPath
{
    public class CheckingPathIndexView : PageQuery
    {
        public CheckingPathIndexView()
        {
            this.Sorting = "Title";
            this.IsDescending = false;
        }

        public PageResult<CheckingPathView> PageResult { get; set; }

        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "路線編號")]
        public int PathID { get; set; }

        [Display(Name = "路線名稱")]
        public string Title { get; set; }

        [Display(Name = "說明")]
        public string Description { get; set; }

        [Display(Name = "巡檢間隔單位")]
        public int IntervalType { get; set; }

        [Display(Name = "巡檢間隔時間")]
        public int Interval { get; set; }

        [Display(Name = "調度員")]
        public string Dispatcher { get; set; }

        [Display(Name = "巡檢人員")]
        public string Inspectors { get; set; }

        [Display(Name = "啟用狀態")]
        public bool? IsOnline { get; set; }

        [Display(Name = "起始檢查時間")]
        public string StartDate { get; set; }

        [Display(Name = "預定檢查時間")]
        public string NextDate { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreateDate { get; set; }
    }
}
//using FairlyInspection.Models;
//using FairlyInspection.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
//using System.Web.Mvc;

namespace FairlyInspection.Areas.Admin.ViewModels.CheckingPoint
{
    public class CheckingPointView
    {
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
        public bool IsOnline { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "建立者")]
        public int Creater { get; set; }

        //public List<SelectListItem> TimeUnit
        //{
        //    get
        //    {
        //        List<SelectListItem> result = new List<SelectListItem>();
        //        var values = EnumHelper.GetValues<TimeUnit>();
        //        foreach (var v in values)
        //        {
        //            result.Add(new SelectListItem()
        //            {
        //                Text = EnumHelper.GetDescription(v),
        //                Value = ((int)v).ToString()
        //            });
        //        }
        //        return result;
        //    }
        //}

        [Display(Name = "排序")]
        public int Sort { get; set; }

        public List<Models.CheckingItem> ItemList { get; set; }
    }
}
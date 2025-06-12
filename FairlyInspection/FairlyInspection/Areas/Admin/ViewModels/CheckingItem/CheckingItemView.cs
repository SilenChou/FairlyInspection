using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FairlyInspection.Areas.Admin.ViewModels.CheckingItem
{
    public class CheckingItemView
    {
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "檢查點編號")]
        public int PointID { get; set; }

        [Display(Name = "路線編號")]
        public int PathID { get; set; }

        [Display(Name = "項目名稱")]
        public string Title { get; set; }

        [Display(Name = "檢查內容/說明")]
        public string Description { get; set; }

        [Display(Name = "啟用狀態")]
        public bool IsOnline { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreateDate { get; set; }
    }
}
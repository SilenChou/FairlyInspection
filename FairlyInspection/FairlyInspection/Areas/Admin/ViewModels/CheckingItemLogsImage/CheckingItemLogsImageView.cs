using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FairlyInspection.Areas.Admin.ViewModels.CheckingItemLogsImage
{
    public class CheckingItemLogsImageView
    {
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "路線編號")]
        public int PathID { get; set; }

        [Display(Name = "檢查點編號")]
        public int PointID { get; set; }

        [Display(Name = "細項編號")]
        public int ItemID { get; set; }

        [Display(Name = "記錄編號")]
        public int LogID { get; set; }

        [Display(Name = "圖片")]
        public string Pics { get; set; }
    }
}
using FairlyInspection.Areas.Admin.ViewModels.CheckingItem;
using FairlyInspection.Areas.Admin.ViewModels.CheckingItemLogsImage;
using FairlyInspection.Areas.Admin.ViewModels.CheckingPath;
using FairlyInspection.Areas.Admin.ViewModels.CheckingPoint;
using FairlyInspection.Models;
using FairlyInspection.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FairlyInspection.Areas.Admin.ViewModels.CheckingPathLog
{
    public class CheckingPathLogView
    {
        AdminRepository adminRepository = new AdminRepository(new InspectionEntities());

        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "路線編號")]
        public int PathID { get; set; }

        [Display(Name = "路線名稱")]
        public string Title { get; set; }

        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "工號")]
        public string PersonId { get; set; }

        //[Required]
        //[Display(Name = "調度員")]
        //public string DispatcherPersonID { get; set; }

        //[Display(Name = "巡檢人員")]
        //public string InspectionPersonID { get; set; }

        [Display(Name = "完成狀態")]
        public bool? IsCompleted { get; set; }

        [Display(Name = "巡查時間")]
        public DateTime CreateDate { get; set; }

        public List<CheckingPointLogView> PointLogList { get; set; }
        public CheckingPathView PathInfo { get; set; }

        //public List<SelectListItem> InspectionPersonOptions
        //{
        //    get
        //    {
        //        List<SelectListItem> result = new List<SelectListItem>();
        //        var Inspectorsvalues = adminRepository.Query(true, (int)AdminType.Inspector);
        //        var Dispatchervalues = adminRepository.Query(true, (int)AdminType.Dispatcher);
        //        foreach (var v in Dispatchervalues)
        //        {
        //            if (v.Name != "周席綸" && v.Name != "測試人")
        //            {
        //                result.Add(new SelectListItem()
        //                {
        //                    Value = v.ID.ToString(),
        //                    Text = v.Name
        //                });
        //            }
        //        }
        //        foreach (var v in Inspectorsvalues)
        //        {
        //            if (v.Name != "周席綸" && v.Name != "測試人")
        //            {
        //                result.Add(new SelectListItem()
        //                {
        //                    Value = v.ID.ToString(),
        //                    Text = v.Name
        //                });
        //            }
        //        }
        //        return result;
        //    }
        //}
    }

    public class CheckingPointLogView
    {
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "路線編號")]
        public int PathID { get; set; }

        [Display(Name = "檢查點編號")]
        public int PointID { get; set; }

        [Display(Name = "巡檢路線紀錄編號")]
        public int PathLogID { get; set; }

        [Display(Name = "完成狀態")]
        public bool IsCompleted { get; set; }

        [Display(Name = "巡查時間")]
        public DateTime CreateDate { get; set; }

        public List<CheckingItemLogView> ItemLogList { get; set; }
        public CheckingPointView PointInfo { get; set; }
    }

    public class CheckingItemLogView
    {
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "路線編號")]
        public int PathID { get; set; }

        [Display(Name = "檢查點編號")]
        public int PointID { get; set; }

        [Display(Name = "細項編號")]
        public int ItemID { get; set; }

        [Display(Name = "巡檢路線紀錄編號")]
        public int PathLogID { get; set; }

        [Display(Name = "檢查點紀錄編號")]
        public int PointLogID { get; set; }

        [Display(Name = "備註")]
        public string Remark { get; set; }

        [Display(Name = "完成狀態")]
        public bool IsCompleted { get; set; }

        [Display(Name = "巡查時間")]
        public DateTime CreateDate { get; set; }
        public CheckingItemView ItemInfo { get; set; }

        [Display(Name = "照片")]
        public string Pics { get; set; }
        public List<CheckingItemLogsImageView> PicList { get; set; }
        public List<HttpPostedFileBase> PicsFiles { get; set; }
    }
}
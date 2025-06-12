using FairlyInspection.Areas.Admin.ViewModels.Items;
using FairlyInspection.Models.Others;
using FairlyInspection.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FairlyInspection.Areas.Admin.ViewModels.ItemLogs
{
    public class ItemLogsIndexView : PageQuery
    {
        public ItemLogsIndexView()
        {
            this.Sorting = "CreateDate";
            this.IsDescending = true;
        }

        [Display(Name = "巡檢間隔時間 (天)")]
        public int Interval { get; set; }

        public PageResult<ItemLogsView> PageResult { get; set; }

        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "設備系統")]
        public int ItemSystem { get; set; }

        [Display(Name = "項目編號")]
        public int ItemID { get; set; }

        [Display(Name = "項目名稱")]
        public string ItemTitle { get; set; }

        [Display(Name = "層級")]
        public int Level { get; set; }

        [Display(Name = "檢查結果")]
        public int CheckResult { get; set; }

        [Display(Name = "照片")]
        public bool MainPic { get; set; }

        [Display(Name = "說明")]
        public string Description { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "建立者")]
        public int Creater { get; set; }

        public List<BreadItem> ParentBreadList { get; set; }

        //public List<SelectListItem> AdminTypeOptions
        //{
        //    get
        //    {
        //        List<SelectListItem> result = new List<SelectListItem>();
        //        var values = EnumHelper.GetValues<AdminType>();
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
    }
}
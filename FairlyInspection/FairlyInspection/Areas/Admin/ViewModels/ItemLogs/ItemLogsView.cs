using FairlyInspection.Areas.Admin.ViewModels.Items;
using FairlyInspection.Models;
using FairlyInspection.Repositories;
using FairlyInspection.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FairlyInspection.Areas.Admin.ViewModels.ItemLogs
{
    public class ItemLogsView
    {
        AdminRepository adminRepository = new AdminRepository(new InspectionEntities());

        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "設備系統")]
        public int ItemSystem { get; set; }

        [Required]
        [Display(Name = "項目編號")]
        public int ItemID { get; set; }

        [Required]
        [Display(Name = "項目名稱")]
        public string ItemTitle { get; set; }

        [Display(Name = "檢查內容/說明")]
        public string ItemDescription { get; set; }

        [Required]
        [Display(Name = "層級")]
        public int Level { get; set; }

        [Required]
        [Display(Name = "檢查結果")]
        public int CheckResult { get; set; }

        [Display(Name = "照片")]
        public string MainPic { get; set; }

        [Display(Name = "照片")]
        public string Pics { get; set; }
        public List<ItemLogsImage> PicList { get; set; }
        public List<HttpPostedFileBase> PicsFiles { get; set; }

        [Display(Name = "巡檢紀錄")]
        public string Description { get; set; }

        [Display(Name = "最新紀錄")]
        public bool IsLatest { get; set; }

        [Required]
        [Display(Name = "建立時間")]
        public DateTime CreateDate { get; set; }

        [Required]
        [Display(Name = "建立者")]
        public int Creater { get; set; }
        public string CreaterStr
        {
            get
            {
                return adminRepository.GetById(this.Creater).Name;
            }
        }

        public List<BreadItem> ParentBreadList { get; set; }

        public List<SelectListItem> CheckResultOptions
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>();
                var values = EnumHelper.GetValues<LogCheckResult>();
                foreach (var v in values)
                {
                    result.Add(new SelectListItem()
                    {
                        Text = EnumHelper.GetDescription(v),
                        Value = ((int)v).ToString()
                    });
                }
                return result;
            }
        }
    }
}
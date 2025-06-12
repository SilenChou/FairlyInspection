using FairlyInspection.Models;
using FairlyInspection.Repositories;
using FairlyInspection.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FairlyInspection.Areas.Admin.ViewModels.Items
{
    public class ItemsTreeView
    {
        ItemsRepository itemsRepository = new ItemsRepository(new InspectionEntities());
        AdminRepository adminRepository = new AdminRepository(new InspectionEntities());

        public ItemsTreeView()
        {
            this.Interval = 14;
            this.Level = 1;
            this.MinLevel = true;
        }
        public bool isLight { get; set; }
        public bool isLatest { get; set; }

        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "項目名稱")]
        public string Title { get; set; }

        [Display(Name = "檢查內容/說明")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "父層編號")]
        public int ParentID { get; set; }
        public string ParentStr
        {
            get
            {
                if (ParentID == 0)
                {
                    return "菲力工業股份有限公司";
                }
                return itemsRepository.FindBy(this.ParentID).Title;
            }
        }

        [Required]
        [Display(Name = "層級")]
        public int Level { get; set; }
        public string LevelStr
        {
            get
            {
                return EnumHelper.GetDescription((LevelClass)this.Level);
            }
        }

        [Display(Name = "巡檢間隔時間 (天)")]
        public bool MinLevel { get; set; }

        [Required]
        [Display(Name = "巡檢間隔時間 (天)")]
        public int Interval { get; set; }

        [Required]
        [Display(Name = "啟用狀態")]
        public bool IsOnline { get; set; }

        [Required]
        [Display(Name = "更新時間")]
        public DateTime UpdateDate { get; set; }

        [Required]
        [Display(Name = "更新者")]
        public int Updater { get; set; }

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

        public bool IsSelected { get; set; }

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
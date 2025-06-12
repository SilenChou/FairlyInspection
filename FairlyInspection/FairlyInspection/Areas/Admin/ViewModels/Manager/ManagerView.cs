using FairlyInspection.Areas.Admin.ViewModels.Items;
using FairlyInspection.Models;
using FairlyInspection.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FairlyInspection.Areas.Admin.ViewModels.Manager
{
    public class ManagerView
    {
        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "名稱")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Required]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "類型")]
        public int Type { get; set; }

        [Required]
        [Display(Name = "啟用狀態")]
        public bool Status { get; set; }

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

        [Display(Name = "巡檢分派")]
        public List<Models.ItemAssign> AssignList { get; set; }
        public List<ItemsView> AllSystemList { get; set; }
        public string newAssignList { get; set; }

        public List<SelectListItem> AdminTypeOptions
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>();
                var values = EnumHelper.GetValues<AdminType>();
                foreach (var v in values)
                {
                    if ((int)v != 1)
                    {
                        result.Add(new SelectListItem()
                        {
                            Text = EnumHelper.GetDescription(v),
                            Value = ((int)v).ToString()
                        });
                    }                 
                }
                return result;
            }
        }
    }
}
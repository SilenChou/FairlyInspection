using FairlyInspection.Models;
using FairlyInspection.Models.Others;
using FairlyInspection.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FairlyInspection.Areas.Admin.ViewModels.Manager
{
    public class ManagerIndexView : PageQuery
    {
        public ManagerIndexView()
        {
            this.Sorting = "Type";
            this.IsDescending = false;
        }

        public PageResult<ManagerView> PageResult { get; set; }

        [Required]
        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Required]
        [Display(Name = "類型")]
        public int Type { get; set; }

        [Display(Name = "上線狀態")]
        public bool? Status { get; set; }

        public List<SelectListItem> AdminTypeOptions
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>();
                var values = EnumHelper.GetValues<AdminType>();
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
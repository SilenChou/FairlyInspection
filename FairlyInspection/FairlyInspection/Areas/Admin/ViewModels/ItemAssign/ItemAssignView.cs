using FairlyInspection.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FairlyInspection.Areas.Admin.ViewModels.ItemAssign
{
    public class ItemAssignView
    {
        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [Display(Name = "項目編號")]
        public int ItemID { get; set; }

        [Required]
        [Display(Name = "工號")]
        public string PersonID { get; set; }

        [Required]
        [Display(Name = "建立時間")]
        public DateTime CreateDate { get; set; }

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
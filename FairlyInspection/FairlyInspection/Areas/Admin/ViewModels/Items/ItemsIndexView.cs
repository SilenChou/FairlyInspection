using FairlyInspection.Models;
using FairlyInspection.Models.Others;
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
    public class ItemsIndexView : PageQuery
    {
        ItemsRepository itemsRepository = new ItemsRepository(new InspectionEntities());

        public ItemsIndexView()
        {
            this.Sorting = "ParentID";
            this.IsDescending = false;
            this.Level = 1;
        }

        public List<BreadItem> ParentBreadList { get; set; }        

        public PageResult<ItemsView> PageResult { get; set; }

        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "項目名稱")]
        public string Title { get; set; }

        [Display(Name = "檢查內容/說明")]
        public string Description { get; set; }

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

        [Display(Name = "層級")]
        public int Level { get; set; }
        public string LevelStr
        {
            get
            {
                return EnumHelper.GetDescription((LevelClass)this.Level);
            }
        }

        [Display(Name = "巡檢間隔時間(天)")]
        public int Interval { get; set; }

        [Display(Name = "上線狀態")]
        public bool? IsOnline { get; set; }

        [Display(Name = "更新時間")]
        public DateTime UpdateDate { get; set; }

        [Display(Name = "更新者")]
        public int Updater { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "建立者")]
        public int Creater { get; set; }

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
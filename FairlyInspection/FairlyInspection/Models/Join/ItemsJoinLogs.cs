using FairlyInspection.Areas.Admin.ViewModels.Items;
using FairlyInspection.Repositories;
using FairlyInspection.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FairlyInspection.Models.Join
{
    public class ItemsJoinLogs
    {
        ItemsRepository itemsRepository = new ItemsRepository(new InspectionEntities());

        //[Display(Name = "ID")]
        //public int ID { get; set; }

        [Display(Name = "項目編號")]
        public int ItemID { get; set; }

        [Display(Name = "設備系統")]
        public int ItemSystem { get; set; }

        [Display(Name = "項目名稱")]
        public string ItemTitle { get; set; }

        [Display(Name = "層級")]
        public int Level { get; set; }

        //[Display(Name = "檢查結果")]
        //public int CheckResult { get; set; }

        //[Display(Name = "照片")]
        //public string MainPic { get; set; }

        //[Display(Name = "說明")]
        //public string Description { get; set; }

        [Display(Name = "最新紀錄")]
        public bool IsLatest { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreateDate { get; set; }
        public DateTime NextCheckDate { get; set; }

        [Required]
        [Display(Name = "建立者")]
        public int Creater { get; set; }

        //------------------------------------------------------------------------------------------

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

        public string LevelStr
        {
            get
            {
                return EnumHelper.GetDescription((LevelClass)this.Level);
            }
        }

        [Display(Name = "巡檢間隔時間 (天)")]
        public int Interval { get; set; }

        [Display(Name = "啟用狀態")]
        public bool IsOnline { get; set; }

        public List<BreadItem> ParentBreadList { get; set; }

        public List<ItemAssign> InspectorsList { get; set; }
        public string Inspectors { get; set; }

        public string Message { get; set; }
    }
}
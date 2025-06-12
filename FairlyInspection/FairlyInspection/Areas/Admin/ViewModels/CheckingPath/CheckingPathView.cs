using FairlyInspection.Areas.Admin.ViewModels.CheckingPoint;
using FairlyInspection.Models;
using FairlyInspection.Repositories;
using FairlyInspection.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FairlyInspection.Areas.Admin.ViewModels.CheckingPath
{
    public class CheckingPathView
    {
        AdminRepository adminRepository = new AdminRepository(new InspectionEntities());

        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "項目名稱")]
        public string Title { get; set; }

        [Display(Name = "說明")]
        public string Description { get; set; }

        [Display(Name = "巡檢間隔單位")]
        public int IntervalType { get; set; }

        [Display(Name = "巡檢間隔時間")]
        public int Interval { get; set; }

        [Display(Name = "調度員")]
        public string Dispatcher { get; set; }

        [Display(Name = "巡檢人員")]
        public string Inspectors { get; set; }

        [Display(Name = "啟用狀態")]
        public bool IsOnline { get; set; }

        [Display(Name = "起始檢查時間")]
        public DateTime? StartDate { get; set; }
        public string StartDateValue { get; set; }

        [Display(Name = "預定檢查時間")]
        public DateTime? NextDate { get; set; }
        public string NextDateValue { get; set; }

        public DateTime? CompleteDate { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "建立者")]
        public int Creater { get; set; }

        public List<SelectListItem> TimeUnit
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>();
                var values = EnumHelper.GetValues<TimeUnit>();
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

        public List<CheckingPointView> PointsList { get; set; }

        public List<SelectListItem> InspectionPersonOptions
        {
            get
            {
                List<SelectListItem> result = new List<SelectListItem>();
                var Inspectorsvalues = adminRepository.Query(true, (int)AdminType.Inspector);
                var Dispatchervalues = adminRepository.Query(true, (int)AdminType.Dispatcher);
                foreach (var v in Dispatchervalues)
                {
                    if (v.Name != "周席綸" && v.Name != "測試人")
                    {
                        result.Add(new SelectListItem()
                        {
                            Value = v.ID.ToString(),
                            Text = v.Name
                        });
                    }
                }
                foreach (var v in Inspectorsvalues)
                {
                    if (v.Name != "周席綸" && v.Name != "測試人")
                    {
                        result.Add(new SelectListItem()
                        {
                            Value = v.ID.ToString(),
                            Text = v.Name
                        });
                    }
                }
                return result;
            }
        }
    }
}
using AutoMapper;
using FairlyInspection.Areas.Admin.ViewModels.CheckingItem;
using FairlyInspection.Areas.Admin.ViewModels.CheckingItemLogsImage;
using FairlyInspection.Areas.Admin.ViewModels.CheckingPath;
using FairlyInspection.Areas.Admin.ViewModels.CheckingPathLog;
using FairlyInspection.Areas.Admin.ViewModels.CheckingPoint;
using FairlyInspection.Areas.Admin.ViewModels.ItemAssign;
using FairlyInspection.Areas.Admin.ViewModels.ItemLogs;
using FairlyInspection.Areas.Admin.ViewModels.Items;
using FairlyInspection.Areas.Admin.ViewModels.Manager;
using FairlyInspection.Models;
//using FairlyInspection.Models.Join;
using FairlyInspection.Models.T8.Join;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.App_Start
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                #region 後台            
                cfg.CreateMap<Admin, ManagerView>();
                cfg.CreateMap<ManagerView, Admin>();

                cfg.CreateMap<Items, ItemsTreeView>();
                cfg.CreateMap<Items, ItemsView>();
                cfg.CreateMap<ItemsView, Items>();

                cfg.CreateMap<ItemLogs, ItemLogsView>();
                cfg.CreateMap<ItemLogsView, ItemLogs>();

                cfg.CreateMap<ItemAssign, ItemAssignView>();
                cfg.CreateMap<ItemAssignView, ItemAssign>();

                cfg.CreateMap<CheckingPath, CheckingPathView>();
                cfg.CreateMap<CheckingPathView, CheckingPath>();

                cfg.CreateMap<CheckingPoint, CheckingPointView>();
                cfg.CreateMap<CheckingPointView, CheckingPoint>();

                cfg.CreateMap<CheckingItem, CheckingItemView>();
                cfg.CreateMap<CheckingItemView, CheckingItem>();

                cfg.CreateMap<CheckingPathLog, CheckingPathLogView>();
                cfg.CreateMap<CheckingPathLogView, CheckingPathLog>();

                cfg.CreateMap<CheckingPointLog, CheckingPointLogView>();
                cfg.CreateMap<CheckingPointLogView, CheckingPointLog>();

                cfg.CreateMap<CheckingItemLog, CheckingItemLogView>();
                cfg.CreateMap<CheckingItemLogView, CheckingItemLog>();

                cfg.CreateMap<CheckingItemLogsImage, CheckingItemLogsImageView>();
                cfg.CreateMap<CheckingItemLogsImageView, CheckingItemLogsImage>();

                //cfg.CreateMap<CheckingInspectors, CheckingInspectorsView>();
                //cfg.CreateMap<CheckingInspectorsView, CheckingInspectors>();
                #endregion

                #region 前台
                //cfg.CreateMap<Meeting, FairlyInspection.ViewModels.Meeting.MeetingView>();
                //cfg.CreateMap<FairlyInspection.ViewModels.Meeting.MeetingView, Meeting>();

                //cfg.CreateMap<Employee, FairlyInspection.ViewModels.Employee.EmployeeView>();
                //cfg.CreateMap<FairlyInspection.ViewModels.Employee.EmployeeView, Employee>();

                //cfg.CreateMap<GroupJoinPersonAndDepartment, FairlyInspection.ViewModels.Employee.EmployeeView>();
                //cfg.CreateMap<FairlyInspection.ViewModels.Employee.EmployeeView, GroupJoinPersonAndDepartment>();

                //cfg.CreateMap<Temperature, FairlyInspection.Areas.Health.ViewModels.TemperatureView>();
                //cfg.CreateMap<FairlyInspection.Areas.Health.ViewModels.TemperatureView, Temperature>();
                #endregion
            });
        }
    }
}
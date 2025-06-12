using FairlyInspection.Areas.Admin.ViewModels.CheckingPath;
using FairlyInspection.Areas.Admin.ViewModels.Items;
using FairlyInspection.Models.Join;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Areas.Admin.ViewModels.Home
{
    public class OverViewView
    {
        public List<ItemsJoinLogs> PendingList { get; set; }

        public List<CheckingPathView> CheckingPendingList { get; set; }
    }
}
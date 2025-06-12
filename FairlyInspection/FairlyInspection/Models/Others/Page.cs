using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Models.Others
{
    public class Page
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }

        /// <summary>
        /// 符合條件的顯示筆數
        /// </summary>
        public int Count { get; set; }
    }
}
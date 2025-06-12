using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspectionNotify.Models.Others
{
    public class PageQuery 
    {
        /// <summary>
        /// 是否遞減
        /// </summary>
        public bool isDescending = false;
        public bool IsDescending
        {
            get { return this.isDescending; }
            set { this.isDescending = value; }
        }

        /// <summary>
        /// 排序的欄位
        /// </summary>
        public string Sorting { get; set; }

        private int currentPage = 1;
        public int CurrentPage
        {
            get
            {
                return this.currentPage;
            }
            set
            {
                this.currentPage = value >= 1 ? value : 1;
            }
        }

        private int pageSize = 10;

        public int PageSize
        {
            get
            {
                return this.pageSize;
            }
            set
            {
                this.pageSize = value;
            }
        }
    }
}
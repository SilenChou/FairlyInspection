using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Models.Others
{
    public class PageResult<T> : Page
    {
        public IEnumerable<T> Data { get; set; }
    }
}
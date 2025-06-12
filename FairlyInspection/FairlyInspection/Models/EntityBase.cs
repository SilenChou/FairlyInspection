using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FairlyInspection.Models
{
    public class EntityBase
    {
        public virtual long ID { get; set; }

        public virtual long Creater { get; set; }

        public virtual DateTime CreateTime { get; set; }
        public virtual DateTime CreateDate { get; set; }

        public virtual long Updater { get; set; }

        public virtual DateTime UpdateTime { get; set; }
        public virtual DateTime UpdateDate { get; set; }
    }
}
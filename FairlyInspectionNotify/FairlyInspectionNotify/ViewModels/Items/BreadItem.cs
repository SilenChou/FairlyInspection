using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairlyInspectionNotify.ViewModels.Items
{
    public class BreadItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public int ParentID { get; set; }
        public string LevelStr { get; set; }
    }
}

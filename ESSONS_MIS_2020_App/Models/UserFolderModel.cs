using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSONS_MIS_2020_App.Models
{
    public class UserFolderModel
    {
        public string empID { get; set; }
        public string empName { get; set; }
        public string depName { get; set; }
        public string depchildName { get; set; }
        public int requestOT { get; set; }
        public int confirmOT { get; set; }
        public int confirmDO { get; set; }
        public int confirmCT { get; set; }
    }
}

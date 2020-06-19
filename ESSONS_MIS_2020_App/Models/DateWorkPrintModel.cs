using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSONS_MIS_2020_App.Models
{
    public class DateWorkPrintModel
    {
        public string empid { get; set; }
        public string empName { get; set; }
        public string depchildName { get; set; }
        public string depName { get; set; }
        public string shiftName { get; set; }
        public string datework { get; set; }
        public string dateworkend { get; set; }
        public string timeStart { get; set; }
        public string timeEnd { get; set; }
        public string username { get; set; }
    }
}

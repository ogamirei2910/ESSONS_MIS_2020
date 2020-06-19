using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSONS_MIS_2020.Models
{
    public class EmpDateWorkModel
    {
        public string requestID { get; set; }
        public string datework { get; set; }
        public string dateworkend { get; set; }
        public string empid { get; set; }
        public int isOT { get; set; }
        public string shiftName { get; set; }

        public string depName { get; set; }
        public string status { get; set; }
        public string indat { get; set; }
        public string username { get; set; }


        public int number { get; set; } /* procedure */
    }
}

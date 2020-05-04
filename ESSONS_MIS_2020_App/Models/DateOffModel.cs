using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSONS_MIS_2020_App.Models
{
    public class DateOffModel
    {
        public string empID { get; set; }
        public int status { get; set; }

        public string dateoffID { get; set; }
        public string dateoffStart { get; set; }
        public string dateoffStartTime { get; set; }
        public string dateoffEnd { get; set; }
        public string dateoffEndTime { get; set; }
        public string dateoffType { get; set; }
        public double dateoffNumber { get; set; }
        public string username { get; set; }


        public string empName { get; set; }
        public string depName { get; set; }
        public string positionID { get; set; }
        public string depID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSONS_MIS_2020_App.Models
{
    public class ChamCongModel
    {
        public string empID { get; set; }
        public string empName { get; set; }
        public string depName { get; set; }
        public string datework { get; set; }
        public string intime { get; set; }
        public string outtime { get; set; }

        public string overtimeName { get; set; }
        public string shiftname { get; set; }
        public double hours { get; set; }
        public double OT { get; set; }
    }
}

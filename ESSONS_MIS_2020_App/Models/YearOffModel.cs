using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSONS_MIS_2020_App.Models
{
    public class YearOffModel
    {
        public string empid { get; set; }
        public string empName { get; set; }
        public string depName { get; set; }
        public string positionName { get; set; }
        public decimal yearOff { get; set; }
        public decimal yearOffActive { get; set; }
        public decimal yearOffUsed { get; set; }
    }
}

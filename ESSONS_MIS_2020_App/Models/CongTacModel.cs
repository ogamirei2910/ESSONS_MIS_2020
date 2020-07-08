using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSONS_MIS_2020_App.Models
{
    public class CongTacModel
    {
        public string congtacID { get; set; }
        public string planName { get; set; }
        public string depName { get; set; }
        public string planDescription { get; set; }
        public string planType { get; set; }
        public int planNumber { get; set; }
        public decimal planEstimatedBudget { get; set; }
        public decimal planSpentBudget { get; set; }
        public string planStart { get; set; }
        public string planEnd { get; set; }
        public int planKM { get; set; }
        public int status { get; set; }
        public string indat { get; set; }
        public string intime { get; set; }
        public string username { get; set; }
        public string empid { get; set; }

        public List<ChildCongTac> empmodel { get; set; }
    }
}

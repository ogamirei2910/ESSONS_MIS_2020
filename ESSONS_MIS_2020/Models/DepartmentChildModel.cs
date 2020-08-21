using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSONS_MIS_2020.Models
{
    public class DepartmentChildModel
    {
        public string DepChildID { get; set; }

        public string DepChildName { get; set; }

        public string DepID { get; set; }
        public DepartmentModel dep { get; set; }
    }
}

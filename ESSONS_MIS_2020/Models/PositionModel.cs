using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSONS_MIS_2020.Models
{
    public class PositionModel
    {
        public string positionID { get; set; }
        public string positionName { get; set; }
        public DepartmentModel dep { get; set; }
        public DepartmentChildModel depchild { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ESSONS_MIS_2020_App.Models
{
    public class QuanLyBoPhan
    {
        public DepartmentModel dep { get; set; }
        public DepartmentChildModel depchild { get; set; }
        public PositionModel position { get; set; }
    }
}

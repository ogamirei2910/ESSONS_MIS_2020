﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSONS_MIS_2020.Models
{
    public class PositionDepEmpModel
    {
        public string empID { get; set; }
        public string positionID { get; set; }
        public string positionName { get; set; }
        public string depID { get; set; }
        public string depchildID { get; set; }

        public string depchildName { get; set; }
        public string typeName { get; set; }
        public string comment { get; set; }
        public int status { get; set; }
        public string confirmDate { get; set; }
        public string username { get; set; }
    }
}

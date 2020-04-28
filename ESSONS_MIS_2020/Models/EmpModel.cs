﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSONS_MIS_2020.Models
{
    public class EmpModel
    {
        public int empID { get; set; }
        public string empIDTemp { get; set; }
        public string empName { get; set; }
        public string empIdentityCard { get; set; }
        public string depID { get; set; }
        public string empAddress { get; set; }
        public string empBirthDay { get; set; }
        public string empEmail { get; set; }
        public string empMarriage { get; set; }
        public string empTel { get; set; }
        public string empSex { get; set; }
        public string empEdu { get; set; }
        public string empInDate { get; set; }
        public string empStandardDate { get; set; }
        public string empLeaveDate { get; set; }

        public string empImage { get; set; }
        public string positionID { get; set; }
        public int status { get; set; }
        public string indat { get; set; }
        public string intime { get; set; }
        public string username { get; set; }
        public IFormFile ProfileImage { get; set; }
    }
}

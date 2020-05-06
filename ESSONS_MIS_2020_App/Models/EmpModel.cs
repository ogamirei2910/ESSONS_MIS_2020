using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ESSONS_MIS_2020_App.Models
{

    public class EmpModel
    {
        public string empID { get; set; }
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

        public string empYearOff { get; set; }
        public string positionID { get; set; }
        public int status { get; set; }
        public string indat { get; set; }
        public string intime { get; set; }
        public string username { get; set; }

        [NotMapped]
        public IFormFile ProfileImage { get; set; }

        public List<PositionModel> positionDB { get; set; }
        public List<DepartmentModel> departmentDB { get; set; }

    }
}

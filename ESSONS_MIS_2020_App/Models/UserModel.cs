using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESSONS_MIS_2020_App.Models
{
    public class UserModel
    {
        public string empID { get; set; }

        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }

        public int status { get; set; }

        public int roleID { get; set; }
    }
}

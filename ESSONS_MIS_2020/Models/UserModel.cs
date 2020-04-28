using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSONS_MIS_2020.Models
{
    public class UserModel
    {
        public string empID { get; set; }

        public string username { get; set; }

        public string password { get; set; }

        public int status { get; set; }

        public int roleID { get; set; }
    }
}

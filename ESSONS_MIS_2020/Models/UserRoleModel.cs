using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSONS_MIS_2020.Models
{
    public class UserRoleModel
    {
        public int roleID { get; set; }
        public int folderID { get; set; }
        public int folderChildID { get; set; }

        public string depName { get; set; }
        public string empName { get; set; }
        public string empImage { get; set; }
        public string empID { get; set; }
        public List<FolderModel> folderList { get; set; }
        public List<FolderChildModel> folderchildList { get; set; }
    }
}

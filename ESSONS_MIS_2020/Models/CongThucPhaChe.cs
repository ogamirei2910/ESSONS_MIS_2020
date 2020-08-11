using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSONS_MIS_2020.Models
{
    public class CongThucPhaChe
    {
        public string sothe { get; set; }
        public string chatkeo { get; set; }
        public string MaKeo1 { get; set; }
        public string MaKeo2 { get; set; }

        public List<CongThucPhaCheE> ctpce {get; set;}
        public List<CongThucPhaCheK> ctpck { get; set; }
    }
}

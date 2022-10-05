using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoWMS.Server.Models.Wcs
{
    public class SetConstance
    {
        public string Set_code { get; set; }
        public string Set_desc { get; set; }
        public Int32? Val_int { get; set; }
        public string Val_vhr { get; set; }
        public decimal? Val_num { get; set; }
        public string Val_chr { get; set; }
        public DateTime? Val_dt { get; set; }

        public Boolean? Val_bit { get; set; }

    }
}

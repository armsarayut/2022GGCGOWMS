using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoWMS.Server.Models.Wcs
{
    public class AsrsLoadtime
    {
        public string Lpncode { get; set; }
        public string Work_code { get; set; }
        public string Work_text_th { get; set; }
        public Int32? Srm_no { get; set; }
        public Int64? Srm_from { get; set; }
        public Int64? Srm_to { get; set; }
        public DateTime? Stime { get; set; }
        public DateTime? Etime { get; set; }
        public string Loadtime { get; set; }
    }
}

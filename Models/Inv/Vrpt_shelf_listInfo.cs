using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoWMS.Server.Models.Inv
{
    public class Vrpt_shelf_listInfo
    {
        public DateTime? Modified { get; set; }
        public Int32? Srm_no { get; set; }
        public Int64? Shelf_no { get; set; }
        public string Shelfcode { get; set; }
        public string Shelfname { get; set; }
        public Int32? Shelfbank { get; set; }
        public Int32? Shelfframe { get; set; }
        public Int32? Shelfbay { get; set; }
        public Int32? Shelflevel { get; set; }
        public Int32? Shelfstatus { get; set; }
        public string Lpncode { get; set; }
        public string Refercode { get; set; }
        public decimal? Actual_weight { get; set; }
        public Int32? Actual_size { get; set; }
        public string Desc_size { get; set; }
        public string St_desc { get; set; }
    }
}

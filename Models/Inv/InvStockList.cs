using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoWMS.Server.Models.Inv
{
    public class InvStockList
    {
        public Int64? Rn { get; set; }
        public string Item_code { get; set; }
        public string Item_name { get; set; }
        public decimal? Qty { get; set; }
        public string Su_no { get; set; }
        public string Palletcode { get; set; }
        public string Shelfname { get; set; }

        public Int32? Efstatus { get; set; }

        public string StorageArae { get; set; }

    }
}

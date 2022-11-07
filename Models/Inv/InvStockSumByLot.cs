using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoWMS.Server.Models.Inv
{
    public class InvStockSumByLot
    {
        public Int64? Rn { get; set; }
        public string Item_code { get; set; }
        public string Item_name { get; set; }
        public string Lot { get; set; }
        public Decimal? Totalstock { get; set; }
        public Int32? Countpallet { get; set; }

        public string Unit { get; set; }
    }
}

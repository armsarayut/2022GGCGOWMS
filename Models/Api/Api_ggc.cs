using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoWMS.Server.Models.Api
{
    public class Api_ggc
    {
        public long? Idx { get; set; }
        public DateTime? Created { get; set; }
        public Int32? Entity_lock { get; set; }
        public DateTime? Modified { get; set; }
        public long? Client_id { get; set; }
        public string Client_ip { get; set; }
        public string Item_code { get; set; }
        public decimal? Total_qty { get; set; }
        public string Doc_ref { get; set; }
        public DateTime? Receiving_date { get; set; }
        public string Create_by { get; set; }
        public DateTime? Create_date { get; set; }
        public string Batch_no { get; set; }
        public string Doc_item_ref { get; set; }
        public bool? Deletion_flag { get; set; }
        public string Remark { get; set; }
        public Int32? Delivery_priority { get; set; }
        public string Flow_type { get; set; }
        public string Api_name { get; set; }
        public decimal? Gr_qty { get; set; }
        public string Gr_remark { get; set; }

    }
}

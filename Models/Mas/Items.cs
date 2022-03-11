using System;
using System.Collections.Generic;

namespace GoWMS.Server.Models.Mas
{
    public class Items
    {
        public long Idx { get; set; }

        public DateTime Created { get; set; }

        public int? Entity_lock { get; set; }

        public DateTime? Modified { get; set; }

        public long? Client_id { get; set; }

        public string Client_ip { get; set; }

        public string Item_cat { get; set; }

        public string Item_code { get; set; }

        public string Item_name { get; set; }

        public string Vendor { get; set; }

        public string Brand { get; set; }

        public decimal? Pack_qty { get; set; }

        public string Pack_uom { get; set; }

        public string Price_uom { get; set; }

        public int? Mode_no { get; set; }

        public int? Status { get; set; }

        public string Error_code { get; set; }

        public string Created_by { get; set; }

        public DateTime? Created_date { get; set; }

        public string Update_by { get; set; }

        public DateTime? Update_date { get; set; }

        public int? Uom_qty { get; set; }

        public string Uom { get; set; }

        public string Pack_size_box { get; set; }

        public string Pack_size_pal { get; set; }

        public bool? Batch_management { get; set; }

        public decimal? Gross_weight { get; set; }

        public decimal? Net_weight { get; set; }

        public string Weight_unit { get; set; }

        public string Class_flag { get; set; }

        public string Consign_flag { get; set; }

        public string Tile_size { get; set; }

        public string Item_flag { get; set; }

        public string Is_rack { get; set; }

    }
}

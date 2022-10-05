using System;

namespace GoWMS.Server.Models
{
    public class ggcTag4x4
    {
  
        public string PalletNo { get; set; }
        public string TagNo { get; set; }
        public string BatchNo { get; set; }
        public string ProductNo { get; set; }
        public string ProductName { get; set; }
        public string QtyPerPallet { get; set; }
        public string Packaging { get; set; }
        public DateTime? PackDate { get; set; }
        public string ReservationNo { get; set; }
        public string CreateBy { get; set; }
    }
}

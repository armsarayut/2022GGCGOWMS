using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoWMS.Server.Models.Wcs
{
    public class Tas_Rgvworks
    {
		public Int64? Idx { get; set; }
		public DateTime? Created { get; set; }
		public Int32? Entity_Lock { get; set; }
		public DateTime? Modified { get; set; }
		public Int64? Client_Id { get; set; }
		public string Client_Ip { get; set; }
		public string Lpncode { get; set; }
		public string Work_Code { get; set; }
		public string Work_Status { get; set; }
		public Int64? Work_Id { get; set; }
		public Int32? Rgv_No { get; set; }
		public Int32? Rgv_From { get; set; }
		public Int32? Rgv_To { get; set; }
		public Int32? Rgv_Status { get; set; }
		public Int32? Cvy_No { get; set; }
		public Int32? Cvy_From { get; set; }
		public Int32? Cvy_To { get; set; }
		public Int32? Cvy_Status { get; set; }
		public string Pallet_No { get; set; }
		public Int32? Pallet_Hight { get; set; }
		public Int32? Pallet_Width { get; set; }
		public Int32? Pallet_Size { get; set; }
		public DateTime? Ctime { get; set; }
		public DateTime? Stime { get; set; }
		public DateTime? Etime { get; set; }
		public Int32? Gate_Out { get; set; }
		public Int32? Work_Priority { get; set; }
	}
}

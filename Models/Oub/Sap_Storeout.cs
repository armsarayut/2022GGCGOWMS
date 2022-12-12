using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoWMS.Server.Models.Oub
{
    public class Sap_Storeout
    {
		public Int64? Idx { get; set; }
		public DateTime? Created { get; set; }
		public Int32? Entity_Lock { get; set; }
		public DateTime? Modified { get; set; }
		public Int64? Client_Id { get; set; }
		public string Client_Ip { get; set; }
		public string Order_No { get; set; }
		public string Ship_To_Code { get; set; }
		public string Ship_Name { get; set; }
		public Int32? Delivery_Priority { get; set; }
		public DateTime? Delivery_Date { get; set; }
		public string Item_Code { get; set; }
		public string Batch_Number { get; set; }
		public Decimal? Request_Qty { get; set; }
		public Int32? Status { get; set; }
		public string Error_Code { get; set; }
		public string Movement_Type { get; set; }
		public string Movement_Reason { get; set; }
		public string To_No { get; set; }
		public string To_Line { get; set; }
		public string Po_Header_Txt { get; set; }
		public string Requisitioner { get; set; }
		public string Po_No { get; set; }
		public string Remark { get; set; }
		public string Doc_Ref { get; set; }
		public string Created_By { get; set; }
		public DateTime? Created_Date { get; set; }
		public string Update_By { get; set; }
		public DateTime? Update_Date { get; set; }
		public string Order_Line { get; set; }
		public string Stock_Consign { get; set; }
		public string Site { get; set; }
		public string Storage_Location { get; set; }
		public string Warehouse { get; set; }
		public string Item_Name { get; set; }
		public string Tracking_Number { get; set; }
		public string Su_No { get; set; }
		public string Pallet_No { get; set; }
		public Decimal? Stock_Qty { get; set; }
		public Decimal? Transfer_Qty { get; set; }
		public string Ref_No { get; set; }
		public string Ref_Line { get; set; }
		public string Unit { get; set; }
		public string Vendor_Code { get; set; }
		public string Batch_No { get; set; }
		public string Palletmapkey { get; set; }
	}
}

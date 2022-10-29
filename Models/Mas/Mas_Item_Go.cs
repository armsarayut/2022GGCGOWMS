using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoWMS.Server.Models.Mas
{
	public class Mas_Item_Go
	{

		public Int64? Efidx { get; set; }
		public Int32? Efstatus { get; set; }
		public DateTime? Created { get; set; }
		public DateTime? Modified { get; set; }
		public Int64? Innovator { get; set; }
		public string Device { get; set; }
		public string Itemcode { get; set; }
		public string Itemname { get; set; }
		public string Itemunit { get; set; }
		public string Itembrand { get; set; }
		public Decimal? Weightnet { get; set; }
		public Decimal? Weightgross { get; set; }
		public string Weightuint { get; set; }
		public string Vendor { get; set; }
		public bool ? IsBatchMgn { get; set; }
        public Decimal? Palqty { get; set; }

    }
}

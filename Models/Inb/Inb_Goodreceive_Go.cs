using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoWMS.Server.Models.Inb
{
	public class Inb_Goodreceive_Go
	{

		public Int64? Efidx { get; set; }
		public Int32? Efstatus { get; set; }
		public DateTime? Created { get; set; }
		public DateTime? Modified { get; set; }
		public Int64? Innovator { get; set; }
		public string Device { get; set; }
		public string Pono { get; set; }
		public string Pallettag { get; set; }
		public string Itemtag { get; set; }
		public string Itemcode { get; set; }
		public string Itemname { get; set; }
		public string Itembar { get; set; }
		public string Unit { get; set; }
		public string Weightunit { get; set; }
		public Decimal? Quantity { get; set; }
		public Decimal? Weight { get; set; }
		public string Lotno { get; set; }
		public Decimal? Totalquantity { get; set; }
		public Decimal? Totalweight { get; set; }
		public string Docno { get; set; }
		public string Docby { get; set; }
		public DateTime? Docdate { get; set; }
		public string Docnote { get; set; }
		public Int64? Grnrefer { get; set; }
		public DateTime? Grntime { get; set; }
		public DateTime? Grtime { get; set; }
		public string Grtype { get; set; }
		public string Pallteno { get; set; }
		public string Palltmapkey { get; set; }
		public DateTime? Storagetime { get; set; }
		public string Storageno { get; set; }
		public string Storagearea { get; set; }
		public string Storagebin { get; set; }

        public string PalltmapkeyNo { get; set; }

    }
}

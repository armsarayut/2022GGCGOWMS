using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GoWMS.Server.Models.Public
{
    public class Class6_7_F
    {
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? W_date { get; set; }
        public Int32? W01 { get; set; }
        public Int32? W07 { get; set; }
        public Int32? W02 { get; set; }
        public Int32? W03 { get; set; }
        public Int32? W05 { get; set; }
        public Int32? W09 { get; set; }
        public Int32? W101 { get; set; }
        public Int32? W102 { get; set; }
        public Int32? Wtotal { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using GoWMS.Server.DataAccess;
using GoWMS.Server.Controllers;
using GoWMS.Server.Models;
using GoWMS.Server.Models.Mas;
using Serilog;

namespace GoWMS.Server.Data
{
    public class MasterBLL
    {
        public IEnumerable<Items> GetItems()
        {
            DataTable dt = new DataTable();
            List<Items> lstobj = new List<Items>();
            StringBuilder sql = new StringBuilder();

            sql.AppendLine("SELECT");
            sql.AppendLine("*");
            sql.AppendLine("FROM");
            sql.Append("dbo.");
            sql.AppendLine("set_itemmaster");
            sql.AppendLine(";");



            return lstobj;
        }
    }
}

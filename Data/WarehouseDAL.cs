using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;
using GoWMS.Server.Controllers;
using GoWMS.Server.Models;
using GoWMS.Server.Models.Public;
using System.Text;
using NpgsqlTypes;
using Serilog;

namespace GoWMS.Server.Data
{
    public class WarehouseDAL
    {
        readonly private string connString = ConnGlobals.GetConnLocalDBPG();
        readonly private string connectionStringSQL = ConnGlobals.GetConnDBSQL();

        public IEnumerable<WhStorageCapacity> GetStorageCapacities()
        {
            List<WhStorageCapacity> lstModels = new List<WhStorageCapacity>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                StringBuilder sqlQurey = new StringBuilder();
                sqlQurey.AppendLine("select row_number() over(order by srm_no asc) AS rn");
                sqlQurey.AppendLine(", srm_name, srm_no, locavl, locemp, plemp, plerr, prohloc, total, percen");
                sqlQurey.AppendLine("from wcs.vrpt_shelfsummary");
                sqlQurey.AppendLine("order by srm_no asc");
                sqlQurey.AppendLine(";");

                SqlCommand cmd = new SqlCommand(sqlQurey.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    WhStorageCapacity listRead = new WhStorageCapacity
                    {
                        Rn =  rdr["rn"] == DBNull.Value ? null : (Int64?)rdr["rn"],
                        Srmname = rdr["srm_name"].ToString(),
                        Srmno = rdr["srm_no"] == DBNull.Value ? null : (Int32?)rdr["srm_no"],
                        Locavlt1 = rdr["locavl"] == DBNull.Value ? null : (Int32?)rdr["locavl"],
                        Locavlt2 = rdr["locavl"] == DBNull.Value ? null : (Int32?)rdr["locavl"],
                        Locemp = rdr["locemp"] == DBNull.Value ? null : (Int32?)rdr["locemp"],
                        Plemp = rdr["plemp"] == DBNull.Value ? null : (Int32?)rdr["plemp"],
                        Perr = rdr["plerr"] == DBNull.Value ? null : (Int32?)rdr["plerr"],
                        Prohloc = rdr["prohloc"] == DBNull.Value ? null : (Int32?)rdr["prohloc"],
                        Total = rdr["total"] == DBNull.Value ? null : (Int32?)rdr["total"],
                        OccRate = rdr["percen"] == DBNull.Value ? null : (decimal?)rdr["percen"],
                    };
                    lstModels.Add(listRead);
                }
                con.Close();
            }
            return lstModels;
        }

        //---------------------------------------------------------------------------------------------

        public IEnumerable<WhStorageList> GetStorageLists()
        {
            List<WhStorageList> lstModels = new List<WhStorageList>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                StringBuilder sqlQurey = new StringBuilder();

                sqlQurey.AppendLine("select row_number() over(order by shelf_no asc) AS rn");
                sqlQurey.AppendLine(", shelf_no, shelfcode, shelfname");
                sqlQurey.AppendLine(", srm_no, shelfbank, shelfbay, shelflevel, shelfstatus");
                sqlQurey.AppendLine(", lpncode, actual_size, desc_size, st_desc, backcolor, focecolor, modified");
                sqlQurey.AppendLine("from wcs.vrpt_shelf_list");
                sqlQurey.AppendLine("order by shelf_no asc");
                sqlQurey.AppendLine(";");


                NpgsqlCommand cmd = new NpgsqlCommand(sqlQurey.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();

                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    WhStorageList listRead = new WhStorageList
                    {
                        Rn = rdr["rn"] == DBNull.Value ? null : (Int64?)rdr["rn"],
                        Shelfno = rdr["shelf_no"] == DBNull.Value ? null : (Int32?)rdr["shelf_no"],
                        Shelfcode = rdr["shelfcode"].ToString(),
                        Shelfname = rdr["shelfname"].ToString(),
                        Srmno = rdr["srm_no"] == DBNull.Value ? null : (Int32?)rdr["srm_no"],
                        Shelfbank = rdr["shelfbank"] == DBNull.Value ? null : (Int16?)rdr["shelfbank"],
                        Shelfbay = rdr["shelfbay"] == DBNull.Value ? null : (Int32?)rdr["shelfbay"],
                        Shelflevel = rdr["shelflevel"] == DBNull.Value ? null : (Int16?)rdr["shelflevel"],
                        Shelfstatus = rdr["shelfstatus"] == DBNull.Value ? null : (Int32?)rdr["shelfstatus"],
                        Lpncode = rdr["lpncode"].ToString(),
                        Actualsize = rdr["actual_size"] == DBNull.Value ? null : (Int32?)rdr["actual_size"],
                        Descsize = rdr["desc_size"].ToString(),
                        Stdesc = rdr["st_desc"].ToString(),
                        Backcolor = rdr["backcolor"].ToString(),
                        Focecolor = rdr["focecolor"].ToString(),
                        Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"]
                    };
                    lstModels.Add(listRead);
                }
                con.Close();
            }
            return lstModels;
        }

        public IEnumerable<Sap_StoreoutInfo> GetPicklist(string sPallet)
        {
            List<Sap_StoreoutInfo> lstModels = new List<Sap_StoreoutInfo>();
            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                StringBuilder sqlQurey = new StringBuilder();

                sqlQurey.AppendLine("select row_number() over(order by t1.item_code asc) AS rn, t1.idx, ");
                sqlQurey.AppendLine("t1.item_code, t1.item_name, t1.request_qty, t1.unit, t1.su_no, ");
                sqlQurey.AppendLine("t1.pallet_no, t1.stock_qty, t1.transfer_qty, t1.movement_type ");
                sqlQurey.AppendLine("from public.sap_storeout t1");
                sqlQurey.AppendLine("where (1=1)");
                sqlQurey.AppendLine("and t1.pallet_no = @pallet_no");
                sqlQurey.AppendLine("order by t1.item_code asc ");
                sqlQurey.AppendLine(";");

                NpgsqlCommand cmd = new NpgsqlCommand(sqlQurey.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                cmd.Parameters.AddWithValue("@pallet_no", NpgsqlDbType.Varchar, sPallet);

                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Sap_StoreoutInfo listRead = new Sap_StoreoutInfo
                    {
                        Idx = rdr["idx"] == DBNull.Value ? null : (Int64?)rdr["idx"],
                        Item_Code = rdr["item_code"].ToString(),
                        Item_Name = rdr["item_name"].ToString(),
                        Request_Qty = rdr["request_qty"] == DBNull.Value ? null : (decimal?)rdr["request_qty"],
                        Unit = rdr["unit"].ToString(),
                        Su_No = rdr["su_no"].ToString(),
                        Pallet_No = rdr["pallet_no"].ToString(),
                        Stock_Qty = rdr["stock_qty"] == DBNull.Value ? null : (decimal?)rdr["stock_qty"],
                        Transfer_Qty = rdr["transfer_qty"] == DBNull.Value ? null : (decimal?)rdr["transfer_qty"],
                        Movement_Type = rdr["movement_type"].ToString(),
                        Bcount=false
                    };
                    lstModels.Add(listRead);
                }
                con.Close();
            }
            return lstModels;
        }

        public bool UpdateCount(List<Sap_StoreoutInfo> listupdate)
        {
            bool bRet = false;

            using NpgsqlConnection con = new NpgsqlConnection(connString);
            try
            {

                StringBuilder sql = new StringBuilder();

                using var cmd = new NpgsqlCommand(connection: con, cmdText: null);
                // cmd.Parameters.AddWithValue("@package_id", pallet);

                var i = 0;
                foreach (var s in listupdate)
                {
                    decimal dRequestqty = (decimal)s.Request_Qty;
                    string sMovementtype = s.Movement_Type;
                    string sSuno = s.Su_No;
                    decimal dStock;

                    if (dRequestqty == 0)
                    {
                        dStock = (decimal)s.Stock_Qty;
                    }
                    else
                    {
                        dStock = 0;
                    }

                    var request_qty = "request_qty" + i.ToString();
                    var movement_type = "movement_type" + i.ToString();
                    var su_no = "su_no" + i.ToString();
                    var su_nostock = "su_no" + i.ToString();
                    var total_qty = "total_qty" + i.ToString();

                    sql.AppendLine("UPDATE public.sap_storeout");
                    sql.AppendLine("SET request_qty = @" + request_qty);
                    sql.AppendLine(", movement_type = @" + movement_type);
                    sql.AppendLine("WHERE su_no = @" + su_no);
                    sql.AppendLine(";");

                    sql.AppendLine("UPDATE public.sap_stock");
                    sql.AppendLine("SET total_qty = @" + total_qty);
                    sql.AppendLine("WHERE su_no = @" + su_nostock);
                    sql.AppendLine(";");

                    cmd.Parameters.Add(new NpgsqlParameter<decimal>(request_qty, dRequestqty));
                    cmd.Parameters.Add(new NpgsqlParameter<string>(movement_type, sMovementtype));
                    cmd.Parameters.Add(new NpgsqlParameter<string>(su_no, sSuno));

                    cmd.Parameters.Add(new NpgsqlParameter<decimal>(total_qty, dStock));
                    cmd.Parameters.Add(new NpgsqlParameter<string>(su_nostock, sSuno));
                    i++;
                }
                con.Open();
                cmd.CommandText = sql.ToString();
                cmd.ExecuteNonQuery();

                bRet = true;

            }
            catch (NpgsqlException ex)
            {
                Log.Error(ex.ToString());
                bRet = false;
            }
            finally
            {
                con.Close();
            }


            return bRet;
        }

        public bool SapComplete(string sPallet)
        {
            bool bRet = false;
            string sRet = "";
            Int32? iRet = 0;

            using (NpgsqlConnection con = new NpgsqlConnection(connString))
            {
                try
                {
                    StringBuilder sqlQurey = new StringBuilder();
                    sqlQurey.AppendLine("select _retchk, _retmsg from public.fuc_pick_completesap(:lpnno);");
                    NpgsqlCommand cmd = new NpgsqlCommand(sqlQurey.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue(":lpnno", NpgsqlDbType.Varchar, sPallet);

                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        iRet = rdr["_retchk"] == DBNull.Value ? null : (Int32?)rdr["_retchk"];
                        sRet = rdr["_retmsg"].ToString();
                    }

                }
                catch (NpgsqlException ex)
                {
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }

                if (iRet == 1)
                {
                    bRet = true;
                }
            }
            return bRet;
        }

    }
}

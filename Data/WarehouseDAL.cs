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
using System.Reflection.Emit;

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
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                StringBuilder sqlQurey = new StringBuilder();

                sqlQurey.AppendLine("select top (1) t0.site,t0.doc_num,cast(t0.trans_num as bigint) trans_num,t0.ref_type,t0.trans_type,t0.trans_date");
                sqlQurey.AppendLine(" ,t0.unit_key,t0.item_bc,t0.item, t1.item_name, t1.uom, t1.weight_unit,t0.qty");
                sqlQurey.AppendLine(",t0.lot,t0.prod_date,t0.stat,t0.source");
                sqlQurey.AppendLine(",t0.reason,t0.createdby,t0.createddate");
                sqlQurey.AppendLine(",t0.whse,t0.loc,t0.pallet_bc,t0.ref_item");
                sqlQurey.AppendLine(",t0.flag,t0.require_detail_id,t0.is_req");
                sqlQurey.AppendLine(",t0.is_hold,t0.is_lock,t0.update2sl,t0.modifie_date");
                sqlQurey.AppendLine("FROM dbo.wms_trans t0");
                sqlQurey.AppendLine("INNER JOIN dbo.set_itemmaster t1");
                sqlQurey.AppendLine("ON t0.item=t1.item_code");
                sqlQurey.AppendLine("WHERE t0.unit_key = @unit_key");
                sqlQurey.AppendLine("AND t0.stat = @stat");
                sqlQurey.AppendLine("AND t0.flag = @flag");
                sqlQurey.AppendLine("AND t0.item_bc = @pallet_bc");
                sqlQurey.AppendLine("AND t0.ref_type = @reftype");
                sqlQurey.AppendLine("AND t0.trans_type = @transtype");
                sqlQurey.AppendLine("ORDER BY t0.trans_num desc");

                   SqlCommand cmd = new SqlCommand(sqlQurey.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();

                cmd.Parameters.AddWithValue("@unit_key", "05");
                cmd.Parameters.AddWithValue("@stat", 0);
                cmd.Parameters.AddWithValue("@flag", 0);
                cmd.Parameters.AddWithValue("@reftype", "J");
                cmd.Parameters.AddWithValue("@transtype", "I");
                cmd.Parameters.AddWithValue("@pallet_bc", sPallet);


                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Sap_StoreoutInfo listRead = new Sap_StoreoutInfo
                    {
                        Idx = rdr["trans_num"] == DBNull.Value ? null : (Int64?)rdr["trans_num"],
                        Item_Code = rdr["item"].ToString(),
                        Item_Name = rdr["item_name"].ToString(),
                        Request_Qty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                        Unit = rdr["uom"].ToString(),
                        Su_No = rdr["item_bc"].ToString(),
                        Pallet_No = rdr["pallet_bc"].ToString(),
                        Batch_No = rdr["lot"].ToString(),
                        Stock_Qty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                        Transfer_Qty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                        Movement_Type = rdr["unit_key"].ToString(),
                        Bcount=false
                    };
                    lstModels.Add(listRead);
                }
                con.Close();
            }
            return lstModels;
        }

        public IEnumerable<Sap_StoreoutInfo> GetCountlist(string sPallet)
        {
            List<Sap_StoreoutInfo> lstModels = new List<Sap_StoreoutInfo>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                StringBuilder sqlQurey = new StringBuilder();

                sqlQurey.AppendLine("select top (1) t0.site,t0.doc_num,cast(t0.trans_num as bigint) trans_num,t0.ref_type,t0.trans_type,t0.trans_date");
                sqlQurey.AppendLine(" ,t0.unit_key,t0.item_bc,t0.item, t1.item_name, t1.uom, t1.weight_unit,t0.qty");
                sqlQurey.AppendLine(",t0.lot,t0.prod_date,t0.stat,t0.source");
                sqlQurey.AppendLine(",t0.reason,t0.createdby,t0.createddate");
                sqlQurey.AppendLine(",t0.whse,t0.loc,t0.pallet_bc,t0.ref_item");
                sqlQurey.AppendLine(",t0.flag,t0.require_detail_id,t0.is_req");
                sqlQurey.AppendLine(",t0.is_hold,t0.is_lock,t0.update2sl,t0.modifie_date");
                sqlQurey.AppendLine("FROM dbo.wms_trans t0");
                sqlQurey.AppendLine("INNER JOIN dbo.set_itemmaster t1");
                sqlQurey.AppendLine("ON t0.item=t1.item_code");
                sqlQurey.AppendLine("WHERE t0.unit_key = @unit_key");
                sqlQurey.AppendLine("AND t0.stat = @stat");
                sqlQurey.AppendLine("AND t0.flag = @flag");
                sqlQurey.AppendLine("AND t0.item_bc = @pallet_bc");
                sqlQurey.AppendLine("AND t0.ref_type = @reftype");
                sqlQurey.AppendLine("AND t0.trans_type = @transtype");
                sqlQurey.AppendLine("ORDER BY t0.trans_num desc");

                SqlCommand cmd = new SqlCommand(sqlQurey.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();

                cmd.Parameters.AddWithValue("@unit_key", "05");
                cmd.Parameters.AddWithValue("@stat", 0);
                cmd.Parameters.AddWithValue("@flag", 0);
                cmd.Parameters.AddWithValue("@reftype", "I");
                cmd.Parameters.AddWithValue("@transtype", "A");
                cmd.Parameters.AddWithValue("@pallet_bc", sPallet);


                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Sap_StoreoutInfo listRead = new Sap_StoreoutInfo
                    {
                        Idx = rdr["trans_num"] == DBNull.Value ? null : (Int64?)rdr["trans_num"],
                        Item_Code = rdr["item"].ToString(),
                        Item_Name = rdr["item_name"].ToString(),
                        Request_Qty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                        Unit = rdr["uom"].ToString(),
                        Su_No = rdr["item_bc"].ToString(),
                        Pallet_No = rdr["pallet_bc"].ToString(),
                        Batch_No = rdr["lot"].ToString(),
                        Stock_Qty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                        Transfer_Qty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                        Movement_Type = rdr["unit_key"].ToString(),
                        Bcount = false
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

            using SqlConnection con = new SqlConnection(connectionStringSQL);
            try
            {

                StringBuilder sql = new StringBuilder();

                using var cmd = new SqlCommand(connection: con, cmdText: null);
                // cmd.Parameters.AddWithValue("@package_id", pallet);

                var i = 0;
                foreach (var s in listupdate)
                {
                    decimal dRequestqty = (decimal)s.Request_Qty;
                    decimal dStocktqty = (decimal)s.Stock_Qty;

                    string sMovementtype = s.Movement_Type;
                    string sSuno = s.Su_No;
                    string sCreatedby = s.Update_By;
                    decimal dStock;

                    if (dRequestqty == 0)
                    {
                        dStock = 0;
                        //dStock = (decimal)s.Stock_Qty;
                    }
                    //else if(dRequestqty == dStocktqty)
                    //{
                    //    dStock = 0;
                    //}
                    else
                    {
                        dStock = dRequestqty;

                    }

                    var request_qty = "Qty" + i.ToString();
                    //var movement_type = "movement_type" + i.ToString();
                    var su_no = "item_bc" + i.ToString();
                    var su_nostock = "item_bc" + i.ToString();
                    var total_qty = "Qty" + i.ToString();
                    var createdby = "createdby" + i.ToString();

                    sql.AppendLine("UPDATE dbo.wms_trans");
                    sql.AppendLine("SET Qty = @" + total_qty);
                    if (dStock == 0 )
                    {
                        sql.AppendLine(",[stat] = 1");
                    }
                    else
                    {
                        sql.AppendLine(",[stat] = 0");
                    }

                    sql.AppendLine(",[createdby] = @" + createdby);

                    sql.AppendLine("WHERE item_bc = @" + su_nostock);
                    sql.AppendLine("AND [unit_key] = '01'");
                    sql.AppendLine("AND [flag] = 0");
                    sql.AppendLine("AND [is_req] = 0");

                    sql.AppendLine(";");                                                                                                                                                                         

                    cmd.Parameters.Add(new SqlParameter(total_qty.ToString(), SqlDbType.Decimal)).Value = dStock;
                    cmd.Parameters.Add(new SqlParameter(su_nostock.ToString(), SqlDbType.VarChar)).Value = sSuno;
                    cmd.Parameters.Add(new SqlParameter(createdby.ToString(), SqlDbType.VarChar)).Value = sCreatedby;

                    i++;
                }
                con.Open();
                cmd.CommandText = sql.ToString();
                cmd.ExecuteNonQuery();

                bRet = true;

            }
            catch (SqlException ex)
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


        public bool SetOrderpick (string jsonOrder, ref string msgReturn)
        {
            Boolean bRet = false;
            string sRet = "";
            Int32? iRet = 0;
            string cmdcode = "Call";
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sqlQurey = new StringBuilder();
                    //sqlQurey.AppendLine("select _retchk, _retmsg from wcs.fuc_create_mccommand(:mccode , :cmdcode, :command);");
                    sqlQurey.Append("dbo.ssp_createoutboundorder_manual_json");
                    SqlCommand cmd = new SqlCommand(sqlQurey.ToString(), con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();

                    cmd.Parameters.AddWithValue("@_pJson", jsonOrder);
    

                    SqlParameter RuturnCheck = new SqlParameter("@_retchk", SqlDbType.Int);
                    RuturnCheck.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnCheck);

                    SqlParameter RuturnMsg = new SqlParameter("@_retmsg", SqlDbType.VarChar, 255);
                    RuturnMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnMsg);

                    cmd.ExecuteNonQuery();

                    iRet = (Int32)cmd.Parameters["@_retchk"].Value;
                    sRet = (string)cmd.Parameters["@_retmsg"].Value;
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

            msgReturn = sRet;

            return bRet;

        }

        public bool SetOrderpickErp(string jsonOrder, ref string msgReturn)
        {
            Boolean bRet = false;
            string sRet = "";
            Int32? iRet = 0;
            string cmdcode = "Call";
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sqlQurey = new StringBuilder();
                    //sqlQurey.AppendLine("select _retchk, _retmsg from wcs.fuc_create_mccommand(:mccode , :cmdcode, :command);");
                    sqlQurey.Append("dbo.ssp_createoutboundorder_manual_json");
                    SqlCommand cmd = new SqlCommand(sqlQurey.ToString(), con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();

                    cmd.Parameters.AddWithValue("@_pJson", jsonOrder);


                    SqlParameter RuturnCheck = new SqlParameter("@_retchk", SqlDbType.Int);
                    RuturnCheck.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnCheck);

                    SqlParameter RuturnMsg = new SqlParameter("@_retmsg", SqlDbType.VarChar, 255);
                    RuturnMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnMsg);

                    cmd.ExecuteNonQuery();

                    iRet = (Int32)cmd.Parameters["@_retchk"].Value;
                    sRet = (string)cmd.Parameters["@_retmsg"].Value;
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

            msgReturn = sRet;

            return bRet;

        }

        public bool SetOrderpickErpTest(string sku, string batch, decimal needqty, string user, long apiid, ref string msgReturn)
        {
            Boolean bRet = false;
            string sRet = "";
            Int32? iRet = 0;
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sqlQurey = new StringBuilder();
                    sqlQurey.Append("dbo.ssp_createoutboundorder_manual_erp");
                    SqlCommand cmd = new SqlCommand(sqlQurey.ToString(), con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();

                    cmd.Parameters.AddWithValue("@_sku", sku);
                    cmd.Parameters.AddWithValue("@_batch", batch);
                    cmd.Parameters.AddWithValue("@_needqty", needqty);
                    cmd.Parameters.AddWithValue("@_pUsername", user);
                    cmd.Parameters.AddWithValue("@_pApiid", apiid);

                    SqlParameter RuturnCheck = new SqlParameter("@_retchk", SqlDbType.Int);
                    RuturnCheck.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnCheck);

                    SqlParameter RuturnMsg = new SqlParameter("@_retmsg", SqlDbType.VarChar, 255);
                    RuturnMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnMsg);

                    cmd.ExecuteNonQuery();

                    iRet = (Int32)cmd.Parameters["@_retchk"].Value;
                    sRet = (string)cmd.Parameters["@_retmsg"].Value;
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

            msgReturn = sRet;

            return bRet;

        }




        public bool SetOrderaudit(string jsonOrder, ref string msgReturn)
        {
            Boolean bRet = false;
            string sRet = "";
            Int32? iRet = 0;
            string cmdcode = "Call";
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sqlQurey = new StringBuilder();
                    sqlQurey.Append("dbo.ssp_createoutbound_manual_tag_audit_json");
                    SqlCommand cmd = new SqlCommand(sqlQurey.ToString(), con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();

                    cmd.Parameters.AddWithValue("@_pJson", jsonOrder);


                    SqlParameter RuturnCheck = new SqlParameter("@_retchk", SqlDbType.Int);
                    RuturnCheck.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnCheck);

                    SqlParameter RuturnMsg = new SqlParameter("@_retmsg", SqlDbType.VarChar, 255);
                    RuturnMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnMsg);

                    cmd.ExecuteNonQuery();

                    iRet = (Int32)cmd.Parameters["@_retchk"].Value;
                    sRet = (string)cmd.Parameters["@_retmsg"].Value;
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

            msgReturn = sRet;

            return bRet;

        }


        public bool SetOrderprepare(string jsonOrder, ref string msgReturn)
        {
            Boolean bRet = false;
            string sRet = "";
            Int32? iRet = 0;
            //string cmdcode = "Call";
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sqlQurey = new StringBuilder();
                    sqlQurey.Append("dbo.ssp_createoutbound_manual_prepare_json");
                    SqlCommand cmd = new SqlCommand(sqlQurey.ToString(), con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();

                    cmd.Parameters.AddWithValue("@_pJson", jsonOrder);


                    SqlParameter RuturnCheck = new SqlParameter("@_retchk", SqlDbType.Int);
                    RuturnCheck.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnCheck);

                    SqlParameter RuturnMsg = new SqlParameter("@_retmsg", SqlDbType.VarChar, 255);
                    RuturnMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnMsg);

                    cmd.ExecuteNonQuery();

                    iRet = (Int32)cmd.Parameters["@_retchk"].Value;
                    sRet = (string)cmd.Parameters["@_retmsg"].Value;
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

            msgReturn = sRet;

            return bRet;

        }


        public bool StartOrderpick(string jsonOrder, ref string msgReturn)
        {
            Boolean bRet = false;
            string sRet = "";
            Int32? iRet = 0;
            string cmdcode = "Call";
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sqlQurey = new StringBuilder();
                    sqlQurey.Append("dbo.ssp_startoutboundorder_manual_json");
                    SqlCommand cmd = new SqlCommand(sqlQurey.ToString(), con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();

                    cmd.Parameters.AddWithValue("@_pJson", jsonOrder);

                    SqlParameter RuturnCheck = new SqlParameter("@_retchk", SqlDbType.Int);
                    RuturnCheck.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnCheck);

                    SqlParameter RuturnMsg = new SqlParameter("@_retmsg", SqlDbType.VarChar, 255);
                    RuturnMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnMsg);

                    cmd.ExecuteNonQuery();

                    iRet = (Int32)cmd.Parameters["@_retchk"].Value;
                    sRet = (string)cmd.Parameters["@_retmsg"].Value;
                }
                catch (SqlException ex)
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

            msgReturn = sRet;

            return bRet;

        }

        public bool CancelOrderpick(string jsonOrder, ref string msgReturn)
        {
            Boolean bRet = false;
            string sRet = "";
            Int32? iRet = 0;
      
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sqlQurey = new StringBuilder();

                    sqlQurey.Append("dbo.wms_cancelorder_wcs");
                    SqlCommand cmd = new SqlCommand(sqlQurey.ToString(), con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();

                    cmd.Parameters.AddWithValue("@_lpnJson", jsonOrder);


                    SqlParameter RuturnCheck = new SqlParameter("@_retchk", SqlDbType.Int);
                    RuturnCheck.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnCheck);

                    SqlParameter RuturnMsg = new SqlParameter("@_retmsg", SqlDbType.VarChar, 255);
                    RuturnMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnMsg);

                    cmd.ExecuteNonQuery();

                    iRet = (Int32)cmd.Parameters["@_retchk"].Value;
                    sRet = (string)cmd.Parameters["@_retmsg"].Value;
                }
                catch (SqlException ex)
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

            msgReturn = sRet;

            return bRet;

        }

    }
}

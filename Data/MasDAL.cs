using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using GoWMS.Server.Controllers;
using GoWMS.Server.Models;
using GoWMS.Server.Models.Mas;
using NpgsqlTypes;
using Serilog;

namespace GoWMS.Server.Data
{
    public class MasDAL
    {
        readonly private string connectionString = ConnGlobals.GetConnLocalDBPG();

        readonly private string connectionStringSQL = ConnGlobals.GetConnDBSQL();

        public IEnumerable<Mas_Pallet_Go> GetAllMasterpalletGo()
        {
            List<Mas_Pallet_Go> lstobj = new List<Mas_Pallet_Go>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                StringBuilder Sql = new StringBuilder();
                Sql.AppendLine("select *");
                Sql.AppendLine("from wcs.tas_pallet");
                Sql.AppendLine("order by idx");

                SqlCommand cmd = new SqlCommand(Sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Mas_Pallet_Go objrd = new Mas_Pallet_Go
                    {
                        Efidx = rdr["idx"] == DBNull.Value ? null : (Int64?)rdr["idx"],
                        Efstatus = rdr["entity_lock"] == DBNull.Value ? null : (Int32?)rdr["entity_lock"],
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                        Innovator = rdr["client_id"] == DBNull.Value ? null : (Int64?)rdr["client_id"],
                        Device = rdr["client_ip"].ToString(),
                        Palletno = rdr["lpncode_no"].ToString(),
                        Pallettype = 1
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public IEnumerable<Mas_Storagebin_Go> GetAllStorageBinGo()
        {
            List<Mas_Storagebin_Go> lstobj = new List<Mas_Storagebin_Go>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder Sql = new StringBuilder();
                Sql.AppendLine("select *");
                Sql.AppendLine("from wms.mas_storagebin_go");
                Sql.AppendLine("order by efidx");

                NpgsqlCommand cmd = new NpgsqlCommand(Sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Mas_Storagebin_Go objrd = new Mas_Storagebin_Go
                    {
                        Efidx = rdr["efidx"] == DBNull.Value ? null : (Int64?)rdr["efidx"],
                        Efstatus = rdr["efstatus"] == DBNull.Value ? null : (Int32?)rdr["efstatus"],
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                        Innovator = rdr["innovator"] == DBNull.Value ? null : (Int64?)rdr["innovator"],
                        Device = rdr["device"].ToString(),
                        Stocode = rdr["stocode"].ToString(),
                        Binno = rdr["binno"].ToString(),
                        Binname = rdr["binname"].ToString(),
                        Pallletno = rdr["pallletno"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public IEnumerable<Mas_Item_Go> GetAllMasteritemGo()
        {
            List<Mas_Item_Go> lstobj = new List<Mas_Item_Go>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                StringBuilder Sql = new StringBuilder();
                Sql.AppendLine("select *");
                Sql.AppendLine("from dbo.set_itemmaster");
                Sql.AppendLine("order by idx");

                SqlCommand cmd = new SqlCommand(Sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Mas_Item_Go objrd = new Mas_Item_Go
                    {
                        Efidx = rdr["idx"] == DBNull.Value ? null : (Int64?)rdr["idx"],
                        Efstatus = rdr["entity_lock"] == DBNull.Value ? null : (Int32?)rdr["entity_lock"],
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                        Innovator = rdr["client_id"] == DBNull.Value ? null : (Int64?)rdr["client_id"],
                        Device = rdr["client_ip"].ToString(),
                        Itemcode = rdr["item_code"].ToString(),
                        Itemname = rdr["item_name"].ToString(),
                        Itembrand = rdr["brand"].ToString(),
                        Itemunit = rdr["uom"].ToString(),
                        Weightnet = rdr["net_weight"] == DBNull.Value ? null : (decimal?)rdr["net_weight"],
                        Weightgross = rdr["gross_weight"] == DBNull.Value ? null : (decimal?)rdr["gross_weight"],
                        Weightuint = rdr["weight_unit"].ToString(),
                        Vendor = rdr["vendor"].ToString(),
                        IsBatchMgn = rdr["batch_management"] == DBNull.Value ? null : (bool?)rdr["batch_management"],
                        Palqty = rdr["pack_qty"] == DBNull.Value ? null : (decimal?)rdr["pack_qty"],
                        Itemcat= rdr["item_cat"].ToString()

                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public IEnumerable<Mas_Storage_Go> GetAllMasterstorageGo()
        {
            List<Mas_Storage_Go> lstobj = new List<Mas_Storage_Go>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                StringBuilder Sql = new StringBuilder();
                Sql.AppendLine("select *");
                Sql.AppendLine("from dbo.tspk_store");
                Sql.AppendLine("order by store_code");

                SqlCommand cmd = new SqlCommand(Sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Mas_Storage_Go objrd = new Mas_Storage_Go
                    {
                        Efidx = null,
                        Efstatus =null,
                        Created = null,
                        Modified = null,
                        Innovator = 0,
                        Device = "127.0.0.1",
                        Stocode = rdr["store_code"].ToString(),
                        Stoname = rdr["description"].ToString(),
                        Stoaddress = "GGC"
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public IEnumerable<Mas_Worktype_Go> GetAllMasterworktypeGo()
        {
            List<Mas_Worktype_Go> lstobj = new List<Mas_Worktype_Go>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                StringBuilder Sql = new StringBuilder();
                Sql.AppendLine("select *");
                Sql.AppendLine("from wcs.set_worktype");
                Sql.AppendLine("order by work_code");

                SqlCommand cmd = new SqlCommand(Sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Mas_Worktype_Go objrd = new Mas_Worktype_Go
                    {
                        Efidx = rdr["idx"] == DBNull.Value ? null : (Int64?)rdr["idx"],
                        Efstatus = rdr["entity_lock"] == DBNull.Value ? null : (Int32?)rdr["entity_lock"],
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                        Innovator = rdr["client_id"] == DBNull.Value ? null : (Int64?)rdr["client_id"],
                        Device = rdr["client_ip"].ToString(),
                        Workcode = rdr["work_code"].ToString(),
                        Description = rdr["work_text_en"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }


        


        public IEnumerable<Mas_Status_Go> GetAllMasterstatusGo()
        {
            List<Mas_Status_Go> lstobj = new List<Mas_Status_Go>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                StringBuilder Sql = new StringBuilder();
                Sql.AppendLine("select *");
                Sql.AppendLine("from dbo.set_status");
                Sql.AppendLine("order by idx");

                SqlCommand cmd = new SqlCommand(Sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Mas_Status_Go objrd = new Mas_Status_Go
                    {
                        Efidx = rdr["idx"] == DBNull.Value ? null : (Int64?)rdr["idx"],
                        Efstatus = rdr["entity_lock"] == DBNull.Value ? null : (Int32?)rdr["entity_lock"],
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                        Innovator = rdr["client_id"] == DBNull.Value ? null : (Int64?)rdr["client_id"],
                        Device = rdr["client_ip"].ToString(),
                        Statcode = rdr["st_no"] == DBNull.Value ? null : (Int32?)rdr["st_no"],
                        Description = rdr["st_desc"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public IEnumerable<Mas_Reason_Go> GetAllMasterreasonGo()
        {
            List<Mas_Reason_Go> lstobj = new List<Mas_Reason_Go>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder Sql = new StringBuilder();
                Sql.AppendLine("select *");
                Sql.AppendLine("from wms.mas_reason_go");
                Sql.AppendLine("order by efidx");

                NpgsqlCommand cmd = new NpgsqlCommand(Sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Mas_Reason_Go objrd = new Mas_Reason_Go
                    {
                        Efidx = rdr["efidx"] == DBNull.Value ? null : (Int64?)rdr["efidx"],
                        Efstatus = rdr["efstatus"] == DBNull.Value ? null : (Int32?)rdr["efstatus"],
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                        Innovator = rdr["innovator"] == DBNull.Value ? null : (Int64?)rdr["innovator"],
                        Device = rdr["device"].ToString(),
                        Rescode = rdr["rescode"].ToString(),
                        Description = rdr["description"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public Boolean ValidateMasterpallet(string spallet)
        {
            Boolean bret = false;

            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder Sql = new StringBuilder();
                    Sql.AppendLine("select *");
                    Sql.AppendLine("from wms.mas_pallet_go");
                    Sql.AppendLine("where palletno = @palletno");

                    NpgsqlCommand cmd = new NpgsqlCommand(Sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    con.Open();

                    cmd.Parameters.AddWithValue("@palletno", NpgsqlDbType.Varchar, spallet);

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        bret = true;
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
            }
            return bret;
        }

        public Boolean ManageMasterAccessory(long idx, string itemcode, string itemname, double packqty, string packuon, string uom, double gosweight, double netweight , bool batchmanage, Int32 managecase ,ref  string retmessage)
        {
            Boolean bRet = false;
            string sRet = "";
            Int32? iRet = 0;
   
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    Int16 ibatchmanage;

                    StringBuilder sqlQurey = new StringBuilder();
                    //sqlQurey.AppendLine("select _retchk, _retmsg from wcs.fuc_create_mccommand(:mccode , :cmdcode, :command);");
                    sqlQurey.Append("dbo.ssp_manage_masteraccessories");
                    SqlCommand cmd = new SqlCommand(sqlQurey.ToString(), con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    con.Open();

                    if (batchmanage)
                        ibatchmanage = 1;
                    else
                        ibatchmanage = 0;

                    cmd.Parameters.AddWithValue("@_idx", idx);
                    cmd.Parameters.AddWithValue("@_itemcode", itemcode);
                    cmd.Parameters.AddWithValue("@_itemname", itemname);
                    cmd.Parameters.AddWithValue("@_packqty", packqty);
                    cmd.Parameters.AddWithValue("@_packuon", packuon);
                    cmd.Parameters.AddWithValue("@_uom", uom);
                    cmd.Parameters.AddWithValue("@_gosweight", gosweight);
                    cmd.Parameters.AddWithValue("@_netweight", netweight);
                    cmd.Parameters.AddWithValue("@_batchmanage", ibatchmanage);
                    cmd.Parameters.AddWithValue("@_managecase", managecase);

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
                retmessage = sRet;

                if (iRet == 1)
                {
                    bRet = true;
                }
            }
            return bRet;
        }

    }
}

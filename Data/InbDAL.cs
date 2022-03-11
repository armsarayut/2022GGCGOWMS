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
using GoWMS.Server.Models.Inb;
using NpgsqlTypes;
using System.Text;

namespace GoWMS.Server.Data
{
    public class InbDAL
    {
        readonly private string connectionString = ConnGlobals.GetConnLocalDBPG();
        readonly private string connectionStringSQL = ConnGlobals.GetConnDBSQL();

        public IEnumerable<Inb_Goodreceipt_Go> GetAllInbGoodreceiptGo()
        {
            List<Inb_Goodreceipt_Go> lstobj = new List<Inb_Goodreceipt_Go>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand cmd = new NpgsqlCommand("select * " +
                    "from wms.inb_goodreceipt_go " +
                    "order by efidx", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Inb_Goodreceipt_Go objrd = new Inb_Goodreceipt_Go
                    {
                        Efidx = rdr["efidx"] == DBNull.Value ? null : (Int64?)rdr["efidx"],
                        Efstatus = rdr["efstatus"] == DBNull.Value ? null : (Int32?)rdr["efstatus"],
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                        Innovator = rdr["innovator"] == DBNull.Value ? null : (Int64?)rdr["innovator"],
                        Device = rdr["device"].ToString(),
                        Pono = rdr["pono"].ToString(),
                        Pallettag = rdr["pallettag"].ToString(),
                        Itemtag = rdr["itemtag"].ToString(),
                        Itemcode = rdr["itemcode"].ToString(),
                        Itemname = rdr["itemname"].ToString(),
                        Itembar = rdr["itembar"].ToString(),
                        Unit = rdr["unit"].ToString(),
                        Weightunit = rdr["weightunit"].ToString(),
                        Quantity = rdr["quantity"] == DBNull.Value ? null : (decimal?)rdr["quantity"],
                        Weight = rdr["weight"] == DBNull.Value ? null : (decimal?)rdr["weight"],
                        Lotno = rdr["lotno"].ToString(),
                        Totalquantity = rdr["totalquantity"] == DBNull.Value ? null : (decimal?)rdr["totalquantity"],
                        Totalweight = rdr["totalweight"] == DBNull.Value ? null : (decimal?)rdr["totalweight"],
                        Docno = rdr["docno"].ToString(),
                        Docby = rdr["docby"].ToString(),
                        Docdate = rdr["docdate"] == DBNull.Value ? null : (DateTime?)rdr["docdate"],
                        Docnote = rdr["docnote"].ToString(),
                        Grntype = rdr["grntype"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public IEnumerable<Inb_Goodreceive_Go> GetAllInbGoodreceiveGo()
        {
            List<Inb_Goodreceive_Go> lstobj = new List<Inb_Goodreceive_Go>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand cmd = new NpgsqlCommand("select * " +
                    "from wms.inb_goodreceive_go " +
                    "order by efidx", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Inb_Goodreceive_Go objrd = new Inb_Goodreceive_Go
                    {
                        Efidx = rdr["efidx"] == DBNull.Value ? null : (Int64?)rdr["efidx"],
                        Efstatus = rdr["efstatus"] == DBNull.Value ? null : (Int32?)rdr["efstatus"],
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                        Innovator = rdr["innovator"] == DBNull.Value ? null : (Int64?)rdr["innovator"],
                        Device = rdr["device"].ToString(),
                        Pono = rdr["pono"].ToString(),
                        Pallettag = rdr["pallettag"].ToString(),
                        Itemtag = rdr["itemtag"].ToString(),
                        Itemcode = rdr["itemcode"].ToString(),
                        Itemname = rdr["itemname"].ToString(),
                        Itembar = rdr["itembar"].ToString(),
                        Unit = rdr["unit"].ToString(),
                        Weightunit = rdr["weightunit"].ToString(),
                        Quantity = rdr["quantity"] == DBNull.Value ? null : (decimal?)rdr["quantity"],
                        Weight = rdr["weight"] == DBNull.Value ? null : (decimal?)rdr["weight"],
                        Lotno = rdr["lotno"].ToString(),
                        Totalquantity = rdr["totalquantity"] == DBNull.Value ? null : (decimal?)rdr["totalquantity"],
                        Totalweight = rdr["totalweight"] == DBNull.Value ? null : (decimal?)rdr["totalweight"],
                        Docno = rdr["docno"].ToString(),
                        Docby = rdr["docby"].ToString(),
                        Docdate = rdr["docdate"] == DBNull.Value ? null : (DateTime?)rdr["docdate"],
                        Docnote = rdr["docnote"].ToString(),
                        Grnrefer = rdr["grnrefer"] == DBNull.Value ? null : (Int64?)rdr["grnrefer"],
                        Grntime = rdr["grntime"] == DBNull.Value ? null : (DateTime?)rdr["grntime"],
                        Grtime = rdr["grtime"] == DBNull.Value ? null : (DateTime?)rdr["grtime"],
                        Grtype = rdr["grtype"].ToString(),
                        Pallteno = rdr["pallteno"].ToString(),
                        Palltmapkey = rdr["palltmapkey"].ToString(),
                        Storagetime = rdr["storagetime"] == DBNull.Value ? null : (DateTime?)rdr["storagetime"],
                        Storageno = rdr["storageno"].ToString(),
                        Storagearea = rdr["storagearea"].ToString(),
                        Storagebin = rdr["storagebin"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public IEnumerable<Inb_Putaway_Go> GetAllInbPutawayGo()
        {
            List<Inb_Putaway_Go> lstobj = new List<Inb_Putaway_Go>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand cmd = new NpgsqlCommand("select * " +
                    "from wms.inb_putaway_go  " +
                    "order by efidx", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Inb_Putaway_Go objrd = new Inb_Putaway_Go
                    {
                        Efidx = rdr["efidx"] == DBNull.Value ? null : (Int64?)rdr["efidx"],
                        Efstatus = rdr["efstatus"] == DBNull.Value ? null : (Int32?)rdr["efstatus"],
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                        Innovator = rdr["innovator"] == DBNull.Value ? null : (Int64?)rdr["innovator"],
                        Device = rdr["device"].ToString(),
                        Palletmapkey = rdr["palletmapkey"].ToString(),
                        Puttype = rdr["puttype"].ToString(),
                        Palletno = rdr["palletno"].ToString(),
                        Started = rdr["started"] == DBNull.Value ? null : (DateTime?)rdr["started"],
                        Loadted = rdr["loadted"] == DBNull.Value ? null : (DateTime?)rdr["loadted"],
                        Completed = rdr["completed"] == DBNull.Value ? null : (DateTime?)rdr["completed"],
                        Storagetime = rdr["storagetime"] == DBNull.Value ? null : (DateTime?)rdr["storagetime"],
                        Storageno = rdr["storageno"].ToString(),
                        Storagearea = rdr["storagearea"].ToString(),
                        Storagebin = rdr["storagebin"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }


        public IEnumerable<Inb_Putaway_Go> GetAllInbPutawayGoBypallet(string pallet)
        {
            List<Inb_Putaway_Go> lstobj = new List<Inb_Putaway_Go>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand cmd = new NpgsqlCommand("select * " +
                    "from wms.inb_putaway_go  " +
                     "where palletno = @palletno  " +
                    "order by efidx", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                cmd.Parameters.AddWithValue("@palletno", NpgsqlDbType.Varchar, pallet);

                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Inb_Putaway_Go objrd = new Inb_Putaway_Go
                    {
                        Efidx = rdr["efidx"] == DBNull.Value ? null : (Int64?)rdr["efidx"],
                        Efstatus = rdr["efstatus"] == DBNull.Value ? null : (Int32?)rdr["efstatus"],
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                        Innovator = rdr["innovator"] == DBNull.Value ? null : (Int64?)rdr["innovator"],
                        Device = rdr["device"].ToString(),
                        Palletmapkey = rdr["palletmapkey"].ToString(),
                        Puttype = rdr["puttype"].ToString(),
                        Palletno = rdr["palletno"].ToString(),
                        Started = rdr["started"] == DBNull.Value ? null : (DateTime?)rdr["started"],
                        Loadted = rdr["loadted"] == DBNull.Value ? null : (DateTime?)rdr["loadted"],
                        Completed = rdr["completed"] == DBNull.Value ? null : (DateTime?)rdr["completed"],
                        Storagetime = rdr["storagetime"] == DBNull.Value ? null : (DateTime?)rdr["storagetime"],
                        Storageno = rdr["storageno"].ToString(),
                        Storagearea = rdr["storagearea"].ToString(),
                        Storagebin = rdr["storagebin"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public void SetStorageComplete(string pallet, string bin)
        {
            Int32? iRet = 0;
            string sRet = "Calling";
            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            try
            {
                con.Open();
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("Call wms.poc_inb_storagebin(");
                sql.AppendLine("@spalletno, @sbin, @retchk, @retmsg)");
                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.AddWithValue("@spalletno", pallet);
                cmd.Parameters.AddWithValue("@sbin", bin);
                cmd.Parameters.AddWithValue("@retchk", iRet);
                cmd.Parameters.AddWithValue("@retmsg", sRet);
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    iRet = rdr["retchk"] == DBNull.Value ? null : (Int32?)rdr["retchk"];
                    sRet = rdr["retmsg"].ToString();
                }
            }
            catch (NpgsqlException exp)
            {
                //Response.Write(exp.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        public async Task<Int64> GetSumOrderAllInbGoodreceiptGo()
        {
            Int64 lRet = 0;
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                StringBuilder sqlQurey = new StringBuilder();

                sqlQurey.AppendLine("select count(*) ");
                sqlQurey.AppendLine("from [dbo].[wms_trans] t ");
                sqlQurey.AppendLine("inner join [dbo].[v_itemmaster] i ");
                sqlQurey.AppendLine("on t.[item]=i.[item] ");
                sqlQurey.AppendLine("where 1=1");
                sqlQurey.AppendLine("and t.[is_req]=0");
                sqlQurey.AppendLine("and t.[stat]=0");
                sqlQurey.AppendLine("and t.[flag]=0");
                sqlQurey.AppendLine("and t.[unit_key] in ('01')");
                sqlQurey.AppendLine(";");

                SqlCommand cmd = new SqlCommand(sqlQurey.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                //NpgsqlCommand cmd = new NpgsqlCommand("select count(*) " +
                //   "from wms.inb_goodreceive_go " +
                //   "where pallteno in (select palletno from wms.inb_putaway_go)", con)
                //{
                //    CommandType = CommandType.Text
                //};

                con.Open();

                lRet= Convert.ToInt64(cmd.ExecuteScalar());
                
                con.Close();
            }

            return lRet;
        }

        public async Task<Int64> GetSumPalletAllInbGoodreceiptGo()
        {
            Int64 lRet = 0;
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                StringBuilder sqlQurey = new StringBuilder();

                sqlQurey.AppendLine("select count(lpncode)");
                sqlQurey.AppendLine("from wcs.tas_works");
                sqlQurey.AppendLine("where work_code='01'");
                sqlQurey.AppendLine("and work_status='AVL'");
                sqlQurey.AppendLine(";");

                SqlCommand cmd = new SqlCommand(sqlQurey.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                //NpgsqlCommand cmd = new NpgsqlCommand("select count(*) " +
                //    "from wms.inb_putaway_go " +
                //    "where efstatus=0 ", con)
                //{
                //    CommandType = CommandType.Text
                //};

                con.Open();
                lRet = Convert.ToInt64(cmd.ExecuteScalar());

                con.Close();
            }

            return lRet;
        }


        public async Task<Int64> GetSumPalletAllOubGoodPickingGo()
        {
            Int64 lRet = 0;
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                SqlCommand cmd = new SqlCommand("select count(lpncode) " +
                    "from wcs.tas_works " +
                    "where work_code='05' " +
                    "and work_status='AVL' ", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                lRet = Convert.ToInt64(cmd.ExecuteScalar());

                con.Close();
            }

            return lRet;
        }

        public async Task<Int64> GetSumOrderAllOubGoodPickingGo()
        {
            Int64 lRet = 0;
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                SqlCommand cmd = new SqlCommand("select count(*) " +
                    "from dbo.sap_storeout " +
                    "where status<3 ", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                lRet = Convert.ToInt64(cmd.ExecuteScalar());

                con.Close();
            }

            return lRet;
        }


        public bool CancelReceivingOrdersBypack(string pallet, string pack)
        {
            bool bret=false;
            try
            {
                using NpgsqlConnection con = new NpgsqlConnection(connectionString);
                StringBuilder sql = new StringBuilder();

                sql.AppendLine("Delete From wms.inb_goodreceive_go");
                sql.AppendLine("Where pallettag = @Pack ");
                sql.AppendLine("and pallteno = @Pallet");
                sql.AppendLine(";");

                sql.AppendLine("Delete From public.sap_storein");
                sql.AppendLine("Where su_no = @su_no ");
                sql.AppendLine("and sap_su = @sap_su");
                sql.AppendLine(";");

                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.AddWithValue("@Pallet", pallet);
                cmd.Parameters.AddWithValue("@Pack", pack);

                cmd.Parameters.AddWithValue("@sap_su", pallet);
                cmd.Parameters.AddWithValue("@su_no", pack);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                bret = true;
            }
            catch
            {

            }
            return bret;
        }


        public bool CancelReceivingOrdersByPallet(string pallet)
        {
            bool bret = false;
            try
            {
                using NpgsqlConnection con = new NpgsqlConnection(connectionString);
                StringBuilder sql = new StringBuilder();

                sql.AppendLine("Delete From wms.inb_goodreceive_go");
                sql.AppendLine("Where pallteno = @pallteno");
                sql.AppendLine(";");

                sql.AppendLine("Delete From public.sap_storein");
                sql.AppendLine("Where sap_su = @sap_su");
                sql.AppendLine(";");

                sql.AppendLine("Delete From wcs.tas_works");
                sql.AppendLine("Where lpncode = @lpncode ");
                sql.AppendLine(";");

                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.AddWithValue("@pallteno", pallet);
                cmd.Parameters.AddWithValue("@sap_su", pallet);
                cmd.Parameters.AddWithValue("@lpncode", pallet);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                bret = true;
            }
            catch
            {

            }
            return bret;
        }
    }
}

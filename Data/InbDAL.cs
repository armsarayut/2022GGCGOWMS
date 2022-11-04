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
using GoWMS.Server.Models.Api;

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


            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendLine("select t0.site,t0.doc_num,cast(t0.trans_num as bigint) trans_num,t0.ref_type,t0.trans_type,t0.trans_date");
                sql.AppendLine(" ,t0.unit_key,t0.item_bc,t0.item, t1.item_name, t1.uom, t1.weight_unit,t0.qty");
                sql.AppendLine(",t0.lot,t0.prod_date,t0.stat,t0.source");
                sql.AppendLine(",t0.reason,t0.createdby,t0.createddate");
                sql.AppendLine(",t0.whse,t0.loc,t0.pallet_bc,t0.ref_item");
                sql.AppendLine(",t0.flag,t0.require_detail_id,t0.is_req");
                sql.AppendLine(",t0.is_hold,t0.is_lock,t0.update2sl,t0.modifie_date");
                sql.AppendLine("FROM dbo.wms_trans t0");
                sql.AppendLine("LEFT JOIN dbo.set_itemmaster t1");
                sql.AppendLine("ON t0.item=t1.item_code");
                sql.AppendLine("WHERE t0.unit_key = @unit_key");
                sql.AppendLine("AND t0.stat = @stat");
                sql.AppendLine("ORDER BY t0.trans_num");

                SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@unit_key", "01");
                cmd.Parameters.AddWithValue("@stat", 0);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Inb_Goodreceive_Go objrd = new Inb_Goodreceive_Go
                    {
                        Efidx = rdr["trans_num"] == DBNull.Value ? null : (Int64?)rdr["trans_num"],
                        Efstatus = 0,
                        Created = rdr["createddate"] == DBNull.Value ? null : (DateTime?)rdr["createddate"],
                        Modified = rdr["modifie_date"] == DBNull.Value ? null : (DateTime?)rdr["modifie_date"],
                        Innovator = 0,
                        Device = null,
                        Pono = rdr["doc_num"].ToString(),
                        Pallettag = rdr["item_bc"].ToString(),
                        Itemtag = rdr["item_bc"].ToString(),
                        Itemcode = rdr["item"].ToString(),
                        Itemname = rdr["item_name"].ToString(),
                        Itembar = rdr["item_bc"].ToString(),
                        Unit = rdr["uom"].ToString(),
                        Weightunit = rdr["weight_unit"].ToString(),
                        Quantity = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                        Weight = 0,
                        Lotno = rdr["lot"].ToString(),
                        Totalquantity = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                        Totalweight = 0,
                        Docno = rdr["doc_num"].ToString(),
                        Docby = rdr["source"].ToString(),
                        Docdate = rdr["trans_date"] == DBNull.Value ? null : (DateTime?)rdr["trans_date"],
                        Docnote = null,
                        Grnrefer = null,
                        Grntime = rdr["trans_date"] == DBNull.Value ? null : (DateTime?)rdr["trans_date"],
                        Grtime = rdr["modifie_date"] == DBNull.Value ? null : (DateTime?)rdr["modifie_date"],
                        Grtype = rdr["unit_key"].ToString(),
                        Pallteno = rdr["pallet_bc"].ToString(),
                        Palltmapkey = rdr["ref_item"].ToString(),
                        Storagetime = rdr["modifie_date"] == DBNull.Value ? null : (DateTime?)rdr["modifie_date"],
                        Storageno = rdr["site"].ToString(),
                        Storagearea = rdr["whse"].ToString(),
                        Storagebin = rdr["loc"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }

            //using (SqlConnection con = new SqlConnection(connectionStringSQL))
            //{
            //    SqlCommand cmd = new SqlCommand("select * " +
            //       "from wms.inb_goodreceive_go " +
            //       "order by efidx", con)
            //    {
            //        CommandType = CommandType.Text
            //    };

            //    con.Open();
            //    NpgsqlDataReader rdr = cmd.ExecuteReader();
            //    while (rdr.Read())
            //    {
            //        Inb_Goodreceive_Go objrd = new Inb_Goodreceive_Go
            //        {
            //            Efidx = rdr["efidx"] == DBNull.Value ? null : (Int64?)rdr["efidx"],
            //            Efstatus = rdr["efstatus"] == DBNull.Value ? null : (Int32?)rdr["efstatus"],
            //            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
            //            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
            //            Innovator = rdr["innovator"] == DBNull.Value ? null : (Int64?)rdr["innovator"],
            //            Device = rdr["device"].ToString(),
            //            Pono = rdr["pono"].ToString(),
            //            Pallettag = rdr["pallettag"].ToString(),
            //            Itemtag = rdr["itemtag"].ToString(),
            //            Itemcode = rdr["itemcode"].ToString(),
            //            Itemname = rdr["itemname"].ToString(),
            //            Itembar = rdr["itembar"].ToString(),
            //            Unit = rdr["unit"].ToString(),
            //            Weightunit = rdr["weightunit"].ToString(),
            //            Quantity = rdr["quantity"] == DBNull.Value ? null : (decimal?)rdr["quantity"],
            //            Weight = rdr["weight"] == DBNull.Value ? null : (decimal?)rdr["weight"],
            //            Lotno = rdr["lotno"].ToString(),
            //            Totalquantity = rdr["totalquantity"] == DBNull.Value ? null : (decimal?)rdr["totalquantity"],
            //            Totalweight = rdr["totalweight"] == DBNull.Value ? null : (decimal?)rdr["totalweight"],
            //            Docno = rdr["docno"].ToString(),
            //            Docby = rdr["docby"].ToString(),
            //            Docdate = rdr["docdate"] == DBNull.Value ? null : (DateTime?)rdr["docdate"],
            //            Docnote = rdr["docnote"].ToString(),
            //            Grnrefer = rdr["grnrefer"] == DBNull.Value ? null : (Int64?)rdr["grnrefer"],
            //            Grntime = rdr["grntime"] == DBNull.Value ? null : (DateTime?)rdr["grntime"],
            //            Grtime = rdr["grtime"] == DBNull.Value ? null : (DateTime?)rdr["grtime"],
            //            Grtype = rdr["grtype"].ToString(),
            //            Pallteno = rdr["pallteno"].ToString(),
            //            Palltmapkey = rdr["palltmapkey"].ToString(),
            //            Storagetime = rdr["storagetime"] == DBNull.Value ? null : (DateTime?)rdr["storagetime"],
            //            Storageno = rdr["storageno"].ToString(),
            //            Storagearea = rdr["storagearea"].ToString(),
            //            Storagebin = rdr["storagebin"].ToString()
            //        };
            //        lstobj.Add(objrd);
            //    }
            //    con.Close();
            //}
            return lstobj;
        }

        public IEnumerable<Inb_Goodreceive_Go> GetAllInbGoodreceiveGobyTag(string sTag)
        {
            List<Inb_Goodreceive_Go> lstobj = new List<Inb_Goodreceive_Go>();


            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendLine("select t0.site,t0.doc_num,cast(t0.trans_num as bigint) trans_num,t0.ref_type,t0.trans_type,t0.trans_date");
                sql.AppendLine(" ,t0.unit_key,t0.item_bc,t0.item, t1.item_name, t1.uom, t1.weight_unit,t0.qty");
                sql.AppendLine(",t0.lot,t0.prod_date,t0.stat,t0.source");
                sql.AppendLine(",t0.reason,t0.createdby,t0.createddate");
                sql.AppendLine(",t0.whse,t0.loc,t0.pallet_bc,t0.ref_item");
                sql.AppendLine(",t0.flag,t0.require_detail_id,t0.is_req");
                sql.AppendLine(",t0.is_hold,t0.is_lock,t0.update2sl,t0.modifie_date");
                sql.AppendLine("FROM dbo.wms_trans t0");
                sql.AppendLine("LEFT JOIN dbo.set_itemmaster t1");
                sql.AppendLine("ON t0.item=t1.item_code");
                sql.AppendLine("WHERE t0.unit_key = @unit_key");
                sql.AppendLine("AND t0.item_bc = @item_bc");
                sql.AppendLine("AND t0.stat = @stat");
                sql.AppendLine("ORDER BY t0.trans_num");

                SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@unit_key", "01");
                cmd.Parameters.AddWithValue("@item_bc", sTag);
                cmd.Parameters.AddWithValue("@stat", 0);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Inb_Goodreceive_Go objrd = new Inb_Goodreceive_Go
                    {
                        Efidx = rdr["trans_num"] == DBNull.Value ? null : (Int64?)rdr["trans_num"],
                        Efstatus = 0,
                        Created = rdr["trans_date"] == DBNull.Value ? null : (DateTime?)rdr["trans_date"],
                        Modified = rdr["modifie_date"] == DBNull.Value ? null : (DateTime?)rdr["modifie_date"],
                        Innovator = 0,
                        Device = null,
                        Pono = rdr["doc_num"].ToString(),
                        Pallettag = rdr["item_bc"].ToString(),
                        Itemtag = rdr["item_bc"].ToString(),
                        Itemcode = rdr["item"].ToString(),
                        Itemname = rdr["item_name"].ToString(),
                        Itembar = rdr["item_bc"].ToString(),
                        Unit = rdr["uom"].ToString(),
                        Weightunit = rdr["weight_unit"].ToString(),
                        Quantity = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                        Weight = 0,
                        Lotno = rdr["lot"].ToString(),
                        Totalquantity = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                        Totalweight = 0,
                        Docno = rdr["doc_num"].ToString(),
                        Docby = rdr["source"].ToString(),
                        Docdate = rdr["trans_date"] == DBNull.Value ? null : (DateTime?)rdr["trans_date"],
                        Docnote = null,
                        Grnrefer = null,
                        Grntime = rdr["trans_date"] == DBNull.Value ? null : (DateTime?)rdr["trans_date"],
                        Grtime = rdr["modifie_date"] == DBNull.Value ? null : (DateTime?)rdr["modifie_date"],
                        Grtype = rdr["unit_key"].ToString(),
                        Pallteno = rdr["pallet_bc"].ToString(),
                        Palltmapkey = rdr["ref_item"].ToString(),
                        Storagetime = rdr["modifie_date"] == DBNull.Value ? null : (DateTime?)rdr["modifie_date"],
                        Storageno = rdr["site"].ToString(),
                        Storagearea = rdr["whse"].ToString(),
                        Storagebin = rdr["loc"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }

            //using (SqlConnection con = new SqlConnection(connectionStringSQL))
            //{
            //    SqlCommand cmd = new SqlCommand("select * " +
            //       "from wms.inb_goodreceive_go " +
            //       "order by efidx", con)
            //    {
            //        CommandType = CommandType.Text
            //    };

            //    con.Open();
            //    NpgsqlDataReader rdr = cmd.ExecuteReader();
            //    while (rdr.Read())
            //    {
            //        Inb_Goodreceive_Go objrd = new Inb_Goodreceive_Go
            //        {
            //            Efidx = rdr["efidx"] == DBNull.Value ? null : (Int64?)rdr["efidx"],
            //            Efstatus = rdr["efstatus"] == DBNull.Value ? null : (Int32?)rdr["efstatus"],
            //            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
            //            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
            //            Innovator = rdr["innovator"] == DBNull.Value ? null : (Int64?)rdr["innovator"],
            //            Device = rdr["device"].ToString(),
            //            Pono = rdr["pono"].ToString(),
            //            Pallettag = rdr["pallettag"].ToString(),
            //            Itemtag = rdr["itemtag"].ToString(),
            //            Itemcode = rdr["itemcode"].ToString(),
            //            Itemname = rdr["itemname"].ToString(),
            //            Itembar = rdr["itembar"].ToString(),
            //            Unit = rdr["unit"].ToString(),
            //            Weightunit = rdr["weightunit"].ToString(),
            //            Quantity = rdr["quantity"] == DBNull.Value ? null : (decimal?)rdr["quantity"],
            //            Weight = rdr["weight"] == DBNull.Value ? null : (decimal?)rdr["weight"],
            //            Lotno = rdr["lotno"].ToString(),
            //            Totalquantity = rdr["totalquantity"] == DBNull.Value ? null : (decimal?)rdr["totalquantity"],
            //            Totalweight = rdr["totalweight"] == DBNull.Value ? null : (decimal?)rdr["totalweight"],
            //            Docno = rdr["docno"].ToString(),
            //            Docby = rdr["docby"].ToString(),
            //            Docdate = rdr["docdate"] == DBNull.Value ? null : (DateTime?)rdr["docdate"],
            //            Docnote = rdr["docnote"].ToString(),
            //            Grnrefer = rdr["grnrefer"] == DBNull.Value ? null : (Int64?)rdr["grnrefer"],
            //            Grntime = rdr["grntime"] == DBNull.Value ? null : (DateTime?)rdr["grntime"],
            //            Grtime = rdr["grtime"] == DBNull.Value ? null : (DateTime?)rdr["grtime"],
            //            Grtype = rdr["grtype"].ToString(),
            //            Pallteno = rdr["pallteno"].ToString(),
            //            Palltmapkey = rdr["palltmapkey"].ToString(),
            //            Storagetime = rdr["storagetime"] == DBNull.Value ? null : (DateTime?)rdr["storagetime"],
            //            Storageno = rdr["storageno"].ToString(),
            //            Storagearea = rdr["storagearea"].ToString(),
            //            Storagebin = rdr["storagebin"].ToString()
            //        };
            //        lstobj.Add(objrd);
            //    }
            //    con.Close();
            //}
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

        public bool CancelReceivingOrdersBypackId(long transId, decimal tramsQty, string tranDoc, string tranItem, string tranTag )
        {
            bool bret = false;
            try
            {
                using SqlConnection con = new SqlConnection(connectionStringSQL);
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("Begin");

                sql.AppendLine("Delete From [dbo].[wms_trans]");
                sql.AppendLine("Where [trans_num] = @trans_num ");
                sql.AppendLine(";");

                sql.AppendLine("Update [dbo].[api_ggc]");
                sql.AppendLine("Set [gr_qty] = [gr_qty] - @gr_qty");
                sql.AppendLine("Where [doc_ref] = @doc_ref ");
                sql.AppendLine("and [item_code] = @item_code");
                sql.AppendLine(";");

                sql.AppendLine("Update [dbo].[wms_lablemaster]");
                sql.AppendLine("Set [palletnote] = 0");
                sql.AppendLine(", [entity_lock] = 99");
                sql.AppendLine("Where [item_bc] = @item_bc ");
                sql.AppendLine(";");

                

                sql.AppendLine("End");
                SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@trans_num", transId);
                cmd.Parameters.AddWithValue("@gr_qty", tramsQty);
                cmd.Parameters.AddWithValue("@doc_ref", tranDoc);
                cmd.Parameters.AddWithValue("@item_code", tranItem);
                cmd.Parameters.AddWithValue("@item_bc", tranTag);
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


        public bool CreateTagno(Int64 valapiref, Int32 valpalletfrom, Int32 valpalletto, string valtranref,string valtrancreateby,DataTable valTransData , string valuuid )
        {
            bool bRet = false;
            string sRet = "";
            Int32? iRet = 0;

            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sqlQurey = new StringBuilder();
                    //sqlQurey.AppendLine("select _retchk, _retmsg from wcs.fuc_create_mcprotocol(:mccode , :startpos, :stoppos, :unittype, :palletid , :weight, :command);");
                    sqlQurey.Append("dbo.ssp_createwmstransbydoc_erp");
                    SqlCommand cmd = new SqlCommand(sqlQurey.ToString(), con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    con.Open();

                    SqlParameter papiref = new SqlParameter("@apiref", SqlDbType.BigInt);
                    papiref.Direction = ParameterDirection.Input;
                    papiref.Value = valapiref;

                    SqlParameter ppalletfrom = new SqlParameter("@palletfrom", SqlDbType.Int);
                    ppalletfrom.Direction = ParameterDirection.Input;
                    ppalletfrom.Value = valpalletfrom;

                    SqlParameter ppalletto = new SqlParameter("@palletto", SqlDbType.Int);
                    ppalletto.Direction = ParameterDirection.Input;
                    ppalletto.Value = valpalletto;

                    SqlParameter ptranref = new SqlParameter("@tranref", SqlDbType.VarChar,20);
                    ptranref.Direction = ParameterDirection.Input;
                    ptranref.Value = valtranref;

                    SqlParameter ptrancreateby = new SqlParameter("@trancreateby", SqlDbType.VarChar, 20);
                    ptrancreateby.Direction = ParameterDirection.Input;
                    ptrancreateby.Value = valtrancreateby;

                    SqlParameter pTransData = new SqlParameter("@TransData", SqlDbType.Structured);
                    pTransData.Direction = ParameterDirection.Input;
                    pTransData.Value = valTransData;


                    cmd.Parameters.Add(papiref);
                    cmd.Parameters.Add(ppalletfrom);
                    cmd.Parameters.Add(ppalletto);
                    cmd.Parameters.Add(ptranref);
                    cmd.Parameters.Add(ptrancreateby);
                    cmd.Parameters.Add(pTransData);

                    //cmd.Parameters.AddWithValue(":mccode", NpgsqlDbType.Varchar, mccode);
                    //cmd.Parameters.AddWithValue(":startpos", NpgsqlDbType.Integer, startpos);
                    //cmd.Parameters.AddWithValue(":stoppos", NpgsqlDbType.Integer, stoppos);
                    //cmd.Parameters.AddWithValue(":unittype", NpgsqlDbType.Integer, unittyp);
                    //cmd.Parameters.AddWithValue(":palletid", NpgsqlDbType.Varchar, palletid);
                    //cmd.Parameters.AddWithValue(":weight", NpgsqlDbType.Integer, weight);
                    //cmd.Parameters.AddWithValue(":command", NpgsqlDbType.Integer, command);

                    SqlParameter RuturnCheck = new SqlParameter("@_retchk", SqlDbType.Int);
                    RuturnCheck.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnCheck);

                    SqlParameter RuturnMsg = new SqlParameter("@_retmsg", SqlDbType.VarChar, 255);
                    RuturnMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnMsg);

                    cmd.ExecuteNonQuery();

                    iRet = (Int32)cmd.Parameters["@_retchk"].Value;
                    sRet = (string)cmd.Parameters["@_retmsg"].Value;

                    //con.Open();
                    //NpgsqlDataReader rdr = cmd.ExecuteReader();
                    //while (rdr.Read())
                    //{
                    //    iRet = rdr["_retchk"] == DBNull.Value ? null : (Int32?)rdr["_retchk"];
                    //    sRet = rdr["_retmsg"].ToString();
                    //}
                }
                catch (SqlException ex)
                {
                    //Log.Error(ex.ToString());
                    sRet = ex.Message.ToString();
                }
                finally
                {
                    con.Close();
                }

                if (iRet == 1)
                {
                    bRet = true;
                }

                //sreturn = sRet;

            }

            return bRet;
        }


        public bool CreateTagnodup(Int64 valapiref, Int32 valpalletfrom, Int32 valpalletto, string valtranref, string valtrancreateby, DataTable valTransData, string valuuid, ref string srtrReturn)
        {
            bool bRet = false;
            string sRet = "";
            Int32? iRet = 0;

            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sqlQurey = new StringBuilder();
                    //sqlQurey.AppendLine("select _retchk, _retmsg from wcs.fuc_create_mcprotocol(:mccode , :startpos, :stoppos, :unittype, :palletid , :weight, :command);");
                    sqlQurey.Append("dbo.ssp_createwmstransbydoc_erp");
                    SqlCommand cmd = new SqlCommand(sqlQurey.ToString(), con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    con.Open();

                    SqlParameter papiref = new SqlParameter("@apiref", SqlDbType.BigInt);
                    papiref.Direction = ParameterDirection.Input;
                    papiref.Value = valapiref;

                    SqlParameter ppalletfrom = new SqlParameter("@palletfrom", SqlDbType.Int);
                    ppalletfrom.Direction = ParameterDirection.Input;
                    ppalletfrom.Value = valpalletfrom;

                    SqlParameter ppalletto = new SqlParameter("@palletto", SqlDbType.Int);
                    ppalletto.Direction = ParameterDirection.Input;
                    ppalletto.Value = valpalletto;

                    SqlParameter ptranref = new SqlParameter("@tranref", SqlDbType.VarChar, 20);
                    ptranref.Direction = ParameterDirection.Input;
                    ptranref.Value = valtranref;

                    SqlParameter ptrancreateby = new SqlParameter("@trancreateby", SqlDbType.VarChar, 20);
                    ptrancreateby.Direction = ParameterDirection.Input;
                    ptrancreateby.Value = valtrancreateby;

                    SqlParameter pTransData = new SqlParameter("@TransData", SqlDbType.Structured);
                    pTransData.Direction = ParameterDirection.Input;
                    pTransData.Value = valTransData;


                    cmd.Parameters.Add(papiref);
                    cmd.Parameters.Add(ppalletfrom);
                    cmd.Parameters.Add(ppalletto);
                    cmd.Parameters.Add(ptranref);
                    cmd.Parameters.Add(ptrancreateby);
                    cmd.Parameters.Add(pTransData);

                    //cmd.Parameters.AddWithValue(":mccode", NpgsqlDbType.Varchar, mccode);
                    //cmd.Parameters.AddWithValue(":startpos", NpgsqlDbType.Integer, startpos);
                    //cmd.Parameters.AddWithValue(":stoppos", NpgsqlDbType.Integer, stoppos);
                    //cmd.Parameters.AddWithValue(":unittype", NpgsqlDbType.Integer, unittyp);
                    //cmd.Parameters.AddWithValue(":palletid", NpgsqlDbType.Varchar, palletid);
                    //cmd.Parameters.AddWithValue(":weight", NpgsqlDbType.Integer, weight);
                    //cmd.Parameters.AddWithValue(":command", NpgsqlDbType.Integer, command);

                    SqlParameter RuturnCheck = new SqlParameter("@_retchk", SqlDbType.Int);
                    RuturnCheck.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnCheck);

                    SqlParameter RuturnMsg = new SqlParameter("@_retmsg", SqlDbType.VarChar, 255);
                    RuturnMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnMsg);

                    cmd.ExecuteNonQuery();

                    iRet = (Int32)cmd.Parameters["@_retchk"].Value;
                    sRet = (string)cmd.Parameters["@_retmsg"].Value;

                    //con.Open();
                    //NpgsqlDataReader rdr = cmd.ExecuteReader();
                    //while (rdr.Read())
                    //{
                    //    iRet = rdr["_retchk"] == DBNull.Value ? null : (Int32?)rdr["_retchk"];
                    //    sRet = rdr["_retmsg"].ToString();
                    //}
                }
                catch (SqlException ex)
                {
                    //Log.Error(ex.ToString());
                    sRet = ex.Message.ToString();
                }
                finally
                {
                    con.Close();
                }

                if (iRet == 1)
                {
                    bRet = true;
                }

                srtrReturn = sRet;

            }

            return bRet;
        }



        public bool CreateTagnoACC(Int64 valapiref, Int32 valpalletfrom, Int32 valpalletto, string valtranref, string valtrancreateby, DataTable valTransData, string valuuid)
        {
            bool bRet = false;
            string sRet = "";
            Int32? iRet = 0;

            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sqlQurey = new StringBuilder();
                    //sqlQurey.AppendLine("select _retchk, _retmsg from wcs.fuc_create_mcprotocol(:mccode , :startpos, :stoppos, :unittype, :palletid , :weight, :command);");
                    sqlQurey.Append("dbo.ssp_createwmstransbydoc_acc");
                    SqlCommand cmd = new SqlCommand(sqlQurey.ToString(), con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    con.Open();

                    SqlParameter papiref = new SqlParameter("@apiref", SqlDbType.BigInt);
                    papiref.Direction = ParameterDirection.Input;
                    papiref.Value = valapiref;

                    SqlParameter ppalletfrom = new SqlParameter("@palletfrom", SqlDbType.Int);
                    ppalletfrom.Direction = ParameterDirection.Input;
                    ppalletfrom.Value = valpalletfrom;

                    SqlParameter ppalletto = new SqlParameter("@palletto", SqlDbType.Int);
                    ppalletto.Direction = ParameterDirection.Input;
                    ppalletto.Value = valpalletto;

                    SqlParameter ptranref = new SqlParameter("@tranref", SqlDbType.VarChar, 20);
                    ptranref.Direction = ParameterDirection.Input;
                    ptranref.Value = valtranref;

                    SqlParameter ptrancreateby = new SqlParameter("@trancreateby", SqlDbType.VarChar, 20);
                    ptrancreateby.Direction = ParameterDirection.Input;
                    ptrancreateby.Value = valtrancreateby;

                    SqlParameter pTransData = new SqlParameter("@TransData", SqlDbType.Structured);
                    pTransData.Direction = ParameterDirection.Input;
                    pTransData.Value = valTransData;


                    cmd.Parameters.Add(papiref);
                    cmd.Parameters.Add(ppalletfrom);
                    cmd.Parameters.Add(ppalletto);
                    cmd.Parameters.Add(ptranref);
                    cmd.Parameters.Add(ptrancreateby);
                    cmd.Parameters.Add(pTransData);

                    //cmd.Parameters.AddWithValue(":mccode", NpgsqlDbType.Varchar, mccode);
                    //cmd.Parameters.AddWithValue(":startpos", NpgsqlDbType.Integer, startpos);
                    //cmd.Parameters.AddWithValue(":stoppos", NpgsqlDbType.Integer, stoppos);
                    //cmd.Parameters.AddWithValue(":unittype", NpgsqlDbType.Integer, unittyp);
                    //cmd.Parameters.AddWithValue(":palletid", NpgsqlDbType.Varchar, palletid);
                    //cmd.Parameters.AddWithValue(":weight", NpgsqlDbType.Integer, weight);
                    //cmd.Parameters.AddWithValue(":command", NpgsqlDbType.Integer, command);

                    SqlParameter RuturnCheck = new SqlParameter("@_retchk", SqlDbType.Int);
                    RuturnCheck.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnCheck);

                    SqlParameter RuturnMsg = new SqlParameter("@_retmsg", SqlDbType.VarChar, 255);
                    RuturnMsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnMsg);

                    cmd.ExecuteNonQuery();

                    iRet = (Int32)cmd.Parameters["@_retchk"].Value;
                    sRet = (string)cmd.Parameters["@_retmsg"].Value;


                    //con.Open();
                    //NpgsqlDataReader rdr = cmd.ExecuteReader();
                    //while (rdr.Read())
                    //{
                    //    iRet = rdr["_retchk"] == DBNull.Value ? null : (Int32?)rdr["_retchk"];
                    //    sRet = rdr["_retmsg"].ToString();
                    //}
                }
                catch (SqlException ex)
                {
                    //Log.Error(ex.ToString());
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


        public IEnumerable<ggcTag4x4> GetTagGGCByIndex(long apiid)
        {
            List<ggcTag4x4> lstobj = new List<ggcTag4x4>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select t1.item_bc, t1.lot, t1.item, t2.item_name, t1.qty, t2.pack_uom, t1.prod_date, t1.doc_num, t1.createdby ");
                sql.AppendLine("from dbo.wms_trans t1");
                sql.AppendLine("left join dbo.set_itemmaster t2");
                sql.AppendLine("on t1.item = t2.item_code");
                sql.AppendLine("where t1.ref_item =  @ref_item");
                sql.AppendLine("and t1.unit_key =  @unit_key");
                sql.AppendLine("order by t1.trans_num");
                sql.AppendLine(";");

                SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();

                cmd.Parameters.AddWithValue("@ref_item", apiid.ToString());
                cmd.Parameters.AddWithValue("@unit_key", "01");

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ggcTag4x4 objrd = new ggcTag4x4
                    {
                        PalletNo = "1",
                        TagNo = rdr["item_bc"].ToString(),
                        BatchNo= rdr["lot"].ToString(),
                        ProductNo = rdr["item"].ToString(),
                        ProductName = rdr["item_name"].ToString(),
                       QtyPerPallet = rdr["qty"].ToString(),
                       Packaging = rdr["pack_uom"].ToString(),
                       PackDate = rdr["prod_date"] == DBNull.Value ? null : (DateTime?)rdr["prod_date"],
                       ReservationNo = rdr["doc_num"].ToString(),
                       CreateBy = rdr["createdby"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }


        public IEnumerable<ggcTag4x4> GetTagGGCByIndexKey(long apiid, string sKey)
        {
            List<ggcTag4x4> lstobj = new List<ggcTag4x4>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("select t1.item_bc, t1.lot, t1.item, t2.item_name, t1.qty, t2.pack_uom, t1.prod_date, t1.doc_num, t1.createdby, RIGHT(t1.site, LEN(t1.site)-4) as palletnote ");
                sql.AppendLine("from dbo.wms_trans t1");
                sql.AppendLine("left join dbo.set_itemmaster t2");
                sql.AppendLine("on t1.item = t2.item_code");
                sql.AppendLine("where t1.ref_item =  @ref_item");
                sql.AppendLine("and t1.unit_key =  @unit_key");
                sql.AppendLine("and t1.whse =  @whse");
                sql.AppendLine("order by t1.trans_num");
                sql.AppendLine(";");

                SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();

                cmd.Parameters.AddWithValue("@ref_item", apiid.ToString());
                cmd.Parameters.AddWithValue("@unit_key", "01");
                cmd.Parameters.AddWithValue("@whse", sKey.ToString());

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ggcTag4x4 objrd = new ggcTag4x4
                    {
                        PalletNo = rdr["palletnote"].ToString(),
                        TagNo = rdr["item_bc"].ToString(),
                        BatchNo = rdr["lot"].ToString(),
                        ProductNo = rdr["item"].ToString(),
                        ProductName = rdr["item_name"].ToString(),
                        QtyPerPallet = rdr["qty"].ToString(),
                        Packaging = rdr["pack_uom"].ToString(),
                        PackDate = rdr["prod_date"] == DBNull.Value ? null : (DateTime?)rdr["prod_date"],
                        ReservationNo = rdr["doc_num"].ToString(),
                        CreateBy = rdr["createdby"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }


        public bool CreatePutawayWms(string lpncode, string tagcode , string gatecode, string worktype, ref string retstring)
        {
            bool bRet = false;
            string sRet = "Error";
            Int32? iRet = 0;

            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sqlQurey = new StringBuilder();
                    sqlQurey.Append("dbo.ssp_create_putaway_wms");
                    SqlCommand cmd = new SqlCommand(sqlQurey.ToString(), con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    con.Open();

                    SqlParameter papiref = new SqlParameter("@_lpncode", SqlDbType.VarChar, 10);
                    papiref.Direction = ParameterDirection.Input;
                    papiref.Value = lpncode;

                    SqlParameter ppalletfrom = new SqlParameter("@_tagcode", SqlDbType.VarChar,20);
                    ppalletfrom.Direction = ParameterDirection.Input;
                    ppalletfrom.Value = tagcode;

                    SqlParameter ppalletto = new SqlParameter("@_gatecode", SqlDbType.VarChar,10);
                    ppalletto.Direction = ParameterDirection.Input;
                    ppalletto.Value = gatecode;

                    SqlParameter ptranref = new SqlParameter("@_worktype", SqlDbType.VarChar, 10);
                    ptranref.Direction = ParameterDirection.Input;
                    ptranref.Value = worktype;


                    cmd.Parameters.Add(papiref);
                    cmd.Parameters.Add(ppalletfrom);
                    cmd.Parameters.Add(ppalletto);
                    cmd.Parameters.Add(ptranref);

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
                    //Log.Error(ex.ToString());
                    sRet = ex.Message.ToString();
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
            retstring = sRet;

            return bRet;
        }


        public bool CreatePutawayEmpty(string lpncode, string tagcode, string gatecode, string worktype, ref string retstring)
        {
            bool bRet = false;
            string sRet = "Error";
            Int32? iRet = 0;

            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sqlQurey = new StringBuilder();
                    sqlQurey.Append("dbo.ssp_create_putaway_emptypallet");
                    SqlCommand cmd = new SqlCommand(sqlQurey.ToString(), con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    con.Open();

                    SqlParameter papiref = new SqlParameter("@_lpncode", SqlDbType.VarChar, 10);
                    papiref.Direction = ParameterDirection.Input;
                    papiref.Value = lpncode;

                    SqlParameter ppalletfrom = new SqlParameter("@_tagcode", SqlDbType.VarChar, 20);
                    ppalletfrom.Direction = ParameterDirection.Input;
                    ppalletfrom.Value = tagcode;

                    SqlParameter ppalletto = new SqlParameter("@_gatecode", SqlDbType.VarChar, 10);
                    ppalletto.Direction = ParameterDirection.Input;
                    ppalletto.Value = gatecode;

                    SqlParameter ptranref = new SqlParameter("@_worktype", SqlDbType.VarChar, 10);
                    ptranref.Direction = ParameterDirection.Input;
                    ptranref.Value = worktype;


                    cmd.Parameters.Add(papiref);
                    cmd.Parameters.Add(ppalletfrom);
                    cmd.Parameters.Add(ppalletto);
                    cmd.Parameters.Add(ptranref);

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
                    //Log.Error(ex.ToString());
                    sRet = ex.Message.ToString();
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
            retstring = sRet;

            return bRet;
        }

    }
}

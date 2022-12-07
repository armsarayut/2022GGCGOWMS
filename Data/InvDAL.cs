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
using GoWMS.Server.Models.Inv;


namespace GoWMS.Server.Data
{
    public class InvDAL
    {
        readonly private string connectionString = ConnGlobals.GetConnLocalDBPG();
        readonly private string connectionStringSQL = ConnGlobals.GetConnDBSQL();
        public  IEnumerable<InvStockList> GetStockList()
        {
    
            List<InvStockList> lstobj = new List<InvStockList>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                StringBuilder Sql = new StringBuilder();
                //Sql.AppendLine("select row_number() over(order by  itemcode asc) AS rn,");
                //Sql.AppendLine("itemcode, itemname, quantity, pallettag, pallteno, storagearea, storagebin");
                //Sql.AppendLine("from wms.inv_stock_go ");
                //Sql.AppendLine("order by itemcode");


                Sql.AppendLine("SELECT t1.prodcode, t1.item, t1.itemdesc, t1.supcode, t1.uom, t1.qty, t1.item_bc");
                Sql.AppendLine(", t1.whse, t1.loc, t1.pallet_bc, t1.is_req, t1.is_hold, t1.is_lock, t1.update2sl");
                Sql.AppendLine(", t1.doc_num, t1.createdby, t1.trans_date, t1.modifie_date, t1.rptStockDate");
                Sql.AppendLine(", cast(t1.trans_num as bigint) as trans_num, t1.lot, t1.exp_date, t1.mfg_date, cast(t1.is_hold as int) as efstatus");
                Sql.AppendLine(",t2.palletnote as palltmapkey");
                Sql.AppendLine("FROM dbo.v_wmstran_stock_rpt_lot t1");
                Sql.AppendLine("LEFT JOIN dbo.wms_lablemaster t2");
                Sql.AppendLine("On t1.item_bc=t2.item_bc");
                Sql.AppendLine("order by item ASC, mfg_date ASC, item_bc ASC");


                SqlCommand cmd = new SqlCommand(Sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    InvStockList objrd = new InvStockList
                    {
                        Rn= rdr["trans_num"] == DBNull.Value ? null : (Int64?)rdr["trans_num"],
                        Item_code = rdr["item"].ToString(),
                        Item_name = rdr["itemdesc"].ToString(),
                        Qty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                        Su_no = rdr["item_bc"].ToString(),
                        Palletcode = rdr["pallet_bc"].ToString(),
                        Shelfname = rdr["loc"].ToString(),
                        StorageArae = rdr["whse"].ToString(),
                        Efstatus = rdr["efstatus"] == DBNull.Value ? null : (Int32?)rdr["efstatus"],
                        BatchNo = rdr["lot"].ToString(),
                        PalletErp = rdr["palltmapkey"].ToString(),
                        Item_unit = rdr["uom"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public async Task<IEnumerable<Inv_Stock_GoInfo>> GetStockListInfo()
        {

            List<Inv_Stock_GoInfo> lstobj = new List<Inv_Stock_GoInfo>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                StringBuilder Sql = new StringBuilder();

                //Sql.AppendLine("SELECT efidx , efstatus, created, modified, innovator, device");
                //Sql.AppendLine(", pono, pallettag, itemtag, itemcode, itemname, itembar, unit");
                //Sql.AppendLine(", weightunit, quantity, weight, lotno, totalquantity, totalweight");
                //Sql.AppendLine(", docno, docby, docdate, docnote, grnrefer, grntime, grtime");
                //Sql.AppendLine(", grtype, pallteno, palltmapkey, storagetime, storageno");
                //Sql.AppendLine(", storagearea, storagebin, gnrefer, allocatequantity, allocateweight");
                //Sql.AppendLine("FROM wms.inv_stock_go");
                //Sql.AppendLine("WHERE allocatequantity < quantity");
                //Sql.AppendLine("order by itemcode ASC, docdate ASC, pallettag ASC");


                Sql.AppendLine("SELECT t1.prodcode, t1.item, t1.itemdesc, t1.supcode, t1.uom, t1.qty, t1.item_bc");
                Sql.AppendLine(", t1.whse, t1.loc, t1.pallet_bc, t1.is_req, t1.is_hold, t1.is_lock, t1.update2sl");
                Sql.AppendLine(", t1.doc_num, t1.createdby, t1.trans_date, t1.modifie_date, t1.rptStockDate");
                Sql.AppendLine(", cast(t1.trans_num as bigint) as trans_num, t1.lot, t1.exp_date, t1.mfg_date ,cast(t1.is_hold as int) as efstatus");
                Sql.AppendLine(",t2.palletnote as palltmapkey");
                Sql.AppendLine("FROM dbo.v_wmstran_stock_rpt_lot t1");
                Sql.AppendLine("LEFT JOIN dbo.wms_lablemaster t2");
                Sql.AppendLine("On t1.item_bc=t2.item_bc");
                //Sql.AppendLine("WHERE allocatequantity < quantity");
                Sql.AppendLine("Order by item ASC, mfg_date ASC, item_bc ASC");

                SqlCommand cmd = new SqlCommand(Sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (await rdr.ReadAsync())
                {
                    Inv_Stock_GoInfo objrd = new Inv_Stock_GoInfo
                    {
                        Efidx = rdr["trans_num"] == DBNull.Value ? null : (long?)rdr["trans_num"],
                        Efstatus = rdr["efstatus"] == DBNull.Value ? null : (Int32?)rdr["efstatus"],
                        Created = rdr["trans_date"] == DBNull.Value ? null : (DateTime?)rdr["trans_date"],
                        Modified = rdr["modifie_date"] == DBNull.Value ? null : (DateTime?)rdr["modifie_date"],
                        Innovator = null,
                        Device = null,
                        Pono = rdr["doc_num"].ToString(),
                        Pallettag = rdr["item_bc"].ToString(),
                        Itemtag = rdr["item_bc"].ToString(),
                        Itemcode = rdr["item"].ToString(),
                        Itemname = rdr["itemdesc"].ToString(),
                        Itembar = null,
                        Unit = rdr["uom"].ToString(),
                        Weightunit = null,
                        Quantity = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                        Weight = null,
                        Lotno = rdr["lot"].ToString(),
                        Totalquantity = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                        Totalweight = null,
                        Docno = rdr["doc_num"].ToString(),
                        Docby = rdr["createdby"].ToString(),
                        Docdate = rdr["trans_date"] == DBNull.Value ? null : (DateTime?)rdr["trans_date"],
                        Docnote = null,
                        Grnrefer = null,
                        Grntime = null,
                        Grtime = null,
                        Grtype = null,
                        Pallteno = rdr["pallet_bc"].ToString(),
                        Palltmapkey = rdr["palltmapkey"].ToString(),
                        Storagetime = rdr["rptStockDate"] == DBNull.Value ? null : (DateTime?)rdr["rptStockDate"],
                        Storageno = null,
                        Storagearea = rdr["whse"].ToString(),
                        Storagebin = rdr["loc"].ToString(),
                        Gnrefer = null,
                        Allocatequantity = null,
                        Allocateweight = null
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public IEnumerable<InvStockSum> GetStockSum()
        {
            List<InvStockSum> lstobj = new List<InvStockSum>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                StringBuilder Sql = new StringBuilder();

                //Sql.AppendLine("select row_number() over(order by itemcode asc) AS rn,");
                //Sql.AppendLine("itemcode, itemname, sum(quantity) as totalstock, count(pallteno) as countpallet");
                //Sql.AppendLine("from wms.inv_stock_go ");
                //Sql.AppendLine("WHERE allocatequantity < quantity");
                //Sql.AppendLine("group by itemcode, itemname");
                //Sql.AppendLine("order by itemcode");

                Sql.AppendLine("select row_number() over(order by item asc) AS rn,");
                Sql.AppendLine("item, itemdesc, uom, sum(qty) as totalstock, count(pallet_bc) as countpallet");
                Sql.AppendLine("from dbo.v_wmstran_stock_rpt_lot ");
                //Sql.AppendLine("WHERE allocatequantity < quantity");
                Sql.AppendLine("group by item, uom, itemdesc");
                Sql.AppendLine("order by item");


                SqlCommand cmd = new SqlCommand(Sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    InvStockSum objrd = new InvStockSum
                    {
                        Rn = rdr["rn"] == DBNull.Value ? null : (Int64?)rdr["rn"],
                        Item_code = rdr["item"].ToString(),
                        Item_name = rdr["itemdesc"].ToString(),
                        Totalstock = rdr["totalstock"] == DBNull.Value ? null : (Decimal?)rdr["totalstock"],
                        Countpallet = rdr["countpallet"] == DBNull.Value ? null : (Int32?)rdr["countpallet"],
                        Unit = rdr["uom"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public IEnumerable<InvStockSumByLot> GetStockSumByLot()
        {
            List<InvStockSumByLot> lstobj = new List<InvStockSumByLot>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                StringBuilder Sql = new StringBuilder();

                Sql.AppendLine("select row_number() over(order by item asc) AS rn,");
                Sql.AppendLine("item, itemdesc, uom, lot, sum(qty) as totalstock, count(pallet_bc) as countpallet");
                Sql.AppendLine("from dbo.v_wmstran_stock_rpt_lot ");
                Sql.AppendLine("group by item, uom, itemdesc, lot");
                Sql.AppendLine("order by item");


                SqlCommand cmd = new SqlCommand(Sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    InvStockSumByLot objrd = new InvStockSumByLot
                    {
                        Rn = rdr["rn"] == DBNull.Value ? null : (Int64?)rdr["rn"],
                        Item_code = rdr["item"].ToString(),
                        Item_name = rdr["itemdesc"].ToString(),
                        Lot = rdr["lot"].ToString(),
                        Totalstock = rdr["totalstock"] == DBNull.Value ? null : (Decimal?)rdr["totalstock"],
                        Countpallet = rdr["countpallet"] == DBNull.Value ? null : (Int32?)rdr["countpallet"],
                        Unit = rdr["uom"].ToString(),

                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }


        public IEnumerable<Vrpt_shelf_listInfo> GetShelfLocation()
        {
            List<Vrpt_shelf_listInfo> lstobj = new List<Vrpt_shelf_listInfo>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                 StringBuilder Sql = new StringBuilder();
                Sql.AppendLine("SELECT modified, srm_no, shelf_no, shelfcode, shelfname");
                Sql.AppendLine(", shelfbank, shelfframe, shelfbay, shelflevel, shelfdeep, shelfstatus");
                Sql.AppendLine(", lpncode, refercode, actual_weight, actual_size, null as desc_size, st_desc, backcolor, focecolor");
                Sql.AppendLine("from  wcs.vrpt_shelf_list");
                Sql.AppendLine("order by shelf_no asc");

                SqlCommand cmd = new SqlCommand(Sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Vrpt_shelf_listInfo objrd = new Vrpt_shelf_listInfo
                    {
                       
                        Srm_no = rdr["srm_no"] == DBNull.Value ? null : (Int32?)rdr["srm_no"],
                        Shelf_no = rdr["shelf_no"] == DBNull.Value ? null : (Int64?)rdr["shelf_no"],
                        Shelfcode = rdr["shelfcode"].ToString(),
                        Shelfname  = rdr["shelfname"].ToString(),
                        Shelfbank = rdr["shelfbank"] == DBNull.Value ? null : (Int32?)rdr["shelfbank"],
                        Shelfbay = rdr["shelfbay"] == DBNull.Value ? null : (Int32?)rdr["shelfbay"],
                        Shelfframe = rdr["shelfframe"] == DBNull.Value ? null : (Int32?)rdr["shelfframe"],
                        Shelflevel = rdr["shelflevel"] == DBNull.Value ? null : (Int32?)rdr["shelflevel"],
                        Shelfdeep = rdr["shelfdeep"] == DBNull.Value ? null : (Int32?)rdr["shelfdeep"],
                        Shelfstatus = rdr["shelfstatus"] == DBNull.Value ? null : (Int32?)rdr["shelfstatus"],
                        Lpncode= rdr["lpncode"].ToString(),
                        Refercode = rdr["refercode"].ToString(),
                        Actual_weight = rdr["actual_weight"] == DBNull.Value ? null : (decimal?)rdr["actual_weight"],
                        Actual_size = rdr["actual_size"] == DBNull.Value ? null : (Int32?)rdr["actual_size"],
                        Desc_size = rdr["desc_size"].ToString(),
                        St_desc = rdr["st_desc"].ToString(),
                        Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                        Backcolor = rdr["backcolor"].ToString(),
                        Focecolor = rdr["focecolor"].ToString()

                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public IEnumerable<InvStockSumByCus> GetStockSumByCustomer()
        {
            List<InvStockSumByCus> lstobj = new List<InvStockSumByCus>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder Sql = new StringBuilder();

                Sql.AppendLine("SELECT t1.itemcode, t1.itemname, t1.palltmapkey, t2.cus_name, t1.pallteno, t1.storagebin, sum(t1.quantity) as totalstock, t1.itembar");
                Sql.AppendLine(",t3.srm_no, t3.shelfbank, t3.shelfbay, t3.shelflevel");
                Sql.AppendLine("FROM wms.inv_stock_go t1");
                Sql.AppendLine("LEFT JOIN public.sap_customermaster_v t2");
                Sql.AppendLine("ON t1.palltmapkey= t2.cus_code");
                Sql.AppendLine("LEFT JOIN wcs.set_shelf t3");
                Sql.AppendLine("ON t1.storagebin = t3.shelfcode");
                Sql.AppendLine("WHERE t1.allocatequantity < t1.quantity");
                Sql.AppendLine("GROUP BY t1.itemcode, t1.itemname, t1.palltmapkey,t1. pallteno, t1.storagebin,  t2.cus_name, t3.srm_no, t3.shelfbank, t3.shelfbay, t3.shelflevel, t1.itembar");
                
                //Sql.AppendLine("ORDER BY t1.itemcode");


                NpgsqlCommand cmd = new NpgsqlCommand(Sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    InvStockSumByCus objrd = new InvStockSumByCus
                    {
                        Itemcode = rdr["itemcode"].ToString(),
                        Itemname = rdr["itemname"].ToString(),
                        Cuscode = rdr["palltmapkey"].ToString(),
                        Cusname = rdr["cus_name"].ToString(),
                        Pallteno = rdr["itembar"].ToString(),
                        Storagebin = rdr["storagebin"].ToString(),
                        Totalstock = rdr["totalstock"] == DBNull.Value ? null : (Decimal?)rdr["totalstock"],
                        StorageLane = rdr["srm_no"] == DBNull.Value ? null : (Int32?)rdr["srm_no"],
                        StorageBank = rdr["shelfbank"] == DBNull.Value ? null : (Int16?)rdr["shelfbank"],
                        StorageBay = rdr["shelfbay"] == DBNull.Value ? null : (Int32?)rdr["shelfbay"],
                        StorageLevel = rdr["shelflevel"] == DBNull.Value ? null : (Int16?)rdr["shelflevel"],
                        Palltego = rdr["pallteno"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public async Task UpdateHoldStock(List<InvStockList> liststock)
        {
            using SqlConnection con = new SqlConnection(connectionStringSQL);
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("Update dbo.wms_trans");
                sql.AppendLine("Set is_hold = @efstatus");
                sql.AppendLine("Where trans_num in (");

                using var cmd = new SqlCommand(connection: con, cmdText: null);

                int iState = 1;
                string sParamState = "@efstatus";

                //cmd.Parameters.Add(new SqlParameter<int>(sParamState, iState));
                cmd.Parameters.AddWithValue(sParamState, iState);

                var i = 0;
                foreach (var s in liststock)
                {
                    if (i != 0) sql.Append(",");
                    var pallettag = "trans_num" + i.ToString();

                    sql.Append("@").Append(pallettag);

                    cmd.Parameters.AddWithValue(pallettag, s.Rn);

                    //cmd.Parameters.Add(new NpgsqlParameter<string>(pallettag, s.Su_no));

                    i++;
                }
                sql.AppendLine(")");

                con.Open();
                cmd.CommandText = sql.ToString();
                await cmd.ExecuteNonQueryAsync();

            }
            catch (SqlException ex)
            {
                //Log.Error(ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        public async Task UpdateReleaseStock(List<InvStockList> liststock)
        {
            using SqlConnection con = new SqlConnection(connectionStringSQL);
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("Update dbo.wms_trans");
                sql.AppendLine("Set is_hold = @efstatus");
                sql.AppendLine("Where trans_num in (");

                using var cmd = new SqlCommand(connection: con, cmdText: null);

                int iState = 0;
                string sParamState = "@efstatus";

                cmd.Parameters.AddWithValue(sParamState, iState);

                var i = 0;
                foreach (var s in liststock)
                {
                    if (i != 0) sql.Append(",");
                    var pallettag = "trans_num" + i.ToString();

                    sql.Append("@").Append(pallettag);

                    cmd.Parameters.AddWithValue(pallettag, s.Rn);

                    i++;
                }
                sql.AppendLine(")");

                con.Open();
                cmd.CommandText = sql.ToString();
                await cmd.ExecuteNonQueryAsync();

            }
            catch (SqlException ex)
            {
                //Log.Error(ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }

    }
}

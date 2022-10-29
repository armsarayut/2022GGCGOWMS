using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;
using GoWMS.Server.Controllers;
using GoWMS.Server.Models.Oub;
using NpgsqlTypes;
using System.Text;

namespace GoWMS.Server.Data
{
    public class OubDAL
    {
        readonly private string connectionString = ConnGlobals.GetConnLocalDBPG();
        readonly private string connectionStringSQL = ConnGlobals.GetConnDBSQL();

        public IEnumerable<Sap_Storeout> GetAllSapStoreout()
        {
            List<Sap_Storeout> lstobj = new List<Sap_Storeout>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {

                StringBuilder sql = new StringBuilder();

                sql.AppendLine("select t0.site as site, t0.doc_num as doc_num, cast(t0.trans_num as bigint) as trans_num, t0.ref_type as ref_type, t0.trans_type as trans_type, t0.trans_date as trans_date");
                sql.AppendLine(", t0.unit_key as unit_key, t0.item_bc as item_bc, t0.item as item, t1.item_name as item_name, t1.uom as uom, t1.weight_unit as weight_unit, t0.qty as qty");
                sql.AppendLine(", t0.lot as lot, t0.prod_date as prod_date, t0.stat as stat,t0.source as [source]");
                sql.AppendLine(", t0.reason as reason, t0.createdby as createdby, t0.createddate as createddate");
                sql.AppendLine(", t0.whse as whse, t0.loc as loc, t0.pallet_bc as pallet_bc, t0.ref_item as ref_item");
                sql.AppendLine(", t0.flag,t0.require_detail_id,t0.is_req");
                sql.AppendLine(", t0.is_hold as is_hold, t0.is_lock as is_lock, t0.update2sl as update2sl, t0.modifie_date as modifie_date");
                sql.AppendLine("FROM dbo.wms_trans t0");
                sql.AppendLine("LEFT JOIN dbo.set_itemmaster t1");
                sql.AppendLine("ON t0.item=t1.item_code");
                sql.AppendLine("WHERE t0.unit_key = @unit_key");
                sql.AppendLine("AND t0.stat = @stat");
                sql.AppendLine("AND t0.flag = @flag");
                sql.AppendLine("ORDER BY t0.trans_num");

                SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.AddWithValue("@unit_key", "05");
                cmd.Parameters.AddWithValue("@stat", 0);
                cmd.Parameters.AddWithValue("@flag", 0);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    decimal? dQty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"];
                    DateTime? tCreated = rdr["createddate"] == DBNull.Value ? null : (DateTime?)rdr["createddate"];
                    string sitembc = rdr["item_bc"].ToString();
                    string slot = rdr["lot"].ToString();

                    Sap_Storeout objrd = new Sap_Storeout
                    {
                        Idx = rdr["trans_num"] == DBNull.Value ? null : (Int64?)rdr["trans_num"],
                        Entity_Lock = 0,
                        Created = tCreated,
                        Modified = rdr["modifie_date"] == DBNull.Value ? null : (DateTime?)rdr["modifie_date"],
                        Client_Id = 0,
                        Client_Ip = null,
                        Order_No = rdr["doc_num"].ToString(),
                        Ship_To_Code = null,
                        Ship_Name = null,
                        Delivery_Priority = 0,
                        Delivery_Date = rdr["trans_date"] == DBNull.Value ? null : (DateTime?)rdr["trans_date"],
                        Item_Code = rdr["item"].ToString(),
                        Batch_Number = slot,
                        Request_Qty = dQty,
                        Status = rdr["stat"] == DBNull.Value ? null : (Int32?)rdr["stat"],
                        Error_Code = null,
                        Movement_Type = rdr["ref_type"].ToString(),
                        Movement_Reason = rdr["trans_type"].ToString(),
                        To_No = null,
                        To_Line = null,
                        Po_Header_Txt = null,
                        Requisitioner = null,
                        Po_No = rdr["doc_num"].ToString(),
                        Remark = rdr["reason"].ToString(),
                        Doc_Ref = null,
                        Created_By = rdr["createdby"].ToString(),
                        Created_Date = tCreated,
                        Update_By = null,
                        Update_Date = null,
                        Order_Line = null,
                        Stock_Consign = null,
                        Site = rdr["site"].ToString(),
                        Storage_Location = rdr["loc"].ToString(),
                        Warehouse = rdr["whse"].ToString(),
                        Item_Name = rdr["item_name"].ToString(),
                        Tracking_Number = sitembc,
                        Su_No = sitembc,
                        Pallet_No = rdr["pallet_bc"].ToString(),
                        Stock_Qty = dQty,
                        Transfer_Qty = dQty,
                        Ref_No = null,
                        Ref_Line = null,
                        Unit = rdr["uom"].ToString(),
                        Vendor_Code = null,
                        Batch_No = slot
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public IEnumerable<Sap_Storeout> GetSapStoreoutSetBatch()
        {
            List<Sap_Storeout> lstobj = new List<Sap_Storeout>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendLine("select t0.site as site, t0.doc_num as doc_num, cast(t0.trans_num as bigint) as trans_num, t0.ref_type as ref_type, t0.trans_type as trans_type, t0.trans_date as trans_date");
                sql.AppendLine(", t0.unit_key as unit_key, t0.item_bc as item_bc, t0.item as item, t1.item_name as item_name, t1.uom as uom, t1.weight_unit as weight_unit,t0.qty as qty");
                sql.AppendLine(", t0.lot as lot, t0.prod_date as prod_date, t0.stat as stat, t0.source as [source]");
                sql.AppendLine(", t0.reason as reason, t0.createdby as createdby, t0.createddate as createddate");
                sql.AppendLine(", t0.whse as whse, t0.loc as loc, t0.pallet_bc as pallet_bc, t0.ref_item as ref_item");
                sql.AppendLine(", t0.flag as flag, t0.require_detail_id as require_detail_id, t0.is_req as is_req");
                sql.AppendLine(", t0.is_hold as is_hold, t0.is_lock as is_lock, t0.update2sl as update2sl, t0.modifie_date as modifie_date");
                sql.AppendLine("FROM dbo.wms_trans t0");
                sql.AppendLine("LEFT JOIN dbo.set_itemmaster t1");
                sql.AppendLine("ON t0.item=t1.item_code");
                sql.AppendLine("WHERE t0.unit_key = @unit_key");
                sql.AppendLine("AND t0.stat = @stat");
                sql.AppendLine("AND t0.flag = @flag");
                sql.AppendLine("AND t0.is_req = @is_req");
                sql.AppendLine("ORDER BY t0.trans_num");

                SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };

                cmd.Parameters.AddWithValue("@unit_key", "05");
                cmd.Parameters.AddWithValue("@stat", 0);
                cmd.Parameters.AddWithValue("@flag", 0);
                cmd.Parameters.AddWithValue("@is_req", 0);

                //NpgsqlCommand cmd = new NpgsqlCommand("select * " +
                //    "from public.sap_storeout  " +
                //    "where (1=1)  " +
                //    "and status =0 " +
                //    "order by idx", con)
                //{
                //    CommandType = CommandType.Text
                //};

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    decimal? dQty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"];
                    DateTime? tCreated = rdr["createddate"] == DBNull.Value ? null : (DateTime?)rdr["createddate"];
                    string sitembc = rdr["item_bc"].ToString();
                    string slot = rdr["lot"].ToString();

                    Sap_Storeout objrd = new Sap_Storeout
                    {

                        

                        Idx = rdr["trans_num"] == DBNull.Value ? null : (Int64?)rdr["trans_num"],
                        Entity_Lock = 0,
                        Created = tCreated,
                        Modified = rdr["modifie_date"] == DBNull.Value ? null : (DateTime?)rdr["modifie_date"],
                        Client_Id = 0,
                        Client_Ip = null,
                        Order_No = rdr["doc_num"].ToString(),
                        Ship_To_Code = null,
                        Ship_Name = null,
                        Delivery_Priority = 0,
                        Delivery_Date = rdr["trans_date"] == DBNull.Value ? null : (DateTime?)rdr["trans_date"],
                        Item_Code = rdr["item"].ToString(),
                        Batch_Number = slot,
                        Request_Qty = dQty,
                        Status = rdr["stat"] == DBNull.Value ? null : (Int32?)rdr["stat"],
                        Error_Code = null,
                        Movement_Type = rdr["ref_type"].ToString(),
                        Movement_Reason = rdr["trans_type"].ToString(),
                        To_No = null,
                        To_Line = null,
                        Po_Header_Txt = null,
                        Requisitioner = null,
                        Po_No = rdr["doc_num"].ToString(),
                        Remark = rdr["reason"].ToString(),
                        Doc_Ref = null,
                        Created_By = rdr["createdby"].ToString(),
                        Created_Date = tCreated,
                        Update_By = null,
                        Update_Date = null,
                        Order_Line = null,
                        Stock_Consign = null,
                        Site = rdr["site"].ToString(),
                        Storage_Location = rdr["loc"].ToString(),
                        Warehouse = rdr["whse"].ToString(),
                        Item_Name = rdr["item_name"].ToString(),
                        Tracking_Number = sitembc,
                        Su_No = sitembc,
                        Pallet_No = rdr["pallet_bc"].ToString(),
                        Stock_Qty = dQty,
                        Transfer_Qty = dQty,
                        Ref_No = null,
                        Ref_Line = null,
                        Unit = rdr["uom"].ToString(),
                        Vendor_Code = null,
                        Batch_No = slot

                        //Idx = rdr["idx"] == DBNull.Value ? null : (Int64?)rdr["idx"],
                        //Entity_Lock = rdr["entity_lock"] == DBNull.Value ? null : (Int32?)rdr["entity_lock"],
                        //Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        //Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                        //Client_Id = rdr["client_id"] == DBNull.Value ? null : (Int64?)rdr["client_Id"],
                        //Client_Ip = rdr["client_ip"].ToString(),
                        //Order_No = rdr["order_no"].ToString(),
                        //Ship_To_Code = rdr["ship_to_code"].ToString(),
                        //Ship_Name = rdr["ship_name"].ToString(),
                        //Delivery_Priority = rdr["delivery_priority"] == DBNull.Value ? null : (Int32?)rdr["delivery_priority"],
                        //Delivery_Date = rdr["delivery_date"] == DBNull.Value ? null : (DateTime?)rdr["delivery_date"],
                        //Item_Code = rdr["item_code"].ToString(),
                        //Batch_Number = rdr["batch_number"].ToString(),
                        //Request_Qty = rdr["request_qty"] == DBNull.Value ? null : (decimal?)rdr["request_qty"],
                        //Status = rdr["status"] == DBNull.Value ? null : (Int32?)rdr["status"],
                        //Error_Code = rdr["error_code"].ToString(),
                        //Movement_Type = rdr["movement_type"].ToString(),
                        //Movement_Reason = rdr["movement_reason"].ToString(),
                        //To_No = rdr["to_no"].ToString(),
                        //To_Line = rdr["to_line"].ToString(),
                        //Po_Header_Txt = rdr["po_header_txt"].ToString(),
                        //Requisitioner = rdr["requisitioner"].ToString(),
                        //Po_No = rdr["po_no"].ToString(),
                        //Remark = rdr["remark"].ToString(),
                        //Doc_Ref = rdr["doc_ref"].ToString(),
                        //Created_By = rdr["created_by"].ToString(),
                        //Created_Date = rdr["created_date"] == DBNull.Value ? null : (DateTime?)rdr["created_date"],
                        //Update_By = rdr["update_by"].ToString(),
                        //Update_Date = rdr["update_date"] == DBNull.Value ? null : (DateTime?)rdr["update_date"],
                        //Order_Line = rdr["order_line"].ToString(),
                        //Stock_Consign = rdr["stock_consign"].ToString(),
                        //Site = rdr["site"].ToString(),
                        //Storage_Location = rdr["storage_location"].ToString(),
                        //Warehouse = rdr["warehouse"].ToString(),
                        //Item_Name = rdr["item_Name"].ToString(),
                        //Tracking_Number = rdr["tracking_number"].ToString(),
                        //Su_No = rdr["su_no"].ToString(),
                        //Pallet_No = rdr["pallet_no"].ToString(),
                        //Stock_Qty = rdr["stock_qty"] == DBNull.Value ? null : (decimal?)rdr["stock_qty"],
                        //Transfer_Qty = rdr["transfer_qty"] == DBNull.Value ? null : (decimal?)rdr["transfer_qty"],
                        //Ref_No = rdr["ref_no"].ToString(),
                        //Ref_Line = rdr["ref_line"].ToString(),
                        //Unit = rdr["unit"].ToString(),
                        //Vendor_Code = rdr["vendor_code"].ToString(),
                        //Batch_No = rdr["batch_no"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public string GetRunnning(string sSeq, Int32 iPad)
        {
            string sRunning = "";
            Int32? iRet =0 ;
            
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sqlQurey = new StringBuilder();
                sqlQurey.AppendLine("select _retchk, _retrunning from public.fuc_create_running(:sSeq , :iPad);");
                NpgsqlCommand cmd = new NpgsqlCommand(sqlQurey.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue(":sSeq", sSeq);
                cmd.Parameters.AddWithValue(":iPad", iPad);

                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    iRet = rdr["_retchk"] == DBNull.Value ? null : (Int32?)rdr["_retchk"];
                    sRunning = rdr["_retrunning"].ToString();
                }
                con.Close();
            }
            return sRunning;
        }

        public Boolean CreateBatchOrder(DateTime deliverydate,Int32 deliveryprio,string orderno, string shiptocode, string sSeq)
        {
            Boolean bRet = false;
            string sRet = "";
            Int32? iRet = 0;
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sqlQurey = new StringBuilder();
                sqlQurey.AppendLine("select _retchk, _retmsg from public.fuc_create_batchorder(:deliverydate , :deliveryprio, :orderno, :shiptocode, :sSeq);");
                NpgsqlCommand cmd = new NpgsqlCommand(sqlQurey.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue(":deliverydate", NpgsqlDbType.Timestamp, deliverydate);
                cmd.Parameters.AddWithValue(":deliveryprio", NpgsqlDbType.Integer,  deliveryprio);
                cmd.Parameters.AddWithValue(":orderno", NpgsqlDbType.Varchar,orderno);
                cmd.Parameters.AddWithValue(":shiptocode", NpgsqlDbType.Varchar, shiptocode);
                cmd.Parameters.AddWithValue(":sSeq", NpgsqlDbType.Varchar, sSeq);

                con.Open();
                try
                {
                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        iRet = rdr["_retchk"] == DBNull.Value ? null : (Int32?)rdr["_retchk"];
                        sRet = rdr["_retmsg"].ToString();
                    }
                    con.Close();
                    if (iRet == 1)
                    {
                        bRet = true;
                    }
                }
                catch (Exception e)
                {
                    string str = e.Message.ToString(); 
                }
                   
                    
               
            }
            return bRet;
        }

        public Boolean CreateBatchSetting (string sSeq, Int32 istation)
        {
            Boolean bRet = false;
            string sRet = "";
            Int32? iRet = 0;
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sqlQurey = new StringBuilder();
                sqlQurey.AppendLine("select _retchk, _retmsg from public.fuc_create_batchsetting(:sSeq , :istation);");
                NpgsqlCommand cmd = new NpgsqlCommand(sqlQurey.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue(":sSeq", NpgsqlDbType.Varchar, sSeq);
                cmd.Parameters.AddWithValue(":istation", NpgsqlDbType.Integer, istation);

                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    iRet = rdr["_retchk"] == DBNull.Value ? null : (Int32?)rdr["_retchk"];
                    sRet = rdr["_retmsg"].ToString();
                }
                con.Close();
                if (iRet == 1)
                {
                    bRet = true;
                }
            }
            return bRet;
        }

        public Boolean StartBatchsetting(string sSeq, Int32 istation)
        {
            Boolean bRet = false;
            string sRet = "";
            Int32? iRet = 0;
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sqlQurey = new StringBuilder();
                sqlQurey.AppendLine("select _retchk, _retmsg from public.fuc_start_batchsetting(:sSeq , :istation);");
                NpgsqlCommand cmd = new NpgsqlCommand(sqlQurey.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue(":sSeq", NpgsqlDbType.Varchar, sSeq);
                cmd.Parameters.AddWithValue(":istation", NpgsqlDbType.Integer, istation);

                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    iRet = rdr["_retchk"] == DBNull.Value ? null : (Int32?)rdr["_retchk"];
                    sRet = rdr["_retmsg"].ToString();
                }
                con.Close();
                if (iRet == 1)
                {
                    bRet = true;
                }
            }
            return bRet;
        }

        public Boolean RollbackBatch(string sSeq)
        {
            Boolean bRet = false;
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sqlQurey = new StringBuilder();
                sqlQurey.AppendLine("delete from public.sap_batchsetting where batch_no = :batch_no1 ;");
                sqlQurey.AppendLine("delete from public.sap_batchorder where batch_no = :batch_no2 ;");

                NpgsqlCommand cmd = new NpgsqlCommand(sqlQurey.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue(":batch_no1", NpgsqlDbType.Varchar, sSeq);
                cmd.Parameters.AddWithValue(":batch_no2", NpgsqlDbType.Varchar, sSeq);

                con.Open();
                cmd.ExecuteNonQuery();
                bRet = true;
                con.Close();
            }
            return bRet;
        }

        public IEnumerable<Oub_CustomerReturn> GetAllCustomerReturn()
        {
            List<Oub_CustomerReturn> lstobj = new List<Oub_CustomerReturn>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendLine("SELECT  c.package_id");
                sql.AppendLine(",c.material_code");
                sql.AppendLine(", c.material_description");
                sql.AppendLine(", c.unit");
                sql.AppendLine(", c.customer_code");
                sql.AppendLine(", 1 as quantity");
                sql.AppendLine(", c.matelement as dnno");
                sql.AppendLine(", c.finished_product as lotno");
                sql.AppendLine(", c.finished_product_description  as batchno");
                sql.AppendLine(", c.created::date as created");
                sql.AppendLine(", c.job");
                sql.AppendLine(", c.pallet_no");

                sql.AppendLine("FROM wms.oub_deliveryorder_go c");
                /*
                sql.AppendLine("WHERE NOT EXISTS");
                sql.AppendLine("(SELECT 1");
                sql.AppendLine("FROM wms.inv_stock_go p");
                //sql.AppendLine("WHERE p.pallettag = c.package_id)");
                sql.AppendLine("WHERE p.pono = c.matelement AND p.pallettag = c.package_id AND p.docno =c.finished_product AND p.docnote = c.finished_product_description )");
                sql.AppendLine("AND c.efstatus < 5");
                */
                sql.AppendLine("WHERE c.efstatus < 5");
                sql.AppendLine("GROUP BY");
                sql.AppendLine("package_id, material_code, material_description, unit,  customer_code, dnno, lotno, batchno, created, job, pallet_no");
                sql.AppendLine(";");

                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Oub_CustomerReturn objrd = new Oub_CustomerReturn
                    {
                        Customer_code= rdr["customer_code"].ToString(),
                        Package_id = rdr["package_id"].ToString(),
                        Material_code = rdr["material_code"].ToString(),
                        Material_description = rdr["material_description"].ToString(),
                        Unit = rdr["unit"].ToString(),
                        Dnno= rdr["dnno"].ToString(),
                        Lotno = rdr["lotno"].ToString(),
                        Batchno = rdr["batchno"].ToString(),
                        Sono = rdr["job"].ToString(),
                        Quantity= rdr["quantity"] == DBNull.Value ? null : (int?)rdr["quantity"],
                        Palletno = rdr["pallet_no"].ToString(),
                        Sodate  = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"]

                };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }


        public Boolean UpdateCustomerReturn(string Spallet)
        {
            Boolean bRet = false;
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sqlQurey = new StringBuilder();
                sqlQurey.AppendLine("UPDATE wms.oub_deliveryorder_go");
                sqlQurey.AppendLine("SET efstatus = 5");
                sqlQurey.AppendLine("WHERE pallteno = @pallteno");
                sqlQurey.AppendLine("AND efstatus < 5");
                NpgsqlCommand cmd = new NpgsqlCommand(sqlQurey.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@pallteno", NpgsqlDbType.Varchar, Spallet);
                con.Open();
                cmd.ExecuteNonQuery();
                bRet = true;
                con.Close();
            }
            return bRet;
        }

    }
}

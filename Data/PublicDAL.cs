using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using NpgsqlTypes;
using GoWMS.Server.Controllers;
using GoWMS.Server.Models;
using GoWMS.Server.Models.Public;
using GoWMS.Server.DataAccess;
using Serilog;

namespace GoWMS.Server.Data
{
    public class PublicDAL
    {
        readonly private string connectionString = ConnGlobals.GetConnLocalDBPG();
        readonly private string connectionStringSQL = ConnGlobals.GetConnDBSQL();

        #region "MENU6.1"
        public IEnumerable<Class6_1> GetAllMenu6_1()
        {
            List<Class6_1> lstobj = new List<Class6_1>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("select r.idx, r.created, r.entity_lock, r.modified, r.client_id, r.menu_name , r.action_desc , u.usid , r.client_ip ");
                    sql.AppendLine("from dbo.rpt_audittrial r");
                    sql.AppendLine("left join dbo.set_users u");
                    sql.AppendLine("on r.id_stuser=u.idx");
                    sql.AppendLine("where 1=1");
                    sql.AppendLine("order by r.idx asc");
                    sql.AppendLine(";");

                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_1 objrd = new Class6_1
                        {
                            Idx = rdr["idx"] == DBNull.Value ? null : (Int64?)rdr["idx"],
                            Entity_Lock = rdr["entity_lock"] == DBNull.Value ? null : (int?)rdr["entity_lock"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                            Client_Id = rdr["client_id"] == DBNull.Value ? null : (long?)rdr["client_id"],
                            Client_ip = rdr["client_ip"].ToString(),
                            Menu_Name = rdr["menu_name"].ToString(),
                            Usid = rdr["usid"].ToString(),
                            Action_Desc = rdr["action_desc"].ToString()
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_1> GetMenu6_1byDate(DateTime dtStart, DateTime dtStop)
        {
            List<Class6_1> lstobj = new List<Class6_1>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("select r.idx, r.created, r.entity_lock, r.modified, r.client_id, r.menu_name , r.action_desc , u.usid , r.client_ip ");
                    sql.AppendLine("from dbo.rpt_audittrial r");
                    sql.AppendLine("left join dbo.set_users u");
                    sql.AppendLine("on r.id_stuser=u.idx");
                    sql.AppendLine("where 1=1");
                    sql.AppendLine("and (r.created >= @startdate and r.created < @stopdate)");
                    sql.AppendLine("order by r.idx asc");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");

                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    cmd.Parameters.AddWithValue("@startdate", dtStart);
                    cmd.Parameters.AddWithValue("@stopdate",  dtStop);

                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_1 objrd = new Class6_1
                        {
                            Idx = rdr["idx"] == DBNull.Value ? null : (Int64?)rdr["idx"],
                            Entity_Lock = rdr["entity_lock"] == DBNull.Value ? null : (int?)rdr["entity_lock"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                            Client_Id = rdr["client_id"] == DBNull.Value ? null : (long?)rdr["client_id"],
                            Client_ip = rdr["client_ip"].ToString(),
                            Menu_Name = rdr["menu_name"].ToString(),
                            Action_Desc = rdr["action_desc"].ToString(),
                            Usid = rdr["usid"].ToString()
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_1> GetMenu6_1byDatelimit(DateTime dtStart, DateTime dtStop, long limitrec, long currentPage)
        {
            List<Class6_1> lstobj = new List<Class6_1>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT r.idx, r.created, r.entity_lock, r.modified, r.client_id, r.menu_name , r.action_desc , u.usid , r.client_ip ");
                    sql.AppendLine("FROM dbo.rpt_audittrial r");
                    sql.AppendLine("LRFT JOIN dbo.set_users u");
                    sql.AppendLine("ON r.id_stuser=u.idx");
                    sql.AppendLine("WHERE 1=1");
                    sql.AppendLine("AND (r.created >= @startdate and r.created < @stopdate)");
                    sql.AppendLine("ORDER BY r.idx asc");
                    sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                    sql.AppendLine(";");

                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    cmd.Parameters.AddWithValue("@startdate",  dtStart);
                    cmd.Parameters.AddWithValue("@stopdate",  dtStop);
                    cmd.Parameters.AddWithValue("@limitrec",  limitrec);
                    cmd.Parameters.AddWithValue("@offsetrec",  (limitrec * currentPage) - limitrec);
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_1 objrd = new Class6_1
                        {
                            Idx = rdr["idx"] == DBNull.Value ? null : (Int64?)rdr["idx"],
                            Entity_Lock = rdr["entity_lock"] == DBNull.Value ? null : (int?)rdr["entity_lock"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                            Client_Id = rdr["client_id"] == DBNull.Value ? null : (long?)rdr["client_id"],
                            Client_ip = rdr["client_ip"].ToString(),
                            Menu_Name = rdr["menu_name"].ToString(),
                            Action_Desc = rdr["action_desc"].ToString(),
                            Usid = rdr["usid"].ToString()
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }
        #endregion

        #region "MENU6.2A"
        public IEnumerable<Class6_2_A> GetAllMenu6_2A()
        {
            List<Class6_2_A> lstobj = new List<Class6_2_A>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();

                    sql.AppendLine("SELECT");
                    sql.AppendLine("created, work_type, create_by, batch_number, item_code,");
                    sql.AppendLine("item_name, su_no, movement_type, movemet_reason, order_no, seq_no,");
                    sql.AppendLine("result_qty, doc_ref , pallet_no, crane_no, location_no,");
                    sql.AppendLine("dest_su_no, to_no, to_line, status, shortge_qty, po_no, invoice_no,");
                    sql.AppendLine("recviving_date, delivery_date, order_line,");
                    sql.AppendLine("queue_no, ship_to_code, ship_name, delivery_priority ");
                    sql.AppendLine("FROM public.sap_operateresult");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("AND (work_type in ('01'))");
                    // sql.AppendLine("and (created>='" & dtpStart.ToString("s") & "' and created<='" & dtpStop.ToString("s") & "')");
                    sql.AppendLine("ORDER BY created ASC");
                    // sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_2_A objrd = new Class6_2_A
                        {
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Work_Type = rdr["work_type"].ToString(),
                            Create_By = rdr["create_by"].ToString(),
                            Batch_Number = rdr["batch_number"].ToString(),
                            Item_Code = rdr["item_code"].ToString(),
                            Item_Name = rdr["item_name"].ToString(),
                            Su_No = rdr["su_no"].ToString(),
                            Movement_Type = rdr["movement_type"].ToString(),
                            Movemet_Reason = rdr["movemet_reason"].ToString(),
                            Order_No = rdr["order_no"].ToString(),
                            Seq_No = rdr["seq_no"].ToString(),
                            Result_Qty = rdr["result_qty"] == DBNull.Value ? null : (decimal?)rdr["result_qty"],
                            Doc_Ref = rdr["doc_ref"].ToString(),
                            Pallet_No = rdr["pallet_no"].ToString(),
                            Crane_No = rdr["crane_no"].ToString(),
                            Location_No = rdr["location_no"].ToString(),
                            Dest_Su_No = rdr["dest_su_no"].ToString(),
                            To_No = rdr["to_no"].ToString(),
                            To_Line = rdr["to_line"].ToString(),
                            Status = rdr["status"] == DBNull.Value ? null : (int?)rdr["status"],
                            Shortge_Qty = rdr["shortge_qty"] == DBNull.Value ? null : (decimal?)rdr["shortge_qty"],
                            Po_No = rdr["po_no"].ToString(),
                            Invoice_No = rdr["invoice_no"].ToString(),
                            Recviving_Date = rdr["recviving_date"] == DBNull.Value ? null : (DateTime?)rdr["recviving_date"],
                            Delivery_Date = rdr["delivery_date"] == DBNull.Value ? null : (DateTime?)rdr["delivery_date"],
                            Order_Line = rdr["order_line"].ToString(),
                            Queue_No = rdr["queue_no"].ToString(),
                            Ship_To_Code = rdr["ship_to_code"].ToString(),
                            Ship_Name = rdr["ship_name"].ToString(),
                            Delivery_Priority = rdr["delivery_priority"] == DBNull.Value ? null : (int?)rdr["delivery_priority"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_2_A> GetMenu6_2AbyDate(DateTime dtStart, DateTime dtStop)
        {
            List<Class6_2_A> lstobj = new List<Class6_2_A>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();

                    //sql.AppendLine("SELECT");
                    //sql.AppendLine("created, work_type, create_by, batch_number, item_code,");
                    //sql.AppendLine("item_name, su_no, movement_type, movemet_reason, order_no, seq_no,");
                    //sql.AppendLine("result_qty, doc_ref , pallet_no, crane_no, location_no,");
                    //sql.AppendLine("dest_su_no, to_no, to_line, status, shortge_qty, po_no, invoice_no,");
                    //sql.AppendLine("recviving_date, delivery_date, order_line,");
                    //sql.AppendLine("queue_no, ship_to_code, ship_name, delivery_priority ");
                    //sql.AppendLine("FROM public.sap_operateresult");
                    //sql.AppendLine("WHERE (1=1)");
                    //sql.AppendLine("AND (work_type in ('01'))");
                    //sql.AppendLine("AND (created >= @startdate AND created < @stopdate)");
                    //sql.AppendLine("ORDER BY created ASC");
                    //sql.AppendLine(";");


                    sql.Append("SELECT row_number() ");
                    sql.AppendLine("OVER(ORDER BY R.[rptStockDate] ASC, R.prodcode ASC, R.item ASC) AS rn");
                    sql.AppendLine(",R.prodcode, R.item, R.itemdesc, T.lot, R.qty");
                    sql.AppendLine(",R.uom, R.item_bc, R.pallet_bc, R.loc, R.whse");
                    sql.AppendLine(",R.is_req, R.is_hold, R.is_lock, R.update2sl, R.doc_num");
                    sql.AppendLine(",R.createdby, R.modifie_date, R.trans_date, R.rptStockDate");
                    sql.AppendLine(",L.palletnote as palletmapkey, L.uom as unit ");
                    sql.AppendLine("FROM dbo.v_wmstran_receipt_rpt R");
                    sql.AppendLine("LEFT JOIN dbo.wms_trans T");
                    sql.AppendLine("ON R.item_bc = T.item_bc");
                    sql.AppendLine("AND T.unit_key = '01'");
                    sql.AppendLine("AND R.trans_date = T.trans_date");
                    sql.AppendLine("AND T.flag = 3");
                    sql.AppendLine("LEFT JOIN dbo.v_wms_lablemaster L");
                    sql.AppendLine("ON R.item_bc = L.item_bc");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("AND (R.rptStockDate >= @startdate AND R.rptStockDate < @stopdate)");
                    sql.AppendLine("ORDER BY R.rptStockDate ASC, R.prodcode ASC, R.item ASC");
                    sql.AppendLine(";");


                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate",  dtStart);
                    cmd.Parameters.AddWithValue("@stopdate",  dtStop);

                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_2_A objrd = new Class6_2_A
                        {
                            Created = rdr["trans_date"] == DBNull.Value ? null : (DateTime?)rdr["trans_date"],
                            Work_Type = "01",
                            Create_By = rdr["createdby"].ToString(),
                            Batch_Number = rdr["lot"].ToString(),
                            Item_Code = rdr["item"].ToString(),
                            Item_Name = rdr["itemdesc"].ToString(),
                            Su_No = rdr["item_bc"].ToString(),
                            Movement_Type = null,
                            Movemet_Reason = null,
                            Order_No = rdr["doc_num"].ToString(),
                            Seq_No = null,
                            Result_Qty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                            Doc_Ref = null,
                            Pallet_No = rdr["pallet_bc"].ToString(),
                            Crane_No = null,
                            Location_No = rdr["loc"].ToString(),
                            Dest_Su_No = null,
                            To_No = null,
                            To_Line = null,
                            Status = 3,
                            Shortge_Qty = 0,
                            Po_No = null,
                            Invoice_No = null,
                            Recviving_Date = rdr["rptStockDate"] == DBNull.Value ? null : (DateTime?)rdr["rptStockDate"],
                            Delivery_Date = null,
                            Order_Line = null,
                            Queue_No = null,
                            Ship_To_Code = null,
                            Ship_Name = null,
                            Delivery_Priority = null,
                            PalletKey = rdr["palletmapkey"].ToString(),
                            Unit = rdr["uom"].ToString()

                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_2_A> GetMenu6_2AbyDatelimit(DateTime dtStart, DateTime dtStop, long limitrec, long currentPage)
        {
            List<Class6_2_A> lstobj = new List<Class6_2_A>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendLine("SELECT");
                sql.AppendLine("created, work_type, create_by, batch_number, item_code,");
                sql.AppendLine("item_name, su_no, movement_type, movemet_reason, order_no, seq_no,");
                sql.AppendLine("result_qty, doc_ref , pallet_no, crane_no, location_no,");
                sql.AppendLine("dest_su_no, to_no, to_line, status, shortge_qty, po_no, invoice_no,");
                sql.AppendLine("recviving_date, delivery_date, order_line,");
                sql.AppendLine("queue_no, ship_to_code, ship_name, delivery_priority ");
                sql.AppendLine("FROM public.sap_operateresult");
                sql.AppendLine("WHERE (1=1)");
                sql.AppendLine("AND (work_type in ('01'))");
                sql.AppendLine("AND created >= @startdate AND created < @stopdate)");
                sql.AppendLine("ORDER BY created ASC");
                sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                sql.AppendLine(";");

                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, dtStart);
                cmd.Parameters.AddWithValue("@stopdate", NpgsqlDbType.Timestamp, dtStop);
                cmd.Parameters.AddWithValue("@limitrec", NpgsqlDbType.Bigint, limitrec);
                cmd.Parameters.AddWithValue("@offsetrec", NpgsqlDbType.Bigint, (limitrec * currentPage) - limitrec);
                con.Open();

                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Class6_2_A objrd = new Class6_2_A
                    {
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        Work_Type = rdr["work_type"].ToString(),
                        Create_By = rdr["create_by"].ToString(),
                        Batch_Number = rdr["batch_number"].ToString(),
                        Item_Code = rdr["item_code"].ToString(),
                        Item_Name = rdr["item_name"].ToString(),
                        Su_No = rdr["su_no"].ToString(),
                        Movement_Type = rdr["movement_type"].ToString(),
                        Movemet_Reason = rdr["movemet_reason"].ToString(),
                        Order_No = rdr["order_no"].ToString(),
                        Seq_No = rdr["seq_no"].ToString(),
                        Result_Qty = rdr["result_qty"] == DBNull.Value ? null : (decimal?)rdr["result_qty"],
                        Doc_Ref = rdr["doc_ref"].ToString(),
                        Pallet_No = rdr["pallet_no"].ToString(),
                        Crane_No = rdr["crane_no"].ToString(),
                        Location_No = rdr["location_no"].ToString(),
                        Dest_Su_No = rdr["dest_su_no"].ToString(),
                        To_No = rdr["to_no"].ToString(),
                        To_Line = rdr["to_line"].ToString(),
                        Status = rdr["status"] == DBNull.Value ? null : (int?)rdr["status"],
                        Shortge_Qty = rdr["shortge_qty"] == DBNull.Value ? null : (decimal?)rdr["shortge_qty"],
                        Po_No = rdr["po_no"].ToString(),
                        Invoice_No = rdr["invoice_no"].ToString(),
                        Recviving_Date = rdr["recviving_date"] == DBNull.Value ? null : (DateTime?)rdr["recviving_date"],
                        Delivery_Date = rdr["delivery_date"] == DBNull.Value ? null : (DateTime?)rdr["delivery_date"],
                        Order_Line = rdr["order_line"].ToString(),
                        Queue_No = rdr["queue_no"].ToString(),
                        Ship_To_Code = rdr["ship_to_code"].ToString(),
                        Ship_Name = rdr["ship_name"].ToString(),
                        Delivery_Priority = rdr["delivery_priority"] == DBNull.Value ? null : (int?)rdr["delivery_priority"]
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        #endregion

        #region "MENU6.2B"
        public IEnumerable<Class6_2_B> GetAllMenu6_2B()
        {
            List<Class6_2_B> lstobj = new List<Class6_2_B>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT ");
                    sql.AppendLine("created::date as created , po_no, batch_number, item_code,");
                    sql.AppendLine("item_name, movement_type, movemet_reason, ");
                    sql.AppendLine("SUM(result_qty) as result_qty");
                    sql.AppendLine("FROM public.sap_operateresult");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("AND (work_type in ('01'))");
                    //sql.AppendLine("and (created>='" & dtpStart.ToString("s") & "' and created<='" & dtpStop.ToString("s") & "')");
                    sql.AppendLine("GROUP BY created::date, po_no, batch_number, item_code,");
                    sql.AppendLine("item_name, movement_type, movemet_reason");
                    sql.AppendLine("ORDER BY created::date , item_code asc");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_2_B objrd = new Class6_2_B
                        {
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Batch_Number = rdr["batch_number"].ToString(),
                            Item_Code = rdr["item_code"].ToString(),
                            Item_Name = rdr["item_name"].ToString(),
                            Movement_Type = rdr["movement_type"].ToString(),
                            Movemet_Reason = rdr["movemet_reason"].ToString(),
                            Result_Qty = rdr["result_qty"] == DBNull.Value ? null : (decimal?)rdr["result_qty"],
                            Po_no = rdr["po_no"].ToString()
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_2_B> GetAllMenu6_2BbyDate(DateTime dtStart, DateTime dtStop)
        {
            List<Class6_2_B> lstobj = new List<Class6_2_B>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    //sql.AppendLine("SELECT ");
                    //sql.AppendLine("created::date as created , po_no, batch_number, item_code,");
                    //sql.AppendLine("item_name, movement_type, movemet_reason, ");
                    //sql.AppendLine("SUM(result_qty) as result_qty");
                    //sql.AppendLine("FROM public.sap_operateresult");
                    //sql.AppendLine("WHERE (1=1)");
                    //sql.AppendLine("AND (work_type in ('01'))");
                    //sql.AppendLine("AND (created >= @startdate AND created < @stopdate)");
                    //sql.AppendLine("GROUP BY created::date, po_no, batch_number, item_code,");
                    //sql.AppendLine("item_name, movement_type, movemet_reason");
                    //sql.AppendLine("ORDER BY created::date , item_code asc");
                    ////sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    //sql.AppendLine(";");



                    sql.Append("select row_number() ");
                    sql.AppendLine("over(order by R.rptStockDate asc, R.item asc) AS rn,");
                    sql.AppendLine("R.item,R.itemdesc,T.lot,R.[uom],");
                    sql.AppendLine("sum(R.qty) as qty,R.whse , R.doc_num");
                    sql.AppendLine(",R.rptStockDate");
                    sql.AppendLine("FROM dbo.v_wmstran_receipt_rpt R");
                    sql.AppendLine("LEFT JOIN dbo.wms_trans T");
                    sql.AppendLine("ON R.item_bc=T.item_bc");
                    sql.AppendLine("AND T.unit_key='01'");
                    sql.AppendLine("AND R.trans_date=T.trans_date");
                    sql.AppendLine("AND T.flag = 3");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("and (R.rptStockDate >= @startdate and R.rptStockDate < @stopdate)");
                    sql.AppendLine("group by R.item,R.itemdesc,T.lot,R.uom,R.whse,R.rptStockDate , R.doc_num");
                    sql.AppendLine("order by R.rptStockDate asc,  R.item asc");

                    sql.AppendLine(";");



                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate",  dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", dtStop);
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_2_B objrd = new Class6_2_B
                        {
                            Created = rdr["rptStockDate"] == DBNull.Value ? null : (DateTime?)rdr["rptStockDate"],
                            Batch_Number = rdr["lot"].ToString(),
                            Item_Code = rdr["item"].ToString(),
                            Item_Name = rdr["itemdesc"].ToString(),
                            Movement_Type = null,
                            Movemet_Reason = null,
                            Result_Qty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                            Po_no = rdr["doc_num"].ToString()
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_2_B> GetAllMenu6_2BbyDatelimit(DateTime dtStart, DateTime dtStop, long limitrec, long currentPage)
        {
            List<Class6_2_B> lstobj = new List<Class6_2_B>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT ");
                sql.AppendLine("created::date as created , po_no, batch_number, item_code,");
                sql.AppendLine("item_name, movement_type, movemet_reason, ");
                sql.AppendLine("SUM(result_qty) as result_qty");
                sql.AppendLine("FROM public.sap_operateresult");
                sql.AppendLine("WHERE (1=1)");
                sql.AppendLine("AND (work_type in ('01'))");
                sql.AppendLine("AND created >= @startdate AND created < @stopdate)");
                sql.AppendLine("GROUP BY created::date, po_no, batch_number, item_code,");
                sql.AppendLine("item_name, movement_type, movemet_reason");
                sql.AppendLine("ORDER BY created::date , item_code asc");
                sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                sql.AppendLine(";");

                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, dtStart);
                cmd.Parameters.AddWithValue("@stopdate", NpgsqlDbType.Timestamp, dtStop);
                cmd.Parameters.AddWithValue("@limitrec", NpgsqlDbType.Bigint, limitrec);
                cmd.Parameters.AddWithValue("@offsetrec", NpgsqlDbType.Bigint, (limitrec * currentPage) - limitrec);
                con.Open();

                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Class6_2_B objrd = new Class6_2_B
                    {
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        Batch_Number = rdr["batch_number"].ToString(),
                        Item_Code = rdr["item_code"].ToString(),
                        Item_Name = rdr["item_name"].ToString(),
                        Movement_Type = rdr["movement_type"].ToString(),
                        Movemet_Reason = rdr["movemet_reason"].ToString(),
                        Result_Qty = rdr["result_qty"] == DBNull.Value ? null : (decimal?)rdr["result_qty"],
                        Po_no = rdr["po_no"].ToString()
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        #endregion

        #region "MENU6.3A"
        public IEnumerable<Class6_3_A> GetAllMenu6_3A()
        {
            List<Class6_3_A> lstobj = new List<Class6_3_A>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();

                    //sql.AppendLine("select row_number() over(order by t2.brand asc, t1.batch_number asc, t1.item_code asc) AS rn,");
                    //sql.AppendLine("t2.brand, t1.batch_number, t1.item_code, t1.item_name, t1.qty, t1.su_no, t1.palletcode, t3.shelfname");
                    //sql.AppendLine(" , t3.srm_no ");
                    //sql.AppendLine("from public.sap_stock t1");
                    //sql.AppendLine("left join public.sap_itemmaster_v t2");
                    //sql.AppendLine("on t1.item_code=t2.article");
                    //sql.AppendLine("left join wcs.set_shelf t3");
                    //sql.AppendLine("on t1.palletcode=t3.lpncode");
                    //sql.AppendLine("where (1=1)");
                    //sql.AppendLine("order by brand,batch_number,item_code");
                    //sql.AppendLine(";");

                    sql.Append("select row_number() ");
                    sql.AppendLine("over(order by R.rptStockDate asc, R.prodcode asc, R.item asc) AS rn,");
                    sql.AppendLine("R.prodcode,R.item,R.itemdesc,R.lot,R.uom,");
                    sql.AppendLine("R.qty,R.item_bc,R.whse,R.loc,R.pallet_bc");
                    sql.AppendLine(",R.is_req,R.is_hold,R.is_lock,R.update2sl,R.doc_num");
                    sql.AppendLine(",R.createdby,R.trans_date,R.modifie_date,R.rptStockDate");
                    sql.AppendLine(",t2.palletnote as palltmapkey");
                    sql.AppendLine("FROM dbo.v_wmstran_stock_rpt_lot R");
                    sql.AppendLine("LEFT JOIN dbo.wms_lablemaster t2");
                    sql.AppendLine("On R.item_bc=t2.item_bc");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("AND R.is_req=0");
                    sql.AppendLine("order by R.rptStockDate asc, R.prodcode asc, R.item asc");
                    sql.AppendLine(";");


                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_3_A objrd = new Class6_3_A
                        {
                            Brand = null,
                            Batch_Number = rdr["lot"].ToString(),
                            Item_Code = rdr["item"].ToString(),
                            Item_Name = rdr["itemdesc"].ToString(),
                            Qty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                            Su_No = rdr["item_bc"].ToString(),
                            Palletcode = rdr["pallet_bc"].ToString(),
                            Shelfname = rdr["loc"].ToString(),
                            Srm_No = null,
                            Palletgo = rdr["palltmapkey"].ToString()
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_3_A> GetAllMenu6_3Abylimit(long limitrec, long currentPage)
        {
            List<Class6_3_A> lstobj = new List<Class6_3_A>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sql = new StringBuilder();

                sql.AppendLine("select row_number() over(order by t2.brand asc, t1.batch_number asc, t1.item_code asc) AS rn,");
                sql.AppendLine("t2.brand, t1.batch_number, t1.item_code, t1.item_name, t1.qty, t1.su_no, t1.palletcode, t3.shelfname");
                sql.AppendLine(" , t3.srm_no ");
                sql.AppendLine("from public.sap_stock t1");
                sql.AppendLine("left join public.sap_itemmaster_v t2");
                sql.AppendLine("on t1.item_code=t2.article");
                sql.AppendLine("left join wcs.set_shelf t3");
                sql.AppendLine("on t1.palletcode=t3.lpncode");
                sql.AppendLine("where (1=1)");
                sql.AppendLine("order by brand,batch_number,item_code");
                sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                sql.AppendLine(";");
                /*
                sql.AppendLine("SELECT ");
                sql.AppendLine("t2.brand, t1.batch_number, t1.item_code, t1.item_name, sum(t1.qty) AS totalstock, count(t1.palletcode) as countpallet ");
                sql.AppendLine(", t3.srm_no ");
                sql.AppendLine("FROM public.sap_stock t1");
                sql.AppendLine("LEFT JOIN public.sap_itemmaster_v t2");
                sql.AppendLine("ON t1.item_code=t2.article");
                sql.AppendLine("LEFT JOIN wcs.set_shelf t3");
                sql.AppendLine("ON t1.palletcode=t3.lpncode");
                sql.AppendLine("WHERE (1=1)");
                sql.AppendLine("GROUP BY t2.brand, t1.batch_number, t1.item_code, t1.item_name, t3.srm_no");
                sql.AppendLine("ORDER BY brand,batch_number,item_code");
                sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                sql.AppendLine(";");
                */
                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@limitrec", NpgsqlDbType.Bigint, limitrec);
                cmd.Parameters.AddWithValue("@offsetrec", NpgsqlDbType.Bigint, (limitrec * currentPage) - limitrec);
                con.Open();

                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Class6_3_A objrd = new Class6_3_A
                    {
                        Brand = rdr["brand"].ToString(),
                        Batch_Number = rdr["batch_number"].ToString(),
                        Item_Code = rdr["item_code"].ToString(),
                        Item_Name = rdr["item_name"].ToString(),
                        Qty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                        Su_No = rdr["su_no"].ToString(),
                        Palletcode = rdr["palletcode"].ToString(),
                        Shelfname = rdr["shelfname"].ToString(),
                        Srm_No = rdr["srm_no"] == DBNull.Value ? null : (int?)rdr["srm_no"]
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        #endregion

        #region"MENU6.3B"
        public IEnumerable<Class6_3_B> GetAllMenu6_3B()
        {
            List<Class6_3_B> lstobj = new List<Class6_3_B>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    //sql.AppendLine("SELECT ");
                    //sql.AppendLine("t2.brand, t1.batch_number, t1.item_code, t1.item_name, sum(t1.qty) AS totalstock, count(t1.palletcode) as countpallet ");
                    //sql.AppendLine("FROM public.sap_stock t1");
                    //sql.AppendLine("LEFT JOIN public.sap_itemmaster_v t2");
                    //sql.AppendLine("ON t1.item_code=t2.article");
                    //sql.AppendLine("WHERE (1=1)");
                    //sql.AppendLine("GROUP BY t2.brand, t1.batch_number, t1.item_code, t1.item_name");
                    //sql.AppendLine("ORDER BY brand,batch_number,item_code");
                    ////sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    //sql.AppendLine(";");


                    sql.Append("select row_number() ");
                    sql.AppendLine("over(order by  R.item asc) AS rn,");
                    sql.AppendLine("R.item,R.itemdesc,R.lot,R.uom,");
                    sql.AppendLine("sum(R.qty) as qty,R.whse");
                    sql.AppendLine("FROM dbo.v_wmstran_stock_rpt_lot R");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("group by R.item,R.itemdesc,R.lot,R.uom,R.whse");
                    sql.AppendLine("order by R.item asc");
                    sql.AppendLine(";");


                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_3_B objrd = new Class6_3_B
                        {
                            Brand = null,
                            Batch_Number = rdr["lot"].ToString(),
                            Item_Code = rdr["item"].ToString(),
                            Item_Name = rdr["itemdesc"].ToString(),
                            Totalstock = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                            Countpallet = null

                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_3_B> GetAllMenu6_3Bbylimit(long limitrec, long currentPage)
        {
            List<Class6_3_B> lstobj = new List<Class6_3_B>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT ");
                    sql.AppendLine("t2.brand, t1.batch_number, t1.item_code, t1.item_name, sum(t1.qty) AS totalstock, count(t1.palletcode) as countpallet ");
                    sql.AppendLine("FROM public.sap_stock t1");
                    sql.AppendLine("LEFT JOIN public.sap_itemmaster_v t2");
                    sql.AppendLine("OB t1.item_code=t2.article");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("GROUP BY t2.brand, t1.batch_number, t1.item_code, t1.item_name");
                    sql.AppendLine("ORDER BY brand,batch_number,item_code");
                    sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                    sql.AppendLine(";");


                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@limitrec", NpgsqlDbType.Bigint, limitrec);
                    cmd.Parameters.AddWithValue("@offsetrec", NpgsqlDbType.Bigint, (limitrec * currentPage) - limitrec);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_3_B objrd = new Class6_3_B
                        {
                            Brand = rdr["brand"].ToString(),
                            Batch_Number = rdr["batch_number"].ToString(),
                            Item_Code = rdr["item_code"].ToString(),
                            Item_Name = rdr["item_name"].ToString(),
                            Totalstock = rdr["totalstock"] == DBNull.Value ? null : (decimal?)rdr["totalstock"],
                            Countpallet = rdr["countpallet"] == DBNull.Value ? null : (long?)rdr["countpallet"]

                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        #endregion

        #region"MENU6.3C"
        public IEnumerable<Class6_3_C> GetAllMenu6_3CbyDetail()
        {
            List<Class6_3_C> lstobj = new List<Class6_3_C>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    //sql.AppendLine("SELECT ");
                    //sql.AppendLine("t2.brand, t1.batch_number, t1.item_code, t1.item_name, t1.qty, t1.su_no, t1.palletcode, t3.shelfname");
                    //sql.AppendLine(" , DATE_PART('day', now()::timestamp - t1.receiving_date::timestamp) as aging");
                    //sql.AppendLine("FROM public.sap_stock t1");
                    //sql.AppendLine("LEFT JOIN public.sap_itemmaster_v t2");
                    //sql.AppendLine("ON t1.item_code=t2.article");
                    //sql.AppendLine("LEFT JOIN wcs.set_shelf t3");
                    //sql.AppendLine("ON t1.palletcode=t3.lpncode");
                    //sql.AppendLine("WHERE (1=1)");
                    //sql.AppendLine("ORDER BY brand,batch_number,item_code");
                    ////sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    //sql.AppendLine(";");


                    sql.AppendLine("SELECT t1.prodcode, t1.item, t1.itemdesc, t1.supcode, t1.uom, t1.qty, t1.item_bc");
                    sql.AppendLine(", t1.whse, t1.loc, t1.pallet_bc, t1.is_req, t1.is_hold, t1.is_lock, t1.update2sl");
                    sql.AppendLine(", t1.doc_num, t1.createdby, t1.trans_date, t1.modifie_date, t1.rptStockDate");
                    sql.AppendLine(", cast(t1.trans_num as bigint) as trans_num, t1.lot, t1.exp_date, t1.mfg_date");
                    sql.AppendLine(", DATEDIFF(day, t1.exp_date, GETDATE()) as aging");
                    sql.AppendLine(",t2.shelfname");
                    sql.AppendLine(",t3.palletnote as palltmapkey");
                    sql.AppendLine("FROM dbo.v_wmstran_stock_rpt_lot t1");
                    sql.AppendLine("LEFT JOIN wcs.set_shelf t2");
                    sql.AppendLine("ON t1.pallet_bc=t2.lpncode");
                    sql.AppendLine("LEFT JOIN dbo.wms_lablemaster t3");
                    sql.AppendLine("On t1.item_bc=t3.item_bc");
                    sql.AppendLine("order by item ASC, mfg_date ASC, item_bc ASC");
                    sql.AppendLine(";");




                    //sql.AppendLine("SELECT prodcode, item, itemdesc, supcode, uom, qty, item_bc");
                    //sql.AppendLine(", whse, loc, pallet_bc, is_req, is_hold, is_lock, update2sl");
                    //sql.AppendLine(", doc_num, createdby, trans_date, modifie_date, rptStockDate");
                    //sql.AppendLine(", cast(trans_num as bigint) as trans_num, lot, exp_date, mfg_date");
                    //sql.AppendLine("FROM dbo.v_wmstran_stock_rpt_lot");
                    //sql.AppendLine("order by item ASC, mfg_date ASC, item_bc ASC");
                    //sql.AppendLine(";");





                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_3_C objrd = new Class6_3_C
                        {
                            Brand = rdr["prodcode"].ToString(),
                            Batch_Number = rdr["lot"].ToString(),
                            Item_Code = rdr["item"].ToString(),
                            Item_Name = rdr["itemdesc"].ToString(),
                            Qty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                            Su_No = rdr["item_bc"].ToString(),
                            Palletcode = rdr["pallet_bc"].ToString(),
                            Shelfname = rdr["shelfname"].ToString(),
                            Aging = rdr["aging"] == DBNull.Value ? null : (Int32?)rdr["aging"],
                            Palletgo = rdr["palltmapkey"].ToString()


                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (SqlException ex)
                {
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return lstobj;
        }

        public IEnumerable<Class6_3_C> GetAllMenu6_3CbyDetaillimit(long limitrec, long currentPage)
        {
            List<Class6_3_C> lstobj = new List<Class6_3_C>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                StringBuilder sql = new StringBuilder();
                //sql.AppendLine("SELECT ");
                //sql.AppendLine("t2.brand, t1.batch_number, t1.item_code, t1.item_name, t1.qty, t1.su_no, t1.palletcode, t3.shelfname");
                //sql.AppendLine(" , DATE_PART('day', now()::timestamp - t1.receiving_date::timestamp) as aging");
                //sql.AppendLine("FROM public.sap_stock t1");
                //sql.AppendLine("LEFT JOIN public.sap_itemmaster_v t2");
                //sql.AppendLine("ON t1.item_code=t2.article");
                //sql.AppendLine("LEFT JOIN wcs.set_shelf t3");
                //sql.AppendLine("ON t1.palletcode=t3.lpncode");
                //sql.AppendLine("WHERE (1=1)");
                //sql.AppendLine("ORDER BY brand,batch_number,item_code");
                //sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                //sql.AppendLine(";");

                sql.AppendLine("SELECT t1.prodcode, t1.item, t1.itemdesc, t1.supcode, t1.uom, t1.qty, t1.item_bc");
                sql.AppendLine(", t1.whse, t1.loc, t1.pallet_bc, t1.is_req, t1.is_hold, t1.is_lock, t1.update2sl");
                sql.AppendLine(", t1.doc_num, t1.createdby, t1.trans_date, t1.modifie_date, t1.rptStockDate");
                sql.AppendLine(", cast(t1.trans_num as bigint) as trans_num, t1.lot, t1.exp_date, t1.mfg_date");
                sql.AppendLine(", DATEDIFF(day, t1.exp_date, GETDATE()) as aging");
                sql.AppendLine(",t2.shelfname");
                sql.AppendLine("FROM dbo.v_wmstran_stock_rpt_lot t1");
                sql.AppendLine("LEFT JOIN wcs.set_shelf t2");
                sql.AppendLine("ON t1.pallet_bc=t2.lpncode");
                sql.AppendLine("order by item ASC, mfg_date ASC, item_bc ASC");
                sql.AppendLine(";");


                NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.AddWithValue("@limitrec", NpgsqlDbType.Bigint, limitrec);
                cmd.Parameters.AddWithValue("@offsetrec", NpgsqlDbType.Bigint, (limitrec * currentPage) - limitrec);
                con.Open();

                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Class6_3_C objrd = new Class6_3_C
                    {
                        Brand = rdr["brand"].ToString(),
                        Batch_Number = rdr["batch_number"].ToString(),
                        Item_Code = rdr["item_code"].ToString(),
                        Item_Name = rdr["item_name"].ToString(),
                        Qty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                        Su_No = rdr["su_no"].ToString(),
                        Palletcode = rdr["palletcode"].ToString(),
                        Shelfname = rdr["shelfname"].ToString(),
                        Aging = rdr["aging"] == DBNull.Value ? null : (Int32?)rdr["aging"]

                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }
        #endregion

        #region"MENU6.3D"
        public IEnumerable<Class6_3_D> GetAllMenu6_3DbyDetail()
        {
            List<Class6_3_D> lstobj = new List<Class6_3_D>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT ");
                    sql.AppendLine("t2.brand, t1.batch_number, t1.item_code, t1.item_name, t1.su_no, t1.qty");
                    sql.AppendLine(", t1.unit ");
                    sql.AppendLine(", t1.palletcode, t3.shelfname ");
                    sql.AppendLine("FROM public.sap_stock t1");
                    sql.AppendLine("LEFT JOIN public.sap_itemmaster_v t2");
                    sql.AppendLine("ON t1.item_code=t2.article");
                    sql.AppendLine("LEFT JOIN wcs.set_shelf t3");
                    sql.AppendLine("ON t1.palletcode=t3.lpncode");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("ORDER BY brand,batch_number,item_code");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");


                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_3_D objrd = new Class6_3_D
                        {
                            Brand = rdr["brand"].ToString(),
                            Batch_Number = rdr["batch_number"].ToString(),
                            Item_Code = rdr["item_code"].ToString(),
                            Item_Name = rdr["item_name"].ToString(),
                            Qty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                            Unit = rdr["unit"].ToString(),
                            Su_No = rdr["su_no"].ToString(),
                            Palletcode = rdr["palletcode"].ToString(),
                            Shelfname = rdr["shelfname"].ToString()
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_3_D> GetAllMenu6_3DbyDetaillimit(long limitrec, long currentPage)
        {
            List<Class6_3_D> lstobj = new List<Class6_3_D>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT ");
                    sql.AppendLine("t2.brand, t1.batch_number, t1.item_code, t1.item_name, t1.su_no, t1.qty");
                    sql.AppendLine(", t1.unit ");
                    sql.AppendLine(", t1.palletcode, t3.shelfname ");
                    sql.AppendLine("FROM public.sap_stock t1");
                    sql.AppendLine("LEFT JOIN public.sap_itemmaster_v t2");
                    sql.AppendLine("ON t1.item_code=t2.article");
                    sql.AppendLine("LEFT JOIN wcs.set_shelf t3");
                    sql.AppendLine("ON t1.palletcode=t3.lpncode");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("ORDER BY brand,batch_number,item_code");
                    sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@limitrec", NpgsqlDbType.Bigint, limitrec);
                    cmd.Parameters.AddWithValue("@offsetrec", NpgsqlDbType.Bigint, (limitrec * currentPage) - limitrec);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_3_D objrd = new Class6_3_D
                        {
                            Brand = rdr["brand"].ToString(),
                            Batch_Number = rdr["batch_number"].ToString(),
                            Item_Code = rdr["item_code"].ToString(),
                            Item_Name = rdr["item_name"].ToString(),
                            Qty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                            Unit = rdr["unit"].ToString(),
                            Su_No = rdr["su_no"].ToString(),
                            Palletcode = rdr["palletcode"].ToString(),
                            Shelfname = rdr["shelfname"].ToString()

                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }
        #endregion

        #region "MENU6.3E"
        public IEnumerable<Class6_3_E> GetAllMenu6_3EbyDetail()
        {
            List<Class6_3_E> lstobj = new List<Class6_3_E>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT ");
                    sql.AppendLine("t2.brand, t1.batch_number, t1.item_code, t1.item_name, t1.qty, t1.su_no, t1.palletcode, t3.shelfname");
                    sql.AppendLine("FROM public.sap_stock t1");
                    sql.AppendLine("LEFT JOIN public.sap_itemmaster_v t2");
                    sql.AppendLine("ON t1.item_code=t2.article");
                    sql.AppendLine("LEFT JOIN wcs.set_shelf t3");
                    sql.AppendLine("ON t1.palletcode=t3.lpncode");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("ORDER BY brand,batch_number,item_code");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_3_E objrd = new Class6_3_E
                        {
                            Brand = rdr["brand"].ToString(),
                            Batch_Number = rdr["batch_number"].ToString(),
                            Item_Code = rdr["item_code"].ToString(),
                            Item_Name = rdr["item_name"].ToString(),
                            Qty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                            Su_No = rdr["su_no"].ToString(),
                            Palletcode = rdr["palletcode"].ToString(),
                            Shelfname = rdr["shelfname"].ToString()
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_3_E> GetAllMenu6_3EbyDetaillimit(long limitrec, long currentPage)
        {
            List<Class6_3_E> lstobj = new List<Class6_3_E>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT ");
                    sql.AppendLine("t2.brand, t1.batch_number, t1.item_code, t1.item_name, t1.qty, t1.su_no, t1.palletcode, t3.shelfname");
                    sql.AppendLine("FROM public.sap_stock t1");
                    sql.AppendLine("LEFT JOIN public.sap_itemmaster_v t2");
                    sql.AppendLine("ON t1.item_code=t2.article");
                    sql.AppendLine("LEFT JOIN wcs.set_shelf t3");
                    sql.AppendLine("ON t1.palletcode=t3.lpncode");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("ORDER BY brand,batch_number,item_code");
                    sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@limitrec", NpgsqlDbType.Bigint, limitrec);
                    cmd.Parameters.AddWithValue("@offsetrec", NpgsqlDbType.Bigint, (limitrec * currentPage) - limitrec);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_3_E objrd = new Class6_3_E
                        {
                            Brand = rdr["brand"].ToString(),
                            Batch_Number = rdr["batch_number"].ToString(),
                            Item_Code = rdr["item_code"].ToString(),
                            Item_Name = rdr["item_name"].ToString(),
                            Qty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                            Su_No = rdr["su_no"].ToString(),
                            Palletcode = rdr["palletcode"].ToString(),
                            Shelfname = rdr["shelfname"].ToString()
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }
        #endregion

        #region"MENU6.4A"
        public IEnumerable<Class6_4_A> GetAllMenu6_4A()
        {
            List<Class6_4_A> lstobj = new List<Class6_4_A>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT ");
                    sql.AppendLine("idx, created, entity_lock, modified, client_id, client_ip, seq_no, ");
                    sql.AppendLine("work_type, order_no, order_line, ship_to_code, ship_name, ");
                    sql.AppendLine("movement_type, movemet_reason, item_code, item_name, ");
                    sql.AppendLine("batch_number, su_no, result_qty, to_no, to_line, ");
                    sql.AppendLine("doc_ref, po_no, delivery_date, delivery_priority, ");
                    sql.AppendLine("ref_no, ref_line,create_by, created_date, ");
                    sql.AppendLine("pallet_no, crane_no, location_no,status ");
                    sql.AppendLine("FROM public.sap_operateresult");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("AND (work_type in ('05'))");
                    //sql.AppendLine("AND (created>='" & dtpStart.ToString("s") & "' and created<='" & dtpStop.ToString("s") & "')");
                    sql.AppendLine("ORDER BY idx asc");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");



                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_4_A objrd = new Class6_4_A
                        {
                            Idx = rdr["idx"] == DBNull.Value ? null : (long?)rdr["idx"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Entity_Lock = rdr["entity_lock"] == DBNull.Value ? null : (int?)rdr["entity_lock"],
                            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                            Client_Id = rdr["client_id"] == DBNull.Value ? null : (long?)rdr["client_id"],
                            Client_Ip = rdr["client_ip"].ToString(),
                            Seq_No = rdr["seq_no"].ToString(),
                            Work_Type = rdr["work_type"].ToString(),
                            Order_No = rdr["order_no"].ToString(),
                            Order_Line = rdr["order_line"].ToString(),
                            Ship_To_Code = rdr["ship_to_code"].ToString(),
                            Ship_Name = rdr["ship_name"].ToString(),
                            Movement_Type = rdr["movement_type"].ToString(),
                            Movemet_Reason = rdr["movemet_reason"].ToString(),
                            Item_Code = rdr["item_code"].ToString(),
                            Item_Name = rdr["item_name"].ToString(),
                            Batch_Number = rdr["batch_number"].ToString(),
                            Su_No = rdr["su_no"].ToString(),
                            Result_Qty = rdr["result_qty"] == DBNull.Value ? null : (decimal?)rdr["result_qty"],
                            To_No = rdr["to_no"].ToString(),
                            To_Line = rdr["to_line"].ToString(),
                            Doc_Ref = rdr["doc_ref"].ToString(),
                            Po_No = rdr["po_no"].ToString(),
                            Delivery_Date = rdr["delivery_date"] == DBNull.Value ? null : (DateTime?)rdr["delivery_date"],
                            Delivery_Priority = rdr["delivery_priority"] == DBNull.Value ? null : (int?)rdr["delivery_priority"],
                            Ref_No = rdr["ref_no"].ToString(),
                            Ref_Line = rdr["ref_line"].ToString(),
                            Create_By = rdr["create_by"].ToString(),
                            Created_Date = rdr["created_date"] == DBNull.Value ? null : (DateTime?)rdr["created_date"],
                            Pallet_No = rdr["pallet_no"].ToString(),
                            Crane_No = rdr["crane_no"].ToString(),
                            Location_No = rdr["location_no"].ToString(),
                            Status = rdr["status"] == DBNull.Value ? null : (int?)rdr["status"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_4_A> GetAllMenu6_4AbyDate(DateTime dtStart, DateTime dtStop)
        {
            List<Class6_4_A> lstobj = new List<Class6_4_A>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    //sql.AppendLine("SELECT ");
                    //sql.AppendLine("idx, created, entity_lock, modified, client_id, client_ip, seq_no, ");
                    //sql.AppendLine("work_type, order_no, order_line, ship_to_code, ship_name, ");
                    //sql.AppendLine("movement_type, movemet_reason, item_code, item_name, ");
                    //sql.AppendLine("batch_number, su_no, result_qty, to_no, to_line, ");
                    //sql.AppendLine("doc_ref, po_no, delivery_date, delivery_priority, ");
                    //sql.AppendLine("ref_no, ref_line,create_by, created_date, ");
                    //sql.AppendLine("pallet_no, crane_no, location_no,status ");
                    //sql.AppendLine("FROM public.sap_operateresult");
                    //sql.AppendLine("WHERE (1=1)");
                    //sql.AppendLine("AND (work_type in ('05'))");
                    //sql.AppendLine("AND (created >= @startdate and created < @stopdate)");
                    //sql.AppendLine("ORDER BY idx asc");
                    ////sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    //sql.AppendLine(";");

                    sql.Append("select row_number() ");
                    sql.AppendLine("over(order by R.rptStockDate asc, R.prodcode asc, R.item asc) AS rn,");
                    sql.AppendLine("R.prodcode,R.item,R.itemdesc,R.supcode,R.uom,");
                    sql.AppendLine("R.qty,R.item_bc,R.whse,R.loc,R.pallet_bc");
                    sql.AppendLine(",R.is_req,R.is_hold,R.is_lock,R.update2sl,R.doc_num");
                    sql.AppendLine(",R.createdby,R.trans_date,R.modifie_date,R.rptStockDate, T.lot");
                    sql.AppendLine(",t2.palletnote as palltmapkey");
                    sql.AppendLine("FROM dbo.v_wmstran_issue_rpt R");
                    sql.AppendLine("LEFT JOIN dbo.wms_trans T");
                    sql.AppendLine("ON R.item_bc=T.item_bc");
                    sql.AppendLine("AND R.trans_date=T.trans_date");
                    sql.AppendLine("AND T.flag = 2");
                    sql.AppendLine("AND T.unit_key = '05'");
                    sql.AppendLine("LEFT JOIN dbo.wms_lablemaster t2");
                    sql.AppendLine("On R.item_bc=t2.item_bc");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("AND (R.rptStockDate>= @startdate AND R.rptStockDate<@stopdate)");
                    sql.AppendLine("order by R.rptStockDate asc, R.prodcode asc, R.item asc");
                    sql.AppendLine(";");


                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate",  dtStart);
                    cmd.Parameters.AddWithValue("@stopdate",  dtStop);
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_4_A objrd = new Class6_4_A
                        {
                            Idx = null,
                            Created = rdr["modifie_date"] == DBNull.Value ? null : (DateTime?)rdr["modifie_date"],
                            Entity_Lock = null,
                            Modified = null,
                            Client_Id = null,
                            Client_Ip = null,
                            Seq_No = null,
                            Work_Type = null,
                            Order_No = rdr["doc_num"].ToString(),
                            Order_Line = null,
                            Ship_To_Code = null,
                            Ship_Name = null,
                            Movement_Type = null,
                            Movemet_Reason = null,
                            Item_Code = rdr["item"].ToString(),
                            Item_Name = rdr["itemdesc"].ToString(),
                            Batch_Number = rdr["lot"].ToString(),
                            Su_No = rdr["item_bc"].ToString(),
                            Result_Qty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"],
                            To_No = null,
                            To_Line = null,
                            Doc_Ref = null,
                            Po_No = null,
                            Delivery_Date = null,
                            Delivery_Priority = null,
                            Ref_No = null,
                            Ref_Line = null,
                            Create_By = null,
                            Created_Date = null,
                            Pallet_No = rdr["pallet_bc"].ToString(),
                            Crane_No = null,
                            Location_No = null,
                            Status = null,
                            Palletgo = rdr["palltmapkey"].ToString(),
                            Unit = rdr["uom"].ToString()
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_4_A> GetAllMenu6_4AbyDatelimit(DateTime dtStart, DateTime dtStop, long limitrec, long currentPage)
        {
            List<Class6_4_A> lstobj = new List<Class6_4_A>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT ");
                    sql.AppendLine("idx, created, entity_lock, modified, client_id, client_ip, seq_no, ");
                    sql.AppendLine("work_type, order_no, order_line, ship_to_code, ship_name, ");
                    sql.AppendLine("movement_type, movemet_reason, item_code, item_name, ");
                    sql.AppendLine("batch_number, su_no, result_qty, to_no, to_line, ");
                    sql.AppendLine("doc_ref, po_no, delivery_date, delivery_priority, ");
                    sql.AppendLine("ref_no, ref_line,create_by, created_date, ");
                    sql.AppendLine("pallet_no, crane_no, location_no,status ");
                    sql.AppendLine("FROM public.sap_operateresult");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("AND (work_type in ('05'))");
                    sql.AppendLine("AND (created >= @startdate and created < @stopdate)");
                    sql.AppendLine("ORDER BY idx asc");
                    sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", NpgsqlDbType.Timestamp, dtStop);
                    cmd.Parameters.AddWithValue("@limitrec", NpgsqlDbType.Bigint, limitrec);
                    cmd.Parameters.AddWithValue("@offsetrec", NpgsqlDbType.Bigint, (limitrec * currentPage) - limitrec);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_4_A objrd = new Class6_4_A
                        {
                            Idx = rdr["idx"] == DBNull.Value ? null : (long?)rdr["idx"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Entity_Lock = rdr["entity_lock"] == DBNull.Value ? null : (int?)rdr["entity_lock"],
                            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                            Client_Id = rdr["client_id"] == DBNull.Value ? null : (long?)rdr["client_id"],
                            Client_Ip = rdr["client_ip"].ToString(),
                            Seq_No = rdr["seq_no"].ToString(),
                            Work_Type = rdr["work_type"].ToString(),
                            Order_No = rdr["order_no"].ToString(),
                            Order_Line = rdr["order_line"].ToString(),
                            Ship_To_Code = rdr["ship_to_code"].ToString(),
                            Ship_Name = rdr["ship_name"].ToString(),
                            Movement_Type = rdr["movement_type"].ToString(),
                            Movemet_Reason = rdr["movemet_reason"].ToString(),
                            Item_Code = rdr["item_code"].ToString(),
                            Item_Name = rdr["item_name"].ToString(),
                            Batch_Number = rdr["batch_number"].ToString(),
                            Su_No = rdr["su_no"].ToString(),
                            Result_Qty = rdr["result_qty"] == DBNull.Value ? null : (decimal?)rdr["result_qty"],
                            To_No = rdr["to_no"].ToString(),
                            To_Line = rdr["to_line"].ToString(),
                            Doc_Ref = rdr["doc_ref"].ToString(),
                            Po_No = rdr["po_no"].ToString(),
                            Delivery_Date = rdr["delivery_date"] == DBNull.Value ? null : (DateTime?)rdr["delivery_date"],
                            Delivery_Priority = rdr["delivery_priority"] == DBNull.Value ? null : (int?)rdr["delivery_priority"],
                            Ref_No = rdr["ref_no"].ToString(),
                            Ref_Line = rdr["ref_line"].ToString(),
                            Create_By = rdr["create_by"].ToString(),
                            Created_Date = rdr["created_date"] == DBNull.Value ? null : (DateTime?)rdr["created_date"],
                            Pallet_No = rdr["pallet_no"].ToString(),
                            Crane_No = rdr["crane_no"].ToString(),
                            Location_No = rdr["location_no"].ToString(),
                            Status = rdr["status"] == DBNull.Value ? null : (int?)rdr["status"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        #endregion

        #region "MENU6.4.B"
        public IEnumerable<Class6_4_B> GetAllMenu6_4B()
        {
            List<Class6_4_B> lstobj = new List<Class6_4_B>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT ");
                    sql.AppendLine("created::date as created, order_no, ship_to_code, ship_name, ");
                    sql.AppendLine("item_code, item_name, movement_type, movemet_reason, ");
                    sql.AppendLine("sum(result_qty) as result_qty");
                    sql.AppendLine("from public.sap_operateresult");
                    sql.AppendLine("where (1=1)");
                    sql.AppendLine("and  (work_type in ('05'))");
                    sql.AppendLine("group  by created::date , order_no, ship_to_code, ship_name,");
                    sql.AppendLine("item_code, item_name, movement_type, movemet_reason ");
                    sql.AppendLine("order by created::date , order_no asc, item_code asc");
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_4_B objrd = new Class6_4_B
                        {
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Order_No = rdr["order_no"].ToString(),
                            Ship_To_Code = rdr["ship_to_code"].ToString(),
                            Ship_Name = rdr["ship_name"].ToString(),
                            Movement_Type = rdr["movement_type"].ToString(),
                            Movemet_Reason = rdr["movemet_reason"].ToString(),
                            Item_Code = rdr["item_code"].ToString(),
                            Item_Name = rdr["item_name"].ToString(),
                            Result_Qty = rdr["result_qty"] == DBNull.Value ? null : (decimal?)rdr["result_qty"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_4_B> GetAllMenu6_4BbyDate(DateTime dtStart, DateTime dtStop)
        {
            List<Class6_4_B> lstobj = new List<Class6_4_B>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    //sql.AppendLine("SELECT ");
                    //sql.AppendLine("created::date as created, order_no, ship_to_code, ship_name, ");
                    //sql.AppendLine("item_code, item_name, movement_type, movemet_reason, ");
                    //sql.AppendLine("sum(result_qty) as result_qty");
                    //sql.AppendLine("from public.sap_operateresult");
                    //sql.AppendLine("where (1=1)");
                    //sql.AppendLine("and  (work_type in ('05'))");
                    //sql.AppendLine("AND (created >= @startdate and created < @stopdate)");
                    //sql.Append("group  by created::date , order_no, ship_to_code, ship_name,");
                    //sql.AppendLine("item_code, item_name, movement_type, movemet_reason ");
                    //sql.AppendLine("order by created::date , order_no asc, item_code asc");
                    ////sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    //sql.AppendLine(";");


                    sql.Append("select row_number() ");
                    sql.AppendLine("over(order by R.rptStockDate asc, R.item asc) AS rn,");
                    sql.AppendLine("R.item,R.itemdesc,R.supcode,R.uom,");
                    sql.AppendLine("sum(R.qty) as qty,R.whse, R.doc_num");
                    sql.AppendLine(",R.rptStockDate,T.lot");
                    sql.AppendLine("FROM dbo.v_wmstran_issue_rpt R");
                    sql.AppendLine("LEFT JOIN dbo.wms_trans T");
                    sql.AppendLine("ON R.item_bc=T.item_bc");
                    sql.AppendLine("AND R.trans_date=T.trans_date");
                    sql.AppendLine("AND T.flag = 2");
                    sql.AppendLine("AND T.unit_key = '05'");
                    sql.AppendLine("where (1=1)");
                    sql.AppendLine("and (R.rptStockDate>=@startdate and R.rptStockDate<@stopdate)");
                    sql.AppendLine("group by R.item,R.itemdesc,R.supcode,R.uom,R.whse,R.rptStockDate,T.lot, R.doc_num");
                    sql.AppendLine("order by R.rptStockDate asc, R.item asc , T.lot asc ");
                    sql.AppendLine(";");



                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", dtStop);
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_4_B objrd = new Class6_4_B
                        {
                            Created = rdr["rptStockDate"] == DBNull.Value ? null : (DateTime?)rdr["rptStockDate"],
                            Order_No = rdr["doc_num"].ToString(),
                            Batch_Number = rdr["lot"].ToString(),
                            Ship_To_Code = null,
                            Ship_Name = null,
                            Movement_Type = null,
                            Movemet_Reason = null,
                            Item_Code = rdr["item"].ToString(),
                            Item_Name = rdr["itemdesc"].ToString(),
                            Result_Qty = rdr["qty"] == DBNull.Value ? null : (decimal?)rdr["qty"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_4_B> GetAllMenu6_4BbyDatelimit(DateTime dtStart, DateTime dtStop, long limitrec, long currentPage)
        {
            List<Class6_4_B> lstobj = new List<Class6_4_B>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT ");
                    sql.AppendLine("created::date as created, order_no, ship_to_code, ship_name, ");
                    sql.AppendLine("item_code, item_name, movement_type, movemet_reason, ");
                    sql.AppendLine("sum(result_qty) as result_qty");
                    sql.AppendLine("from public.sap_operateresult");
                    sql.AppendLine("where (1=1)");
                    sql.AppendLine("and  (work_type in ('05'))");
                    sql.AppendLine("AND (created >= @startdate and created < @stopdate)");
                    sql.Append("group  by created::date , order_no, ship_to_code, ship_name,");
                    sql.AppendLine("item_code, item_name, movement_type, movemet_reason ");
                    sql.AppendLine("order by created::date , order_no asc, item_code asc");
                    sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", NpgsqlDbType.Timestamp, dtStop);
                    cmd.Parameters.AddWithValue("@limitrec", NpgsqlDbType.Bigint, limitrec);
                    cmd.Parameters.AddWithValue("@offsetrec", NpgsqlDbType.Bigint, (limitrec * currentPage) - limitrec);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_4_B objrd = new Class6_4_B
                        {
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Order_No = rdr["order_no"].ToString(),
                            Ship_To_Code = rdr["ship_to_code"].ToString(),
                            Ship_Name = rdr["ship_name"].ToString(),
                            Movement_Type = rdr["movement_type"].ToString(),
                            Movemet_Reason = rdr["movemet_reason"].ToString(),
                            Item_Code = rdr["item_code"].ToString(),
                            Item_Name = rdr["item_name"].ToString(),
                            Result_Qty = rdr["result_qty"] == DBNull.Value ? null : (decimal?)rdr["result_qty"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }
        #endregion

        #region "MENU6.4C"
        public IEnumerable<Class6_4_C> GetAllMenu6_4C()
        {
            List<Class6_4_C> lstobj = new List<Class6_4_C>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT ");
                    sql.AppendLine("o.order_no ,o.ship_to_code,o.ship_name,o.item_code,o.item_name,");
                    sql.AppendLine("o.batch_number,o.su_no,o.need_qty,o.pallet_qty,o.state,o.palletcode,");
                    sql.AppendLine("o.ctime::timestamp (0) AS ctime ,o.stime::timestamp (0) AS stime ,o.mtime::timestamp (0) AS mtime,");
                    sql.AppendLine("CASE");
                    sql.AppendLine("WHEN o.state=0  THEN  wcs.fuc_get_srmoutready(o.su_no) ");
                    sql.AppendLine("WHEN o.state=1  THEN  wcs.fuc_get_srmoutready(o.su_no) ");
                    sql.AppendLine("WHEN o.state=2  THEN  'Inprocess' ");
                    sql.AppendLine("WHEN o.state=3  THEN  'Complete'");
                    sql.AppendLine("WHEN o.state=4  THEN  'Error'");
                    sql.AppendLine("ELSE ''");
                    sql.AppendLine("END as msgsrm,");
                    sql.AppendLine("(SELECT wcs.fuc_datetimediff(o.stime::time without time zone, o.mtime::time without time zone) AS fuc_datetimediff) AS loadtime");
                    sql.AppendLine("FROM public.sap_batchorder o");
                    sql.AppendLine("WHERE (1=1)");
                    //sql.AppendLine("and (o.ctime>='" & dtpStart.ToString("s") & "' and o.ctime<='" & dtpStop.ToString("s") & "')");
                    sql.AppendLine("ORDER BY o.stime asc, o.su_n");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_4_C objrd = new Class6_4_C
                        {

                            Order_No = rdr["order_no"].ToString(),
                            Ship_To_Code = rdr["ship_to_code"].ToString(),
                            Ship_Name = rdr["ship_name"].ToString(),
                            Item_Code = rdr["item_code"].ToString(),
                            Item_Name = rdr["item_name"].ToString(),
                            Batch_Number = rdr["batch_number"].ToString(),
                            Su_No = rdr["su_no"].ToString(),
                            Need_Qty = rdr["need_qty"] == DBNull.Value ? null : (decimal?)rdr["need_qty"],
                            Pallet_Qty = rdr["pallet_qty"] == DBNull.Value ? null : (decimal?)rdr["pallet_qty"],
                            State = rdr["state"] == DBNull.Value ? null : (int?)rdr["state"],
                            Palletcode = rdr["palletcode"].ToString(),
                            Ctime = rdr["ctime"] == DBNull.Value ? null : (DateTime?)rdr["ctime"],
                            Stime = rdr["stime"] == DBNull.Value ? null : (DateTime?)rdr["stime"],
                            Mtime = rdr["mtime"] == DBNull.Value ? null : (DateTime?)rdr["mtime"],
                            Msgsrm = rdr["msgsrm"].ToString(),
                            Loadtime = rdr["loadtime"] == DBNull.Value ? null : (DateTime?)rdr["loadtime"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_4_C> GetAllMenu6_4CbyDate(DateTime dtStart, DateTime dtStop)
        {
            List<Class6_4_C> lstobj = new List<Class6_4_C>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT ");
                    sql.AppendLine("o.order_no ,o.ship_to_code,o.ship_name,o.item_code,o.item_name,");
                    sql.AppendLine("o.batch_number,o.su_no,o.need_qty,o.pallet_qty,o.state,o.palletcode,");
                    sql.AppendLine("o.ctime::timestamp (0) AS ctime ,o.stime::timestamp (0) AS stime ,o.mtime::timestamp (0) AS mtime,");
                    sql.AppendLine("CASE");
                    sql.AppendLine("WHEN o.state=0  THEN  wcs.fuc_get_srmoutready(o.su_no) ");
                    sql.AppendLine("WHEN o.state=1  THEN  wcs.fuc_get_srmoutready(o.su_no) ");
                    sql.AppendLine("WHEN o.state=2  THEN  'Inprocess' ");
                    sql.AppendLine("WHEN o.state=3  THEN  'Complete'");
                    sql.AppendLine("WHEN o.state=4  THEN  'Error'");
                    sql.AppendLine("ELSE ''");
                    sql.AppendLine("END as msgsrm,");
                    sql.AppendLine("(SELECT wcs.fuc_datetimediff(o.stime::time without time zone, o.mtime::time without time zone) AS fuc_datetimediff) AS loadtime");
                    sql.AppendLine("FROM public.sap_batchorder o");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("AND (o.ctime >= @startdate and o.ctime < @stopdate)");
                    sql.AppendLine("ORDER BY o.stime asc, o.su_n");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", NpgsqlDbType.Timestamp, dtStop);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_4_C objrd = new Class6_4_C
                        {

                            Order_No = rdr["order_no"].ToString(),
                            Ship_To_Code = rdr["ship_to_code"].ToString(),
                            Ship_Name = rdr["ship_name"].ToString(),
                            Item_Code = rdr["item_code"].ToString(),
                            Item_Name = rdr["item_name"].ToString(),
                            Batch_Number = rdr["batch_number"].ToString(),
                            Su_No = rdr["su_no"].ToString(),
                            Need_Qty = rdr["need_qty"] == DBNull.Value ? null : (decimal?)rdr["need_qty"],
                            Pallet_Qty = rdr["pallet_qty"] == DBNull.Value ? null : (decimal?)rdr["pallet_qty"],
                            State = rdr["state"] == DBNull.Value ? null : (int?)rdr["state"],
                            Palletcode = rdr["palletcode"].ToString(),
                            Ctime = rdr["ctime"] == DBNull.Value ? null : (DateTime?)rdr["ctime"],
                            Stime = rdr["stime"] == DBNull.Value ? null : (DateTime?)rdr["stime"],
                            Mtime = rdr["mtime"] == DBNull.Value ? null : (DateTime?)rdr["mtime"],
                            Msgsrm = rdr["msgsrm"].ToString(),
                            Loadtime = rdr["loadtime"] == DBNull.Value ? null : (DateTime?)rdr["loadtime"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_4_C> GetAllMenu6_4CbyDatelimit(DateTime dtStart, DateTime dtStop, long limitrec, long currentPage)
        {
            List<Class6_4_C> lstobj = new List<Class6_4_C>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT ");
                    sql.AppendLine("o.order_no ,o.ship_to_code,o.ship_name,o.item_code,o.item_name,");
                    sql.AppendLine("o.batch_number,o.su_no,o.need_qty,o.pallet_qty,o.state,o.palletcode,");
                    sql.AppendLine("o.ctime::timestamp (0) AS ctime ,o.stime::timestamp (0) AS stime ,o.mtime::timestamp (0) AS mtime,");
                    sql.AppendLine("CASE");
                    sql.AppendLine("WHEN o.state=0  THEN  wcs.fuc_get_srmoutready(o.su_no) ");
                    sql.AppendLine("WHEN o.state=1  THEN  wcs.fuc_get_srmoutready(o.su_no) ");
                    sql.AppendLine("WHEN o.state=2  THEN  'Inprocess' ");
                    sql.AppendLine("WHEN o.state=3  THEN  'Complete'");
                    sql.AppendLine("WHEN o.state=4  THEN  'Error'");
                    sql.AppendLine("ELSE ''");
                    sql.AppendLine("END as msgsrm,");
                    sql.AppendLine("(SELECT wcs.fuc_datetimediff(o.stime::time without time zone, o.mtime::time without time zone) AS fuc_datetimediff) AS loadtime");
                    sql.AppendLine("FROM public.sap_batchorder o");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("AND (o.ctime >= @startdate and o.ctime < @stopdate)");
                    sql.AppendLine("ORDER BY o.stime asc, o.su_n");
                    sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", NpgsqlDbType.Timestamp, dtStop);
                    cmd.Parameters.AddWithValue("@limitrec", NpgsqlDbType.Bigint, limitrec);
                    cmd.Parameters.AddWithValue("@offsetrec", NpgsqlDbType.Bigint, (limitrec * currentPage) - limitrec);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_4_C objrd = new Class6_4_C
                        {

                            Order_No = rdr["order_no"].ToString(),
                            Ship_To_Code = rdr["ship_to_code"].ToString(),
                            Ship_Name = rdr["ship_name"].ToString(),
                            Item_Code = rdr["item_code"].ToString(),
                            Item_Name = rdr["item_name"].ToString(),
                            Batch_Number = rdr["batch_number"].ToString(),
                            Su_No = rdr["su_no"].ToString(),
                            Need_Qty = rdr["need_qty"] == DBNull.Value ? null : (decimal?)rdr["need_qty"],
                            Pallet_Qty = rdr["pallet_qty"] == DBNull.Value ? null : (decimal?)rdr["pallet_qty"],
                            State = rdr["state"] == DBNull.Value ? null : (int?)rdr["state"],
                            Palletcode = rdr["palletcode"].ToString(),
                            Ctime = rdr["ctime"] == DBNull.Value ? null : (DateTime?)rdr["ctime"],
                            Stime = rdr["stime"] == DBNull.Value ? null : (DateTime?)rdr["stime"],
                            Mtime = rdr["mtime"] == DBNull.Value ? null : (DateTime?)rdr["mtime"],
                            Msgsrm = rdr["msgsrm"].ToString(),
                            Loadtime = rdr["loadtime"] == DBNull.Value ? null : (DateTime?)rdr["loadtime"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }
        #endregion

        #region "MENU6.4D"
        public IEnumerable<Class6_4_D> GetAllMenu6_4D()
        {
            List<Class6_4_D> lstobj = new List<Class6_4_D>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT ");
                    sql.AppendLine("t1.batch_no, t1.delivery_pio, t1.work_pio, t1.ctime::Date AS cdate, pp.stdesc, t1.count_do, t1.state ,");
                    sql.AppendLine("(select count(idx) from public.sap_batchorder where state in (1) and batch_no=t1.batch_no) AS cQue ,");
                    sql.AppendLine("(select count(idx) from public.sap_batchorder where state in (2) and batch_no=t1.batch_no) AS cInp ,");
                    sql.AppendLine("(select count(idx) from public.sap_batchorder where state=3 and batch_no=t1.batch_no) AS cCom ,");
                    sql.AppendLine("t1.ctime::timestamp (0) as ctime ,t1.stime::timestamp (0) as stime ,t1.mtime::timestamp (0) as mtime ,");
                    sql.AppendLine("( SELECT wcs.fuc_datetimediff(t1.stime::time without time zone, t1.mtime::time without time zone) AS fuc_datetimediff) AS loadtime ");
                    sql.AppendLine("FROM public.sap_batchsetting t1");
                    sql.AppendLine("LEFT JOIN public.set_workstation pp ");
                    sql.AppendLine("ON t1.pick_station=pp.idx ");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("AND t1.state in ( 99,3,100 )");
                    //sql.AppendLine("and (t1.ctime>='" & dtpStart.ToString("s") & "' and t1.ctime<='" & dtpStop.ToString("s") & "')");
                    sql.AppendLine("GROUP BY t1.batch_no, t1.delivery_pio, t1.work_pio, t1.ctime::Date, t1.count_do, t1.state, pp.stdesc");
                    sql.AppendLine("ORDER BY t1.stime, t1.ctime::date asc, t1.delivery_pio desc, t1.batch_no asc ");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_4_D objrd = new Class6_4_D
                        {
                            Batch_No = rdr["batch_no"].ToString(),
                            Delivery_Pio = rdr["delivery_pio"] == DBNull.Value ? null : (short?)rdr["delivery_pio"],
                            Work_Pio = rdr["work_pio"] == DBNull.Value ? null : (short?)rdr["work_pio"],
                            Cdate = rdr["cdate"] == DBNull.Value ? null : (DateTime?)rdr["cdate"],
                            Stdesc = rdr["ship_to_code"].ToString(),
                            Count_Do = rdr["count_do"] == DBNull.Value ? null : (int?)rdr["count_do"],
                            State = rdr["state"] == DBNull.Value ? null : (short?)rdr["state"],
                            Cque = rdr["cQue"] == DBNull.Value ? null : (long?)rdr["cQue"],
                            Cinp = rdr["cInp"] == DBNull.Value ? null : (long?)rdr["cInp"],
                            Ccom = rdr["cCom"] == DBNull.Value ? null : (long?)rdr["cCom"],
                            Ctime = rdr["ctime"] == DBNull.Value ? null : (DateTime?)rdr["ctime"],
                            Stime = rdr["stime"] == DBNull.Value ? null : (DateTime?)rdr["stime"],
                            Mtime = rdr["mtime"] == DBNull.Value ? null : (DateTime?)rdr["mtime"],
                            Loadtime = rdr["loadtime"] == DBNull.Value ? null : (DateTime?)rdr["loadtime"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_4_D> GetAllMenu6_4DbyDate(DateTime dtStart, DateTime dtStop)
        {
            List<Class6_4_D> lstobj = new List<Class6_4_D>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT ");
                    sql.AppendLine("t1.batch_no, t1.delivery_pio, t1.work_pio, t1.ctime::Date AS cdate, pp.stdesc, t1.count_do, t1.state ,");
                    sql.AppendLine("(select count(idx) from public.sap_batchorder where state in (1) and batch_no=t1.batch_no) AS cQue ,");
                    sql.AppendLine("(select count(idx) from public.sap_batchorder where state in (2) and batch_no=t1.batch_no) AS cInp ,");
                    sql.AppendLine("(select count(idx) from public.sap_batchorder where state=3 and batch_no=t1.batch_no) AS cCom ,");
                    sql.AppendLine("t1.ctime::timestamp (0) as ctime ,t1.stime::timestamp (0) as stime ,t1.mtime::timestamp (0) as mtime ,");
                    sql.AppendLine("( SELECT wcs.fuc_datetimediff(t1.stime::time without time zone, t1.mtime::time without time zone) AS fuc_datetimediff) AS loadtime ");
                    sql.AppendLine("FROM public.sap_batchsetting t1");
                    sql.AppendLine("LEFT JOIN public.set_workstation pp ");
                    sql.AppendLine("ON t1.pick_station=pp.idx ");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("AND t1.state in ( 99,3,100 )");
                    sql.AppendLine("AND (t1.ctime >= @startdate AND t1.ctime < @stopdate)");
                    sql.AppendLine("GROUP BY t1.batch_no, t1.delivery_pio, t1.work_pio, t1.ctime::Date, t1.count_do, t1.state, pp.stdesc");
                    sql.AppendLine("ORDER BY t1.stime, t1.ctime::date asc, t1.delivery_pio desc, t1.batch_no asc ");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", NpgsqlDbType.Timestamp, dtStop);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_4_D objrd = new Class6_4_D
                        {
                            Batch_No = rdr["batch_no"].ToString(),
                            Delivery_Pio = rdr["delivery_pio"] == DBNull.Value ? null : (short?)rdr["delivery_pio"],
                            Work_Pio = rdr["work_pio"] == DBNull.Value ? null : (short?)rdr["work_pio"],
                            Cdate = rdr["cdate"] == DBNull.Value ? null : (DateTime?)rdr["cdate"],
                            Stdesc = rdr["ship_to_code"].ToString(),
                            Count_Do = rdr["count_do"] == DBNull.Value ? null : (int?)rdr["count_do"],
                            State = rdr["state"] == DBNull.Value ? null : (short?)rdr["state"],
                            Cque = rdr["cQue"] == DBNull.Value ? null : (long?)rdr["cQue"],
                            Cinp = rdr["cInp"] == DBNull.Value ? null : (long?)rdr["cInp"],
                            Ccom = rdr["cCom"] == DBNull.Value ? null : (long?)rdr["cCom"],
                            Ctime = rdr["ctime"] == DBNull.Value ? null : (DateTime?)rdr["ctime"],
                            Stime = rdr["stime"] == DBNull.Value ? null : (DateTime?)rdr["stime"],
                            Mtime = rdr["mtime"] == DBNull.Value ? null : (DateTime?)rdr["mtime"],
                            Loadtime = rdr["loadtime"] == DBNull.Value ? null : (DateTime?)rdr["loadtime"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_4_D> GetAllMenu6_4DbyDatelimit(DateTime dtStart, DateTime dtStop, long limitrec, long currentPage)
        {
            List<Class6_4_D> lstobj = new List<Class6_4_D>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT ");
                    sql.AppendLine("t1.batch_no, t1.delivery_pio, t1.work_pio, t1.ctime::Date AS cdate, pp.stdesc, t1.count_do, t1.state ,");
                    sql.AppendLine("(select count(idx) from public.sap_batchorder where state in (1) and batch_no=t1.batch_no) AS cQue ,");
                    sql.AppendLine("(select count(idx) from public.sap_batchorder where state in (2) and batch_no=t1.batch_no) AS cInp ,");
                    sql.AppendLine("(select count(idx) from public.sap_batchorder where state=3 and batch_no=t1.batch_no) AS cCom ,");
                    sql.AppendLine("t1.ctime::timestamp (0) as ctime ,t1.stime::timestamp (0) as stime ,t1.mtime::timestamp (0) as mtime ,");
                    sql.AppendLine("( SELECT wcs.fuc_datetimediff(t1.stime::time without time zone, t1.mtime::time without time zone) AS fuc_datetimediff) AS loadtime ");
                    sql.AppendLine("FROM public.sap_batchsetting t1");
                    sql.AppendLine("LEFT JOIN public.set_workstation pp ");
                    sql.AppendLine("ON t1.pick_station=pp.idx ");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("AND t1.state in ( 99,3,100 )");
                    sql.AppendLine("AND (t1.ctime >= @startdate AND t1.ctime < @stopdate)");
                    sql.AppendLine("GROUP BY t1.batch_no, t1.delivery_pio, t1.work_pio, t1.ctime::Date, t1.count_do, t1.state, pp.stdesc");
                    sql.AppendLine("ORDER BY t1.stime, t1.ctime::date asc, t1.delivery_pio desc, t1.batch_no asc ");
                    sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", NpgsqlDbType.Timestamp, dtStop);
                    cmd.Parameters.AddWithValue("@limitrec", NpgsqlDbType.Bigint, limitrec);
                    cmd.Parameters.AddWithValue("@offsetrec", NpgsqlDbType.Bigint, (limitrec * currentPage) - limitrec);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_4_D objrd = new Class6_4_D
                        {
                            Batch_No = rdr["batch_no"].ToString(),
                            Delivery_Pio = rdr["delivery_pio"] == DBNull.Value ? null : (short?)rdr["delivery_pio"],
                            Work_Pio = rdr["work_pio"] == DBNull.Value ? null : (short?)rdr["work_pio"],
                            Cdate = rdr["cdate"] == DBNull.Value ? null : (DateTime?)rdr["cdate"],
                            Stdesc = rdr["ship_to_code"].ToString(),
                            Count_Do = rdr["count_do"] == DBNull.Value ? null : (int?)rdr["count_do"],
                            State = rdr["state"] == DBNull.Value ? null : (short?)rdr["state"],
                            Cque = rdr["cQue"] == DBNull.Value ? null : (long?)rdr["cQue"],
                            Cinp = rdr["cInp"] == DBNull.Value ? null : (long?)rdr["cInp"],
                            Ccom = rdr["cCom"] == DBNull.Value ? null : (long?)rdr["cCom"],
                            Ctime = rdr["ctime"] == DBNull.Value ? null : (DateTime?)rdr["ctime"],
                            Stime = rdr["stime"] == DBNull.Value ? null : (DateTime?)rdr["stime"],
                            Mtime = rdr["mtime"] == DBNull.Value ? null : (DateTime?)rdr["mtime"],
                            Loadtime = rdr["loadtime"] == DBNull.Value ? null : (DateTime?)rdr["loadtime"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }
        #endregion

        #region "MENU6.5A"
        public IEnumerable<Class6_5_A> GetAllMenu6_5A()
        {
            List<Class6_5_A> lstobj = new List<Class6_5_A>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT  idx, created, entity_lock, modified, client_id, client_ip,");
                    sql.AppendLine("mccode, st_no, desc_th, desc_en, remark,");
                    sql.AppendLine("CASE");
                    sql.AppendLine("WHEN is_alert=true THEN 4");
                    sql.AppendLine("ELSE 0 ");
                    sql.AppendLine("END AS status");
                    sql.AppendLine("FROM wcs.vrpt_mcaudittrail ");
                    //sql.AppendLine("where (created>='" & dtpStart.ToString("s") & "' and created<='" & dtpStop.ToString("s") & "')");
                    sql.AppendLine("ORDER BY idx asc");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");


                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_5_A objrd = new Class6_5_A
                        {
                            Idx = rdr["idx"] == DBNull.Value ? null : (long?)rdr["idx"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Entity_lock = rdr["entity_lock"] == DBNull.Value ? null : (int?)rdr["entity_lock"],
                            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                            Client_Id = rdr["client_id"] == DBNull.Value ? null : (long?)rdr["client_id"],
                            Client_Ip = rdr["client_ip"].ToString(),
                            Mccode = rdr["mccode"].ToString(),
                            St_No = rdr["st_no"] == DBNull.Value ? null : (int?)rdr["st_no"],
                            Desc_Th = rdr["desc_th"].ToString(),
                            Desc_En = rdr["desc_en"].ToString(),
                            Remark = rdr["remark"].ToString(),
                            Status = rdr["status"] == DBNull.Value ? null : (int?)rdr["status"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_5_A> GetAllMenu6_5AbyDate(DateTime dtStart, DateTime dtStop)
        {
            List<Class6_5_A> lstobj = new List<Class6_5_A>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT  idx, created, entity_lock, modified, client_id, client_ip,");
                    sql.AppendLine("mccode, st_no, desc_th, desc_en, remark,");
                    sql.AppendLine("IIF(is_alert=1, 4, 0) AS status");

                    //sql.AppendLine("CASE");
                    //sql.AppendLine("WHEN is_alert=true THEN 4");
                    //sql.AppendLine("ELSE 0 ");
                    //sql.AppendLine("END AS status");
                    sql.AppendLine("FROM wcs.vrpt_mcaudittrail ");
                    sql.AppendLine("WHERE (created >= @startdate AND created < @stopdate)");
                    sql.AppendLine("ORDER BY idx asc");
                   
                    sql.AppendLine(";");


                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate",  dtStart);
                    cmd.Parameters.AddWithValue("@stopdate",  dtStop);
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_5_A objrd = new Class6_5_A
                        {
                            Idx = rdr["idx"] == DBNull.Value ? null : (long?)rdr["idx"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Entity_lock = rdr["entity_lock"] == DBNull.Value ? null : (int?)rdr["entity_lock"],
                            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                            Client_Id = rdr["client_id"] == DBNull.Value ? null : (long?)rdr["client_id"],
                            Client_Ip = rdr["client_ip"].ToString(),
                            Mccode = rdr["mccode"].ToString(),
                            St_No = rdr["st_no"] == DBNull.Value ? null : (int?)rdr["st_no"],
                            Desc_Th = rdr["desc_th"].ToString(),
                            Desc_En = rdr["desc_en"].ToString(),
                            Remark = rdr["remark"].ToString(),
                            Status = rdr["status"] == DBNull.Value ? null : (int?)rdr["status"]
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (SqlException ex)
                {
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return lstobj;
        }

        public IEnumerable<Class6_5_A> GetAllMenu6_5AbyDatelimit(DateTime dtStart, DateTime dtStop, long limitrec, long currentPage)
        {
            List<Class6_5_A> lstobj = new List<Class6_5_A>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT  idx, created, entity_lock, modified, client_id, client_ip,");
                    sql.AppendLine("mccode, st_no, desc_th, desc_en, remark,");
                    sql.AppendLine("CASE");
                    sql.AppendLine("WHEN is_alert=true THEN 4");
                    sql.AppendLine("ELSE 0 ");
                    sql.AppendLine("END AS status");
                    sql.AppendLine("FROM wcs.vrpt_mcaudittrail ");
                    sql.AppendLine("WHERE (created >= @startdate AND created < @stopdate)");
                    sql.AppendLine("ORDER BY idx asc");
                    sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                    sql.AppendLine(";");


                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", NpgsqlDbType.Timestamp, dtStop);
                    cmd.Parameters.AddWithValue("@limitrec", NpgsqlDbType.Bigint, limitrec);
                    cmd.Parameters.AddWithValue("@offsetrec", NpgsqlDbType.Bigint, (limitrec * currentPage) - limitrec);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_5_A objrd = new Class6_5_A
                        {
                            Idx = rdr["idx"] == DBNull.Value ? null : (long?)rdr["idx"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Entity_lock = rdr["entity_lock"] == DBNull.Value ? null : (int?)rdr["entity_lock"],
                            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                            Client_Id = rdr["client_id"] == DBNull.Value ? null : (long?)rdr["client_id"],
                            Client_Ip = rdr["client_ip"].ToString(),
                            Mccode = rdr["mccode"].ToString(),
                            St_No = rdr["st_no"] == DBNull.Value ? null : (int?)rdr["st_no"],
                            Desc_Th = rdr["desc_th"].ToString(),
                            Desc_En = rdr["desc_en"].ToString(),
                            Remark = rdr["remark"].ToString(),
                            Status = rdr["status"] == DBNull.Value ? null : (int?)rdr["status"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }
        #endregion

        #region"MENU6.5B"
        public IEnumerable<Class6_5_B> GetAllMenu6_5B()
        {
            List<Class6_5_B> lstobj = new List<Class6_5_B>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.AppendLine("mccode, st_no, ");
                    sql.AppendLine("desc_th, desc_en,");
                    sql.AppendLine("is_alert, count(idx) AS cunt,");
                    sql.AppendLine("created::date AS created ");
                    sql.AppendLine("FROM wcs.vrpt_mcaudittrail ");
                    sql.AppendLine("WHERE is_alert=true");
                    //sql.AppendLine("and (created >='" & dtpStart.ToString("s") & "' and created < '" & dtpStop.ToString("s") & "')");
                    sql.AppendLine("GROUP BY mccode, st_no, desc_th, desc_en, remark, is_alert, created::date ");
                    sql.AppendLine("ORDER BY mccode asc , created asc");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");


                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_5_B objrd = new Class6_5_B
                        {
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Mccode = rdr["mccode"].ToString(),
                            St_No = rdr["st_no"] == DBNull.Value ? null : (int?)rdr["st_no"],
                            Desc_Th = rdr["desc_th"].ToString(),
                            Desc_En = rdr["desc_en"].ToString(),
                            Is_Alert = (bool)rdr["is_alert"],
                            Cunt = rdr["cunt"] == DBNull.Value ? null : (Int32?)rdr["cunt"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_5_B> GetAllMenu6_5BbyDate(DateTime dtStart, DateTime dtStop)
        {
            List<Class6_5_B> lstobj = new List<Class6_5_B>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.AppendLine("mccode, st_no, ");
                    sql.AppendLine("desc_th, desc_en,");
                    sql.AppendLine("is_alert, count(idx) AS cunt,");
                    sql.AppendLine("TRY_CONVERT(DATE,created) AS created ");
                    sql.AppendLine("FROM wcs.vrpt_mcaudittrail ");
                    sql.AppendLine("WHERE is_alert = @is_alert");
                    sql.AppendLine("AND (created >= @startdate AND created < @stopdate)");
                    sql.AppendLine("GROUP BY mccode, st_no, desc_th, desc_en, remark, is_alert, TRY_CONVERT(DATE,created) ");
                    sql.AppendLine("ORDER BY mccode asc , created asc");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");


                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@is_alert", 1);
                    cmd.Parameters.AddWithValue("@startdate", dtStart);
                    cmd.Parameters.AddWithValue("@stopdate",  dtStop);
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_5_B objrd = new Class6_5_B
                        {
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Mccode = rdr["mccode"].ToString(),
                            St_No = rdr["st_no"] == DBNull.Value ? null : (int?)rdr["st_no"],
                            Desc_Th = rdr["desc_th"].ToString(),
                            Desc_En = rdr["desc_en"].ToString(),
                            Is_Alert = (bool)rdr["is_alert"],
                            Cunt = rdr["cunt"] == DBNull.Value ? null : (Int32?)rdr["cunt"]
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (SqlException ex)
                {
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return lstobj;
        }

        public IEnumerable<Class6_5_B> GetAllMenu6_5BbyDatelimit(DateTime dtStart, DateTime dtStop, long limitrec, long currentPage)
        {
            List<Class6_5_B> lstobj = new List<Class6_5_B>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.AppendLine("mccode, st_no, ");
                    sql.AppendLine("desc_th, desc_en,");
                    sql.AppendLine("is_alert, count(idx) AS cunt,");
                    sql.AppendLine("created::date AS created ");
                    sql.AppendLine("FROM wcs.vrpt_mcaudittrail ");
                    sql.AppendLine("WHERE is_alert = @is_alert");
                    sql.AppendLine("AND (created >= @startdate AND created < @stopdate)");
                    sql.AppendLine("GROUP BY mccode, st_no, desc_th, desc_en, remark, is_alert, created::date ");
                    sql.AppendLine("ORDER BY mccode asc , created asc");
                    sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@is_alert", 1);
                    cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", NpgsqlDbType.Timestamp, dtStop);
                    cmd.Parameters.AddWithValue("@limitrec", NpgsqlDbType.Bigint, limitrec);
                    cmd.Parameters.AddWithValue("@offsetrec", NpgsqlDbType.Bigint, (limitrec * currentPage) - limitrec);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_5_B objrd = new Class6_5_B
                        {
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Mccode = rdr["mccode"].ToString(),
                            St_No = rdr["st_no"] == DBNull.Value ? null : (int?)rdr["st_no"],
                            Desc_Th = rdr["desc_th"].ToString(),
                            Desc_En = rdr["desc_en"].ToString(),
                            Is_Alert = (bool)rdr["is_alert"],
                            Cunt = rdr["cunt"] == DBNull.Value ? null : (Int32?)rdr["cunt"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }
        #endregion

        #region"MENU6.6"
        public IEnumerable<Class6_6> GetAllMenu6_6BbyDate(DateTime dtStart, DateTime dtStop)
        {
            List<Class6_6> lstobj = new List<Class6_6>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.AppendLine("* ");
                    sql.AppendLine("FROM public.fuc_rpt_operationresult");
                    sql.AppendLine("(@startdate , @stopdate)");
                    sql.AppendLine(";");


                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", NpgsqlDbType.Timestamp, dtStop);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_6 objrd = new Class6_6
                        {
                            R_Itemcode = rdr["r_itemcode"].ToString(),
                            R_Itendesc = rdr["r_itemdesc"].ToString(),
                            R_Batchno = rdr["r_batchno"].ToString(),
                            R_Qty = rdr["r_qty"] == DBNull.Value ? null : (decimal?)rdr["r_qty"],
                            R_01 = rdr["r_01"] == DBNull.Value ? null : (decimal?)rdr["r_01"],
                            R_02 = rdr["r_02"] == DBNull.Value ? null : (decimal?)rdr["r_02"],
                            R_03 = rdr["r_03"] == DBNull.Value ? null : (decimal?)rdr["r_03"],
                            R_04 = rdr["r_04"] == DBNull.Value ? null : (decimal?)rdr["r_04"],
                            R_05 = rdr["r_05"] == DBNull.Value ? null : (decimal?)rdr["r_05"],
                            R_07 = rdr["r_07"] == DBNull.Value ? null : (decimal?)rdr["r_07"],
                            R_08 = rdr["r_08"] == DBNull.Value ? null : (decimal?)rdr["r_08"],
                            R_09 = rdr["r_09"] == DBNull.Value ? null : (decimal?)rdr["r_09"],
                            T_Total = rdr["t_total"] == DBNull.Value ? null : (decimal?)rdr["t_total"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_6> GetAllMenu6_6BbyDateitem(DateTime dtStart, DateTime dtStop, string item)
        {
            List<Class6_6> lstobj = new List<Class6_6>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.AppendLine("* ");
                    sql.AppendLine("FROM public.fuc_rpt_operationresult");
                    sql.AppendLine("(@startdate , @stopdate, @item)");
                    sql.AppendLine(";");


                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", NpgsqlDbType.Timestamp, dtStop);
                    cmd.Parameters.AddWithValue("@item", NpgsqlDbType.Varchar, item);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_6 objrd = new Class6_6
                        {
                            R_Itemcode = rdr["r_itemcode"].ToString(),
                            R_Itendesc = rdr["r_itemdesc"].ToString(),
                            R_Batchno = rdr["r_batchno"].ToString(),
                            R_Qty = rdr["r_qty"] == DBNull.Value ? null : (decimal?)rdr["r_qty"],
                            R_01 = rdr["r_01"] == DBNull.Value ? null : (decimal?)rdr["r_01"],
                            R_02 = rdr["r_02"] == DBNull.Value ? null : (decimal?)rdr["r_02"],
                            R_03 = rdr["r_03"] == DBNull.Value ? null : (decimal?)rdr["r_03"],
                            R_04 = rdr["r_04"] == DBNull.Value ? null : (decimal?)rdr["r_04"],
                            R_05 = rdr["r_05"] == DBNull.Value ? null : (decimal?)rdr["r_05"],
                            R_07 = rdr["r_07"] == DBNull.Value ? null : (decimal?)rdr["r_07"],
                            R_08 = rdr["r_08"] == DBNull.Value ? null : (decimal?)rdr["r_08"],
                            R_09 = rdr["r_09"] == DBNull.Value ? null : (decimal?)rdr["r_09"],
                            T_Total = rdr["t_total"] == DBNull.Value ? null : (decimal?)rdr["t_total"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }
        #endregion

        #region"MENU6.7A"
        public IEnumerable<Class6_7_A> GetAllMenu6_7A()
        {
            List<Class6_7_A> lstobj = new List<Class6_7_A>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.Append("date_trunc('hour', stime) AS w_hour,");
                    sql.Append("count(su_no) AS w_count ");
                    sql.AppendLine("FROM wcs.vrptqueueloadtime ");
                    sql.AppendLine("WHERE 1=1");
                    //sql.AppendLine("and (stime >='" & dtpStart.ToString("s") & "' and stime < '" & dtpStop.ToString("s") & "')");
                    sql.AppendLine("GROUP BY date_trunc('hour', stime) ");
                    sql.AppendLine("ORDER BY w_hour asc ");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");



                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_7_A objrd = new Class6_7_A
                        {
                            W_Hour = rdr["w_hour"] == DBNull.Value ? null : (DateTime?)rdr["w_hour"],
                            W_Count = rdr["w_count"] == DBNull.Value ? null : (Int32?)rdr["w_count"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_7_A> GetAllMenu6_7AbyDate(DateTime dtStart, DateTime dtStop)
        {
            List<Class6_7_A> lstobj = new List<Class6_7_A>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.Append("dateadd(hour, datediff(hour, 0, sretime), 0) as w_hour,");
                    sql.Append("count(su_no) AS w_count ");
                    sql.AppendLine("FROM dbo.vrpt_loadtime_inbound ");
                    sql.AppendLine("WHERE 1=1");
                    sql.AppendLine("AND (sretime >= @startdate AND sretime < @stopdate)");
                    sql.AppendLine("GROUP BY dateadd(hour, datediff(hour, 0, sretime), 0) ");
                    sql.AppendLine("ORDER BY w_hour asc ");
                    sql.AppendLine(";");
                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", dtStart);
                    cmd.Parameters.AddWithValue("@stopdate",  dtStop);
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_7_A objrd = new Class6_7_A
                        {
                            W_Hour = rdr["w_hour"] == DBNull.Value ? null : (DateTime?)rdr["w_hour"],
                            W_Count = rdr["w_count"] == DBNull.Value ? null : (Int32?)rdr["w_count"]
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (SqlException ex)
                {
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return lstobj;
        }

        public IEnumerable<Class6_7_A> GetAllMenu6_7AbyDatelimit(DateTime dtStart, DateTime dtStop, long limitrec, long currentPage)
        {
            List<Class6_7_A> lstobj = new List<Class6_7_A>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.Append("date_trunc('hour', stime) AS w_hour,");
                    sql.Append("count(su_no) AS w_count ");
                    sql.AppendLine("FROM wcs.vrptqueueloadtime ");
                    sql.AppendLine("WHERE 1=1");
                    sql.AppendLine("AND (stime >= @startdate AND stime < @stopdate)");
                    sql.AppendLine("GROUP BY date_trunc('hour', stime) ");
                    sql.AppendLine("ORDER BY w_hour asc ");
                    sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                    sql.AppendLine(";");
                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", NpgsqlDbType.Timestamp, dtStop);
                    cmd.Parameters.AddWithValue("@limitrec", NpgsqlDbType.Bigint, limitrec);
                    cmd.Parameters.AddWithValue("@offsetrec", NpgsqlDbType.Bigint, (limitrec * currentPage) - limitrec);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_7_A objrd = new Class6_7_A
                        {
                            W_Hour = rdr["w_hour"] == DBNull.Value ? null : (DateTime?)rdr["w_hour"],
                            W_Count = rdr["w_count"] == DBNull.Value ? null : (Int32?)rdr["w_count"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }
        #endregion

        #region"MENU6.7B"
        public IEnumerable<Class6_7_B> GetAllMenu6_7B()
        {
            List<Class6_7_B> lstobj = new List<Class6_7_B>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.Append("su_no, lpncode, work_code, srm_no, loc_no, rgv_from,  ");
                    sql.Append("rgv_to, wctime, srstime, sretime, rvloadtime, srloadtime, loadtime ");
                    sql.AppendLine("FROM public.vrpt_loadtime_inbound ");
                    sql.AppendLine("WHERE 1=1");
                    //sql.AppendLine("and (wctime >='" & dtpStart.ToString("s") & "' and srstime <= '" & dtpStop.ToString("s") & "')");
                    sql.AppendLine("ORDER BY wctime ASC ");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_7_B objrd = new Class6_7_B
                        {
                            Su_No = rdr["su_no"].ToString(),
                            Lpncode = rdr["lpncode"].ToString(),
                            Work_code = rdr["work_code"].ToString(),
                            Srm_No = rdr["srm_no"].ToString(),
                            Loc_No = rdr["loc_no"].ToString(),
                            Rgv_From = rdr["rgv_from"] == DBNull.Value ? null : (int?)rdr["rgv_from"],
                            Rgv_To = rdr["rgv_to"] == DBNull.Value ? null : (int?)rdr["rgv_to"],
                            Wctime = rdr["wctime"] == DBNull.Value ? null : (DateTime?)rdr["wctime"],
                            Srstime = rdr["srstime"] == DBNull.Value ? null : (DateTime?)rdr["srstime"],
                            Sretime = rdr["sretime"] == DBNull.Value ? null : (DateTime?)rdr["srstime"],
                            Rvloadtime = rdr["rvloadtime"].ToString(),
                            Srloadtime = rdr["srloadtime"].ToString(),
                            Loadtime = rdr["loadtime"].ToString()
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_7_B> GetAllMenu6_7BbyDate(DateTime dtStart, DateTime dtStop)
        {
            List<Class6_7_B> lstobj = new List<Class6_7_B>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.Append("su_no, lpncode, work_code, srm_no, loc_no, rgv_from,  ");
                    sql.Append("rgv_to, wctime, srstime, sretime, rvloadtime, srloadtime, loadtime ");
                    sql.AppendLine("FROM dbo.vrpt_loadtime_inbound ");
                    sql.AppendLine("WHERE 1=1");
                    sql.AppendLine("AND (wctime >= @startdate AND wctime < @stopdate)");
                    sql.AppendLine("ORDER BY wctime ASC ");

        

                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");

                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate",  dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", dtStop);
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_7_B objrd = new Class6_7_B
                        {
                            Su_No = rdr["su_no"].ToString(),
                            Lpncode = rdr["lpncode"].ToString(),
                            Work_code = rdr["work_code"].ToString(),
                            Srm_No = rdr["srm_no"].ToString(),
                            Loc_No = rdr["loc_no"].ToString(),
                            Rgv_From = rdr["rgv_from"] == DBNull.Value ? null : (int?)rdr["rgv_from"],
                            Rgv_To = rdr["rgv_to"] == DBNull.Value ? null : (int?)rdr["rgv_to"],
                            Wctime = rdr["wctime"] == DBNull.Value ? null : (DateTime?)rdr["wctime"],
                            Srstime = rdr["srstime"] == DBNull.Value ? null : (DateTime?)rdr["srstime"],
                            Sretime = rdr["sretime"] == DBNull.Value ? null : (DateTime?)rdr["srstime"],
                            Rvloadtime = rdr["rgvloadtime"].ToString(),
                            Srloadtime = rdr["srloadtime"].ToString(),
                            Loadtime = rdr["loadtime"].ToString()
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (SqlException ex)
                {
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return lstobj;
        }

        public IEnumerable<Class6_7_B> GetAllMenu6_7BbyDatelimit(DateTime dtStart, DateTime dtStop, long limitrec, long currentPage)
        {
            List<Class6_7_B> lstobj = new List<Class6_7_B>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.Append("su_no, lpncode, work_code, srm_no, loc_no, rgv_from,  ");
                    sql.Append("rgv_to, wctime, srstime, sretime, rvloadtime, srloadtime, loadtime ");
                    sql.AppendLine("FROM dbo.vrpt_loadtime_inbound ");
                    sql.AppendLine("WHERE 1=1");
                    sql.AppendLine("AND (wctime >= @startdate AND wctime < @stopdate)");
                    sql.AppendLine("ORDER BY wctime ASC ");
                    sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", NpgsqlDbType.Timestamp, dtStop);
                    cmd.Parameters.AddWithValue("@limitrec", NpgsqlDbType.Bigint, limitrec);
                    cmd.Parameters.AddWithValue("@offsetrec", NpgsqlDbType.Bigint, (limitrec * currentPage) - limitrec);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_7_B objrd = new Class6_7_B
                        {
                            Su_No = rdr["su_no"].ToString(),
                            Lpncode = rdr["lpncode"].ToString(),
                            Work_code = rdr["work_code"].ToString(),
                            Srm_No = rdr["srm_no"].ToString(),
                            Loc_No = rdr["loc_no"].ToString(),
                            Rgv_From = rdr["rgv_from"] == DBNull.Value ? null : (int?)rdr["rgv_from"],
                            Rgv_To = rdr["rgv_to"] == DBNull.Value ? null : (int?)rdr["rgv_to"],
                            Wctime = rdr["wctime"] == DBNull.Value ? null : (DateTime?)rdr["wctime"],
                            Srstime = rdr["srstime"] == DBNull.Value ? null : (DateTime?)rdr["srstime"],
                            Sretime = rdr["sretime"] == DBNull.Value ? null : (DateTime?)rdr["srstime"],
                            Rvloadtime = rdr["rgvloadtime"].ToString(),
                            Srloadtime = rdr["srloadtime"].ToString(),
                            Loadtime = rdr["loadtime"].ToString()
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }
        #endregion

        #region"MENU6.7C"
        public IEnumerable<Class6_7_C> GetAllMenu6_7C()
        {
            List<Class6_7_C> lstobj = new List<Class6_7_C>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.Append("date_trunc('hour', modified) AS w_hour,");
                    sql.Append("count(su_no) AS w_count ");
                    sql.AppendLine("FROM public.vrpt_loadtime_oubrgv ");
                    sql.AppendLine("WHERE 1=1");
                    //sql.AppendLine("and (modified >='" & dtpStart.ToString("s") & "' and modified < '" & dtpStop.ToString("s") & "')");
                    sql.AppendLine("GROUP BY date_trunc('hour', modified) ");
                    sql.AppendLine("ORDER BY w_hour asc ");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_7_C objrd = new Class6_7_C
                        {
                            W_Hour = rdr["w_hour"] == DBNull.Value ? null : (DateTime?)rdr["w_hour"],
                            W_Count = rdr["w_count"] == DBNull.Value ? null : (Int32?)rdr["w_count"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_7_C> GetAllMenu6_7CbyDate(DateTime dtStart, DateTime dtStop)
        {
            List<Class6_7_C> lstobj = new List<Class6_7_C>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();

                    //sql.AppendLine("SELECT");
                    //sql.Append("dateadd(hour, datediff(hour, 0, modified), 0) AS w_hour,");
                    //sql.Append("count(su_no) AS w_count ");
                    //sql.AppendLine("FROM dbo.vrpt_loadtime_outbound ");
                    //sql.AppendLine("WHERE 1=1");
                    //sql.AppendLine("AND (modified >= @startdate AND modified < @stopdate)");
                    //sql.AppendLine("GROUP BY dateadd(hour, datediff(hour, 0, modified), 0) ");
                    //sql.AppendLine("ORDER BY w_hour ASC ");
                    ////sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    //sql.AppendLine(";");


                    sql.AppendLine("SELECT");
                    sql.Append("dateadd(hour, datediff(hour, 0, rvetime), 0) as w_hour,");
                    sql.Append("count(su_no) AS w_count ");
                    sql.AppendLine("FROM dbo.vrpt_loadtime_outbound ");
                    sql.AppendLine("WHERE 1=1");
                    sql.AppendLine("AND (rvetime >= @startdate AND rvetime < @stopdate)");
                    sql.AppendLine("GROUP BY dateadd(hour, datediff(hour, 0, rvetime), 0) ");
                    sql.AppendLine("ORDER BY w_hour asc ");
                    sql.AppendLine(";");


                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", dtStop);
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_7_C objrd = new Class6_7_C
                        {
                            W_Hour = rdr["w_hour"] == DBNull.Value ? null : (DateTime?)rdr["w_hour"],
                            W_Count = rdr["w_count"] == DBNull.Value ? null : (Int32?)rdr["w_count"]
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (SqlException ex)
                {
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return lstobj;
        }

        public IEnumerable<Class6_7_C> GetAllMenu6_7CbyDatelimit(DateTime dtStart, DateTime dtStop, long limitrec, long currentPage)
        {
            List<Class6_7_C> lstobj = new List<Class6_7_C>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.Append("date_trunc('hour', modified) AS w_hour,");
                    sql.Append("count(su_no) AS w_count ");
                    sql.AppendLine("FROM public.vrpt_loadtime_oubrgv ");
                    sql.AppendLine("WHERE 1=1");
                    sql.AppendLine("AND (modified >= @startdate AND modified < @stopdate)");
                    sql.AppendLine("GROUP BY date_trunc('hour', modified) ");
                    sql.AppendLine("ORDER BY w_hour ASC ");
                    sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", NpgsqlDbType.Timestamp, dtStop);
                    cmd.Parameters.AddWithValue("@limitrec", NpgsqlDbType.Bigint, limitrec);
                    cmd.Parameters.AddWithValue("@offsetrec", NpgsqlDbType.Bigint, (limitrec * currentPage) - limitrec);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_7_C objrd = new Class6_7_C
                        {
                            W_Hour = rdr["w_hour"] == DBNull.Value ? null : (DateTime?)rdr["w_hour"],
                            W_Count = rdr["w_count"] == DBNull.Value ? null : (Int32?)rdr["w_count"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }
        #endregion

        #region"MENU6.7D"
        public IEnumerable<Class6_7_D> GetAllMenu6_7D()
        {
            List<Class6_7_D> lstobj = new List<Class6_7_D>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.AppendLine("batch_no, batchtime, work_priority, su_no, lpn_no, work_type,  srm_no, loc_no, ");
                    sql.AppendLine("gate_out, srstime, sretime, rvetime, ");
                    sql.Append("( SELECT wcs.fuc_datetimediff(srstime::time without time zone, sretime::time without time zone) AS fuc_datetimediff) AS srloadtime,");
                    sql.AppendLine("( SELECT wcs.fuc_datetimediff(rvstime::time without time zone, rvetime::time without time zone) AS fuc_datetimediff) AS rvloadtime,");
                    sql.AppendLine("( SELECT wcs.fuc_datetimediff(batchtime::time without time zone, rvetime::time without time zone) AS fuc_datetimediff) AS loadtime ");
                    sql.AppendLine("FROM public.vrpt_loadtime_outbound ");
                    sql.AppendLine("WHERE 1=1");
                    //sql.AppendLine("and (rvetime >='" & dtpStart.ToString("s") & "' and rvetime <= '" & dtpStop.ToString("s") & "')");
                    sql.AppendLine("ORDER BY srstime ASC ");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");


                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_7_D objrd = new Class6_7_D
                        {
                            Batch_No = rdr["batch_no"].ToString(),
                            Batchtime = rdr["batchtime"] == DBNull.Value ? null : (DateTime?)rdr["batchtime"],
                            WorkPrioity = rdr["work_priority"] == DBNull.Value ? null : (int?)rdr["work_priority"],
                            Su_No = rdr["su_no"].ToString(),
                            Lpn_No = rdr["lpn_no"].ToString(),
                            Work_Type = rdr["work_type"].ToString(),
                            Srm_No = rdr["srm_no"].ToString(),
                            Loc_No = rdr["loc_no"].ToString(),
                            Gate_Out = rdr["gate_out"] == DBNull.Value ? null : (int?)rdr["gate_out"],
                            Srstime = rdr["srstime"] == DBNull.Value ? null : (DateTime?)rdr["srstime"],
                            Sretime = rdr["sretime"] == DBNull.Value ? null : (DateTime?)rdr["srstime"],
                            Rvetime = rdr["rvetime"] == DBNull.Value ? null : (DateTime?)rdr["rvetime"],
                            Srloadtime = rdr["srloadtime"].ToString(),
                            Rvloadtime = rdr["rvloadtime"].ToString(),
                            Loadtime = rdr["loadtime"].ToString()
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_7_D> GetAllMenu6_7DbyDate(DateTime dtStart, DateTime dtStop)
        {
            List<Class6_7_D> lstobj = new List<Class6_7_D>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.AppendLine("batch_no, batchtime, work_priority, su_no, lpn_no, work_type,  srm_no, loc_no, ");
                    sql.AppendLine("gate_out, srstime, sretime, rvetime, ");
                    sql.Append("( SELECT wcs.fuc_datetimediff(srstime::time without time zone, sretime::time without time zone) AS fuc_datetimediff) AS srloadtime,");
                    sql.AppendLine("( SELECT wcs.fuc_datetimediff(rvstime::time without time zone, rvetime::time without time zone) AS fuc_datetimediff) AS rvloadtime,");
                    sql.AppendLine("( SELECT wcs.fuc_datetimediff(batchtime::time without time zone, rvetime::time without time zone) AS fuc_datetimediff) AS loadtime ");
                    sql.AppendLine("FROM public.vrpt_loadtime_outbound ");
                    sql.AppendLine("WHERE 1=1");
                    sql.AppendLine("AND (rvetime >= @startdate AND rvetime < @stopdate)");
                    sql.AppendLine("ORDER BY srstime ASC ");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");


                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", NpgsqlDbType.Timestamp, dtStop);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_7_D objrd = new Class6_7_D
                        {
                            Batch_No = rdr["batch_no"].ToString(),
                            Batchtime = rdr["batchtime"] == DBNull.Value ? null : (DateTime?)rdr["batchtime"],
                            WorkPrioity = rdr["workprioity"] == DBNull.Value ? null : (int?)rdr["workprioity"],
                            Su_No = rdr["su_no"].ToString(),
                            Lpn_No = rdr["lpn_no"].ToString(),
                            Work_Type = rdr["work_type"].ToString(),
                            Srm_No = rdr["srm_no"].ToString(),
                            Loc_No = rdr["loc_no"].ToString(),
                            Gate_Out = rdr["gete_out"] == DBNull.Value ? null : (int?)rdr["gete_out"],
                            Srstime = rdr["srstime"] == DBNull.Value ? null : (DateTime?)rdr["srstime"],
                            Sretime = rdr["sretime"] == DBNull.Value ? null : (DateTime?)rdr["srstime"],
                            Rvetime = rdr["rvetime"] == DBNull.Value ? null : (DateTime?)rdr["rvetime"],
                            Srloadtime = rdr["srloadtime"].ToString(),
                            Rvloadtime = rdr["rgvloadtime"].ToString(),
                            Loadtime = rdr["loadtime"].ToString()
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_7_D> GetAllMenu6_7DbyDatelimit(DateTime dtStart, DateTime dtStop, long limitrec, long currentPage)
        {
            List<Class6_7_D> lstobj = new List<Class6_7_D>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.AppendLine("batch_no, batchtime, work_priority, su_no, lpn_no, work_type,  srm_no, loc_no, ");
                    sql.AppendLine("gate_out, srstime, sretime, rvetime, ");
                    sql.Append("( SELECT wcs.fuc_datetimediff(srstime::time without time zone, sretime::time without time zone) AS fuc_datetimediff) AS srloadtime,");
                    sql.AppendLine("( SELECT wcs.fuc_datetimediff(rvstime::time without time zone, rvetime::time without time zone) AS fuc_datetimediff) AS rvloadtime,");
                    sql.AppendLine("( SELECT wcs.fuc_datetimediff(batchtime::time without time zone, rvetime::time without time zone) AS fuc_datetimediff) AS loadtime ");
                    sql.AppendLine("FROM public.vrpt_loadtime_outbound ");
                    sql.AppendLine("WHERE 1=1");
                    sql.AppendLine("AND (rvetime >= @startdate AND rvetime < @stopdate)");
                    sql.AppendLine("ORDER BY srstime ASC ");
                    sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", NpgsqlDbType.Timestamp, dtStop);
                    cmd.Parameters.AddWithValue("@limitrec", NpgsqlDbType.Bigint, limitrec);
                    cmd.Parameters.AddWithValue("@offsetrec", NpgsqlDbType.Bigint, (limitrec * currentPage) - limitrec);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_7_D objrd = new Class6_7_D
                        {
                            Batch_No = rdr["batch_no"].ToString(),
                            Batchtime = rdr["batchtime"] == DBNull.Value ? null : (DateTime?)rdr["batchtime"],
                            WorkPrioity = rdr["workprioity"] == DBNull.Value ? null : (int?)rdr["workprioity"],
                            Su_No = rdr["su_no"].ToString(),
                            Lpn_No = rdr["lpn_no"].ToString(),
                            Work_Type = rdr["work_type"].ToString(),
                            Srm_No = rdr["srm_no"].ToString(),
                            Loc_No = rdr["loc_no"].ToString(),
                            Gate_Out = rdr["gete_out"] == DBNull.Value ? null : (int?)rdr["gete_out"],
                            Srstime = rdr["srstime"] == DBNull.Value ? null : (DateTime?)rdr["srstime"],
                            Sretime = rdr["sretime"] == DBNull.Value ? null : (DateTime?)rdr["srstime"],
                            Rvetime = rdr["rvetime"] == DBNull.Value ? null : (DateTime?)rdr["rvetime"],
                            Srloadtime = rdr["srloadtime"].ToString(),
                            Rvloadtime = rdr["rgvloadtime"].ToString(),
                            Loadtime = rdr["loadtime"].ToString()
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        #endregion

        #region"MENU6.7E"
        public IEnumerable<Class6_7_E> GetAllMenu6_7E()
        {
            List<Class6_7_E> lstobj = new List<Class6_7_E>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.AppendLine("idx, created, entity_lock, modified, client_id, client_ip, worktype,");
                    sql.AppendLine(" pono, orderno, su_no, item_code, item_name, movement_type, movement_reason,");
                    sql.AppendLine("tblflag");
                    sql.AppendLine("FROM public.vrpt_workinprocess");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("ORDER BY created asc, modified asc");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");


                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_7_E objrd = new Class6_7_E
                        {
                            Idx = rdr["idx"] == DBNull.Value ? null : (long?)rdr["idx"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Entity_Lock = rdr["entity_lock"] == DBNull.Value ? null : (int?)rdr["entity_lock"],
                            Modified = rdr["rvetime"] == DBNull.Value ? null : (DateTime?)rdr["rvetime"],
                            Client_Id = rdr["client_id"] == DBNull.Value ? null : (long?)rdr["client_id"],
                            Client_Ip = rdr["client_ip"].ToString(),
                            Worktype = rdr["worktype"].ToString(),
                            Pono = rdr["pono"].ToString(),
                            Orderno = rdr["orderno"].ToString(),
                            Su_No = rdr["su_no"].ToString(),
                            Item_Code = rdr["item_code"].ToString(),
                            Item_Name = rdr["item_name"].ToString(),
                            Movement_Type = rdr["movement_type"].ToString(),
                            Movement_Reason = rdr["movement_reason"].ToString(),
                            Tblflag = rdr["tblflag"].ToString()
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_7_E> GetAllMenu6_7EbyDate(DateTime dtStart, DateTime dtStop)
        {
            List<Class6_7_E> lstobj = new List<Class6_7_E>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.AppendLine("idx, created, entity_lock, modified, client_id, client_ip, worktype,");
                    sql.AppendLine(" pono, orderno, su_no, item_code, item_name, movement_type, movement_reason,");
                    sql.AppendLine("tblflag");
                    sql.AppendLine("FROM public.vrpt_workinprocess");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("AND (created >= @startdate AND created < @stopdate)");
                    sql.AppendLine("ORDER BY created asc, modified asc");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");


                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", NpgsqlDbType.Timestamp, dtStop);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_7_E objrd = new Class6_7_E
                        {
                            Idx = rdr["idx"] == DBNull.Value ? null : (long?)rdr["idx"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Entity_Lock = rdr["entity_lock"] == DBNull.Value ? null : (int?)rdr["entity_lock"],
                            Modified = rdr["rvetime"] == DBNull.Value ? null : (DateTime?)rdr["rvetime"],
                            Client_Id = rdr["client_id"] == DBNull.Value ? null : (long?)rdr["client_id"],
                            Client_Ip = rdr["client_ip"].ToString(),
                            Worktype = rdr["worktype"].ToString(),
                            Pono = rdr["pono"].ToString(),
                            Orderno = rdr["orderno"].ToString(),
                            Su_No = rdr["su_no"].ToString(),
                            Item_Code = rdr["item_code"].ToString(),
                            Item_Name = rdr["item_name"].ToString(),
                            Movement_Type = rdr["movement_type"].ToString(),
                            Movement_Reason = rdr["movement_reason"].ToString(),
                            Tblflag = rdr["tblflag"].ToString()
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_7_E> GetAllMenu6_7EbyDatelimit(DateTime dtStart, DateTime dtStop, long limitrec, long currentPage)
        {
            List<Class6_7_E> lstobj = new List<Class6_7_E>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.AppendLine("idx, created, entity_lock, modified, client_id, client_ip, worktype,");
                    sql.AppendLine(" pono, orderno, su_no, item_code, item_name, movement_type, movement_reason,");
                    sql.AppendLine("tblflag");
                    sql.AppendLine("FROM public.vrpt_workinprocess");
                    sql.AppendLine("WHERE (1=1)");
                    sql.AppendLine("AND (created >= @startdate AND created < @stopdate)");
                    sql.AppendLine("ORDER BY created asc, modified asc");
                    sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                    sql.AppendLine(";");


                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", NpgsqlDbType.Timestamp, dtStop);
                    cmd.Parameters.AddWithValue("@limitrec", NpgsqlDbType.Bigint, limitrec);
                    cmd.Parameters.AddWithValue("@offsetrec", NpgsqlDbType.Bigint, (limitrec * currentPage) - limitrec);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_7_E objrd = new Class6_7_E
                        {
                            Idx = rdr["idx"] == DBNull.Value ? null : (long?)rdr["idx"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Entity_Lock = rdr["entity_lock"] == DBNull.Value ? null : (int?)rdr["entity_lock"],
                            Modified = rdr["rvetime"] == DBNull.Value ? null : (DateTime?)rdr["rvetime"],
                            Client_Id = rdr["client_id"] == DBNull.Value ? null : (long?)rdr["client_id"],
                            Client_Ip = rdr["client_ip"].ToString(),
                            Worktype = rdr["worktype"].ToString(),
                            Pono = rdr["pono"].ToString(),
                            Orderno = rdr["orderno"].ToString(),
                            Su_No = rdr["su_no"].ToString(),
                            Item_Code = rdr["item_code"].ToString(),
                            Item_Name = rdr["item_name"].ToString(),
                            Movement_Type = rdr["movement_type"].ToString(),
                            Movement_Reason = rdr["movement_reason"].ToString(),
                            Tblflag = rdr["tblflag"].ToString()
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        #endregion

        #region"MENU6.7F"
        public IEnumerable<Class6_7_F> GetAllMenu6_7F()
        {
            List<Class6_7_F> lstobj = new List<Class6_7_F>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.Append("w_date, ");
                    sql.Append("COALESCE(NULLIF(w01  ,0),0) AS w01, ");
                    sql.Append("COALESCE(NULLIF(w07  ,0),0) AS w07, ");
                    sql.Append("COALESCE(NULLIF(w02  ,0),0) AS w02, ");
                    sql.Append("COALESCE(NULLIF(w03  ,0),0) AS w03, ");
                    sql.Append("COALESCE(NULLIF(w05  ,0),0) AS w05, ");
                    sql.Append("COALESCE(NULLIF(w08  ,0),0) AS w08, ");
                    sql.Append("COALESCE(NULLIF(w09  ,0),0) AS w09, ");
                    sql.Append("COALESCE(NULLIF(w101  ,0),0) AS w101, ");
                    sql.Append("COALESCE(NULLIF(w102  ,0),0) AS w102, ");
                    sql.Append("COALESCE(NULLIF(wtotal  ,0),0) AS wtotal ");
                    sql.AppendLine("FROM wcs.vrpt_workendofday ");
                    sql.AppendLine("WHERE 1=1");
                    //sql.AppendLine("and (w_date >='" & dtpStart.ToString("s") & "' and w_date < '" & dtpStop.ToString("s") & "')");
                    sql.AppendLine("ORDER BY w_date ASC ");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");


                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_7_F objrd = new Class6_7_F
                        {
                            W_date = rdr["w_date"] == DBNull.Value ? null : (DateTime?)rdr["w_date"],
                            W01 = rdr["w01"] == DBNull.Value ? null : (Int32?)rdr["w01"],
                            W02 = rdr["w02"] == DBNull.Value ? null : (Int32?)rdr["w02"],
                            W03 = rdr["w03"] == DBNull.Value ? null : (Int32?)rdr["w03"],
                            W05 = rdr["w05"] == DBNull.Value ? null : (Int32?)rdr["w05"],
                            W07 = rdr["W07"] == DBNull.Value ? null : (Int32?)rdr["W07"],
                            W09 = rdr["w09"] == DBNull.Value ? null : (Int32?)rdr["w09"],
                            W101 = rdr["w101"] == DBNull.Value ? null : (Int32?)rdr["w101"],
                            W102 = rdr["w102"] == DBNull.Value ? null : (Int32?)rdr["w102"],
                            Wtotal = rdr["wtotal"] == DBNull.Value ? null : (Int32?)rdr["wtotal"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        public IEnumerable<Class6_7_F> GetAllMenu6_7FbyDate(DateTime dtStart, DateTime dtStop)
        {
            List<Class6_7_F> lstobj = new List<Class6_7_F>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.Append("w_date, ");
                    sql.Append("COALESCE(NULLIF(w01  ,0),0) AS w01, ");
                    sql.Append("COALESCE(NULLIF(w07  ,0),0) AS w07, ");
                    sql.Append("COALESCE(NULLIF(w02  ,0),0) AS w02, ");
                    sql.Append("COALESCE(NULLIF(w03  ,0),0) AS w03, ");
                    sql.Append("COALESCE(NULLIF(w05  ,0),0) AS w05, ");
                    sql.Append("COALESCE(NULLIF(w08  ,0),0) AS w08, ");
                    sql.Append("COALESCE(NULLIF(w09  ,0),0) AS w09, ");
                    sql.Append("COALESCE(NULLIF(w101  ,0),0) AS w101, ");
                    sql.Append("COALESCE(NULLIF(w102  ,0),0) AS w102, ");
                    sql.Append("COALESCE(NULLIF(wtotal  ,0),0) AS wtotal ");
                    sql.AppendLine("FROM wcs.vrpt_workendofday ");
                    sql.AppendLine("WHERE 1=1");
                    sql.AppendLine("AND (w_date >= @startdate AND w_date < @stopdate)");
                    //sql.AppendLine("and (w_date >='" & dtpStart.ToString("s") & "' and w_date < '" & dtpStop.ToString("s") & "')");
                    sql.AppendLine("ORDER BY w_date ASC ");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");


                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", dtStop);
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_7_F objrd = new Class6_7_F
                        {
                            W_date = rdr["w_date"] == DBNull.Value ? null : (DateTime?)rdr["w_date"],
                            W01 = rdr["w01"] == DBNull.Value ? null : (Int32?)rdr["w01"],
                            W02 = rdr["w02"] == DBNull.Value ? null : (Int32?)rdr["w02"],
                            W03 = rdr["w03"] == DBNull.Value ? null : (Int32?)rdr["w03"],
                            W05 = rdr["w05"] == DBNull.Value ? null : (Int32?)rdr["w05"],
                            W07 = rdr["W07"] == DBNull.Value ? null : (Int32?)rdr["W07"],
                            W09 = rdr["w09"] == DBNull.Value ? null : (Int32?)rdr["w09"],
                            W101 = rdr["w101"] == DBNull.Value ? null : (Int32?)rdr["w101"],
                            W102 = rdr["w102"] == DBNull.Value ? null : (Int32?)rdr["w102"],
                            Wtotal = rdr["wtotal"] == DBNull.Value ? null : (Int32?)rdr["wtotal"]
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (SqlException ex)
                {
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return lstobj;
        }

        public IEnumerable<Class6_7_F> GetAllMenu6_7FbyDatelimit(DateTime dtStart, DateTime dtStop, long limitrec, long currentPage)
        {
            List<Class6_7_F> lstobj = new List<Class6_7_F>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.Append("w_date, ");
                    sql.Append("COALESCE(NULLIF(w01  ,0),0) AS w01, ");
                    sql.Append("COALESCE(NULLIF(w07  ,0),0) AS w07, ");
                    sql.Append("COALESCE(NULLIF(w02  ,0),0) AS w02, ");
                    sql.Append("COALESCE(NULLIF(w03  ,0),0) AS w03, ");
                    sql.Append("COALESCE(NULLIF(w05  ,0),0) AS w05, ");
                    sql.Append("COALESCE(NULLIF(w08  ,0),0) AS w08, ");
                    sql.Append("COALESCE(NULLIF(w09  ,0),0) AS w09, ");
                    sql.Append("COALESCE(NULLIF(w101  ,0),0) AS w101, ");
                    sql.Append("COALESCE(NULLIF(w102  ,0),0) AS w102, ");
                    sql.Append("COALESCE(NULLIF(wtotal  ,0),0) AS wtotal ");
                    sql.AppendLine("FROM wcs.vrpt_workendofday ");
                    sql.AppendLine("WHERE 1=1");
                    sql.AppendLine("AND (w_date >= @startdate AND w_date < @stopdate)");
                    sql.AppendLine("ORDER BY w_date ASC ");
                    sql.AppendLine("LIMIT @limitrec  OFFSET @offsetrec");
                    sql.AppendLine(";");


                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    cmd.Parameters.AddWithValue("@startdate", NpgsqlDbType.Timestamp, dtStart);
                    cmd.Parameters.AddWithValue("@stopdate", NpgsqlDbType.Timestamp, dtStop);
                    cmd.Parameters.AddWithValue("@limitrec", NpgsqlDbType.Bigint, limitrec);
                    cmd.Parameters.AddWithValue("@offsetrec", NpgsqlDbType.Bigint, (limitrec * currentPage) - limitrec);
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Class6_7_F objrd = new Class6_7_F
                        {
                            W_date = rdr["w_date"] == DBNull.Value ? null : (DateTime?)rdr["w_date"],
                            W01 = rdr["w01"] == DBNull.Value ? null : (Int32?)rdr["w01"],
                            W02 = rdr["w02"] == DBNull.Value ? null : (Int32?)rdr["w02"],
                            W03 = rdr["w03"] == DBNull.Value ? null : (Int32?)rdr["w03"],
                            W05 = rdr["w05"] == DBNull.Value ? null : (Int32?)rdr["w05"],
                            W07 = rdr["W07"] == DBNull.Value ? null : (Int32?)rdr["W07"],
                            W09 = rdr["w09"] == DBNull.Value ? null : (Int32?)rdr["w09"],
                            W101 = rdr["w101"] == DBNull.Value ? null : (Int32?)rdr["w101"],
                            W102 = rdr["w102"] == DBNull.Value ? null : (Int32?)rdr["w102"],
                            Wtotal = rdr["wtotal"] == DBNull.Value ? null : (Int32?)rdr["wtotal"]
                        };
                        lstobj.Add(objrd);
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
            return lstobj;
        }

        #endregion

        public IEnumerable<Helpdesk> GetAllHelpdesk()
        {
            List<Helpdesk> lstobj = new List<Helpdesk>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT");
                    sql.AppendLine("idx, created, entity_lock, modified, client_id, client_ip ");
                    sql.AppendLine(",hlp_name, hlp_desc, hlp_tel, hlp_mail");
                    sql.AppendLine("FROM dbo.set_helpdesk ");
                    sql.AppendLine("WHERE 1=1");
                    //sql.AppendLine("and (w_date >='" & dtpStart.ToString("s") & "' and w_date < '" & dtpStop.ToString("s") & "')");
                    sql.AppendLine("ORDER BY idx ASC ");
                    //sql.AppendLine("limit " & LimitRecoard & " offset " & (LimitRecoard * CurrentPage) - LimitRecoard);
                    sql.AppendLine(";");


                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Helpdesk objrd = new Helpdesk
                        {
                            Idx = rdr["idx"] == DBNull.Value ? null : (long?)rdr["idx"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Entity_lock = rdr["entity_lock"] == DBNull.Value ? null : (int?)rdr["entity_lock"],
                            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                            Client_id = rdr["client_id"] == DBNull.Value ? null : (long?)rdr["client_id"],
                            Client_ip = rdr["client_ip"].ToString(),
                            Hlp_name = rdr["hlp_name"].ToString(),
                            Hlp_desc = rdr["hlp_desc"].ToString(),
                            Hlp_tel = rdr["hlp_tel"].ToString(),
                            Hlp_mail = rdr["hlp_mail"].ToString()
                        };
                        lstobj.Add(objrd);
                    }
                }
                catch (SqlException ex)
                {
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }
            return lstobj;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using GoWMS.Server.Models.Wcs;
using Npgsql;
using NpgsqlTypes;
using System.Text;
using Serilog;


namespace GoWMS.Server.Data
{
    public class WcsDAL
    {
        readonly private string connectionString = ConnGlobals.GetConnLocalDBPG();
        readonly private string connectionStringSQL = ConnGlobals.GetConnDBSQL();

        public IEnumerable<Vmachine> GetAllMachine()
        {
            List<Vmachine> lstobj = new List<Vmachine>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("select mccode, information, st_no, desc_th, CAST(IIF(is_alert = 1, 1, 0) AS Bit)  as is_alert , backcolor, focecolor, CAST(IIF(is_cmd = 1, 1, 0) AS Bit) as is_cmd");
                    sql.AppendLine("from wcs.view_machine_status");
                    //sql.AppendLine("order by mccode");
                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        Vmachine objrd = new Vmachine
                        {
                            Mccode = rdr["mccode"].ToString(),
                            Information = rdr["information"].ToString(),
                            St_no = rdr["st_no"] == DBNull.Value ? null : (Int32?)rdr["st_no"],
                            Desc_th = rdr["desc_th"].ToString(),
                            //Is_alert = rdr["is_alert"] == DBNull.Value ? true : (bool?)rdr["is_alert"],
                            Is_alert = !Convert.IsDBNull(rdr["is_alert"]) ? (bool?)rdr["is_alert"] : null,
                            Backcolor = rdr["backcolor"].ToString(),
                            Focecolor = rdr["focecolor"].ToString(),
                            //Is_cmd = rdr["is_cmd"] == DBNull.Value ? false : (bool?)rdr["is_cmd"]
                            Is_cmd =  !Convert.IsDBNull(rdr["is_cmd"]) ? (bool?)rdr["is_cmd"] : null

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

        public IEnumerable<Vmachine_command> GetCommandMachine(string mccode)
        {
            List<Vmachine_command> lstobj = new List<Vmachine_command>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("select mccode, st_no, st_desc_th, sg_no, sg_desc_th");
                    sql.AppendLine("from wcs.vmachine_command");
                    sql.AppendLine("where  mccode=@mccode");
                    sql.AppendLine("order by st_no");
                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();

                    cmd.Parameters.AddWithValue("@mccode", mccode);

                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        Vmachine_command objrd = new Vmachine_command
                        {
                            Mccode = rdr["mccode"].ToString(),
                            St_no = (int)rdr["st_no"] ,
                            St_desc_th = rdr["st_desc_th"].ToString(),
                            Sg_no = rdr["sg_no"].ToString(),
                            Sg_desc_th = rdr["sg_desc_th"].ToString()
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

        public Boolean CreateCommandMC(string mccode, Int32 command)
        {
            Boolean bRet = false;
            string sRet = "";
            Int32? iRet = 0;
            string cmdcode = "CMD08";
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sqlQurey = new StringBuilder();
                    //sqlQurey.AppendLine("select _retchk, _retmsg from wcs.fuc_create_mccommand(:mccode , :cmdcode, :command);");
                    sqlQurey.Append("wcs.ssp_create_mccommand");
                    SqlCommand cmd = new SqlCommand(sqlQurey.ToString(), con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    con.Open();

                    cmd.Parameters.AddWithValue("@_mccode", mccode);
                    cmd.Parameters.AddWithValue("@_cmdcode", cmdcode) ;
                    cmd.Parameters.AddWithValue("@_command", command);
  
                    SqlParameter RuturnCheck = new SqlParameter("@_retchk", SqlDbType.Int);
                    RuturnCheck.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(RuturnCheck);

                    SqlParameter RuturnMsg = new SqlParameter("@_retmsg", SqlDbType.VarChar,255);
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
            return bRet;
        }


        public Boolean CreatePotocalMC(string mccode, Int32 startpos, Int32 stoppos, Int32 unittyp, string palletid, Int32 weight, Int32 command)
        {
            Boolean bRet = false;
            string sRet = "";
            Int32? iRet = 0;
            
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sqlQurey = new StringBuilder();
                    //sqlQurey.AppendLine("select _retchk, _retmsg from wcs.fuc_create_mcprotocol(:mccode , :startpos, :stoppos, :unittype, :palletid , :weight, :command);");
                    sqlQurey.Append("wcs.ssp_create_mcprotocol");
                    SqlCommand cmd = new SqlCommand(sqlQurey.ToString(), con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    con.Open();

                    cmd.Parameters.AddWithValue("@_mccode", mccode);
                    cmd.Parameters.AddWithValue("@_startpos", startpos);
                    cmd.Parameters.AddWithValue("@_stoppos", stoppos);
                    cmd.Parameters.AddWithValue("@_unittype", unittyp);
                    cmd.Parameters.AddWithValue("@_entry", startpos);
                    cmd.Parameters.AddWithValue("@_exit", stoppos);
                    cmd.Parameters.AddWithValue("@_palletid", palletid);
                    cmd.Parameters.AddWithValue("@_weight", weight);
                    cmd.Parameters.AddWithValue("@_command", command);

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


        public IEnumerable<Vset_gate_rgv> GetGateRgv()
        {
            List<Vset_gate_rgv> lstobj = new List<Vset_gate_rgv>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("select st_code, st_no, st_type");
                    sql.AppendLine("from wcs.vset_station_rgv");
                    sql.AppendLine("order by st_no");
                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();


                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        Vset_gate_rgv objrd = new Vset_gate_rgv
                        {
                            Mccode = rdr["st_code"].ToString(),
                            St_no = rdr["st_no"] == DBNull.Value ? null : (Int32?)rdr["st_no"],
                            Type = rdr["st_type"].ToString()

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


        public IEnumerable<Tas_WorksInfo> GetASRSWork()
        {
            List<Tas_WorksInfo> lstobj = new List<Tas_WorksInfo>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT idx, created, entity_lock, modified, client_id, client_ip");
                    sql.AppendLine(", su_no, lpncode, work_code, work_status, work_srm, work_location");
                    sql.AppendLine(", work_weight, actual_weight, work_size, actual_size, work_gate, work_ref");
                    sql.AppendLine(", ctime, stime, etime, work_priority");
                    sql.AppendLine("from wcs.tas_works");
                    sql.AppendLine("order by created");

                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();


                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        Tas_WorksInfo objrd = new Tas_WorksInfo
                        {
                            Idx = rdr["idx"] == DBNull.Value ? null : (Int64?)rdr["idx"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Entity_Lock = rdr["entity_lock"] == DBNull.Value ? null : (Int32?)rdr["entity_lock"],
                            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                            Client_Id = rdr["client_id"] == DBNull.Value ? null : (Int64?)rdr["client_id"],
                            Client_Ip = rdr["client_ip"].ToString(),
                            Su_No = rdr["su_no"].ToString(),
                            Lpncode = rdr["lpncode"].ToString(),
                            Work_Code = rdr["work_code"].ToString(),

                            Work_Status = rdr["work_status"].ToString(),
                            Work_Srm = rdr["work_srm"].ToString(),
                            Work_Location = rdr["work_location"].ToString(),
                            Work_Weight = rdr["work_weight"] == DBNull.Value ? null : (decimal?)rdr["work_weight"],
                            Actual_Weight = rdr["actual_weight"] == DBNull.Value ? null : (decimal?)rdr["actual_weight"],
                            Work_Size = rdr["work_size"] == DBNull.Value ? null : (Int32?)rdr["work_size"],
                            Actual_Size = rdr["actual_size"] == DBNull.Value ? null : (Int32?)rdr["actual_size"],
                            Work_Gate = rdr["work_gate"].ToString(),
                            Work_Ref = rdr["work_ref"].ToString(),

                            Ctime = rdr["ctime"] == DBNull.Value ? null : (DateTime?)rdr["ctime"],
                            Stime = rdr["stime"] == DBNull.Value ? null : (DateTime?)rdr["stime"],
                            Etime = rdr["etime"] == DBNull.Value ? null : (DateTime?)rdr["etime"],
                            Work_Priority = rdr["work_priority"] == DBNull.Value ? null : (Int32?)rdr["work_priority"],


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

        public IEnumerable<Tas_Mcworks> GetASRSWorks()
        {
            List<Tas_Mcworks> lstobj = new List<Tas_Mcworks>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT idx, created, entity_lock, modified, client_id, client_ip");
                    sql.AppendLine(", lpncode, work_code, work_status, work_id");
                    sql.AppendLine(", srm_no, srm_from, srm_to, srm_status");
                    sql.AppendLine(", rgv_no, rgv_from, rgv_to, rgv_status");
                    sql.AppendLine(", cvy_no, cvy_from, cvy_to, cvy_status");
                    sql.AppendLine(", pallet_no, pallet_hight, pallet_width, pallet_size");
                    sql.AppendLine(", ctime, stime, etime, gate_out, work_priority");
                    sql.AppendLine("FROM wcs.tas_mcworks");
                    sql.AppendLine("order by ctime");

                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();


                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        Tas_Mcworks objrd = new Tas_Mcworks
                        {
                            Idx = rdr["idx"] == DBNull.Value ? null : (Int64?)rdr["idx"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Entity_Lock = rdr["entity_lock"] == DBNull.Value ? null : (Int32?)rdr["entity_lock"],
                            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                            Client_Id = rdr["client_id"] == DBNull.Value ? null : (Int64?)rdr["client_id"],
                            Client_Ip = rdr["client_ip"].ToString(),
                            Lpncode = rdr["lpncode"].ToString(),
                            Work_Code = rdr["work_code"].ToString(),
                            Work_Status = rdr["work_status"] == DBNull.Value ? null : (Int32?)rdr["work_status"],
                            Work_Id = rdr["work_id"] == DBNull.Value ? null : (Int64?)rdr["work_id"],
                            Srm_No = rdr["srm_no"] == DBNull.Value ? null : (Int32?)rdr["srm_no"],
                            Srm_From = rdr["srm_from"] == DBNull.Value ? null : (Int32?)rdr["srm_from"],
                            Srm_To = rdr["srm_to"] == DBNull.Value ? null : (Int32?)rdr["srm_to"],
                            Srm_Status = rdr["srm_status"] == DBNull.Value ? null : (Int32?)rdr["srm_status"],

                            Rgv_No = rdr["rgv_no"] == DBNull.Value ? null : (Int32?)rdr["rgv_no"],
                            Rgv_From = rdr["rgv_from"] == DBNull.Value ? null : (Int32?)rdr["rgv_from"],
                            Rgv_To = rdr["rgv_to"] == DBNull.Value ? null : (Int32?)rdr["rgv_to"],
                            Rgv_Status = rdr["rgv_status"] == DBNull.Value ? null : (Int32?)rdr["rgv_status"],

                            Cvy_No = rdr["cvy_no"] == DBNull.Value ? null : (Int32?)rdr["cvy_no"],
                            Cvy_From = rdr["cvy_from"] == DBNull.Value ? null : (Int32?)rdr["cvy_from"],
                            Cvy_To = rdr["cvy_to"] == DBNull.Value ? null : (Int32?)rdr["cvy_to"],
                            Cvy_Status = rdr["cvy_status"] == DBNull.Value ? null : (Int32?)rdr["cvy_status"],

                            Pallet_No = rdr["pallet_no"] == DBNull.Value ? null : (Int32?)rdr["pallet_no"],
                            Pallet_Hight = rdr["pallet_hight"] == DBNull.Value ? null : (Int32?)rdr["pallet_hight"],
                            Pallet_Width = rdr["pallet_width"] == DBNull.Value ? null : (Int32?)rdr["pallet_width"],
                            Pallet_Size = rdr["pallet_size"] == DBNull.Value ? null : (Int32?)rdr["pallet_size"],



                            Ctime = rdr["ctime"] == DBNull.Value ? null : (DateTime?)rdr["ctime"],
                            Stime = rdr["stime"] == DBNull.Value ? null : (DateTime?)rdr["stime"],
                            Etime = rdr["etime"] == DBNull.Value ? null : (DateTime?)rdr["etime"],
                            Gate_Out = rdr["gate_out"] == DBNull.Value ? null : (Int32?)rdr["gate_out"],
                            Work_Priority = rdr["work_priority"] == DBNull.Value ? null : (Int32?)rdr["work_priority"]

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

        public IEnumerable<Tas_Rgvworks> GetRGVWorks()
        {
            List<Tas_Rgvworks> lstobj = new List<Tas_Rgvworks>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT idx, created, entity_lock, modified, client_id, client_ip");
                    sql.AppendLine(", lpncode, work_code, work_status, work_id");
                    sql.AppendLine(", rgv_no, rgv_from, rgv_to, rgv_status");
                    sql.AppendLine(", cvy_no, cvy_from, cvy_to, cvy_status");
                    sql.AppendLine(", pallet_no, pallet_hight, pallet_width, pallet_size");
                    sql.AppendLine(", ctime, stime, etime, gate_out, work_priority");
                    sql.AppendLine("FROM wcs.tas_rgvworks");
                    sql.AppendLine("order by ctime");

                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();


                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        Tas_Rgvworks objrd = new Tas_Rgvworks
                        {
                            Idx = rdr["idx"] == DBNull.Value ? null : (Int64?)rdr["idx"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Entity_Lock = rdr["entity_lock"] == DBNull.Value ? null : (Int32?)rdr["entity_lock"],
                            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                            Client_Id = rdr["client_id"] == DBNull.Value ? null : (Int64?)rdr["client_id"],
                            Client_Ip = rdr["client_ip"].ToString(),
                            Lpncode = rdr["lpncode"].ToString(),
                            Work_Code = rdr["work_code"].ToString(),
                            Work_Status = rdr["work_status"] == DBNull.Value ? null : (Int32?)rdr["work_status"],
                            Work_Id = rdr["work_id"] == DBNull.Value ? null : (Int64?)rdr["work_id"],

                            Rgv_No = rdr["rgv_no"] == DBNull.Value ? null : (Int32?)rdr["rgv_no"],
                            Rgv_From = rdr["rgv_from"] == DBNull.Value ? null : (Int32?)rdr["rgv_from"],
                            Rgv_To = rdr["rgv_to"] == DBNull.Value ? null : (Int32?)rdr["rgv_to"],
                            Rgv_Status = rdr["rgv_status"] == DBNull.Value ? null : (Int32?)rdr["rgv_status"],

                            Cvy_No = rdr["cvy_no"] == DBNull.Value ? null : (Int32?)rdr["cvy_no"],
                            Cvy_From = rdr["cvy_from"] == DBNull.Value ? null : (Int32?)rdr["cvy_from"],
                            Cvy_To = rdr["cvy_to"] == DBNull.Value ? null : (Int32?)rdr["cvy_to"],
                            Cvy_Status = rdr["cvy_status"] == DBNull.Value ? null : (Int32?)rdr["cvy_status"],

                            Pallet_No = rdr["pallet_no"] == DBNull.Value ? null : (Int32?)rdr["pallet_no"],
                            Pallet_Hight = rdr["pallet_hight"] == DBNull.Value ? null : (Int32?)rdr["pallet_hight"],
                            Pallet_Width = rdr["pallet_width"] == DBNull.Value ? null : (Int32?)rdr["pallet_width"],
                            Pallet_Size = rdr["pallet_size"] == DBNull.Value ? null : (Int32?)rdr["pallet_size"],

                            Ctime = rdr["ctime"] == DBNull.Value ? null : (DateTime?)rdr["ctime"],
                            Stime = rdr["stime"] == DBNull.Value ? null : (DateTime?)rdr["stime"],
                            Etime = rdr["etime"] == DBNull.Value ? null : (DateTime?)rdr["etime"],
                            Gate_Out = rdr["gate_out"] == DBNull.Value ? null : (Int32?)rdr["gate_out"],
                            Work_Priority = rdr["work_priority"] == DBNull.Value ? null : (Int32?)rdr["work_priority"]

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


        public IEnumerable<AsrsPerformance> GetAsrsPerformance(DateTime Fmtime, DateTime Totime)
        {
            List<AsrsPerformance> lstobj = new List<AsrsPerformance>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT * ");
                    sql.AppendLine("FROM wcs.pfs_view_rptperformancetime(@p_stime, @p_etime)");
                    sql.AppendLine(";");

                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    cmd.Parameters.AddWithValue("@p_stime", Fmtime);
                    cmd.Parameters.AddWithValue("@p_etime", Totime);

                    con.Open();


                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        AsrsPerformance objrd = new AsrsPerformance
                        {
                            Mccode = rdr["r_mccode"].ToString(),
                            Inbound = rdr["r_inbound"] == DBNull.Value ? null : (Int32?)rdr["r_inbound"],
                            Outbound = rdr["r_outbound"] == DBNull.Value ? null : (Int32?)rdr["r_outbound"],
                            Total = rdr["r_total"] == DBNull.Value ? null : (Int32?)rdr["r_total"]

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


        public IEnumerable<SetConstance> GetConstance()
        {
            List<SetConstance> lstobj = new List<SetConstance>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT set_code, set_desc, val_dt, val_int, val_num, val_chr, val_vhr ");
                    sql.AppendLine("FROM wcs.set_constant");
                    sql.AppendLine("order by set_code");

                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();


                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        SetConstance objrd = new SetConstance
                        {
                            Set_code = rdr["set_code"].ToString(),
                            Set_desc = rdr["set_desc"].ToString(),
                            Val_int = rdr["val_int"] == DBNull.Value ? null : (Int32?)rdr["val_int"],
                            Val_vhr = rdr["val_vhr"].ToString(),
                            Val_chr = rdr["val_chr"].ToString(),
                            Val_dt = rdr["val_dt"] == DBNull.Value ? null : (DateTime?)rdr["val_dt"],
                            Val_num = rdr["val_num"] == DBNull.Value ? null : (decimal?)rdr["val_num"],
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

        public bool SetConstance(string setcode, Int32 setval)
        {
            bool bRet = false;

            using SqlConnection con = new SqlConnection(connectionStringSQL);
            try
            {

                StringBuilder sql = new StringBuilder();

                using var cmd = new SqlCommand(connection: con, cmdText: null);
                sql.AppendLine("UPDATE wcs.set_constant");
                sql.AppendLine("SET val_int = @val_int");
                sql.AppendLine("WHERE set_code = @set_code");
                sql.AppendLine(";");

                cmd.Parameters.AddWithValue("@val_int", setval);
                cmd.Parameters.AddWithValue("@set_code", setcode);


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

        public IEnumerable<Set_Operate> GetOperate()
        {
            List<Set_Operate> lstobj = new List<Set_Operate>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT idx, created, entity_lock, modified, client_id, client_ip, mc_type, mc_name, mc_no");
                    sql.AppendLine("FROM wcs.set_operate");
                    sql.AppendLine("order by mc_no");

                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();


                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        Set_Operate objrd = new Set_Operate
                        {
                            Idx = rdr["idx"] == DBNull.Value ? null : (Int64?)rdr["idx"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Entity_Lock = rdr["entity_lock"] == DBNull.Value ? null : (Int32?)rdr["entity_lock"],
                            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                            Client_Id = rdr["client_id"] == DBNull.Value ? null : (Int64?)rdr["client_id"],
                            Client_Ip = rdr["client_ip"].ToString(),
                            Mc_Type = rdr["mc_type"].ToString(),
                            Mc_Name = rdr["mc_name"].ToString(),
                            Mc_No = rdr["mc_no"] == DBNull.Value ? null : (Int32?)rdr["mc_no"],
                            Enable = rdr["entity_lock"] == DBNull.Value ? true : !Convert.ToBoolean(rdr["entity_lock"])
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

        public bool SetOperate(Int64 setcode, bool setval)
        {
            bool bRet = false;
            Int32 setvalue = 1;

            using SqlConnection con = new SqlConnection(connectionStringSQL);
            try
            {
                if (setval)
                {
                    setvalue = 0;
                }


                StringBuilder sql = new StringBuilder();

                using var cmd = new SqlCommand(connection: con, cmdText: null);
                sql.AppendLine("UPDATE wcs.set_operate");
                sql.AppendLine("SET entity_lock = @entity_lock");
                sql.AppendLine("WHERE idx = @idx");
                sql.AppendLine(";");


                cmd.Parameters.AddWithValue("@entity_lock", setvalue);
                cmd.Parameters.AddWithValue("@idx", setcode);


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


        public IEnumerable<Set_Srm_Operate> GetSRMOperate()
        {
            List<Set_Srm_Operate> lstobj = new List<Set_Srm_Operate>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT idx, created, entity_lock, modified, client_id, client_ip, mc_name, mc_no, inbound, outbound");
                    sql.AppendLine("FROM wcs.set_srm_operate");
                    sql.AppendLine("order by mc_no");

                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();


                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        Set_Srm_Operate objrd = new Set_Srm_Operate
                        {
                            Idx = rdr["idx"] == DBNull.Value ? null : (Int64?)rdr["idx"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Entity_Lock = rdr["entity_lock"] == DBNull.Value ? null : (Int32?)rdr["entity_lock"],
                            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                            Client_Id = rdr["client_id"] == DBNull.Value ? null : (Int64?)rdr["client_id"],
                            Client_Ip = rdr["client_ip"].ToString(),
                            Mc_Name = rdr["mc_name"].ToString(),
                            Mc_No = rdr["mc_no"] == DBNull.Value ? null : (Int32?)rdr["mc_no"],
                            Inbound = rdr["inbound"] == DBNull.Value ? null : (bool?)rdr["inbound"],
                            Outbound = rdr["outbound"] == DBNull.Value ? null : (bool?)rdr["outbound"]
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

        public bool SetSRMOperate(Int64 setcode, bool setvalin, bool setvalout)
        {
            bool bRet = false;


            using SqlConnection con = new SqlConnection(connectionStringSQL);
            try
            {



                StringBuilder sql = new StringBuilder();

                using var cmd = new SqlCommand(connection: con, cmdText: null);
                sql.AppendLine("UPDATE wcs.set_srm_operate");
                sql.AppendLine("SET inbound = @inbound");
                sql.AppendLine(", outbound = @outbound");
                sql.AppendLine("WHERE idx =  @idx");
                sql.AppendLine(";");


                cmd.Parameters.AddWithValue("@inbound", setvalin);
                cmd.Parameters.AddWithValue("@outbound", setvalout);

                cmd.Parameters.AddWithValue("@idx", setcode);


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


        public IEnumerable<AsrsLoadtime> GetAsrsloadtime(DateTime stime, DateTime etime)
        {
            List<AsrsLoadtime> lstobj = new List<AsrsLoadtime>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT a.lpncode, a.work_code");
                    sql.AppendLine(", c.work_text_th");
                    sql.AppendLine(", a.srm_no, a.srm_from, a.srm_to ");
                    sql.AppendLine(", a.stime, a.etime, a.loadtime ");
                    sql.AppendLine("FROM wcs.vrptqueueloadtimesrm a ");
                    sql.AppendLine("LEFT JOIN  wcs.set_worktype c on a.work_code=c.work_code");
                    sql.AppendLine("WHERE a.stime >= @stimefm");
                    sql.AppendLine("AND a.stime <= @stimeto");
                    sql.AppendLine("ORDER BY a.stime");
                    sql.AppendLine(";");

                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    cmd.Parameters.AddWithValue("@stimefm", stime);
                    cmd.Parameters.AddWithValue("@stimeto", etime);

                    con.Open();


                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        AsrsLoadtime objrd = new AsrsLoadtime
                        {
                            Lpncode = rdr["lpncode"].ToString(),
                            Work_code = rdr["work_code"].ToString(),
                            Work_text_th = rdr["work_text_th"].ToString(),
                            Srm_no = rdr["srm_no"] == DBNull.Value ? null : (Int32?)rdr["srm_no"],
                            Srm_from = rdr["srm_from"] == DBNull.Value ? null : (Int32?)rdr["srm_from"],
                            Srm_to = rdr["srm_to"] == DBNull.Value ? null : (Int32?)rdr["srm_to"],
                            Stime = rdr["stime"] == DBNull.Value ? null : (DateTime?)rdr["stime"],
                            Etime = rdr["etime"] == DBNull.Value ? null : (DateTime?)rdr["etime"],
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


        public IEnumerable<Rpt_Ejectgate> GetReportEject(DateTime stime, DateTime etime)
        {
            List<Rpt_Ejectgate> lstobj = new List<Rpt_Ejectgate>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("SELECT idx, created, entity_lock, modified, client_id, client_ip");
                    sql.AppendLine(", su_no, lpncode, work_code, work_gate, actual_weight, actual_size, msg");
                    sql.AppendLine("FROM wcs.vrpt_eject");
                    sql.AppendLine("WHERE created >= @stimefm");
                    sql.AppendLine("AND created <= @stimeto");
                    sql.AppendLine("ORDER BY created");
                    sql.AppendLine(";");

                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    cmd.Parameters.AddWithValue("@stimefm", stime);
                    cmd.Parameters.AddWithValue("@stimeto", etime);

                    con.Open();


                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {

                        Rpt_Ejectgate objrd = new Rpt_Ejectgate
                        {
                            Idx = rdr["idx"] == DBNull.Value ? null : (Int64?)rdr["idx"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Entity_Lock = rdr["entity_lock"] == DBNull.Value ? null : (Int32?)rdr["entity_lock"],
                            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                            Client_Id = rdr["client_id"] == DBNull.Value ? null : (Int64?)rdr["client_id"],
                            Client_Ip = rdr["client_ip"].ToString(),
                            Su_No = rdr["su_no"].ToString(),
                            Lpncode = rdr["lpncode"].ToString(),
                            Work_Code = rdr["work_code"].ToString(),
                            Work_Gate = rdr["work_gate"].ToString(),
                            Actual_Weight = rdr["actual_weight"] == DBNull.Value ? null : (decimal?)rdr["actual_weight"],
                            Actual_Size = rdr["actual_size"] == DBNull.Value ? null : (Int32?)rdr["actual_size"],
                            Msg = rdr["msg"].ToString()
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

        public bool SetUrgentAsrsQueueByPallet(string spallet)
        {
            bool bret = false;
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("Update wcs.tas_mcworks");
                    sql.AppendLine("Set work_priority = (SELECT max(work_priority) + 1 FROM wcs.tas_mcworks WHERE work_code = @work_code)");
                    sql.AppendLine("where lpncode = @Pallet");

                    SqlCommand cmd = new SqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };

                    cmd.Parameters.AddWithValue("@work_code", "05");
                    cmd.Parameters.AddWithValue("@Pallet", spallet);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    bret = true;
                }
                catch (SqlException ex)
                {
                    bret = false;
                    Log.Error(ex.ToString());
                }
                finally
                {
                    con.Close();
                }
            }

            return bret;
        }








    }
}

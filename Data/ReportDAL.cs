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
using GoWMS.Server.Models.Rpt;
using NpgsqlTypes;
using System.Text;
using GoWMS.Server.DataAccess;
using Serilog;


namespace GoWMS.Server.Data
{
    public class ReportDAL
    {
        readonly private string connectionString = ConnGlobals.GetConnLocalDBPG();

        public IEnumerable<RptAudittrial> GetAllAudittrial()
        {
            List<RptAudittrial> lstobj = new List<RptAudittrial>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();
                    sql.AppendLine("select * ");
                    sql.AppendLine("from public.api_cylinder_go");
                    sql.AppendLine("order by efidx");
                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        RptAudittrial objrd = new RptAudittrial
                        {
                            /*
                            Idx = rdr["efidx"] == DBNull.Value ? null : (Int64?)rdr["efidx"],
                            Entity_Lock = rdr["efstatus"] == DBNull.Value ? null : (int?)rdr["efstatus"],
                            Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                            Modified = rdr["modified"] == DBNull.Value ? null : (DateTime?)rdr["modified"],
                            Client_Id = rdr["innovator"] == DBNull.Value ? null : (long?)rdr["innovator"],
                            Client_Ip = rdr["device"].ToString(),
                            */
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
        public Boolean InsertAudittrial(String actdesc, String munname, long user)
        {
            long iUser = user;
            long iClient = 0;
            string sClient = "127.0.0.1";
            bool bRet = false;
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("insert into dbo.rpt_audittrial(");
            sql.AppendLine("client_id, client_ip, id_stuser, menu_name, action_desc");
            sql.AppendLine(")");
            sql.AppendLine("Values(");
            sql.AppendLine("@client_id, @client_ip, @id_stuser, @menu_name, @action_desc");
            sql.AppendLine(")");
            sql.AppendLine(";");


            SqlCommand cmd = new SqlCommand(sql.ToString())
            {
                CommandType = CommandType.Text
            };
            cmd.Parameters.AddWithValue("@client_id",  iClient);
            cmd.Parameters.AddWithValue("@client_ip",  sClient);
            cmd.Parameters.AddWithValue("@id_stuser",  iUser);
            cmd.Parameters.AddWithValue("@menu_name",  munname);
            cmd.Parameters.AddWithValue("@action_desc",  actdesc);



            SqlDAL sqlDAL = new SqlDAL();
            bRet = sqlDAL.SyncInsertsqlData(cmd);

            return bRet;
        }
    }
}

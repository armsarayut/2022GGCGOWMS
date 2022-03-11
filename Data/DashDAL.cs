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
using GoWMS.Server.Models.Das;
using GoWMS.Server.Models.Public;
using GoWMS.Server.Models.Wcs;
using NpgsqlTypes;
using System.Text;
using Serilog;

namespace GoWMS.Server.Data
{
    public class DashDAL
    {
        readonly private string connectionString = ConnGlobals.GetConnLocalDBPG();
        readonly private string connectionStringSQL = ConnGlobals.GetConnDBSQL();

        public IEnumerable<Vrpt_operationresult_sum> GetAllOrderofDay()
        {
            List<Vrpt_operationresult_sum> lstobj = new List<Vrpt_operationresult_sum>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                NpgsqlCommand cmd = new NpgsqlCommand("select created, sum(c01) as c01, sum(c02) as c02, sum(c03) as c03, sum(c04) as c04, sum(c05) as c05, sum(c07) as c07, sum(c08) as c08, sum(c09) as c09 " +
                    ", sum(s01) as s01, sum(s02) as s02, sum(s03) as s03, sum(s04) as s04, sum(s05) as s05, sum(s07) as s07, sum(s08) as s08, sum(s09) as s09 " +
                    "FROM public.vrpt_operationresult_sum " +
                    "WHERE created = now()::DATE " +
                    "GROUP BY created", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                NpgsqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Vrpt_operationresult_sum objrd = new Vrpt_operationresult_sum
                    {
                        Created = rdr["created"] == DBNull.Value ? null : (DateTime?)rdr["created"],
                        C01 = rdr["C01"] == DBNull.Value ? null : (decimal?)rdr["C01"],
                        C02 = rdr["C02"] == DBNull.Value ? null : (decimal?)rdr["C02"],
                        C03 = rdr["C03"] == DBNull.Value ? null : (decimal?)rdr["C03"],
                        C04 = rdr["C04"] == DBNull.Value ? null : (decimal?)rdr["C04"],
                        C05 = rdr["C05"] == DBNull.Value ? null : (decimal?)rdr["C05"],
                        C07 = rdr["C07"] == DBNull.Value ? null : (decimal?)rdr["C07"],
                        C08 = rdr["C08"] == DBNull.Value ? null : (decimal?)rdr["C08"],
                        C09 = rdr["C09"] == DBNull.Value ? null : (decimal?)rdr["C09"],
                        S01 = rdr["S01"] == DBNull.Value ? null : (decimal?)rdr["S01"],
                        S02 = rdr["S02"] == DBNull.Value ? null : (decimal?)rdr["S02"],
                        S03 = rdr["S03"] == DBNull.Value ? null : (decimal?)rdr["S03"],
                        S04 = rdr["S04"] == DBNull.Value ? null : (decimal?)rdr["S04"],
                        S05 = rdr["S05"] == DBNull.Value ? null : (decimal?)rdr["S05"],
                        S07 = rdr["S07"] == DBNull.Value ? null : (decimal?)rdr["S07"],
                        S08 = rdr["S08"] == DBNull.Value ? null : (decimal?)rdr["S08"],
                        S09 = rdr["S09"] == DBNull.Value ? null : (decimal?)rdr["S09"]
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }


        public IEnumerable<Vrpt_shelfsummary> GetAllLocationSummary()
        {
            List<Vrpt_shelfsummary> lstobj = new List<Vrpt_shelfsummary>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                SqlCommand cmd = new SqlCommand("select srm_name, srm_no, locavl, locemp, plemp, plerr, prohloc, total, percen " +
                    "FROM wcs.vrpt_shelfsummary " +
                    "ORDER BY srm_no", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Vrpt_shelfsummary objrd = new Vrpt_shelfsummary
                    {
                        Srm_Name = rdr["srm_name"].ToString(),
                        Srm_No = rdr["srm_no"] == DBNull.Value ? null : (Int32?)rdr["srm_no"],
                        Locavl = rdr["locavl"] == DBNull.Value ? null : (Int32?)rdr["locavl"],
                        Locemp = rdr["locemp"] == DBNull.Value ? null : (Int32?)rdr["locemp"],
                        Plemp = rdr["plemp"] == DBNull.Value ? null : (Int32?)rdr["plemp"],
                        Plerr = rdr["plerr"] == DBNull.Value ? null : (Int32?)rdr["plerr"],
                        Prohloc = rdr["prohloc"] == DBNull.Value ? null : (Int32?)rdr["prohloc"],
                        Total = rdr["total"] == DBNull.Value ? null : (Int32?)rdr["total"],
                        Percen = rdr["percen"] == DBNull.Value ? null : (decimal?)rdr["percen"]
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }


        public IEnumerable<VLocationDash> GetAllTasworkofday()
        {
            List<VLocationDash> lstobj = new List<VLocationDash>();
            using (SqlConnection con = new SqlConnection(connectionStringSQL))
            {
                SqlCommand cmd = new SqlCommand("SELECT  work_code ,count(lpncode)as clpncode " +
                    "FROM wcs.rpt_works " +
                    "WHERE Convert(date, etime) = Convert(date, getdate())" +
                    "AND work_status='COM' " +
                    "GROUP BY work_code", con)
                {
                    CommandType = CommandType.Text
                };
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    VLocationDash objrd = new VLocationDash
                    {
                        Work_Code = rdr["work_code"].ToString(),
                        Clpncode = rdr["clpncode"] == DBNull.Value ? null : (Int64?)rdr["clpncode"]
                    };
                    lstobj.Add(objrd);
                }
                con.Close();
            }
            return lstobj;
        }

        public IEnumerable<DashTaskTime> GetASRSDashboardComplete()

        {
            List<DashTaskTime> lstobj = new List<DashTaskTime>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();

                    sql.AppendLine("SELECT checkday, tasktime, counttask");
                    sql.AppendLine("FROM wcs.vrptqueuecompleteasrs");
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        DashTaskTime objrd = new DashTaskTime
                        {
                            Checkday = rdr["checkday"].ToString(),
                            Tasktime = rdr["tasktime"].ToString(),
                            Counttask = rdr["counttask"] == DBNull.Value ? null : (Int64?)rdr["counttask"]
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

        public IEnumerable<AsrsTaskSummary> GetTaskofday()
        {
            List<AsrsTaskSummary> lstobj = new List<AsrsTaskSummary>();
            using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sql = new StringBuilder();

                    sql.AppendLine("SELECT w_date, w01 , w101 ,  coalesce(w01, 0) + coalesce(w101, 0) as sumin");
                    sql.AppendLine(", w05, w102 , coalesce(w05, 0) + coalesce(w102, 0) as sumout");
                    sql.AppendLine(", wtotal");
                    sql.AppendLine("FROM wcs.vrpt_workendofday");
                    sql.AppendLine("where w_date = now()::date");
                    sql.AppendLine(";");

                    NpgsqlCommand cmd = new NpgsqlCommand(sql.ToString(), con)
                    {
                        CommandType = CommandType.Text
                    };
                    con.Open();

                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        AsrsTaskSummary objrd = new AsrsTaskSummary
                        {
                            W_date = rdr["w_date"] == DBNull.Value ? null : (DateTime?)rdr["w_date"],
                            W01 = rdr["w01"] == DBNull.Value ? null : (long?)rdr["w01"],
                            W101 = rdr["w101"] == DBNull.Value ? null : (long?)rdr["w101"],
                            Sumin = rdr["sumin"] == DBNull.Value ? null : (long?)rdr["sumin"],
                            W05 = rdr["w05"] == DBNull.Value ? null : (long?)rdr["w05"],
                            W102 = rdr["w102"] == DBNull.Value ? null : (long?)rdr["w102"],
                            Sumout = rdr["sumout"] == DBNull.Value ? null : (long?)rdr["sumout"],
                            Wtotal = rdr["wtotal"] == DBNull.Value ? null : (long?)rdr["wtotal"]
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


    }
}

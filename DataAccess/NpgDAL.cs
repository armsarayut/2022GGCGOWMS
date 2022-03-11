using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;
using NpgsqlTypes;
using System.Text;
using GoWMS.Server.Data;

namespace GoWMS.Server.DataAccess
{
    public class NpgDAL
    {
        readonly private string connectionString = ConnGlobals.GetConnLocalDBPG();

        public DataSet SQLExecuteSQLQueryFill(NpgsqlCommand sqlCmd)
        {
            DataSet retDS = new DataSet();
            //    NpgsqlConnection objConn = new NpgsqlConnection();
            //    try
            //    {
            //        objConn.Open();
            //    }
            //    catch (NpgsqlException exp)
            //    {
            //        string msgexcep = exp.Message.ToString();
            //        goto ConnClose;
            //    }

            //    try
            //    {
            //        sqlCmd.Connection = objConn;
            //        NpgDataAdapter da = new NpgDataAdapter();
            //        da.SelectCommand = sqlCmd;
            //        da.SelectCommand.CommandTimeout = 0;
            //        da.Fill(retDS);

            //    }
            //    catch (NpgsqlException exp)
            //    {
            //        string msgexcep = exp.Message.ToString();
            //    }
            //    finally
            //    {
            //        objConn.Close();
            //    }
            //ConnClose:
            return retDS;
        }

        public DataSet RetrieveSqlData(NpgsqlCommand cmd)
        {
            DataSet retDs = new DataSet();
            NpgsqlConnection objConn = new NpgsqlConnection(connectionString);
            try
            {
                objConn.Open();
            }
            catch (NpgsqlException exp)
            {
                string msgexcep = exp.Message.ToString();
                goto RetrieveSqlDataConnClose;
            }

            try
            {
                cmd.Connection = objConn;
                NpgsqlDataAdapter da = new NpgsqlDataAdapter
                {
                    SelectCommand = cmd
                };
                da.SelectCommand.CommandTimeout = 0;
                da.Fill(retDs);
            }
            catch (NpgsqlException ex)
            {
                string msgexcep = ex.Message.ToString();
            }
            finally
            {
                objConn.Close();
            }
        RetrieveSqlDataConnClose:
            return retDs;
        }
       
        //----SyncSql
        public Boolean SyncUpdatesqlData(NpgsqlCommand cmd)
        {
            Boolean bRet = false;
            NpgsqlConnection objConn = new NpgsqlConnection(connectionString);
            try
            {
                objConn.Open();
            }
            catch (NpgsqlException exp)
            {
                string msgexcep = exp.Message.ToString();
                goto UpdatesqlDataConnClose;
            }
            NpgsqlTransaction trans = objConn.BeginTransaction();
            try
            {
                cmd.Connection = objConn;
                cmd.Transaction = trans;

                cmd.ExecuteNonQuery();
                trans.Commit();
                bRet = true;
            }
            catch (NpgsqlException ex)
            {
                string msgExcep = ex.Message.ToString();
                trans.Rollback();
            }
            finally
            {
                objConn.Close();
                objConn.Dispose();
            }
        UpdatesqlDataConnClose:
            return bRet;
        }
        public Boolean SyncInsertsqlData(NpgsqlCommand cmd)
        {
            Boolean bRet = false;
            NpgsqlConnection objConn = new NpgsqlConnection(connectionString);
            try
            {
                objConn.Open();
            }
            catch (NpgsqlException exp)
            {
                string msgexcep = exp.Message.ToString();
                goto InsertsqlDataConnClose;
            }
            NpgsqlTransaction trans = objConn.BeginTransaction();
            try
            {
                cmd.Connection = objConn;
                cmd.Transaction = trans;

                cmd.ExecuteNonQuery();
                trans.Commit();
                bRet = true;

            }
            catch (NpgsqlException ex)
            {
                string msgExcep = ex.Message.ToString();
                trans.Rollback();
            }
            finally
            {
                objConn.Close();
                objConn.Dispose();
            }
        InsertsqlDataConnClose:
            return bRet;
        }
        public Boolean SyncDeletesqlData(NpgsqlCommand cmd)
        {
            Boolean bRet = false;
            NpgsqlConnection objConn = new NpgsqlConnection(connectionString);
            try
            {
                objConn.Open();
            }
            catch (NpgsqlException exp)
            {
                string msgexcep = exp.Message.ToString();
                goto DeletesqlDataConnClose;
            }
            NpgsqlTransaction trans = objConn.BeginTransaction();
            try
            {
                cmd.Connection = objConn;
                cmd.Transaction = trans;
                cmd.ExecuteNonQuery();
                trans.Commit();
                bRet = true;
            }
            catch (NpgsqlException ex)
            {
                string msgExcep = ex.Message.ToString();
                trans.Rollback();
            }
            finally
            {
                objConn.Close();
                objConn.Dispose();
            }
        DeletesqlDataConnClose:
            return bRet;
        }
        
        //-----AsyncSql
        public Boolean AsyncUpdatesqlData(NpgsqlCommand cmd)
        {
            Boolean bRet = false;
            NpgsqlConnection objConn = new NpgsqlConnection(connectionString);
            try
            {
                objConn.OpenAsync();
            }
            catch (NpgsqlException exp)
            {
                string msgexcep = exp.Message.ToString();
                goto UpdatesqlDataConnClose;
            }
            NpgsqlTransaction trans = objConn.BeginTransaction();
            try
            {
                cmd.Connection = objConn;
                cmd.Transaction = trans;

                cmd.ExecuteNonQueryAsync();
                trans.CommitAsync();
                bRet = true;

            }
            catch (NpgsqlException ex)
            {
                string msgExcep = ex.Message.ToString();
                trans.RollbackAsync();
            }
            finally
            {
                objConn.CloseAsync();
                objConn.DisposeAsync();
            }
        UpdatesqlDataConnClose:
            return bRet;
        }
        public Boolean AsyncInsertsqlData(NpgsqlCommand cmd)
        {
            Boolean bRet = false;
            NpgsqlConnection objConn = new NpgsqlConnection(connectionString);
            try
            {
                objConn.OpenAsync();
            }
            catch (NpgsqlException exp)
            {
                string msgexcep = exp.Message.ToString();
                goto InsertsqlDataConnClose;
            }
            NpgsqlTransaction trans = objConn.BeginTransaction();
            try
            {
                cmd.Connection = objConn;
                cmd.Transaction = trans;

                cmd.ExecuteNonQueryAsync();
                trans.CommitAsync();
                bRet = true;

            }
            catch (NpgsqlException ex)
            {
                string msgExcep = ex.Message.ToString();
                trans.Rollback();
            }
            finally
            {
                objConn.CloseAsync();
                objConn.DisposeAsync();
            }
        InsertsqlDataConnClose:
            return bRet;
        }
        public static async Task<int> AsyncDeletesqlData(NpgsqlCommand cmd)
        {
        //NpgsqlConnection conn = new NpgsqlConnection(connectionString);
        //try
        //{
        //    //SqlCommand cmd = new SqlCommand("Select * from StudentTable WAITFOR DELAY '00:00:01' ", conn);
        //    //int result = Connection.Method(conn, cmd).Result;
        //    await conn.OpenAsync();
        //    await cmd.ExecuteNonQueryAsync();

        //    SqlDataReader read = cmd.ExecuteReader();
        //    while (read.Read())
        //    {
        //        Console.WriteLine(String.Format("{0},{1},{2},{3}", read[0], read[1], read[2], read[3]));
        //    }
        //    Console.ReadLine();
        //}
        //catch (SqlException)
        //{

        //}
        //finally
        //{
        //    conn.Close();
        //}




        //using (var connection = new NpgsqlConnection(connectionString))
        //{
        //    await connection.OpenAsync();
        //    using (var tran = connection.BeginTransaction())
        //    using (var command = new SqlCommand(sqlQuery, connection, tran))
        //    {
        //        try
        //        {
        //            await command.ExecuteNonQueryAsync();
        //        }
        //        catch
        //        {
        //            tran.Rollback();
        //            throw;
        //        }
        //        tran.Commit();
        //    }
        //}


        //using (var cn = new NpgsqlConnection(connectionString))
        //{

        //    await cn.OpenAsync().ConfigureAwait(false);
        //    await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

        //}

        //Boolean bRet = false;
        //NpgsqlConnection objConn = new NpgsqlConnection(connectionString);
        //try
        //{
        //    await objConn.OpenAsync();
        //}
        //catch (NpgsqlException exp)
        //{
        //    string msgexcep = exp.Message.ToString();
        //    goto DeletesqlDataConnClose;
        //}
        //NpgsqlTransaction trans = objConn.BeginTransaction();
        //try
        //{
        //    cmd.Connection = objConn;
        //    cmd.Transaction = trans;
        //    await cmd.ExecuteNonQueryAsync();
        //    await trans.CommitAsync();
        //    bRet = true;
        //}
        //catch (NpgsqlException ex)
        //{
        //    string msgExcep = ex.Message.ToString();
        //    await trans.RollbackAsync();
        //}
        //finally
        //{
        //    await objConn.CloseAsync();
        //    await objConn.DisposeAsync();
        //}
        DeletesqlDataConnClose:
            return 1;
        }

    }
}

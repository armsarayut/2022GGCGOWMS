using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using GoWMS.Server.Data;

namespace GoWMS.Server.DataAccess
{
    public class SqlDAL
    {
        readonly private string connectionString = ConnGlobals.GetConnDBSQL();

        public DataTable RetrieveSqlData(SqlCommand cmd)
        {
            DataTable retDs = new DataTable();
            SqlConnection objConn = new SqlConnection(connectionString);
            try
            {
                objConn.Open();
            }
            catch (SqlException exp)
            {
                string msgexcep = exp.Message.ToString();
                goto RetrieveSqlDataConnClose;
            }

            try
            {
                cmd.Connection = objConn;
                SqlDataAdapter da = new SqlDataAdapter
                {
                    SelectCommand = cmd
                };
                da.SelectCommand.CommandTimeout = 0;
                da.Fill(retDs);
            }
            catch (SqlException ex)
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
        #region SyncSql
        public Boolean SyncUpdatesqlData(SqlCommand cmd)
        {
            Boolean bRet = false;
            SqlConnection objConn = new SqlConnection(connectionString);
            try
            {
                objConn.Open();
            }
            catch (SqlException exp)
            {
                string msgexcep = exp.Message.ToString();
                goto UpdatesqlDataConnClose;
            }
            SqlTransaction trans = objConn.BeginTransaction();
            try
            {
                cmd.Connection = objConn;
                cmd.Transaction = trans;

                cmd.ExecuteNonQuery();
                trans.Commit();
                bRet = true;
            }
            catch (SqlException ex)
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
        public Boolean SyncInsertsqlData(SqlCommand cmd)
        {
            Boolean bRet = false;
            SqlConnection objConn = new SqlConnection(connectionString);
            try
            {
                objConn.Open();
            }
            catch (SqlException exp)
            {
                string msgexcep = exp.Message.ToString();
                goto InsertsqlDataConnClose;
            }
            SqlTransaction trans = objConn.BeginTransaction();
            try
            {
                cmd.Connection = objConn;
                cmd.Transaction = trans;

                cmd.ExecuteNonQuery();
                trans.Commit();
                bRet = true;

            }
            catch (SqlException ex)
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
        public Boolean SyncDeletesqlData(SqlCommand cmd)
        {
            Boolean bRet = false;
            SqlConnection objConn = new SqlConnection(connectionString);
            try
            {
                objConn.Open();
            }
            catch (SqlException exp)
            {
                string msgexcep = exp.Message.ToString();
                goto DeletesqlDataConnClose;
            }
            SqlTransaction trans = objConn.BeginTransaction();
            try
            {
                cmd.Connection = objConn;
                cmd.Transaction = trans;
                cmd.ExecuteNonQuery();
                trans.Commit();
                bRet = true;
            }
            catch (SqlException ex)
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

        #endregion



        #region AsyncSql
        public Boolean AsyncUpdatesqlData(SqlCommand cmd)
        {
            Boolean bRet = false;
            SqlConnection objConn = new SqlConnection(connectionString);
            try
            {
                objConn.OpenAsync();
            }
            catch (SqlException exp)
            {
                string msgexcep = exp.Message.ToString();
                goto UpdatesqlDataConnClose;
            }
            SqlTransaction trans = objConn.BeginTransaction();
            try
            {
                cmd.Connection = objConn;
                cmd.Transaction = trans;

                cmd.ExecuteNonQueryAsync();
                trans.CommitAsync();
                bRet = true;

            }
            catch (SqlException ex)
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
        public Boolean AsyncInsertsqlData(SqlCommand cmd)
        {
            Boolean bRet = false;
            SqlConnection objConn = new SqlConnection(connectionString);
            try
            {
                objConn.OpenAsync();
            }
            catch (SqlException exp)
            {
                string msgexcep = exp.Message.ToString();
                goto InsertsqlDataConnClose;
            }
            SqlTransaction trans = objConn.BeginTransaction();
            try
            {
                cmd.Connection = objConn;
                cmd.Transaction = trans;

                cmd.ExecuteNonQueryAsync();
                trans.CommitAsync();
                bRet = true;

            }
            catch (SqlException ex)
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

        #endregion


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using System.Data.Common;
using Npgsql;
using NpgsqlTypes;
using Elsy.UoCommon.Models;
using Serilog;
namespace Elsy.UoCommon.Db
{
    // Assumes one connection string per provider in the config file.


    public class DbCommon<Conn, Trans, Cmd, Param>  
        where Conn : DbConnection, new()
        where Trans : DbTransaction
        where Cmd : DbCommand
        where Param : DbParameter, new ()
    {

        internal Conn conn;
        internal Trans transaction;
        string strConnect;


        public DbCommon(string _strConnect)
        {
            strConnect = _strConnect;
        }

        internal Conn openConnection()
        {
            if (conn == null)
            {
                conn = new Conn();
                conn.ConnectionString = strConnect;
            }

            if (conn.State == ConnectionState.Closed || conn.State ==
                        ConnectionState.Broken)
            {
                try
                {
                    conn.Open();
                    // LogHelper.log.Info("openConnection()");
                }
                catch (Exception ex)
                {
                    //LogHelper.log.Info(ex.ToString());
                }


            }
            return conn;
        }

        internal Conn closeConnection()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();

            }
            return conn;
        }


        public DataTable Select(string query, Param[] sqlParameter, bool isProc, bool openconnect, out string errmess)
        {
            errmess = null;
            conn = openConnection();

            DbDataAdapter da = conn as DbDataAdapter;
            da.SelectCommand.CommandTimeout = 120;

            Param oResParam = sqlParameter != null ?
                   sqlParameter.FirstOrDefault(f => f.Direction == ParameterDirection.Output) : null;



            if (isProc)
            {
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

            }

            if (sqlParameter != null)
            {
                da.SelectCommand.Parameters.AddRange(sqlParameter);
            }

            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                errmess = "";
                errmess = ex.Message;
                dt = null;
            }
            finally
            {
                if (!openconnect)
                {
                    conn.Close();
                }

            }

            return dt;

        }

        //public DataTable Select(string query, SqlParameter[] sqlParameter, bool isProc, out string errmess)
        //{
        //    errmess = null;
        //    conn = openConnection();
        //    SqlDataAdapter da = new SqlDataAdapter(query, conn);

        //    SqlParameter oResParam = sqlParameter != null ?
        //           sqlParameter.FirstOrDefault(f => f.Direction == ParameterDirection.Output) : null;



        //    if (isProc)
        //    {
        //        da.SelectCommand.CommandType = CommandType.StoredProcedure;

        //    }

        //    if (sqlParameter != null)
        //    {
        //        da.SelectCommand.Parameters.AddRange(sqlParameter);
        //    }

        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        da.Fill(dt);
        //    }
        //    catch (Exception ex)
        //    {

        //        if (ex is SqlException)
        //        {
        //            errmess = ((SqlException)ex).Message;
        //        }
        //        else
        //        {

        //            errmess = ex.InnerException.Message;
        //        }

        //        //Logger.Log.Error(ex.ToString());

        //        dt = null;
        //    }
        //    finally
        //    {
        //        //conn.Close();
        //    }

        //    return dt;

        //}

        //public int ExecuteNonQuery(string query, SqlParameter[] sqlParameter, bool openconnect)
        //{
        //    conn = openConnection();
        //    int result = 0;
        //    try
        //    {
        //        SqlParameter oResParam = null;
        //        SqlCommand m_Cmd = new SqlCommand(query);
        //        if (sqlParameter != null)
        //        {
        //            oResParam = sqlParameter.First(f => f.Direction == ParameterDirection.ReturnValue);
        //            m_Cmd.Parameters.AddRange(sqlParameter);
        //        }



        //        if (transaction != null)
        //        {
        //            m_Cmd.Transaction = transaction;
        //        }

        //        // m_Cmd.ExecuteScalar();
        //        result = m_Cmd.ExecuteNonQuery();


        //        if (oResParam != null)
        //        {
        //            string oRes = String.Empty;
        //            oRes = oResParam.Value.ToString();
        //            int.TryParse(oRes, out result);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        if (transaction != null)
        //        {
        //            transaction.Rollback();
        //        }

        //        //LogHelper.log.Error("ExecuteNonQuery: " + ex.ToString());

        //    }
        //    finally
        //    {
        //        if (!openconnect)
        //        {
        //            conn.Close();
        //        }
        //    }

        //    return result;


        //}

        //public object ExecuteScalar(string query, SqlParameter[] sqlParameter, bool isProc)
        //{
        //    conn = openConnection();

        //    object result = 0;

        //    try
        //    {
        //        SqlParameter oResParam = sqlParameter != null ?
        //            sqlParameter.FirstOrDefault(f => f.Direction == ParameterDirection.Output) : null;
        //        SqlCommand m_Cmd = new SqlCommand(query, conn);

        //        if (isProc)
        //        {
        //            m_Cmd.CommandType = CommandType.StoredProcedure;

        //        }

        //        if (sqlParameter != null)
        //        {
        //            m_Cmd.Parameters.AddRange(sqlParameter);
        //        }

        //        if (transaction != null)
        //        {
        //            m_Cmd.Transaction = transaction;
        //        }

        //        // result = m_Cmd.ExecuteNonQuery();

        //        result = m_Cmd.ExecuteScalar();

        //        if (oResParam != null)
        //        {
        //            string oRes = String.Empty;
        //            oRes = oResParam.Value.ToString();
        //            result = oRes;
        //            // int.TryParse(oRes, out result);
        //        }

        //    }

        //    catch (Exception ex)
        //    {

        //        if (transaction != null)
        //        {
        //            transaction.Rollback();
        //        }

        //        //  LogHelper.log.Error("UpdateInsert: " + ex.ToString()
        //        //      + "  " +
        //        //      sqlParameter.Select(f => f.ParameterName + " " + f.Value));



        //    }
        //    finally
        //    {
        //        //if (!openconnect)
        //        //{
        //        //    conn.Close();
        //        //}
        //    }

        //    return result;


        //}

        //public int UpdateInsert(string query, SqlParameter[] sqlParameter, bool openconnect)
        //{
        //    conn = openConnection();
        //    int result = 0;
        //    try
        //    {
        //        SqlCommand m_Cmd = new SqlCommand(query, conn)
        //        { CommandType = CommandType.StoredProcedure };
        //        m_Cmd.Parameters.AddRange(sqlParameter);
        //        result = m_Cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        // conn.Close();
        //    }

        //    return result;
        //}


        //public int UpdateInsert(string query, SqlParameter[] sqlParameter)
        //{
        //    int result = 0;
        //    try
        //    {
        //        SqlParameter oResParam = sqlParameter != null ?
        //            sqlParameter.FirstOrDefault(f => f.Direction == ParameterDirection.Output) : null;
        //        SqlCommand m_Cmd = new SqlCommand(query, conn)
        //        {
        //            CommandType = CommandType.StoredProcedure,

        //        };

        //        if (sqlParameter != null)
        //        {
        //            m_Cmd.Parameters.AddRange(sqlParameter);
        //        }

        //        if (transaction != null)
        //        {
        //            m_Cmd.Transaction = transaction;
        //        }
        //        result = m_Cmd.ExecuteNonQuery();

        //        if (oResParam != null)
        //        {
        //            string oRes = String.Empty;
        //            oRes = oResParam.Value.ToString();
        //            int.TryParse(oRes, out result);
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        if (transaction != null)
        //        {
        //            transaction.Rollback();
        //        }

        //        //  LogHelper.log.Error("UpdateInsert: " + ex.ToString() + "  " + sqlParameter.Select(f => f.ParameterName + " " + f.Value));

        //    }
        //    finally
        //    {
        //        //if (!openconnect)
        //        //{
        //        //    conn.Close();
        //        //}
        //    }

        //    return result;
        //}

        //public object UpdateInsert1(string query, SqlParameter[] sqlParameter, bool isProc, bool openconnect, out string errmess)
        //{
        //    errmess = null;

        //    conn = openConnection();

        //    object result = 0;

        //    try
        //    {
        //        SqlParameter oResParam = sqlParameter != null ?
        //            sqlParameter.FirstOrDefault(f => f.Direction == ParameterDirection.Output) : null;
        //        SqlCommand m_Cmd = new SqlCommand(query, conn);

        //        if (isProc)
        //        {
        //            m_Cmd.CommandType = CommandType.StoredProcedure;

        //        }

        //        if (sqlParameter != null)
        //        {
        //            m_Cmd.Parameters.AddRange(sqlParameter);
        //        }

        //        if (transaction != null)
        //        {
        //            m_Cmd.Transaction = transaction;
        //        }
        //        result = m_Cmd.ExecuteNonQuery();

        //        //result = m_Cmd.ExecuteScalar();

        //        if (oResParam != null)
        //        {
        //            string oRes = String.Empty;
        //            oRes = oResParam.Value.ToString();
        //            result = oRes;
        //            // int.TryParse(oRes, out result);
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        if (transaction != null)
        //        {
        //            transaction.Rollback();
        //        }




        //        if (ex is SqlException)
        //        {
        //            errmess = ((SqlException)ex).Message;
        //        }
        //        else
        //        {
        //            // Logger.Log.Error(ex.ToString());
        //        }

        //    }
        //    finally
        //    {
        //        if (!openconnect)
        //        {
        //            conn.Close();
        //        }
        //    }

        //    return result;
        //}

        //public object SelectCellValue(string query, SqlParameter[] sqlParameter)
        //{

        //    conn = openConnection();

        //    object result = null;
        //    try
        //    {
        //        SqlCommand m_Cmd = new SqlCommand(query, conn);

        //        if (sqlParameter != null)
        //        {
        //            m_Cmd.Parameters.AddRange(sqlParameter);
        //        }
        //        using (SqlDataReader reader = m_Cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                result = reader[0];
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //LogHelper.log.Error("SelectValue: " + ex.ToString());
        //    }
        //    finally
        //    {
        //        // conn.Close();
        //    }



        //    return result;

        //}

        //public object SelectCellValue(string query, SqlParameter[] sqlParameter, bool openconnect)
        //{
        //    conn = openConnection();

        //    object result = null;
        //    try
        //    {
        //        SqlCommand m_Cmd = new SqlCommand(query, conn);

        //        if (sqlParameter != null)
        //        {
        //            m_Cmd.Parameters.AddRange(sqlParameter);
        //        }
        //        using (SqlDataReader reader = m_Cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                result = reader[0];
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        // LogHelper.log.Error("SelectValue: " + ex.ToString());
        //    }
        //    finally
        //    {
        //        if (!openconnect)
        //        {
        //            conn.Close();
        //        }

        //    }

        //    return result;

        //}

        //public string ExecuteReader(string query, SqlParameter[] sqlParameter, bool openconnect)
        //{
        //    conn = openConnection();
        //    string result = String.Empty;
        //    try
        //    {
        //        SqlCommand m_Cmd = new SqlCommand(query, conn);

        //        //if (transaction != null)
        //        //{
        //        //    m_Cmd.Transaction = transaction;
        //        //}

        //        if (sqlParameter != null)
        //        {
        //            m_Cmd.Parameters.AddRange(sqlParameter);
        //        }
        //        using (SqlDataReader reader = m_Cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {

        //                result = reader.GetValue(0).ToString();

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // LogHelper.log.Error("SelectValue: " + ex.ToString());
        //    }
        //    finally
        //    {
        //        if (!openconnect) conn.Close();
        //    }

        //    return result;

        //}
    }



}

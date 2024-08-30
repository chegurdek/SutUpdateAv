using System;
using System.Collections.Generic;
using System.Linq;
//using System.Data.SqlClient;
using System.Data;
using System.Text;
using Npgsql;
using NpgsqlTypes;
namespace Elsy.UoCommon.Db
{
    public  class Db
    {

        internal NpgsqlConnection conn;
        internal NpgsqlTransaction  transaction;
        string  strConnect;


        public Db(string _strConnect)
        {
        //!!!!!!!!!
        //https://www.npgsql.org/doc/release-notes/7.0.html#a-namecommandtypestoredprocedure-commandtypestoredprocedure-now-invokes-procedures-instead-of-functions
           AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);
            strConnect = _strConnect;
        }


        internal NpgsqlConnection  openConnection()
        {
            if (conn == null)
            {
                conn = new NpgsqlConnection (strConnect);
            }

            if (conn.State == ConnectionState.Closed || conn.State ==
                        ConnectionState.Broken)
            {
                try
                {
                    conn.Open();
                    //Logger.Log.Debug("open");
                }
                catch (Exception ex)
                {
                    Logger.Log.Error(ex.ToString());
                }


            }
            return conn;
        }

        internal NpgsqlConnection  closeConnection()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();

            }
            return conn;
        }

        string getLogReqString(NpgsqlCommand  cmd)
        {

            // string res = null;
            StringBuilder sb = new StringBuilder();
            sb.Append("exec " + cmd.CommandText + " ");
            string formstr = "{0}={1}, ";

            foreach (NpgsqlParameter parameter in cmd.Parameters)
            {

                try
                {
                    if (parameter.NpgsqlDbType ==  NpgsqlDbType.Array)
                       // SqlDbType.Structured)
                    {
                        //var fg = getLogTableString((DataTable)parameter.Value);

                        sb.AppendFormat("{0}=({1})", parameter.ParameterName,  getLogTableString((DataTable)parameter.Value));
                    }
                    else
                    {

                        sb.AppendFormat(formstr, parameter.ParameterName, parameter.Value);
                    }

                }
                catch (Exception ex)
                {
                    
                }




            }

            return sb?.ToString()?.Trim(',');

        }

        string getLogTableString(DataTable dt)
        {

            // string res = null;
            StringBuilder sb = new StringBuilder();
            string formstr = "{0}={1},";

            foreach (DataRow dr in dt.Rows)
            {
                try
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dr.Field<object>(dc.ColumnName) != null)
                        {
                            sb.AppendFormat(formstr, dc.ColumnName, dr.Field<object>(dc.ColumnName)?.ToString());
                        }
                      
                    }

                }
                catch (Exception ex)
                {

                }




            }

            return sb?.ToString()?.Trim(',');

        }


        string getLogResString(DataTable dt)
        {

            // string res = null;
            StringBuilder sb = new StringBuilder();
            sb.Append("result ");
            sb.Append(getLogTableString(dt));
            return sb?.ToString();

            //string formstr = "{0}={1}, ";

            //foreach (DataRow dr in dt.Rows)
            //{
            //    try
            //    {
            //        foreach (DataColumn dc in dt.Columns)
            //        {
            //            sb.AppendFormat(formstr, dc.ColumnName, dr.Field<object>(dc.ColumnName)?.ToString());
            //        }

            //    }
            //    catch (Exception ex)
            //    {

            //    }




            //}

            //return sb?.ToString()?.Trim(',');

        }


        public DataTable Select(string query, NpgsqlParameter[] sqlParameter,
        bool isProc, bool openconnect, bool isLog,  out string errmess)
        {


            errmess = null;
            conn = openConnection();

            if (conn.State != ConnectionState.Open)
            {
                throw new ConnectException();
            }



            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, conn);
            da.SelectCommand.CommandTimeout = 120;

            NpgsqlParameter oResParam = sqlParameter != null ?
                   sqlParameter.FirstOrDefault(f => f.Direction == ParameterDirection.Output) : null;

            if (isProc)
            {
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
            }

            if (sqlParameter != null)
            {
                da.SelectCommand.Parameters.AddRange(sqlParameter);
            }

            if (isLog)
            {
                Logger.Log.Info(getLogReqString(da.SelectCommand));
            }


            DataTable dt = new DataTable();
            try
            {
               
                da.Fill(dt);
                Logger.Log.Info(getLogResString(dt));

            }
            catch (Exception ex)
            {
                errmess = "";
                errmess = ex.Message;
                dt = null;

                Logger.Log.Error(ex.ToString());
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




        public DataTable Select(string query, NpgsqlParameter[] sqlParameter,
            bool isProc, bool openconnect, out string errmess)
        {
           

            errmess = null;
            conn = openConnection();

            if (conn.State != ConnectionState.Open)
            {
                throw new ConnectException();
            }

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(query, conn);
            da.SelectCommand.CommandTimeout = 120;

            NpgsqlParameter oResParam = sqlParameter != null ?
                   sqlParameter.FirstOrDefault(f => f.Direction == ParameterDirection.Output) : null;



            if (isProc)
            {

                AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);
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

                Logger.Log.Error(ex.ToString());
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

    

     
        public object UpdateInsert1(string query, NpgsqlParameter[] sqlParameter, bool isProc, bool openconnect, out string errmess)
        {
            errmess = null;

            conn = openConnection();


            if (conn.State != ConnectionState.Open)
            {
                throw new ConnectException();
            }

            object result = 0;

            try
            {
                NpgsqlParameter oResParam = sqlParameter != null ?
                    sqlParameter.FirstOrDefault(f => f.Direction == ParameterDirection.Output) : null;
                NpgsqlCommand m_Cmd = new NpgsqlCommand(query, conn);

                if (isProc)
                {
                    m_Cmd.CommandType = CommandType.StoredProcedure;
                   
                }

                if (sqlParameter != null)
                {
                    m_Cmd.Parameters.AddRange(sqlParameter);
                }

                if (transaction != null)
                {
                    m_Cmd.Transaction = transaction;
                }
                result = m_Cmd.ExecuteNonQuery();

                //result = m_Cmd.ExecuteScalar();

                if (oResParam != null)
                {
                    string oRes = String.Empty;
                    oRes = oResParam.Value.ToString();
                    result = oRes;
                    // int.TryParse(oRes, out result);
                }
                
            }
            catch (Exception ex)
            {
               
                if (transaction != null)
                {
                    transaction.Rollback();
                }

               
           

                if (ex is PostgresException)
                {
                    errmess = ((PostgresException)ex).Message;
                }
                
                
                Logger.Log.Error(ex.ToString());
                

            }
            finally
            {
                if (!openconnect)
                {
                    conn.Close();
                }
            }

            return result;
        }


        public object UpdateInsert1(string query, NpgsqlParameter[] sqlParameter, bool isProc, bool openconnect, bool isLog,  out string errmess)
        {
            errmess = null;

            conn = openConnection();

            //if (conn.State != ConnectionState.Open)
            //{
            //    throw new ConnectException();
            //}

          
            object result = 0;

            try
            {
                

                NpgsqlParameter oResParam = sqlParameter != null ?
                    sqlParameter.FirstOrDefault(f => f.Direction == ParameterDirection.Output) : null;
                NpgsqlCommand m_Cmd = new NpgsqlCommand(query, conn);

                if (isProc)
                {
                    AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", false);
                    m_Cmd.CommandType = CommandType.StoredProcedure;

                }

                if (sqlParameter != null)
                {

                   // m_Cmd.Parameters.Clear();
                    m_Cmd.Parameters.AddRange(sqlParameter);
                }

                if (transaction != null)
                {
                    m_Cmd.Transaction = transaction;
                }

                if (isLog)
                {
                    Logger.Log.Info(getLogReqString(m_Cmd));
                }

                result = m_Cmd.ExecuteNonQuery();

                //result = m_Cmd.ExecuteScalar();

                if (oResParam != null)
                {
                    string oRes = String.Empty;
                    oRes = oResParam.Value.ToString();
                    result = oRes;
                    // int.TryParse(oRes, out result);
                }

            }

           // 42883: процедура uo_add_update_record
           // (oRes => unknown,
           // name => text,
           // descr => text,
           // bin => bytea,
           // soid => integer,
           // createduser => text,
           // filename => text,
           // isdisabled => integer,
           // components => bigint)
           // не существует\r\n\r\nPOSITION: 6"}


            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                if (ex is PostgresException)
                {
                    errmess = ((PostgresException)ex).Message;
                }
                Logger.Log.Error(ex.ToString());
            }
            finally
            {
             

                if (!openconnect)
                {
                    conn.Close();
                }
            }
            Logger.Log.Info($"result {result}");
            return result;
        }
        public object SelectCellValue(string query, NpgsqlParameter[] sqlParameter)
        {
            conn = openConnection();
            object result = null;
            try
            {
                NpgsqlCommand m_Cmd = new NpgsqlCommand(query, conn);

                if (sqlParameter != null)
                {
                    m_Cmd.Parameters.AddRange(sqlParameter);
                }
                using (NpgsqlDataReader reader = m_Cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = reader[0];
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }
            finally
            {
                // conn.Close();
            }



            return result;

        }

        public  object SelectCellValue(string query, NpgsqlParameter[] sqlParameter, bool openconnect)
        {
            conn = openConnection();

            object result = null;
            try
            {
                NpgsqlCommand m_Cmd = new NpgsqlCommand(query, conn);

                if (sqlParameter != null)
                {
                    m_Cmd.Parameters.AddRange(sqlParameter);
                }
                using (NpgsqlDataReader reader = m_Cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = reader[0];
                    }
                }

            }
            catch (Exception ex)
            {
               // LogHelper.log.Error("SelectValue: " + ex.ToString());
            }
            finally
            {
                if (!openconnect)
                {
                    conn.Close();
                }

            }

            return result;

        }

    

        public int ExecuteNonQuery(string query, NpgsqlParameter[] sqlParameter, bool openconnect)
        {
            conn = openConnection();
            int result = 0;
            try
            {
                NpgsqlParameter oResParam = null;
                NpgsqlCommand m_Cmd = new NpgsqlCommand(query);
                if (sqlParameter != null)
                {
                    oResParam = sqlParameter.First(f => f.Direction == ParameterDirection.ReturnValue);
                    m_Cmd.Parameters.AddRange(sqlParameter);
                }



                if (transaction != null)
                {
                    m_Cmd.Transaction = transaction;
                }

                // m_Cmd.ExecuteScalar();
                result = m_Cmd.ExecuteNonQuery();


                if (oResParam != null)
                {
                    string oRes = String.Empty;
                    oRes = oResParam.Value.ToString();
                    int.TryParse(oRes, out result);
                }

            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                Logger.Log.Error(ex.ToString());

            }
            finally
            {
                if (!openconnect)
                {
                    conn.Close();
                }
            }

            return result;


        }

        public object ExecuteScalar(string query, NpgsqlParameter[] sqlParameter, bool isProc)
        {
            conn = openConnection();

            object result = 0;

            try
            {
                NpgsqlParameter oResParam = sqlParameter != null ?
                    sqlParameter.FirstOrDefault(f => f.Direction == ParameterDirection.Output) : null;
                NpgsqlCommand m_Cmd = new NpgsqlCommand(query, conn);

                if (isProc)
                {
                    m_Cmd.CommandType = CommandType.StoredProcedure;

                }

                if (sqlParameter != null)
                {
                    m_Cmd.Parameters.AddRange(sqlParameter);
                }

                if (transaction != null)
                {
                    m_Cmd.Transaction = transaction;
                }

                // result = m_Cmd.ExecuteNonQuery();

                result = m_Cmd.ExecuteScalar();

                if (oResParam != null)
                {
                    string oRes = String.Empty;
                    oRes = oResParam.Value.ToString();
                    result = oRes;
                    // int.TryParse(oRes, out result);
                }

            }

            catch (Exception ex)
            {

                if (transaction != null)
                {
                    transaction.Rollback();
                }

                Logger.Log.Error(ex.ToString());


            }
            finally
            {
                //if (!openconnect)
                //{
                //    conn.Close();
                //}
            }

            return result;


        }

        public int UpdateInsert(string query, NpgsqlParameter[] sqlParameter, bool openconnect)
        {
            conn = openConnection();
            int result = 0;
            try
            {
                NpgsqlCommand m_Cmd = new NpgsqlCommand(query, conn)
                { CommandType = CommandType.StoredProcedure };
                m_Cmd.Parameters.AddRange(sqlParameter);
                result = m_Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.ToString());
            }
            finally
            {
                // conn.Close();
            }

            return result;
        }

        public int UpdateInsert(string query, NpgsqlParameter[] sqlParameter)
        {
            int result = 0;
            try
            {
                NpgsqlParameter oResParam = sqlParameter != null ?
                    sqlParameter.FirstOrDefault(f => f.Direction == ParameterDirection.Output) : null;
                NpgsqlCommand m_Cmd = new NpgsqlCommand(query, conn)
                {
                    CommandType = CommandType.StoredProcedure,

                };

                if (sqlParameter != null)
                {
                    m_Cmd.Parameters.AddRange(sqlParameter);
                }

                if (transaction != null)
                {
                    m_Cmd.Transaction = transaction;
                }
                result = m_Cmd.ExecuteNonQuery();

                if (oResParam != null)
                {
                    string oRes = String.Empty;
                    oRes = oResParam.Value.ToString();
                    int.TryParse(oRes, out result);
                }

            }
            catch (Exception ex)
            {

                if (transaction != null)
                {
                    transaction.Rollback();
                }

                Logger.Log.Error(ex.ToString());

            }
            finally
            {
                //if (!openconnect)
                //{
                //    conn.Close();
                //}
            }

            return result;
        }



    }

}

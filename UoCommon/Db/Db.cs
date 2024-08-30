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
using Microsoft.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;

namespace Elsy.UoCommon.Db
{
    // Assumes one connection string per provider in the config file.
   



    public  class Db 
    {


        //internal  SqlConnection conn;
        internal  DbTransaction  transaction;
        string  strConnect;
        string providerName;
        DbProviderFactory dbProvider;

        //DbProviderFactory dbProvider = DbProviderFactories.GetFactory("Microsoft.Data.SqlClient");

        DbConnection dbConnection;

        //

        // con.ConnectionString = WebConfigurationManager.ConnectionStrings["Northwind"].ConnectionString;


        DbProviderFactory DbProvider
        {
            get
            {
                if (!String.IsNullOrEmpty(providerName))
                {
                    if (providerName == "NPGSQL")
                    {
                        DbProviderFactories.RegisterFactory("Npgsql", NpgsqlFactory.Instance);
                        dbProvider = DbProviderFactories.GetFactory("Npgsql");
                        AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);
                    }
                    else if (providerName == "MSSQL")
                    {
                        DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);
                        dbProvider = DbProviderFactories.GetFactory("Microsoft.Data.SqlClient");
                      
                    }
                    else
                    {
                        DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);
                        dbProvider = DbProviderFactories.GetFactory("Microsoft.Data.SqlClient");
                    }

                }
                else
                {
                    DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);
                    dbProvider = DbProviderFactories.GetFactory("Microsoft.Data.SqlClient");
                }

                return dbProvider;
            }
        }


        public Db(string _strConnect)
        {
            strConnect = _strConnect;
        }


        public Db(string _strConnect, string _providerName)
        {

            providerName = _providerName;


            //if (_providerName == "NPGSQL")
            //{
            //    //DbProviderFactories.RegisterFactory("Npgsql", NpgsqlFactory.Instance);
            //    _providerName = "Npgsql";

            //    ////!!!!!!
            //    //https://www.npgsql.org/doc/release-notes/7.0.html#a-namecommandtypestoredprocedure-commandtypestoredprocedure-now-invokes-procedures-instead-of-functions
            //    AppContext.SetSwitch("Npgsql.EnableStoredProcedureCompatMode", true);

            //}


            //if (_providerName == "MSSQL")
            //{
            //     DbProviderFactories.RegisterFactory("System.Data.SqlClient",
            //         SqlClientFactory.Instance);
            //    //System.Data.SqlClient
            //    _providerName = "System.Data.SqlClient";
            //}

            //if (_providerName == "MSSQL")
            //{
            //    //DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient",
            //       // SqlClientFactory.Instance);
            //    //System.Data.SqlClient
            //    _providerName = "Microsoft.Data.SqlClient";
            //}


            //dbProvider = DbProviderFactories.GetFactory(_providerName);


            strConnect = _strConnect;



            Log.Information(_providerName);

            Log.Information(_strConnect);




            //   getConnectionStringByProvider(_providerName);
            //dbConnection = dbProvider.CreateConnection();
            // dbConnection = dbProvider.CreateConnection();       
        }


       //string getConnectionStringByProvider(string providerName)
       // {
       //     // Return null on failure.
       //     string returnValue = null;

       //     // Get the collection of connection strings.

       //     return providerName;
       // }


        public DbConnection openConnection()
        {

            if (dbConnection == null)
            {
                dbConnection = DbProvider.CreateConnection();
                    //dbProvider.CreateConnection();
                dbConnection.ConnectionString = strConnect;
            }

            if (dbConnection.State == ConnectionState.Closed || dbConnection.State ==
                        ConnectionState.Broken)
            {
                try
                {
                    dbConnection.Open();
                    //Logger.Log.Debug("open");
                }
                catch (Exception ex)
                {

                    Log.Information(ex.ToString());
                    // Logger.Log.Error(ex.ToString());
                }


            }


            return dbConnection;  ///?????
        }

        internal void closeConnection()
        {
            if (dbConnection.State == ConnectionState.Open)
            {
                dbConnection.Close();

            }
          
        }

        string getLogReqString(DbCommand  cmd)
        {

            // string res = null;
            StringBuilder sb = new StringBuilder();
            sb.Append("exec " + cmd.CommandText + " ");
            string formstr = "{0}={1}, ";

            foreach (DbParameter parameter in cmd.Parameters)
            {

                try
                {

                    var prmtr = parameter as NpgsqlParameter;

                    if (prmtr != null)
                    {

                        if (prmtr.NpgsqlDbType ==  NpgsqlDbType.Array)
                        {
                            //var fg = getLogTableString((DataTable)parameter.Value);

                            sb.AppendFormat("{0}=({1})", parameter.ParameterName, getLogTableString((DataTable)parameter.Value));
                        }
                        else
                        {

                            sb.AppendFormat(formstr, parameter.ParameterName, parameter.Value);
                        }
                    }


                    var  sprmtr = parameter as SqlParameter;

                    if (sprmtr != null)
                    {

                        if (sprmtr.SqlDbType ==  SqlDbType.Structured)
                        {
                            //var fg = getLogTableString((DataTable)parameter.Value);

                            sb.AppendFormat("{0}=({1})", parameter.ParameterName, getLogTableString((DataTable)parameter.Value));
                        }
                        else
                        {

                            sb.AppendFormat(formstr, parameter.ParameterName, parameter.Value);
                        }
                    }




                }
                catch (Exception ex)
                {
                    Log.Information(ex.ToString());
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
                    Log.Information(ex.ToString());
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

        public DataTable SelectNpgsql(string query, NpgsqlParameter [] dbParameters, bool isProc, bool openconnect, bool isLog, out string errmess)
        {

            errmess = null;
            openConnection();

            if (dbConnection.State != ConnectionState.Open)
            {
                throw new ConnectException();
            }

            NpgsqlDataAdapter da = new NpgsqlDataAdapter();
            NpgsqlCommand m_Cmd = new NpgsqlCommand();
            m_Cmd.CommandText = query;
            m_Cmd.Connection = (NpgsqlConnection)dbConnection;

            da.SelectCommand = m_Cmd;
            da.SelectCommand.CommandTimeout = 120;

            //Parameter oResParam = dbParameters != null ?
            //       dbParameters.FirstOrDefault(f => f.Direction == ParameterDirection.Output) : null;


            if (isProc)
            {

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
            }

            if (dbParameters != null)
            {

               // var rr = dbParameters.ToParamerArray(m_Cmd);
                m_Cmd.Parameters.AddRange(dbParameters);

            }

            if (isLog)
            {
                // Logger.Log.Info(getLogReqString(da.SelectCommand));
            }


            DataTable dt = new DataTable();
            try
            {

                da.Fill(dt);
                //Logger.Log.Info(getLogResString(dt));

            }

            //42883: функция uo_add_update_fact_5(updid => integer, creatuser => text, vestid => smallint, idline => smallint, DevIds => integer[]) не существует

            catch (Exception ex)
            {
                errmess = "";
                errmess = ex.Message;
                dt = null;

                Log.Information(ex.ToString());
            }
            finally
            {
                if (!openconnect)
                {
                    dbConnection.Close();
                }

            }

            return dt;

        }



        public DataTable Select(string query, Parameter [] dbParameters, bool isProc, bool openconnect, bool isLog, out string errmess)

        {
         
            errmess = null;
            openConnection();

            if (dbConnection.State != ConnectionState.Open)
            {
                throw new ConnectException();
            }


            DbDataAdapter da = dbProvider.CreateDataAdapter();
            DbCommand m_Cmd = dbProvider.CreateCommand();
            m_Cmd.CommandText = query;
            m_Cmd.Connection = dbConnection;

            da.SelectCommand = m_Cmd;
            da.SelectCommand.CommandTimeout = 120;

            //Parameter oResParam = dbParameters != null ?
            //       dbParameters.FirstOrDefault(f => f.Direction == ParameterDirection.Output) : null;


            if (isProc)
            {

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
            }

            if (dbParameters != null)
            {

                var rr =  dbParameters.ToParamerArray(m_Cmd);
                m_Cmd.Parameters.AddRange(dbParameters.ToParamerArray(m_Cmd));

            }

            if (isLog)
            {
               // Logger.Log.Info(getLogReqString(da.SelectCommand));
            }


            DataTable dt = new DataTable();
            try
            {
               
                da.Fill(dt);
                //Logger.Log.Info(getLogResString(dt));

            }
            catch (Exception ex)
            {
                errmess = "";
                errmess = ex.Message;
                dt = null;

                Log.Information(ex.ToString());
            }
            finally
            {
                if (!openconnect)
                {
                    dbConnection.Close();
                }

            }

            return dt;

        }




        public DataTable Select(string query, Parameter[] sqlParameter,
            bool isProc, bool openconnect, out string errmess)
        {
           

            errmess = null;
            openConnection();

            if (dbConnection.State != ConnectionState.Open)
            {
                throw new ConnectException();
            }


            DbDataAdapter da = dbProvider.CreateDataAdapter();
            DbCommand m_Cmd = dbProvider.CreateCommand();
            m_Cmd.CommandText = query;
            m_Cmd.Connection = dbConnection;


            da.SelectCommand = m_Cmd;
            da.SelectCommand.CommandTimeout = 120;


            //DbParameter oResParam = sqlParameter != null ?
            //       sqlParameter.FirstOrDefault(f => f.Direction == ParameterDirection.Output) : null;

            DbParameter[] dbParameters = null;

            if (isProc)
            {
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

            }

            if (sqlParameter != null)
            {

                dbParameters = sqlParameter.ToParamerArray(m_Cmd);
                m_Cmd.Parameters.AddRange(dbParameters);

            }

            DataTable dt = new DataTable();
            try
            {            
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                //42883: функция uo_get_dev_choice(lineId => integer, stationId => smallint, updId => integer, isAdded => smallint) не существует


                errmess = "";
                errmess = ex.Message;
                dt = null;

                Log.Information(ex.ToString());



                //Logger.Log.Error(ex.ToString());
            }
            finally
            {
                if (!openconnect)
                {
                    dbConnection.Close();
                }

            }

            return dt;

        }




        //public object UpdateInsert1(string query, SqlParameter[] sqlParameter, bool isProc, bool openconnect, out string errmess)
        //{
        //    errmess = null;

        //    openConnection();


        //    if (dbConnection.State != ConnectionState.Open)
        //    {
        //        throw new ConnectException();
        //    }

        //    object result = 0;

        //    try
        //    {
        //        SqlParameter oResParam = sqlParameter != null ?
        //            sqlParameter.FirstOrDefault(f => f.Direction == ParameterDirection.Output) : null;
        //        SqlCommand m_Cmd = new SqlCommand(query, dbConnection);

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


        //        Logger.Log.Error(ex.ToString());


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

        public object UpdateInsertNpgsql(string query, NpgsqlParameter[] sqlParameter,
            bool isProc, bool openconnect, bool isLog, out string errmess)
        {
            errmess = null;

            openConnection();

            if (dbConnection.State != ConnectionState.Open)
            {
                throw new ConnectException();
            }


            object result = 0;

            try
            {
                //Parameter oResParam = sqlParameter != null ?
                //    sqlParameter.FirstOrDefault(f => f.Direction == ParameterDirection.Output) : null;
                NpgsqlCommand m_Cmd = new NpgsqlCommand();
                m_Cmd.CommandText = query;
                m_Cmd.Connection = (NpgsqlConnection)dbConnection;

           
                if (isProc)
                {
                    m_Cmd.CommandType = CommandType.StoredProcedure;
                }

                if (sqlParameter != null)
                {
                 
                    m_Cmd.Parameters.AddRange(sqlParameter);

                    //m_Cmd.Parameters.AddRange(sqlParameter);
                }

                if (transaction != null)
                {
                    m_Cmd.Transaction = (NpgsqlTransaction)transaction;
                }

                if (isLog)
                {
                    //  Logger.Log.Info(getLogReqString(m_Cmd));
                }



                result = m_Cmd.ExecuteNonQuery();

                // result = m_Cmd.ExecuteScalar();

                NpgsqlParameter oResParam = sqlParameter != null ?  sqlParameter.FirstOrDefault(f => f.Direction == ParameterDirection.Output) : null;
           
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
                //42883: функция uo_update_disable_field(updtId => integer, isDisadled => integer) не существует\r\n\r\nPOSITION: 15

                //Процедура с данными именем и типами аргументов не найдена. Возможно, вам следует добавить явные приведения типов.
                //{"42883: процедура uo_add_update_record(oRes => unknown,
                //name => text,
                //descr => text,
                //bin => bytea,
                //soid => integer,
                //createduser => text,
                //filename => text,
                //isdisabled => smallint,
                //components => bigint
                //) не существует\r\n\r\nPOSITION: 6"}

                if (transaction != null)
                {
                    transaction.Rollback();
                }




                if (ex is SqlException)
                {
                    errmess = ((SqlException)ex).Message;
                }


                Log.Information(ex.ToString());


            }
            finally
            {
                if (!openconnect)
                {
                    dbConnection.Close();
                }
            }

            //Logger.Log.Info($"result {result}");

            return result;
        }


        public object UpdateInsert1(string query, Parameter[] sqlParameter, bool isProc, bool openconnect, bool isLog,  out string errmess)
        {
            errmess = null;

            openConnection();

            if (dbConnection.State != ConnectionState.Open)
            {
                throw new ConnectException();
            }


            object result = 0;

            try
            {
                //Parameter oResParam = sqlParameter != null ?
                //    sqlParameter.FirstOrDefault(f => f.Direction == ParameterDirection.Output) : null;
                DbCommand m_Cmd = dbProvider.CreateCommand();
                m_Cmd.CommandText = query;
                m_Cmd.Connection = dbConnection;

                DbParameter[] dbParameters = null; 
         
                if (isProc)
                {
                    m_Cmd.CommandType = CommandType.StoredProcedure;
               
                  

                }

                if (sqlParameter != null)
                {
                    dbParameters = sqlParameter.ToParamerArray(m_Cmd);
                    m_Cmd.Parameters.AddRange(dbParameters);
              

                    //m_Cmd.Parameters.AddRange(sqlParameter);
                }

                if (transaction != null)
                {
                    m_Cmd.Transaction = transaction;
                }

                if (isLog)
                {
                  //  Logger.Log.Info(getLogReqString(m_Cmd));
                }

        

               result = m_Cmd.ExecuteNonQuery();

               // result = m_Cmd.ExecuteScalar();

                DbParameter oResParam = dbParameters != null ?
                    dbParameters.FirstOrDefault(f => f.Direction == ParameterDirection.Output) : null;


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
                //42883: функция uo_update_disable_field(updtId => integer, isDisadled => integer) не существует\r\n\r\nPOSITION: 15

                //Процедура с данными именем и типами аргументов не найдена. Возможно, вам следует добавить явные приведения типов.
                //{"42883: процедура uo_add_update_record(oRes => unknown,
                //name => text,
                //descr => text,
                //bin => bytea,
                //soid => integer,
                //createduser => text,
                //filename => text,
                //isdisabled => smallint,
                //components => bigint
                //) не существует\r\n\r\nPOSITION: 6"}

                if (transaction != null)
                {
                    transaction.Rollback();
                }




                if (ex is SqlException)
                {
                    errmess = ((SqlException)ex).Message;
                }


                Log.Information(ex.ToString());


            }
            finally
            {
                if (!openconnect)
                {
                    dbConnection.Close();
                }
            }

            //Logger.Log.Info($"result {result}");

            return result;
        }

        public object SelectCellValue(string query, DbParameter[] sqlParameter)
        {

            openConnection();

            object result = null;
            try
            {

                DbCommand m_Cmd = dbProvider.CreateCommand();
                m_Cmd.CommandText = query;
                m_Cmd.Connection = dbConnection;

              
                if (sqlParameter != null)
                {
                    m_Cmd.Parameters.AddRange(sqlParameter);
                }
                using (DbDataReader reader = m_Cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = reader[0];
                    }
                }

            }
            catch (Exception ex)
            {
               // Logger.Log.Error(ex.ToString());
            }
            finally
            {
                // conn.Close();
            }



            return result;

        }

        public object SelectCellValue(string query, DbParameter[] sqlParameter, bool openconnect)
        {
            openConnection();

            object result = null;
            try
            {

                DbCommand m_Cmd = dbProvider.CreateCommand();
                m_Cmd.CommandText = query;
                m_Cmd.Connection = dbConnection;


                if (sqlParameter != null)
                {
                    m_Cmd.Parameters.AddRange(sqlParameter);
                }
                using (DbDataReader reader = m_Cmd.ExecuteReader())
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
                    dbConnection.Close();
                }

            }

            return result;

        }

        public string ExecuteReader(string query, DbParameter[] sqlParameter, bool openconnect)
        {
            openConnection();
            string result = String.Empty;
            try
            {
                DbCommand m_Cmd = dbProvider.CreateCommand();
                m_Cmd.CommandText = query;
                m_Cmd.Connection = dbConnection;

         
                if (sqlParameter != null)
                {
                    m_Cmd.Parameters.AddRange(sqlParameter);
                }
                using (DbDataReader reader = m_Cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        result = reader.GetValue(0).ToString();
                     
                    }
                }
            }
            catch (Exception ex)
            {
               // LogHelper.log.Error("SelectValue: " + ex.ToString());
            }
            finally
            {
                if (!openconnect) dbConnection.Close();
            }

            return result;

        }

        public int ExecuteNonQuery(string query, DbParameter[] sqlParameter, bool openconnect)
        {
            openConnection();
            int result = 0;
            try
            {
                DbParameter oResParam = null;

                DbCommand m_Cmd = dbProvider.CreateCommand();
                m_Cmd.CommandText = query;
                m_Cmd.Connection = dbConnection;


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

               // Logger.Log.Error(ex.ToString());

            }
            finally
            {
                if (!openconnect)
                {
                    dbConnection.Close();
                }
            }

            return result;


        }

        public object ExecuteScalar(string query, DbParameter[] sqlParameter, bool isProc)
        {
            openConnection();

            object result = 0;

            try
            {
                DbParameter oResParam = sqlParameter != null ?
                    sqlParameter.FirstOrDefault(f => f.Direction == ParameterDirection.Output) : null;

                DbCommand m_Cmd = dbProvider.CreateCommand();
                m_Cmd.CommandText = query;
                m_Cmd.Connection = dbConnection;

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

               // Logger.Log.Error(ex.ToString());


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

        public int UpdateInsert(string query, DbParameter[] sqlParameter, bool openconnect)
        {
            openConnection();
            int result = 0;
            try
            {
                DbCommand m_Cmd = dbProvider.CreateCommand();
                m_Cmd.CommandText = query;
                m_Cmd.Connection = dbConnection;
                m_Cmd.CommandType = CommandType.StoredProcedure;

                m_Cmd.Parameters.AddRange(sqlParameter);
                result = m_Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
              //  Logger.Log.Error(ex.ToString());
            }
            finally
            {
                // conn.Close();
            }

            return result;
        }

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

        //        Logger.Log.Error(ex.ToString());

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



    }

}

using System;
using System.Collections.Generic;
using System.Text;
//using System.Linq;
//using System.Configuration;
using System.Data;
using System.Data.Common;
//using System.Data.SqlClient;
using Elsy.UoCommon.Models;
//using System.IO;
//using System.Reflection;
//using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;
using Serilog;
using Npgsql;
using Microsoft.Data.SqlClient;

namespace Elsy.UoCommon
{
    public class SutLayer : ISutLayer
    {
        Db.Db Db;

        string provider;

        public SutLayer()
        {

        }


        public SutLayer(string connStr)
        {

            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();


            Log.Information("uocommon");

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            //XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

         

            Db = //new Db.Db(ConfigurationManager.ConnectionStrings["SutConnection"].ConnectionString);
                new Db.Db(connStr);
        }

        public SutLayer(string connStr, string provider)
        {

            DbProviderFactories.RegisterFactory("System.Data.SqlClient",
                  SqlClientFactory.Instance);

            DbProviderFactories.RegisterFactory("Npgsql", NpgsqlFactory.Instance);

            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

            Log.Information("uocommon");


            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            //XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));



            Db = //new Db.Db(ConfigurationManager.ConnectionStrings["SutConnection"].ConnectionString);
                new Db.Db(connStr, provider);

            this.provider = provider;
        }




        public bool CheckConnect(string connStr)
        {
            Db.Db testDb = new Db.Db(connStr);

            if (testDb.openConnection().State == ConnectionState.Open)
            {
                testDb.closeConnection();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckConnect()
        {
        

            if (Db.openConnection().State == ConnectionState.Open)
            {
                Db.closeConnection();
                return true;
            }
            else
            {
                return false;
            }
        }



        public DataTable getVestGroupVests(int groupId)
        {
            string errmess;

            List<Parameter> lParams = new List<Parameter>();
          
            lParams.Add(new Parameter { ParameterName = "GroupId", DbType = DbType.Int32, Value = groupId });

            DataTable dt = null;

            dt = Db.Select("UO_GET_VEST_GROUP_VESTS", lParams.ToArray(), true, false, out errmess);

            //if (provider == "Npgsql")
            //{
            //    dt = Db.Select("select * from public.UO_GET_VEST_GROUP_VESTS", lParams.ToArray(), false, false, out errmess);
            //}
            //else
            //{
            //    dt = Db.Select("UO_GET_VEST_GROUP_VESTS", lParams.ToArray(), true, false, out errmess);
            //}

            return dt;
        }




        public DataTable getComponentsTable()
        {
            string errmess;

            DataTable dt = null;

            if (provider == "Npgsql")
            {
                dt = Db.Select("select * from public.UO_GET_COMPONENTS", null, false, false, out errmess);
            }
            else
            {
                dt = Db.Select("UO_GET_COMPONENTS", null, true, false, out errmess);
            }

          
            return dt;
        }



        public DataTable getGroupVests()
        {
            string errmess;

            DataTable dt;

            dt = Db.Select("UO_GET_GROUP_VESTS", null, true, false, out errmess);

            //if (provider == "Npgsql")
            //{
            //    dt = Db.Select("select * from public.UO_GET_GROUP_VESTS", null, false, false, out errmess);
            //}
            //else
            //{
            //    dt = Db.Select("UO_GET_GROUP_VESTS", null, true, false, out errmess);
            //}

          
            return dt;
        }


        public DataTable getSoftware()
        {
            string errmess;

            DataTable dt;

            if (provider == "Npgsql")
            {
                dt = Db.Select("select * from public.UO_GET_SOFTWARE()", null, false, false, out errmess);
            }
            else
            {
                dt = Db.Select("UO_GET_SOFTWARE", null, true, false, out errmess);
            }

          
            return dt;
        }

        public DataTable getVest(int nLineID)
        {
            string errmess;

            List<Parameter> lParams = new List<Parameter>();
            lParams.Add(new Parameter { DbType  = DbType.Int32, ParameterName = "nlineid", Value = nLineID });
                
        

            DataTable dt;

            dt = Db.Select("UO_SELECT_VESTS", lParams.ToArray(), true, false, out errmess);

            //if (provider == "NPGSQL")

                
            //{
            //    dt = Db.Select("uo_select_vests", lParams.ToArray(), true, false, out errmess);
            //}
            //else
            //{
            //    dt = Db.Select("UO_SELECT_VESTS", lParams.ToArray(), true, false, out errmess);
            //}

            return dt;
        }


        public DataTable getLines()
        {
            string errmess;

            DataTable dt;

            dt = Db.Select("UO_SELECT_LINES", null, true, false, out errmess);

            //if (provider == "Npgsql")
            //{
            //    dt = Db.Select("select * from public.UO_SELECT_LINES", null,false, false, out errmess);
            //}
            //else
            //{
            //    dt = Db.Select("UO_SELECT_LINES", null, true, false, out errmess);
            //}

            return dt;
        }


        public DataTable getStatusDevicesUpdate(int UpdId)
        {
            string errmess;

            List<Parameter> lParams = new List<Parameter>();

            lParams.Add(new Parameter { Value = UpdId, DbType = DbType.Int32, ParameterName = "updid" });

            DataTable dt;

            dt = Db.Select("UO_GET_DEVICES_UPDATE", lParams.ToArray(), true, false, out errmess);

            //if (provider == "Npgsql")
            //{
            //    dt = Db.Select("select * from public.UO_GET_DEVICES_UPDATE", lParams.ToArray(), false, false, out errmess);
            //}
            //else
            //{
                
            //}


        
            return dt;

        }



        public DataTable getDevItemsForUpdate(int? lineId, int? stationId, int? groupVestId,  int? updId, int? isAdded)
        {
            string errmess;
            List<Parameter> lParams = new List<Parameter>();


            if (lineId != null && lineId > 0)
            {

                lParams.Add(new Parameter { ParameterName = "lineid", DbType = DbType.Int32, Value = lineId.Value }); 
            }

            if (stationId != null &&  stationId > 0)
            {
                lParams.Add(new Parameter {ParameterName = "stationid", Value = stationId.Value, DbType = DbType.Int16 });
            }

            


            if (groupVestId != null && groupVestId > 0)
            {
                lParams.Add(new Parameter { ParameterName = "groupvestid", Value = groupVestId.Value, DbType = DbType.Int16 });

            }

            if (updId != null)
            {
                lParams.Add(new Parameter { ParameterName = "updid", Value = updId.Value, DbType = DbType.Int32});

            }
            if (isAdded != null)
            {
                lParams.Add(new Parameter { ParameterName = "isadded", Value = isAdded.Value, DbType = DbType.Int16});

            }



            DataTable dt;

            dt = Db.Select("UO_GET_DEV_CHOICE", lParams.ToArray(), true, false, out errmess);

            //if (provider == "Npgsql")
            //{
            //    dt = Db.Select("select * from public.UO_GET_DEV_CHOICE()", lParams.ToArray(), true, false, out errmess);
            //}
            //else
            //{
            //    dt = Db.Select("UO_GET_DEV_CHOICE", lParams.ToArray(), true, false, out errmess);
            //}


            return dt;
        }

        public DataTable getUpdates()
        {
            string errmess;
            DataTable dt;


            dt = Db.Select("UO_GET_UPDATES", null, true, false, out errmess);

            //if (provider == "Npgsql")
            //{
            //    dt = Db.Select("select * from public.UO_GET_UPDATES()", null, false, false, out errmess);
              
            //}
            //else
            //{
            //   dt = Db.Select("UO_GET_UPDATES", null, true, false, out errmess);
            //}

            return dt;
        }


        public DataTable checkIsAddedToFact(
            int UpdId, int? UpdType, int? VestId,
            int? TurNum, int? IdLine, int? VestGroupId, List<int> DevIds)
        {

            List<Parameter> lParams = new List<Parameter>();

            lParams.Add(new Parameter { ParameterName = "UpdId", Value = UpdId, DbType = DbType.Int32 });


            if (UpdType > 0)
            {
                //lParams.Add(new DbParameter("UpdType", SqlDbType.Int) { Value = UpdType });
                lParams.Add(new Parameter { ParameterName = "UpdType", Value = UpdType, DbType = DbType.Int32 });
            }

            if (VestId > 0)
            {
                lParams.Add(new Parameter { ParameterName = "VestId", Value = VestId, DbType = DbType.Int16});
            }


            if (TurNum > 0)
            {
                lParams.Add(new Parameter { ParameterName = "TurNum", Value = TurNum, DbType = DbType.Int16 });

                //lParams.Add(new DbParameter("TurNum", SqlDbType.SmallInt) { Value = TurNum });
            }

            if (IdLine > 0)
            {
                lParams.Add(new Parameter { ParameterName = "IdLine", Value = IdLine, DbType = DbType.Int16 });

                //lParams.Add(new Parameter("IdLine", SqlDbType.SmallInt) { Value = IdLine });
            }

            if (VestGroupId > 0)
            {
                lParams.Add(new Parameter { ParameterName = "VestGroupId", Value = VestGroupId, DbType = DbType.Int32 });

                // lParams.Add(new DbParameter("VestGroupId", SqlDbType.Int) { Value = VestGroupId });
            }


            DataTable dtids = null;

            // DataTable dtids = new DataTable();

            if (DevIds != null && DevIds.Count > 0)
            {
                dtids = new DataTable();
                dtids.Columns.Add(new DataColumn("ID", typeof(int)));
                dtids.Columns.Add(new DataColumn("VAL", typeof(string)));

                foreach (var id in DevIds)
                {
                    dtids.Rows.Add(id, null);
                }


               // lParams.Add(new DbParameter("DevIds", dtids));

            }

            string errmess;
            DataTable dt = Db.Select("UO_CHECK_ISADDED_TO_FACT", lParams.ToArray(), true, false, out errmess);

            return dt;
        }



        public int changeGroupVests(int? groupId, List<int> vestIds, string nameGroup)
        {
            

            //@GroupId int = null,
            //@VestIds  as dbo.IDVALList readonly,
            //@NameGroup varchar(200) = null,
            //@oRes int output

            List<Parameter> lParams = new List<Parameter>();

            if (groupId != null)
            {
                //lParams.Add(new DbParameter("GroupId", SqlDbType.Int) { Value = groupId.Value });
            }

            if (!String.IsNullOrEmpty(nameGroup))
            {
               // lParams.Add(new DbParameter("NameGroup", SqlDbType.VarChar) { Value = nameGroup });
            }


            DataTable dtids = null;

            // DataTable dtids = new DataTable();

            if (vestIds != null && vestIds.Count > 0)
            {
                dtids = new DataTable();
                dtids.Columns.Add(new DataColumn("ID", typeof(int)));
                dtids.Columns.Add(new DataColumn("VAL", typeof(string)));
                dtids.Columns.Add(new DataColumn("LONG_ID", typeof(long)));


                foreach (var id in vestIds)
                {
                    dtids.Rows.Add(id, null, null);
                }

                //lParams.Add(new DbParameter("VestIds", dtids));

            }

           // DbParameter outParam = new DbParameter("@oRes", SqlDbType.Int);
           // outParam.Direction = ParameterDirection.Output;
           // lParams.Add(outParam);

            string errmess;
            object res = Db.UpdateInsert1("UO_CREATE_UPDATE_GROUP_VESTS", lParams.ToArray(), true, false, true, out errmess);

            return int.Parse(res.ToString());


        }

        public int addUpdateFact(int UpdId, string CreatUser, int? UpdType, int? VestId,
            int? TurNum, int? IdLine, int? VestGroupId, DateTime? FromDate, bool isForceRefresh, List<int> DevIds, out string errmess)
        {

            int res = 0;
            DataTable dt = null;
            errmess = null;

            if (provider.ToUpper() != "NPGSQL")
            {

                List<Parameter> lParams = new List<Parameter>();

                lParams.Add(new Parameter { ParameterName = "updid", Value = UpdId, DbType = DbType.Int32 });

                lParams.Add(new Parameter { ParameterName = "creatuser", Value = CreatUser, DbType = DbType.String });


                if (UpdType > 0)
                {
                    lParams.Add(new Parameter { ParameterName = "updtype", Value = UpdType, DbType = DbType.Int32 });
                }

                if (VestId > 0)
                {
                    lParams.Add(new Parameter { ParameterName = "vestid", Value = VestId, DbType = DbType.Int16 });
                }


                if (TurNum > 0)
                {
                    lParams.Add(new Parameter { ParameterName = "turnum", Value = TurNum, DbType = DbType.Int16 });
                }

                if (IdLine > 0)
                {
                    lParams.Add(new Parameter { ParameterName = "idline", Value = IdLine, DbType = DbType.Int16 });

                }

                if (VestGroupId > 0)
                {
                    lParams.Add(new Parameter { ParameterName = "vestgroupid", Value = VestGroupId, DbType = DbType.Int32 });

                }


                if (FromDate != null)
                {
                    lParams.Add(new Parameter { ParameterName = "fromdate", Value = FromDate.Value, DbType = DbType.DateTime });
                    // lParams.Add(new DbParameter("FromDate", SqlDbType.SmallDateTime) { Value = FromDate.Value });
                }
                if (isForceRefresh)
                {
                    // convert bool to int

                    lParams.Add(new Parameter { ParameterName = "isforcerefresh", Value = 1, DbType = DbType.Int16 });
                    // lParams.Add(new DbParameter("IsForceRefresh", SqlDbType.TinyInt) { Value = 1});
                }


                DataTable dtids = null;

                // DataTable dtids = new DataTable();

                if (DevIds != null && DevIds.Count > 0)
                {
                    dtids = new DataTable();
                    dtids.Columns.Add(new DataColumn("ID", typeof(int)));
                    dtids.Columns.Add(new DataColumn("VAL", typeof(string)));
                    dtids.Columns.Add(new DataColumn("LONG_ID", typeof(long)));


                    foreach (var id in DevIds)
                    {
                        dtids.Rows.Add(id, null, null);
                    }


                    lParams.Add(new Parameter { ParameterName = "DevIds", Value = dtids });

                    // lParams.Add(new Parameter("DevIds", dtids));

                }

                dt = Db.Select("uo_add_update_fact_5", lParams.ToArray(), true, false, true, out errmess);
            }
            else 
            {

                List<NpgsqlParameter> lParams = new List<NpgsqlParameter>();

                lParams.Add(new NpgsqlParameter { ParameterName = "updid", Value = UpdId, DbType = DbType.Int32 });

                lParams.Add(new NpgsqlParameter { ParameterName = "creatuser", Value = CreatUser, DbType = DbType.String });


                if (UpdType > 0)
                {
                    lParams.Add(new NpgsqlParameter { ParameterName = "updtype", Value = UpdType, DbType = DbType.Int32 });
                }

                if (VestId > 0)
                {
                    lParams.Add(new NpgsqlParameter { ParameterName = "vestid", Value = VestId, DbType = DbType.Int16 });
                }


                if (TurNum > 0)
                {
                    lParams.Add(new NpgsqlParameter { ParameterName = "turnum", Value = TurNum, DbType = DbType.Int16 });
                }

                if (IdLine > 0)
                {
                    lParams.Add(new NpgsqlParameter { ParameterName = "idline", Value = IdLine, DbType = DbType.Int16 });

                }

                if (VestGroupId > 0)
                {
                    lParams.Add(new NpgsqlParameter { ParameterName = "vestgroupid", Value = VestGroupId, DbType = DbType.Int32 });

                }


                if (FromDate != null)
                {
                    lParams.Add(new NpgsqlParameter { ParameterName = "fromdate", Value = FromDate.Value, NpgsqlDbType = NpgsqlDbType.Timestamp });
                    // lParams.Add(new DbParameter("FromDate", SqlDbType.SmallDateTime) { Value = FromDate.Value });
                }
                if (isForceRefresh)
                {
                    // convert bool to int

                    lParams.Add(new NpgsqlParameter { ParameterName = "isforcerefresh", Value = 1, DbType = DbType.Int16 });
                    // lParams.Add(new DbParameter("IsForceRefresh", SqlDbType.TinyInt) { Value = 1});
                }


                if (DevIds != null && DevIds.Count > 0)
                {
                    lParams.Add(new NpgsqlParameter  { ParameterName = "devids", Value = DevIds.ToArray(), NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Array | NpgsqlDbType.Integer});
                }


                dt = Db.SelectNpgsql("uo_add_update_fact_5", lParams.ToArray(), true, false, true, out errmess);
            }







            //string errmess;


            /*
                       * @UpdId int,
          @CreatUser varchar(100),
          @UpdType  int,
          @DevId int,
          @VestId smallint = null,
          @TurNum smallint  = null , -- ? -- DEV_ORIGINAL_CODE
          @IdLine smallint = null,
          @DevIds as dbo.IDVALList readonly \
          @FromDate smalldatetime  = null,
          @VestGroupId int = null,
               * 
               */



       


     

            //DataTable dt = Db.Select("UO_ADD_UPDATE_FACT", lParams.ToArray(), true, false, true, out errmess);

            if (dt != null && dt.Rows.Count > 0)
            {

                int.TryParse(dt.Rows[0].Field<object>("resCode")?.ToString(), out res);

                if (dt.Rows[0].Field<string>("resMess") != null)
                {
                    errmess = dt.Rows[0].Field<string>("resMess");
                }

               //res = int.Parse( dt.Rows[0].Field<object>("resCode").ToString());
       
            }

            return res;

        
        }
        //UO_DELETE_RECORD_FROM_UPDATE

        public int deleteRecordFromUpdate(int updtId)
        {
            string errmess;

            List<Parameter> lParams = new List<Parameter>();


            lParams.Add(new Parameter { ParameterName = "updid", Value = updtId, DbType = DbType.Int32 });

         
            Parameter outParam = new Parameter { ParameterName =  "ores", DbType = DbType.Int32, Direction = ParameterDirection.Output};
            lParams.Add(outParam);

            // DbParameter outParam = new DbParameter("@oRes", SqlDbType.Int);
            //  outParam.Direction = ParameterDirection.Output;
            // lParams.Add(outParam);

     


            object res = Db.UpdateInsert1("uo_delete_record_from_update", lParams.ToArray(), true, false, true, out errmess);

            return int.Parse(res.ToString());
        }



        public int deleteGroupVests(int groupId)
        {
            string errmess;

            List<Parameter> lParams = new List<Parameter>();

           // DbParameter outParam = new DbParameter("@oRes", SqlDbType.Int);
          //  outParam.Direction = ParameterDirection.Output;
          //  lParams.Add(outParam);

           // lParams.Add(new DbParameter("GroupId", SqlDbType.Int) { Value = groupId });


            object res = Db.UpdateInsert1("UO_DELETE_GROUP_VESTS", lParams.ToArray(), true, false, true, out errmess);

            return int.Parse(res.ToString());
        }

        public int deleteRecordFromDevUpdates(List<long> duIds)
        {
            string errmess;
            object res = null;


            if (provider.ToUpper() != "NPGSQL")
            {
                List<Parameter> lParams = new List<Parameter>();

                Parameter outParam = new Parameter { ParameterName = "ores", DbType = DbType.Int32, Direction = ParameterDirection.Output };
                lParams.Add(outParam);


                DataTable duids = new DataTable();
                duids.Columns.Add(new DataColumn("ID", typeof(int)));
                duids.Columns.Add(new DataColumn("VAL", typeof(string)));
                duids.Columns.Add(new DataColumn("LONG_ID", typeof(long)));


                foreach (var id in duIds)
                {
                    duids.Rows.Add(null, null, id);
                }

               res = Db.UpdateInsert1("UO_DELETE_DEV_UPDATES", lParams.ToArray(), true, false, true, out errmess);
            }
            else
            {
                List<NpgsqlParameter> lParams = new List<NpgsqlParameter>();

                NpgsqlParameter outParam = new NpgsqlParameter { ParameterName = "ores", 
                    DbType = DbType.Int32, Direction = ParameterDirection.Output };
                lParams.Add(outParam);

                lParams.Add(new NpgsqlParameter { ParameterName = "duids", Value = duIds.ToArray(), 
                    NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Array | NpgsqlDbType.Bigint });

                res = Db.UpdateInsertNpgsql("uo_delete_dev_updates", lParams.ToArray(), true, false, true, out errmess);
                
            }

          

            return int.Parse(res.ToString());
        }


        /*
          * @updtId int,
            @isDisadled smallint,
            @oRes int output
         */

        public int updateDisabledField(int updtId, int isDisadled)
        {
            string errmess;

            List<Parameter> lParams = new List<Parameter>();

       


            lParams.Add(new Parameter { ParameterName = "updtid", DbType = DbType.Int32, Value = updtId });

            lParams.Add(new Parameter { ParameterName = "isdisadled", DbType = DbType.Int16, Value = isDisadled });

            lParams.Add(new Parameter { ParameterName = "ores", DbType = DbType.Int32, Direction = ParameterDirection.Output });

            object res = Db.UpdateInsert1("UO_UPDATE_DISABLE_FIELD", lParams.ToArray(), true, false, true,out errmess);

            return int.Parse(res.ToString());
        }


        //public int addUpdateItem(Models.UpdateItem updateItem)
        //{
        //    string errmess;

        //    List<NpgsqlParameter> lParams = new List<NpgsqlParameter>();


        //    //NpgsqlParameter ores = new NpgsqlParameter();
        //    //ores.ParameterName = "ores";
        //    //ores.DbType = DbType.Int32;
        //    //ores.Direction = ParameterDirection.Output;
        //    //lParams.Add(ores);


        //    /*
        //      @name varchar (250),
        //      @desc varchar(max),
        //      @bin varbinary(max),
        //      @filename varchar(250),
        //      @components bigint,
        //      @createdate datetime,
        //      @createduser varchar(100),
        //      @soid int,
        //      @isdisabled smallint
        //                   */




        //    lParams.Add(new NpgsqlParameter("name", NpgsqlDbType.Text) { Value = updateItem.Name });

        //    lParams.Add(new NpgsqlParameter("descr", NpgsqlDbType.Text) { Value = updateItem.Description });

        //    lParams.Add(new NpgsqlParameter("bin", NpgsqlDbType.Bytea) { Value = updateItem.Binary });

        //    lParams.Add(new NpgsqlParameter("soid", NpgsqlDbType.Integer) { Value = updateItem.SoId });

        //    lParams.Add(new NpgsqlParameter("createduser", NpgsqlDbType.Text) { Value = updateItem.CreateDateByUser });

        //    lParams.Add(new NpgsqlParameter("filename", NpgsqlDbType.Text) { Value = updateItem.FileName });

        //    lParams.Add(new NpgsqlParameter("isdisabled", NpgsqlDbType.Smallint) { Value = Convert.ToInt32(updateItem.IsDisabled) });


        //    NpgsqlParameter ores = new NpgsqlParameter();
        //    ores.ParameterName = "ores";
        //    ores.DbType = DbType.Int32;
        //    ores.Direction = ParameterDirection.Output;
        //    lParams.Add(ores);




        //    //lParams.Add(new NpgsqlParameter("components", NpgsqlDbType.Bigint) { Value = updateItem.Components }); // tmp!!!!





        //    //NpgsqlParameter outParam = new NpgsqlParameter("ores", NpgsqlDbType.Varchar);
        //    //outParam.Direction = ParameterDirection.Output;
        //    //lParams.Add(outParam);

        //    object res = Db.UpdateInsert1("public.uo_add_update_record", lParams.ToArray(), true, false, true, out errmess);

        //    return int.Parse(res.ToString());
        //}




        public int addUpdateItem(Models.UpdateItem updateItem)
        {
            string errmess;

            List<Parameter> lParams = new List<Parameter>();

            /*
                 * name text, 
                descr text, 
                bin bytea, 
                filename text, 
                createduser text, 
                soid integer, 
                isdisabled smallint
                --INOUT ores integer DEFAULT NULL::integer
             */




            lParams.Add(new Parameter { ParameterName = "name", DbType = DbType.String, Value = updateItem.Name });

            //   lParams.Add(new DbParameter("name", SqlDbType.VarChar) { Value = updateItem.Name});

            if (provider == "NPGSQL")
            {
                //!!!!!!!
                lParams.Add(new Parameter { ParameterName = "descr", DbType = DbType.String, Value = updateItem.Description });
            }
            else
            {
                lParams.Add(new Parameter { ParameterName = "desc", DbType = DbType.String, Value = updateItem.Description });
            }

            lParams.Add(new Parameter { ParameterName = "bin", DbType = DbType.Binary, Value = updateItem.Binary });

            lParams.Add(new Parameter { ParameterName = "soid", DbType = DbType.Int32, Value = updateItem.SoId });

            lParams.Add(new Parameter { ParameterName = "createduser", DbType = DbType.String, Value = updateItem.CreateDateByUser });

            lParams.Add(new Parameter { ParameterName = "filename", DbType = DbType.String, Value = updateItem.FileName });

            lParams.Add(new Parameter { ParameterName = "isdisabled", DbType = DbType.Int16, Value = Convert.ToInt16(updateItem.IsDisabled) });

            //lParams.Add(new Parameter { ParameterName = "components", DbType = DbType.Int64, Value = updateItem.Components });

            lParams.Add(new Parameter { ParameterName = "ores", DbType = DbType.Int32,  Direction = ParameterDirection.Output});


            object res = Db.UpdateInsert1("UO_ADD_UPDATE_RECORD", lParams.ToArray(), true, false, true,  out errmess);

            return int.Parse(res.ToString());
        }


    

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Configuration;
using System.Data;
//using System.Data.SqlClient;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;
using Npgsql;
using NpgsqlTypes;
using Elsy.UoCommon.Db;

namespace Elsy.UoCommon
{
    public class SutLayer : ISutLayer
    {
        Db.Db Db;

        public SutLayer()
        {

        }


        public SutLayer(string connStr)
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

         

            Db = //new Db.Db(ConfigurationManager.ConnectionStrings["SutConnection"].ConnectionString);
                new Db.Db(connStr);
        }


        public SutLayer(string connStr, string provider)
        {

           Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));


            Db = //new Db.Db(ConfigurationManager.ConnectionStrings["SutConnection"].ConnectionString);
             new Db.Db(connStr);


            //Db = //new Db.Db(ConfigurationManager.ConnectionStrings["SutConnection"].ConnectionString);
            //    new Db.Db(connStr, provider);

            //this.provider = provider;
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



        public DataTable getVestGroupVests(int groupId)
        {
            string errmess;

            List<NpgsqlParameter > lParams = new List<NpgsqlParameter>();


            lParams.Add(new NpgsqlParameter("GroupId", SqlDbType.Int) { Value = groupId });

            DataTable dt = Db.Select("UO_GET_VEST_GROUP_VESTS", lParams.ToArray(), true, false, out errmess);
            return dt;
        }




        public DataTable getComponentsTable()
        {
            string errmess;
            DataTable dt  = Db.Select("UO_GET_COMPONENTS", null, false, false, out errmess);
            return dt;
        }



        public DataTable getGroupVests()
        {
            string errmess;
            DataTable dt = Db.Select("UO_GET_GROUP_VESTS", null, false, false, out errmess);
            return dt;
        }


        public DataTable getSoftware()
        {
            string errmess;
            DataTable dt = Db.Select( "public.UO_GET_SOFTWARE", null, true, false, out errmess);
            return dt;
        }

        public DataTable getVest(int nLineID)
        {
            string errmess;

            List<NpgsqlParameter> lParams = new List<NpgsqlParameter>();


            lParams.Add(new NpgsqlParameter("nLineID", SqlDbType.Int) { Value = nLineID });
             
            DataTable dt = Db.Select("UO_SELECT_VESTS", lParams.ToArray(), true, false, out errmess);
            return dt;
        }


        public DataTable getLines()
        {
            string errmess;
            DataTable dt = Db.Select("UO_SELECT_LINES", null, true, false, out errmess);
            return dt;
        }


        public DataTable getStatusDevicesUpdate(int UpdId)
        {
            string errmess;

            List<NpgsqlParameter> lParams = new List<NpgsqlParameter>();

            lParams.Add(new NpgsqlParameter("UpdId", SqlDbType.Int) { Value = UpdId });

            DataTable dt = Db.Select("UO_GET_DEVICES_UPDATE", lParams.ToArray(), true, false, out errmess);
            return dt;

        }





        //public List<Item> getLines()
        //{
        //    List<Item> res = null;
        //    string errmess;

        //    DataTable dt = Db.Select("UO_GET_COMPONENTS", null, false, false, out errmess);

        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        res = dt.AsEnumerable().Select
        //            (
        //               s => new Item
        //               {
        //                   Id = s.Field<int>("CMP_ID"),
        //                   Value = s.Field<string>("CMP_NAME")
        //               }
        //            ).ToList();
        //    }
        //    return res;
        //}
        //@lineID int = null,
        // @stationID smallint = null



        public DataTable getDevItemsForUpdate(int? lineId, int? stationId, int? groupVestId,  int? updId, int? isAdded)
        {
            string errmess;
            List<NpgsqlParameter> lParams = new List<NpgsqlParameter>();


            if (lineId != null && lineId > 0)
            {
                lParams.Add(new NpgsqlParameter("lineId", SqlDbType.Int) { Value = lineId.Value });
            }

            if (stationId != null &&  stationId > 0)
            {
                lParams.Add(new NpgsqlParameter("stationId", SqlDbType.SmallInt) { Value = stationId.Value });
            }

            if (groupVestId != null &&  groupVestId > 0)
            {
                lParams.Add(new NpgsqlParameter("groupVestID", SqlDbType.SmallInt) { Value = groupVestId.Value });
            }         

            if (updId != null)
            {
                lParams.Add(new NpgsqlParameter("updId", SqlDbType.Int) { Value = updId.Value });
            }
            if (isAdded != null)
            {
                lParams.Add(new NpgsqlParameter("isAdded", SqlDbType.TinyInt) { Value = isAdded.Value });
            }



            DataTable dt = Db.Select( "UO_GET_DEV_CHOICE", lParams.ToArray(), true, false, out errmess);


            return dt;
        }

        public DataTable getUpdates()
        {
            string errmess;
            List<NpgsqlParameter> lParams = new List<NpgsqlParameter>();


            DataTable dt = Db.Select("select * from  public.UO_GET_UPDATES()", null, false, false, out errmess);
            return dt;
        }


        public DataTable checkIsAddedToFact(
            int UpdId, int? UpdType, int? VestId,
            int? TurNum, int? IdLine, int? VestGroupId, List<int> DevIds)
        {

            List<NpgsqlParameter> lParams = new List<NpgsqlParameter>();

            lParams.Add(new NpgsqlParameter("UpdId", SqlDbType.Int) { Value = UpdId });

            if (UpdType > 0)
            {
                lParams.Add(new NpgsqlParameter("UpdType", SqlDbType.Int) { Value = UpdType });
            }

            if (VestId > 0)
            {
                lParams.Add(new NpgsqlParameter("VestId", SqlDbType.SmallInt) { Value = VestId });
            }


            if (TurNum > 0)
            {
                lParams.Add(new NpgsqlParameter("TurNum", SqlDbType.SmallInt) { Value = TurNum });
            }

            if (IdLine > 0)
            {
                lParams.Add(new NpgsqlParameter("IdLine", SqlDbType.SmallInt) { Value = IdLine });
            }

            if (VestGroupId > 0)
            {
                lParams.Add(new NpgsqlParameter("VestGroupId", SqlDbType.Int) { Value = VestGroupId });
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


                lParams.Add(new NpgsqlParameter("DevIds", dtids));

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

            List<NpgsqlParameter> lParams = new List<NpgsqlParameter>();

            if (groupId != null)
            {
                lParams.Add(new NpgsqlParameter("GroupId", SqlDbType.Int) { Value = groupId.Value });
            }

            if (!String.IsNullOrEmpty(nameGroup))
            {
                lParams.Add(new NpgsqlParameter("NameGroup", SqlDbType.VarChar) { Value = nameGroup });
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

                lParams.Add(new NpgsqlParameter("VestIds", dtids));

            }

            NpgsqlParameter outParam = new NpgsqlParameter("@oRes", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;
            lParams.Add(outParam);

            string errmess;
            object res = Db.UpdateInsert1("UO_CREATE_UPDATE_GROUP_VESTS", lParams.ToArray(), true, false, true, out errmess);

            return int.Parse(res.ToString());


        }

        public int addUpdateFact(int UpdId, string CreatUser, int? UpdType, int? VestId,
            int? TurNum, int? IdLine, int? VestGroupId, DateTime?  FromDate, bool isForceRefresh , List<int> DevIds, out string errmess) 
        {
            //string errmess;

            int res = 0;

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

            List<NpgsqlParameter> lParams = new List<NpgsqlParameter>();

            lParams.Add(new NpgsqlParameter("UpdId", SqlDbType.Int) { Value = UpdId });

            lParams.Add(new NpgsqlParameter("CreatUser", SqlDbType.VarChar) { Value = CreatUser });
           

            if (UpdType > 0)
            {
                lParams.Add(new NpgsqlParameter("UpdType", SqlDbType.Int) { Value = UpdType });
            }

            if (VestId > 0)
            {
                lParams.Add(new NpgsqlParameter("VestId", SqlDbType.SmallInt) { Value = VestId });
            }


            if (TurNum > 0)
            {
                lParams.Add(new NpgsqlParameter("TurNum", SqlDbType.SmallInt) { Value = TurNum });
            }

            if (IdLine > 0)
            {
                lParams.Add(new NpgsqlParameter("IdLine", SqlDbType.SmallInt) { Value = IdLine });
            }

            if (VestGroupId > 0)
            {
                lParams.Add(new NpgsqlParameter("VestGroupId", SqlDbType.Int) { Value = VestGroupId });
            }


            if (FromDate != null)
            {
                lParams.Add(new NpgsqlParameter("FromDate", SqlDbType.SmallDateTime) { Value = FromDate.Value });
            }
            if (isForceRefresh)
            {
                lParams.Add(new NpgsqlParameter("IsForceRefresh", SqlDbType.TinyInt) { Value = 1});
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



                lParams.Add(new NpgsqlParameter("DevIds", dtids));

            }


            DataTable dt = Db.Select("UO_ADD_UPDATE_FACT", lParams.ToArray(), true, false, true, out errmess);

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

            List<NpgsqlParameter> lParams = new List<NpgsqlParameter>();

            NpgsqlParameter outParam = new NpgsqlParameter("@oRes", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;
            lParams.Add(outParam);

            lParams.Add(new NpgsqlParameter("UpdId", SqlDbType.Int) { Value = updtId });


            object res = Db.UpdateInsert1("UO_DELETE_RECORD_FROM_UPDATE", lParams.ToArray(), true, false, true, out errmess);

            return int.Parse(res.ToString());
        }



        public int deleteGroupVests(int groupId)
        {
            string errmess;

            List<NpgsqlParameter> lParams = new List<NpgsqlParameter>();

            NpgsqlParameter outParam = new NpgsqlParameter("@oRes", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;
            lParams.Add(outParam);

            lParams.Add(new NpgsqlParameter("GroupId", SqlDbType.Int) { Value = groupId });


            object res = Db.UpdateInsert1("UO_DELETE_GROUP_VESTS", lParams.ToArray(), true, false, true, out errmess);

            return int.Parse(res.ToString());
        }

        public int deleteRecordFromDevUpdates(List<long> duIds)
        {
            string errmess;

            List<NpgsqlParameter> lParams = new List<NpgsqlParameter>();

            NpgsqlParameter outParam = new NpgsqlParameter("@oRes", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;
            lParams.Add(outParam);


            // DataTable dtids = new DataTable();


            DataTable duids = new DataTable();
            duids.Columns.Add(new DataColumn("ID", typeof(int)));
            duids.Columns.Add(new DataColumn("VAL", typeof(string)));
            duids.Columns.Add(new DataColumn("LONG_ID", typeof(long)));

       
            foreach (var id in duIds)
            {
                duids.Rows.Add(null, null, id);
            }


            lParams.Add(new NpgsqlParameter("DuIds", duids));

            object res = Db.UpdateInsert1("UO_DELETE_DEV_UPDATES", lParams.ToArray(), true, false, true, out errmess);

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

            List<NpgsqlParameter> lParams = new List<NpgsqlParameter>();

            NpgsqlParameter outParam = new NpgsqlParameter("@oRes", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;
            lParams.Add(outParam);


            lParams.Add(new NpgsqlParameter("updtId", SqlDbType.Int) { Value = updtId });

            lParams.Add(new NpgsqlParameter("isDisadled", SqlDbType.Int) { Value = isDisadled });

            object res = Db.UpdateInsert1("UO_UPDATE_DISABLE_FIELD", lParams.ToArray(), true, false, true,out errmess);

            return int.Parse(res.ToString());
        }


        //public int addUpdateItem(Models.UpdateItem updateItem)
        //{
        //    int res = 0;
        //    string errmess;
        //    List<NpgsqlParameter> parameterList = new List<NpgsqlParameter>();

        //    NpgsqlParameter newId = new NpgsqlParameter();
        //    newId.ParameterName = "newid";
        //    newId.DbType = DbType.Int64;
        //    newId.Direction = ParameterDirection.Output;
        //    parameterList.Add(newId);

        //   // parameterList.Add(new NpgsqlParameter("idsvul", NpgsqlDbType.Bigint) { Value = id });

        //    string query = "public.testinsert";

        //    //" select * from   public.insert_to_egrulsv( " + id +  " ) ";
        //    //" public.insert_to_egrulsv";

        //    var r = Db.UpdateInsert1(query, parameterList.ToArray(), true, false, true, out errmess);

        //    int.TryParse(r.ToString(), out res);

        //    //long.TryParse(r.ToString(), out res);

        //    return res;
        //}


        public int addUpdateItem(Models.UpdateItem updateItem)
        {
            string errmess;

            List<NpgsqlParameter> lParams = new List<NpgsqlParameter>();


            //NpgsqlParameter ores = new NpgsqlParameter();
            //ores.ParameterName = "ores";
            //ores.DbType = DbType.Int32;
            //ores.Direction = ParameterDirection.Output;
            //lParams.Add(ores);


            /*
              @name varchar (250),
              @desc varchar(max),
              @bin varbinary(max),
              @filename varchar(250),
              @components bigint,
              @createdate datetime,
              @createduser varchar(100),
              @soid int,
              @isdisabled smallint
                           */




            lParams.Add(new NpgsqlParameter("name", NpgsqlDbType.Text) { Value = updateItem.Name });

            lParams.Add(new NpgsqlParameter("descr", NpgsqlDbType.Text) { Value = updateItem.Description });

            lParams.Add(new NpgsqlParameter("bin", NpgsqlDbType.Bytea) { Value = updateItem.Binary });

            lParams.Add(new NpgsqlParameter("soid", NpgsqlDbType.Integer) { Value = updateItem.SoId });

            lParams.Add(new NpgsqlParameter("createduser", NpgsqlDbType.Text) { Value = updateItem.CreateDateByUser });

            lParams.Add(new NpgsqlParameter("filename", NpgsqlDbType.Text) { Value = updateItem.FileName });

            lParams.Add(new NpgsqlParameter("isdisabled", NpgsqlDbType.Smallint) { Value = Convert.ToInt32(updateItem.IsDisabled) });


            NpgsqlParameter ores = new NpgsqlParameter();
            ores.ParameterName = "ores";
            ores.DbType = DbType.Int32;
            ores.Direction = ParameterDirection.Output;
            lParams.Add(ores);




            //lParams.Add(new NpgsqlParameter("components", NpgsqlDbType.Bigint) { Value = updateItem.Components }); // tmp!!!!





            //NpgsqlParameter outParam = new NpgsqlParameter("ores", NpgsqlDbType.Varchar);
            //outParam.Direction = ParameterDirection.Output;
            //lParams.Add(outParam);

            object res = Db.UpdateInsert1("public.uo_add_update_record", lParams.ToArray(), true, false, true, out errmess);

            return int.Parse(res.ToString());
        }




    }
}

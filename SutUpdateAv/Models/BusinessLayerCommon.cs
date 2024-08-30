using System;
using System.Collections.Generic;
using System.Text;
using Elsy.UoCommon; 
using System.Data;
using System.Linq;
using System.Data;
using System.Data.Common;



namespace SutUpdateAv.Models
{
    public sealed class BusinessLayerCommon<Conn, Trans, Cmd, Param>
        where Conn : DbConnection, new()
        where Trans : DbTransaction
        where Cmd : DbCommand
        where Param : DbParameter, new()
    {

        private static readonly BusinessLayerCommon<Conn, Trans, Cmd, Param>
        instance = new BusinessLayerCommon<Conn, Trans, Cmd, Param>(UserSetting.SutConnectionString, UserSetting.SutProvider);

        SutLayerCommon<Conn, Trans, Cmd, Param> sLayer;


        //string conn = Models.ConnectSetting.ConnectionString;
        //ConfigurationManager.ConnectionStrings["SutConnection"].ConnectionString;
        public BusinessLayerCommon()
        {
            
        }

        //public BusinessLayer(string conn)
        //{
        //    sLayer = new SutLayer(conn);
        //}

        public BusinessLayerCommon(string conn, string prov)
        {
            sLayer = new SutLayerCommon<Conn, Trans, Cmd, Param>(conn);
        }


        //
        public static BusinessLayerCommon<Conn, Trans, Cmd, Param> GetInstance()
        {
            return instance;
        }


        // перепиcать!!!!
        public bool CheckConnect(string connStr)
        {
            SutLayerCommon<Conn, Trans, Cmd, Param> sL = new SutLayerCommon<Conn, Trans, Cmd, Param>(connStr);
            return sL.CheckConnect();
        }


        //// переписать!!!!
        //public bool CheckConnect(string connStr, string prov)
        //{
        //    SutLayer sL = new SutLayer(connStr, prov);
        //    //return sL.CheckConnect(connStr);
        //    return sL.CheckConnect();
        //}


        // в статик ?

      //  public List<Item> getLines()
      //  {
      //      List<Item> res = null;

      //      //LINENAME, LINECODE
      //      DataTable dt = sLayer.getLines();

      //      if (dt != null && dt.Rows.Count > 0)
      //      {
      //          try
      //          {
      //              res = dt.AsEnumerable().Select
      //             (
      //             s => new Item
      //             {
      //                 Id = int.Parse( s.Field<object>("LINECODE").ToString()),
      //                 Value = s.Field<string>("LINENAME")
      //             }
      //          ).OrderBy(f => f.Id).ToList();

      //          }
      //          catch (Exception ex)
      //          {
      //              //Logger.Log.Error(ex.ToString());
      //          }

      //      }

      //      return res;
      //  }

      //  //CONVERT(varchar(5), STATION_ID ) + ' ' + STATION_NAME, STATION_ID 

      //  public List<Item> getVest(int lineId)
      //  {
      //      List<Item> res = null;

      //      //LINENAME, LINECODE
      //      DataTable dt = sLayer.getVest(lineId);

      //      if (dt != null && dt.Rows.Count > 0)
      //      {
      //          try
      //          {
      //             res = dt.AsEnumerable().Select
      //             (
      //                 s => new Item
      //                 {
      //                     Id = int.Parse( s.Field<object>("STATION_ID").ToString()),
      //                     Value = s.Field<string>("Column1")
      //                 }
      //              ).ToList();


      //              res.Insert(0, new Item { Id = 0, Value = "Вестибюль не указан" });
      //          }
      //          catch (Exception ex)
      //          {
      //              //Logger.Log.Error(ex.ToString());
      //          }




      //      }

      //      return res;
      //  }

      //  public List<DeviceUpdateItem> getStatusDevicesUpdateItems(int UpdId)
      //  {
      //      List<DeviceUpdateItem> res = null;

      //      DataTable dt = sLayer.getStatusDevicesUpdate(UpdId);

      //      if (dt != null && dt.Rows.Count > 0)
      //      {
      //          try
      //          {
      //              res = dt.AsEnumerable().Select
      //                 (

      //                      /*
      //                    *   DEV_CVEND_SERIAL,
      //                       UF_VEST_ID,
      //                       UF_TUR_NUM,
      //                       UF_LINE,
      //                       DU_ID,
      //                       DU_UPD_ID,
      //                       DU_DEV_ID,
      //                       STATUS_NAME,
      //                       DU_STATUS_TIME,
      //                       DU_CREATED_BY_USER,
      //                       DU_CREATE_DATE,
      //                       DU_UF_ID 
      //                       */

      //                      s => new  DeviceUpdateItem
      //                      {
      //                          DevId =  s.Field<int>("DU_DEV_ID"),
      //                          CreateDateTime = s.Field<object>("DU_CREATE_DATE") == null ? null :
      //                            new DateTime? ( s.Field<DateTime>("DU_CREATE_DATE")),
      //                          CreatedUser = s.Field<string>("DU_CREATED_BY_USER"),
      //                          LineId = s.Field<object>("UF_LINE") == null ? 0 : int.Parse(s.Field<object>("UF_LINE").ToString()),
      //                           SatusDateTime = s.Field<object>("DU_STATUS_TIME") == null ? null :
      //                             new DateTime?( s.Field<DateTime>("DU_STATUS_TIME")),
      //                            Sn = s.Field<object>("DEV_CVEND_SERIAL") == null ? null : new long? (s.Field<long>("DEV_CVEND_SERIAL")),
      //                             Status = s.Field<string>("STATUS_NAME"),
      //                              VestName =   s.Field<string>("DU_VEST"),
      //                               FromDateTime =s.Field<object>("UF_FROM_DATE")  == null ? null :
      //                                new DateTime? (s.Field<DateTime>("UF_FROM_DATE"))  ,
      //                                DuId = s.Field<long>("DU_ID"),
      //                                 StatusId = s.Field<int>("STATUS_ID"),
      //                                  TurNum = s.Field<object>("DEV_ORIGINAL_CODE") == null ? null : new int?(s.Field<int>("DEV_ORIGINAL_CODE")),
      //                                   PoCodeName = s.Field<string>("SO_NAME")



      //                      }
      //                 ).ToList();

      //          }
      //          catch (Exception ex)
      //          {
      //             // Logger.Log.Error(ex.ToString());
      //          }
      //      }

      //      return res;
      //  }


      //  public List<int> checkIsAddedToFact(int UpdId,  int? UpdType, int? VestId,
      //   int? TurNum, int? IdLine, int? VestGroupId,  List<int> DevIds)
      //  {
      //      List<int> res = null;

      //      DataTable dt = sLayer.checkIsAddedToFact(UpdId, UpdType, VestId,
      //       TurNum, IdLine, VestGroupId, DevIds);

      //      if (dt != null && dt.Rows.Count > 0)
      //      {
      //          try
      //          {
      //             res = dt.AsEnumerable().Select(s => s.Field<int>("DEV_ID")).ToList();

      //          }
      //          catch (Exception ex)
      //          {

      //              //Logger.Log.Error(ex.ToString());
      //          }

      //      }

      //      return res;

      //  }


      //  public int addUpdateFact(int UpdId, string CreatUser, int? UpdType, int? VestId,
      //    int? TurNum, int? IdLine, int? VestGroupId, DateTime? FromDate, bool isForceRefresh, List<int> DevIds, out string errmess)
      //  {
      //      return sLayer.addUpdateFact(UpdId, CreatUser, UpdType, VestId,
      //       TurNum,  IdLine, VestGroupId, FromDate, isForceRefresh, DevIds, out errmess);

      //  }

      //  public List<CheckItem> getComponets()
      //  {
      //      List<CheckItem> res = null;

      //      DataTable dt = sLayer.getComponentsTable();

      //      if (dt != null && dt.Rows.Count > 0)
      //      {
      //          try
      //          {
      //              res = dt.AsEnumerable().Select
      //             (
      //             s => new CheckItem
      //             {
      //                 Id = s.Field<int>("CMP_ID"),
      //                 Value = s.Field<string>("CMP_NAME")
      //             }
      //          ).ToList();

      //          }
      //          catch (Exception ex)
      //          {

      //              //Logger.Log.Error(ex.ToString());
      //          }

            

             
      //      }

      //      return res;
      //  }




      //  public List<Item> getGroupVests()
      //  {
      //      List<Item> res = null;

      //      DataTable dt = sLayer.getGroupVests();

      //      if (dt != null && dt.Rows.Count > 0)
      //      {
      //          // VG_ID, VG_NAME
      //          try
      //          {
      //            res = dt.AsEnumerable().Select
      //             (
      //                 s => new Item
      //                 {
      //                     Id = s.Field<int>("VG_ID"),
      //                     Value = s.Field<string>("VG_NAME")
      //                 }
      //             ).ToList();

      //             res.Insert(0, new Item { Id = 0, Value = "Группа не указана" });

      //          }
      //          catch (Exception ex)
      //          {
      //              //Logger.Log.Error(ex.ToString());
      //          }
      //      }

      //      return res;
      //  }


      //  public List<Item> getSoftware()
      //  {
      //      List<Item> res = null;

      //      DataTable dt = sLayer.getSoftware();

      //      if (dt != null && dt.Rows.Count > 0)
      //      {
      //          try
      //          {
      //             res = dt.AsEnumerable().Select
      //             (
      //             s => new Item
      //             {
   
      //                 Id = s.Field<int>("SO_ID"),
      //                 Value = s.Field<string>("SO_NAME")
      //             }
      //          ).ToList();

      //          }
      //          catch (Exception ex)
      //          {
      //              //Logger.Log.Error(ex.ToString());
      //          }

      //      }

      //      return res;
      //  }

      //  public int addUpdateItem(UpdateItem item)
      //  {
      //      var v = sLayer.addUpdateItem(item);

      //      return v;
      //  }



      //  public bool updateDisabledField(int updtId, int isDisadled)
      //  {
      //      var r = sLayer.updateDisabledField(updtId, isDisadled);

      //      return r > 0;
      //  }

      //  public bool deleteRecordFromUpdate(int updtId)
      //  {
      //      var r = sLayer.deleteRecordFromUpdate(updtId);

      //      return r > 0;
      //  }

      //  public bool deleteRecordFromDevUpdates(List<long> duIds)
      //  {
      //      var r = sLayer.deleteRecordFromDevUpdates(duIds);

      //      return r > 0;
      //  }


      //  public List<DeviceItem> getDevItemsForUpdate(int? idLine, int? idVest, int? grVestId, int? updtId, int? isAdded)
      //  {
      //      List<DeviceItem> res = null;

            

      //      DataTable dt = sLayer.getDevItemsForUpdate(idLine, idVest, grVestId, updtId, isAdded);

      //      if (dt != null && dt.Rows.Count > 0)
      //      {
      //          try
      //          {
      //              res = dt.AsEnumerable().Select
      //                 (

      //                      /*
      //                       * 
      //                              SELECT 
      //                              DEV_ID, 
      //                              DEV_STATION_ID, 	-- код вестибюля
      //                              STATION_NAME,
      //                              D.DEV_ORIGINAL_CODE, 
      //                              T.CMPT_NAME, 
      //                              C.CMP_NAME, 
      //                              D.DEV_CMP_TYPE_ID,
      //                              DEV_SOFTWARE_VERSION
      //                       */

      //                      s => new DeviceItem
      //                      {
      //                          DevId = int.Parse( s.Field<object>("DEV_ID").ToString()),
      //                           DevStationId =int.Parse( s.Field<object>("DEV_STATION_ID").ToString()),
      //                           Sn = s.Field<object>("DEV_CVEND_SERIAL") == null ? null : new long?(s.Field<long>("DEV_CVEND_SERIAL")),
      //                            StationName = s.Field<string>("STATION_NAME"),
      //                             CmpName = s.Field<string>("CMP_NAME"),
      //                              CmptName = s.Field<string>("CMPT_NAME"),
      //                               DevCmpTypeId = int.Parse( s.Field<object>("DEV_CMP_TYPE_ID").ToString()),
      //                                DevSoftwareVer = s.Field<object>("DEV_SOFTWARE_VERSION") == null ? null : 
      //                                   s.Field<object>("DEV_SOFTWARE_VERSION").ToString(),
      //                                  DevOriginalCode = s.Field<object>("DEV_ORIGINAL_CODE") == null ? 0 : s.Field<int>("DEV_ORIGINAL_CODE"),
      //                         // IsCheck = true
      //                         IsCheck = false
      //                          //HexSn = s.Field<object>("HexSn")?.ToString()

      //                      }
      //                 ).ToList();

      //          }
      //          catch (Exception ex)
      //          {
      //              //Logger.Log.Error(ex.ToString());

      //          }
      //      }

      //      return res;
      //  }



      //  /*
      //   * SELECT [UPD_ID]
      //,[UPD_NAME]
      //,[UPD_DESCRIPTION]
      //,[UPD_BINARY]
      //,[UPD_FILENAME]
      //,[UPD_COMPONENTS]
      //,[UPD_CREATE_DATE]
      //,[UPD_CREATED_BY_USER]
      //,[UPD_SO_ID]
      //,[UPD_DISABLED]
      //   */
      //  public List<UpdateItem> getUpdateItems() // параметры фильтрации
      //  {
      //      List<UpdateItem> res = null;


      //      DataTable dt = sLayer.getUpdates();

      //      if (dt != null && dt.Rows.Count > 0)
      //      {
      //          try
      //          {
      //              res = dt.AsEnumerable().Select
      //                  (
      //                       s=> new UpdateItem 
      //                       { 
      //                           Id = s.Field<int>("UPD_ID"),
      //                            Components = s.Field<long>("UPD_COMPONENTS"), // tmp
      //                             CreateDate = s.Field<DateTime>("UPD_CREATE_DATE"),
      //                              CreateDateByUser = s.Field<string>("UPD_CREATED_BY_USER"),
      //                               SoName  = s.Field<string>("SO_NAME"),
      //                                IsDisabled = Convert.ToBoolean (int.Parse(s.Field<object>("UPD_DISABLED").ToString())),
      //                                 FileName = s.Field<string>("UPD_FILENAME"),
      //                                  Name = s.Field<string>("UPD_NAME"),
      //                                   UfId = s.Field<object>("UF_ID") == null ? null :  new int?(s.Field<int>("UF_ID")),
      //                                    Description = s.Field<string>("UPD_DESCRIPTION"),
      //                                     DuId = s.Field<object>("DU_ID") == null ? null : new long?(s.Field<long>("DU_ID"))


      //                       }
      //                  ).ToList();
                        
      //          }
      //          catch (Exception ex)
      //          {
      //              //Logger.Log.Error(ex.ToString());
      //          }
      //      }

      //      return res;
      //  }
    }
}

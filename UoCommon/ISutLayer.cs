using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Elsy.UoCommon
{
    public interface  ISutLayer
    {

        public int addUpdateItem(Models.UpdateItem updateItem);

        public int addUpdateFact(int UpdId, string CreatUser, int? UpdType, int? VestId,
        int? TurNum, int? IdLine, int? VestGroupId, DateTime? FromDate, bool isForceRefresh, List<int> DevIds, out string errmess);

        public bool CheckConnect(string connStr);

        public int deleteRecordFromUpdate(int updtId);

        public DataTable getVestGroupVests(int groupId);

        public DataTable getGroupVests();

        public DataTable getSoftware();


        public DataTable getVest(int nLineID);


        public DataTable getLines();


        public DataTable getStatusDevicesUpdate(int UpdId);

        public DataTable getDevItemsForUpdate(int? lineId, int? stationId, int? groupVestId, int? updId, int? isAdded);


        public DataTable getUpdates();

        public int deleteRecordFromDevUpdates(List<long> duIds);

        public int deleteGroupVests(int groupId);

        public int updateDisabledField(int updtId, int isDisadled);

        public int changeGroupVests(int? groupId, List<int> vestIds, string nameGroup);
    }


}

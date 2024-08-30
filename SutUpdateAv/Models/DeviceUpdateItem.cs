using System;
using System.Collections.Generic;
using System.Text;

namespace SutUpdateAv.Models
{
    public class DeviceUpdateItem
    {

 

        /*
         *  DEV_CVEND_SERIAL,
           UF_VEST_ID,
           UF_TUR_NUM,
	       UF_LINE,
           DU_ID,
           DU_UPD_ID,
           DU_DEV_ID,
           DU_STATUS,
           DU_STATUS_TIME,
           DU_CREATED_BY_USER,
           DU_CREATE_DATE,
           DU_UF_ID 
         */


        long? sn;
        public long? Sn 
        {
            get
            {
                return sn;
            }
            set
            {
                sn = value;
            }

        }


        string hexSn;
        public string HexSn 
        {
            get
            {
                if (String.IsNullOrEmpty(hexSn))
                {
                    hexSn = sn?.ToString("X");
                }
                return hexSn;
            }
            
        }


        public int VestId { get; set; }



        //номер турникета - RTM_DEVICES.DEV_ORIGINAL_CODE
        public int? TurNum { get; set; }


        //SO_NAME - код ПО 
        public string PoCodeName { get; set; }
      


        public int LineId { get; set; }

        public int DevId { get; set; }

        public DateTime? SatusDateTime { get; set; }

        public DateTime? CreateDateTime { get; set; }

        public string Status { get; set;  }

        public string CreatedUser { get; set; }

        public string VestName { get; set; }

        /// <summary>
        /// Дата/время доступности обновления
        /// </summary>
        public DateTime? FromDateTime { get; set; }

       // dbo.SUT_UPDATE_FACTS.UF_ID, 
	  // dbo.SUT_STATUSES.STATUS_ID


        public long DuId { get; set; }


        public int StatusId { get; set; }


        public bool IsDel
        {
            get
            {
                return StatusId == 1;
            }

        }

        bool isSelected;
        public bool IsSelected 
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (value != isSelected)
                {
                    isSelected = value;
                }
            }
        }



    }
}

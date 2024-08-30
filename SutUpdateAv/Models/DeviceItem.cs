using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SutUpdateAv.Models
{
    public  class DeviceItem : INotifyPropertyChanged
    {
      

        private bool isCheck;

        public bool IsCheck
        {
            get => isCheck;
            set
            {
                //if (sn != null)
                //{

                    if (value != isCheck)
                    {
                      isCheck = value;
                      OnPropertyChanged("IsCheck");
                    }

                    //if (value == isCheck) return;
                    //isCheck = value;
                    //OnPropertyChanged();
                //}

            }
        }

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


        /*
          * // Store integer 182
            int intValue = 182;
            // Convert integer 182 as a hex in a string variable
            string hexValue = intValue.ToString("X");
            // Convert the hex string back to the number
            int intAgain = int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
                     * 
         */

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

        public int DevId { get; set; }

        public int DevStationId { get; set; }

        public string StationName { get; set; }

        public string CmptName { get; set; }

        public string CmpName { get; set; }

        public int DevCmpTypeId { get; set; }

        public string DevSoftwareVer { get; set; }

        public int DevOriginalCode { get; set; }  //номер терминала



        public event PropertyChangedEventHandler PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

using System;
using System.ComponentModel;


namespace SutUpdateAv.Models
{
   
    public class UpdateItem : Elsy.UoCommon.Models.UpdateItem,  INotifyPropertyChanged
    {
        string isAppointed;
        public bool IsCheck { get; set; }


        public string IsAppointed 
        {
            get
            {
                if (String.IsNullOrEmpty(isAppointed))
                {
                    // isAppointed = this.UfId == null ? "Нет" : "Да";
                     isAppointed = (this.UfId == null || this.DuId == null) ? "Нет" : "Да";
                }

                return isAppointed;
            }
        }


      
        public bool IsVis
        {
            get
            {
                return (this.UfId == null || this.DuId == null);
              
            }
           
        }

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


    }
}

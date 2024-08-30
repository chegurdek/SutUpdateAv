using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using ReactiveUI;
using SutUpdateAv.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace SutUpdateAv.ViewModels
{
    public class FactUpdate2ViewModel :  ViewModelBase, INotifyPropertyChanged
    {
        //cmbLines.SelectionChanged += Cmb_SelectionChanged;
        //cmbVest.SelectionChanged += Cmb_SelectionChanged;
        //cmbGroupVest.SelectionChanged += Cmb_SelectionChanged;
        //    //cmdChekViewDev.SelectionChanged += CmdChekViewDev_SelectionChanged;
        //cmdChekViewDev.SelectionChanged += Cmb_SelectionChanged;
        // Checked="chkSelectAll_Click"  Unchecked="chkSelectAll_Click"


        BusinessLayer bLayer = BusinessLayer.GetInstance();

        int CodeUpdt; 
 

        public ReactiveCommand<object, Unit> CmbLinesSelectionChanged { get; }
       // public ReactiveCommand<object, Unit> chkSelectAll { get; }


        List<Item> linesItems;
        public List<Item> LinesItems
        {
            get
            {
                return linesItems;
            }
            set
            {
                if (linesItems != value)
                {
                    linesItems = value;
                    RaisePropertyChanged("LinesItems");
                }
            }
        }

        List<Item> vestsItems;
        public List<Item> VestsItems
        {
            get
            {
                return vestsItems;
            }
            set
            {
                if (vestsItems != value)
                {
                    vestsItems = value;
                    RaisePropertyChanged("VestsItems");
                }
            }
        }

        List<Item> vestsGroupItems;
        public List<Item> VestsGroupItems
        {
            get
            {
                return vestsGroupItems;
            }
            set
            {
                if (vestsGroupItems != value)
                {
                    vestsGroupItems = value;
                    RaisePropertyChanged("VestsGroupItems");
                }
            }
        }

       // List<Item> cmdChekViewDevItems;
        public List<Item> СmdChekViewDevItems
        {
            get
            {
                return Common.Constants.ChoiceViewDevList;
            }
          
        }


        Item selectedcmdChekViewDev;
        public Item SelectedcmdChekViewDev
        {
            get
            {
                return selectedcmdChekViewDev;
            }
            set
            {
                if (selectedcmdChekViewDev != value)
                {
                    selectedcmdChekViewDev = value;
                    RaisePropertyChanged("SelectedcmdChekViewDev");
                }
            }
        }

        Item selectedItemLine;
        public Item SelectedItemLine
        {
            get
            {
                return selectedItemLine;
            }
            set
            {
                if (selectedItemLine != value)
                {
                    selectedItemLine = value;

                    try
                    {
                       // на null
                       VestsItems  = bLayer.getVest(selectedItemLine.Id);
                    }
                    catch (Exception ex)
                    {
                        // лог
                        return;
                    }
                    RaisePropertyChanged("SelectedItemLine");
                    refresh();
                }
            }
        }

        Item selectedItemVest;
        public Item SelectedItemVest
        {
            get
            {
                return selectedItemVest;
            }
            set
            {
                if (selectedItemVest != value)
                {
                    selectedItemVest = value;
                    RaisePropertyChanged("SelectedItemVest");
                    refresh();
                }
            }
        }

        Item selectedItemGroupVest;
        public Item SelectedItemGroupVest
        {
            get
            {
                return selectedItemGroupVest;
            }
            set
            {
                if (selectedItemGroupVest != value)
                {
                    selectedItemGroupVest = value;
                    RaisePropertyChanged("SelectedItemGroupVest");
                    refresh();
                }
            }
        }

        bool chkSelectAll = false;
        public bool ChkSelectAll
        {
            get
            {
                return chkSelectAll;
            }
            set
            {
                if (chkSelectAll != value)
                {
                    chkSelectAll = value;
                    RaisePropertyChanged("ChkSelectAll");
                    chkSelectAllCheked();
                }
            }
        }


       ObservableCollection<DeviceItem> lstGridCheckDevices;
        public ObservableCollection<DeviceItem> LstGridCheckDevices
        {
            get
            {
                return lstGridCheckDevices;
            }
            set
            {
                if (lstGridCheckDevices != value)
                {
                    lstGridCheckDevices = value;
                    RaisePropertyChanged("LstGridCheckDevices");
                }
            }
        }


        //List<DeviceItem> lstGridCheckDevices;
        //public List<DeviceItem> LstGridCheckDevices
        //{
        //    get
        //    {
        //        return lstGridCheckDevices;
        //    }
        //    set
        //    {
        //        if (lstGridCheckDevices != value)
        //        {
        //            lstGridCheckDevices = value;
        //            RaisePropertyChanged("LstGridCheckDevices");
        //        }
        //    }
        //}


        public FactUpdate2ViewModel()
        {
       
        }

        public FactUpdate2ViewModel(int codeUpdt, string nameUpd)
        {
            CodeUpdt = codeUpdt;
            LinesItems = bLayer.getLines();
          // CmbLinesSelectionChanged = ReactiveCommand.Create<object>(CmbSelectionChanged);

        }



        private void chkSelectAllCheked()
        {

            if (lstGridCheckDevices == null || lstGridCheckDevices.Count == 0) return;


            //var sl = (List<DeviceItem>)dtGridCheckDevices.ItemsSource;

            //// var sl = (List<DeviceItem>)dtGridCheckDevices.ItemsSource;

            //if (sl == null)
            //{
            //    chkSelectAll.IsChecked = false;
            //    return;
            //}


            foreach (DeviceItem di in lstGridCheckDevices)
            {
                di.IsCheck = chkSelectAll;
            }

        }



        void refresh()
        {
            var lineId = SelectedItemLine?.Id;
            var vestGrId = SelectedItemGroupVest?.Id;

            if (lineId == 0 && vestGrId == null) return;


            var vestId = SelectedItemVest?.Id;
            var isAllDev = selectedcmdChekViewDev?.Id;

            // var lst = bLayer.getDevItemsForUpdate(lineId, vestId.Value );

            List<DeviceItem> lst = null;

            try
            {
                //!!!!!
                lst = bLayer.getDevItemsForUpdate(lineId, vestId, vestGrId, new int?(CodeUpdt), isAllDev);



            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                //return;
            }



             LstGridCheckDevices = new ObservableCollection<DeviceItem>( lst);

            //chkSelectAll.IsChecked = false;

        }



        private void CmbSelectionChanged(object obj)
        {
            
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}

using SutUpdateAv.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Dto;
using ReactiveUI;
using Avalonia.Controls;
using Avalonia.Interactivity;
using SutUpdateAv.Common;
//using MsBox.Avalonia.Dto;
//using MsBox.Avalonia.Enums;
//using SutUpdateAv.Common;
using MsBox.Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;
using System.Linq;
using System.Reactive;

namespace SutUpdateAv.ViewModels
{
    public class DeviceUpdateViewViewModel : ViewModelBase, INotifyPropertyChanged
    {
        BusinessLayer bLayer = BusinessLayer.GetInstance();

        List<DeviceUpdateItem> deviceUpdateItems;

        Window? WndDeviceUpdateView;

        public ICommand IsDisplayUnusedCommand { get; }


        public ICommand DeleteSelectedCommand { get; } //cntxMenuItem_Click

        public ReactiveCommand <object, Unit> DeleteClickCommand { get; } //cntxMenuItem_Click


        //public ICommand CollectionSelectionChangedCommand { get; }



        //public List<DeviceUpdateItem> SelectedItems { get; set; }


        // public IList SelectedItems { get; set; }


        public List<DeviceUpdateItem> DeviceUpdateItems
        {
            get
            { 
                return deviceUpdateItems; 
            }
            set
            {
                if (deviceUpdateItems != value)
                {
                    deviceUpdateItems = value;
                    RaisePropertyChanged("DeviceUpdateItems");
                }
            }
            
        }

        int codeUpdt;
        public int CodeUpdt
        {
            get
            {
                return codeUpdt;
            }
        }
        
        string nameUpdt;

        public string NameUpdt
        {
            get
            {
                return nameUpdt;
            }
        }

        public DeviceUpdateViewViewModel()
        {
          
        }



        public DeviceUpdateViewViewModel(int codeUpdt, string nameUpdt)
        {
            this.codeUpdt = codeUpdt;

            this.nameUpdt = nameUpdt;

            DeleteSelectedCommand = ReactiveCommand.Create<object>(DeleteSelected);

            DeleteClickCommand = ReactiveCommand.Create<object>(DeleteClick);

        

     

            //CollectionSelectionChangedCommand = ReactiveCommand.Create<object>(SelectionChanged);

            Refresh();

        }

        //private void SelectionChanged(object selitems)
        //{

        //}

            private void DeleteClick(object obj)
            {

            if (WndDeviceUpdateView == null)
            {
                WndDeviceUpdateView = ((IClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime).Windows?
                 .FirstOrDefault(f => f.Name == "WndDeviceUpdateView");
            }

              var v = MessageBoxHelper.ShowMessageBoxNoAsync(new MessageBoxStandardParams
                {
                    ContentMessage = "Хотите удалить выбранные назначения?",
                    ButtonDefinitions = ButtonEnum.YesNo,
                    ContentHeader = "Подтвердите действие",
                    Icon = MsBox.Avalonia.Enums.Icon.Question,
                    //ContentTitle = "Внимание",
                    MinWidth = 400,
                    MinHeight = 200,
                    CanResize = false,
                    ShowInCenter = true,
                    Width = 400,
                    Height = 200

                }, WndDeviceUpdateView);



                if (v == ButtonResult.Yes)
                {
                    DeleteRecordFromDevUpdates(new List<long> {  Convert.ToInt64(obj)});
                }

        }

        private void DeleteSelected(object datagrid)
        {

            DataGrid dataGrid = datagrid as DataGrid; // разобраться как в параметр передавать только selecteditems!!!!! 

            if (dataGrid == null) return;


            var slctd = dataGrid.SelectedItems.Cast<DeviceUpdateItem>().ToList();

            List<long> duids = ((List<DeviceUpdateItem>)slctd).Select(f => f.DuId).ToList(); // после messagebox selecteditems убирается из грида!!!


            if (duids != null && duids.Count() > 0)
            {
                if (WndDeviceUpdateView == null)
                {
                    WndDeviceUpdateView = ((IClassicDesktopStyleApplicationLifetime)Application.Current?.ApplicationLifetime).Windows?
                     .FirstOrDefault(f => f.Name == "WndDeviceUpdateView");
                }

                //var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;




                var v = MessageBoxHelper.ShowMessageBoxNoAsync(new MessageBoxStandardParams
                {
                    ContentMessage = "Хотите удалить выбранные назначения?",
                    ButtonDefinitions = ButtonEnum.YesNo,
                    ContentHeader = "Подтвердите действие",
                    Icon = MsBox.Avalonia.Enums.Icon.Question,
                    //ContentTitle = "Внимание",
                    MinWidth = 400,
                    MinHeight = 200,
                    CanResize = false,
                    ShowInCenter = true,
                    Width = 400,
                    Height = 200

                }, WndDeviceUpdateView);



                if (v == ButtonResult.Yes)
                {
                    DeleteRecordFromDevUpdates(duids);
                }

            }




        }

        private void DeleteRecordFromDevUpdates(List<long> duIds)
        {
            bool d = false;


            try
            {
                d = bLayer.deleteRecordFromDevUpdates(duIds);
            }
            catch (Exception ex)
            {


                MessageBoxHelper.ShowMessageBoxNoAsync(new MessageBoxStandardParams
                {
                    ContentMessage = ex.ToString(),
                   ButtonDefinitions = ButtonEnum.Ok,
                   ContentHeader = "Ошибка",
                   Icon = Icon.Error,
                   //ContentTitle = "Внимание",
                    MinWidth = 400,
                    MinHeight = 200,
                    CanResize = false,
                    ShowInCenter = true,
                    Width = 400,
                    Height = 200

                }, WndDeviceUpdateView);

                return;
            }



            if (d)
            {

                MessageBoxHelper.ShowMessageBoxNoAsync(new MessageBoxStandardParams
                {
                    ContentMessage = "Удалено",
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentHeader = "Информация",
                    Icon = Icon.Info,
                    //ContentTitle = "Внимание",
                    MinWidth = 400,
                    MinHeight = 100,
                    CanResize = false,
                    ShowInCenter = true,
                    Width = 400,
                    Height = 100
                }, WndDeviceUpdateView);


                 Refresh();
            }
            else
            {

                MessageBoxHelper.ShowMessageBoxNoAsync(new MessageBoxStandardParams
                {
                    ContentMessage = "Ошибка выполнения в БД, подробности в файле лога приложения",
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentHeader = "Ошибка",
                    Icon = Icon.Error,
                    //ContentTitle = "Внимание",
                    MinWidth = 400,
                    MinHeight = 100,
                    CanResize = false,
                    ShowInCenter = true,
                    Width = 400,
                    Height = 100


                }, WndDeviceUpdateView);

            }

        }

        private void Refresh()
        {
            try
            {
                DeviceUpdateItems = bLayer.getStatusDevicesUpdateItems(CodeUpdt);
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }









        public event PropertyChangedEventHandler? PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

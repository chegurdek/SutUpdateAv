using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Dto;
using ReactiveUI;
using SutUpdateAv.Common;
using SutUpdateAv.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Collections;


namespace SutUpdateAv
{
    public partial class DeviceUpdateView : Window
    {

        BusinessLayer bLayer = BusinessLayer.GetInstance();
        int CodeUpdt;
        string NameUpdt;

        //public ICommand DeleteSelectedCommand { get; } //cntxMenuItem_Click

        ViewModels.DeviceUpdateViewViewModel vModel = null;
            
            
            

        public DeviceUpdateView(int codeUpdt, string nameUpdt)
        {
            InitializeComponent();

            vModel = new ViewModels.DeviceUpdateViewViewModel(codeUpdt, nameUpdt);

            this.DataContext = vModel;



            this.Title += UserSetting.Title;

            this.KeyDown += DeviceUpdateView_KeyDown;

          //  dtGridDeviceUpdateItems.SelectionChanged += DtGridDeviceUpdateItems_SelectionChanged;
           // Load();
        }

        private void DtGridDeviceUpdateItems_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            try
            {


                var fgfhgf = dtGridDeviceUpdateItems.SelectedIndex;




                //vModel.SelectedItems = dtGridDeviceUpdateItems.SelectedItems.Cast<DeviceUpdateItem>().ToList();
            }
            catch (Exception ex)
            {
            }
      


         
        }



        //public DeviceUpdateView(int codeUpdt, string nameUpdt)
        //{
        //    InitializeComponent();

        //    this.Title += UserSetting.Title;

        //    CodeUpdt = codeUpdt;
        //    NameUpdt = nameUpdt;

        //    lblCodeUpdt.Content += codeUpdt.ToString();
        //    lblNameUpdt.Content += nameUpdt;


        //   // DeleteSelectedCommand  = ReactiveCommand.Create(DeleteSelected);

        //    this.KeyDown += DeviceUpdateView_KeyDown;

        //    Load();
        //}

        private void DeviceUpdateView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (((MenuItem)sender).Name == "ExitMenuItem")
            {
                this.Close();
            }

        }

        //private void Load()
        //{

        //    //bLayer = new BusinessLayer(UserSetting.SutConnectionString, UserSetting.SutProvider);
        //    Refresh();

        //}

        //private void Refresh()
        //{
        //    try
        //    {
        //        dtGridDeviceUpdateItems.ItemsSource = bLayer.getStatusDevicesUpdateItems(CodeUpdt);
        //    }
        //    catch ( Exception ex)
        //    {
        //       // MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //        return;
        //    }
        //}
         

        private void ContextMenu_ContextMenuOpening(object sender, RoutedEventArgs e)
        {


            if (dtGridDeviceUpdateItems == null || dtGridDeviceUpdateItems.SelectedItem == null)
            {
                //gridCntxMenu.IsOpen = false;
                return;

            }
            else
            {
                var v = dtGridDeviceUpdateItems.SelectedItems.Cast<DeviceUpdateItem>().Where(f => f.IsDel);

                if (v?.Count() == 0)
                {
                    var t = ((IEnumerable<ContextMenu>)gridCntxMenu.Items).ElementAt(0);

                  // (( MenuItem )t).

                  //  ((MenuItem)gridCntxMenu.Items[0]).IsEnabled = false;
                }
                else
                {
                  //  ((MenuItem)gridCntxMenu.Items[0]).IsEnabled = true;
                }


                ///////
                ///


                try
                {

                    //foreach (var si in dtGridDeviceUpdateItems.Items)
                    //{

                    //    DataGridRow row = (DataGridRow)dtGridDeviceUpdateItems.ItemContainerGenerator.ContainerFromItem(si);

                    //    if (row != null)
                    //    {
                    //        var b = dtGridDeviceUpdateItems.Columns[9].GetCellContent(row);

                    //        if (!((DeviceUpdateItem)b.DataContext).IsDel)
                    //        {
                    //            row.IsSelected = false;
                    //        }
                    //    }

                    //}
                }
                catch (Exception ex)
                {

                }



            }

        }


        public void DeleteSelectedCommand()
        {

            if (dtGridDeviceUpdateItems == null || dtGridDeviceUpdateItems.SelectedItems == null) return;


            List<DeviceUpdateItem> SelectedItemsList = dtGridDeviceUpdateItems.SelectedItems.Cast<DeviceUpdateItem>().ToList();

            var dels = dtGridDeviceUpdateItems.SelectedItems.Cast<DeviceUpdateItem>().Where(f => f.IsDel).Select(ff => ff.DuId).ToList();


            var v = MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
            {
                ContentMessage = "Хотите удалить выбранные назначения?",
                ButtonDefinitions = ButtonEnum.YesNo,
                ContentHeader = "Подтвердите действие",
                Icon = MsBox.Avalonia.Enums.Icon.Question,
                //ContentTitle = "Внимание",
                MinWidth = 400,
                MinHeight = 100,
                CanResize = false,
                ShowInCenter = true,
                Width = 400,
                Height = 100


            }, this);


            if (v == ButtonResult.Yes)
            {
                DeleteRecordFromDevUpdates(dels);
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

                MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
                {
                    ContentMessage = ex.ToString(),
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentHeader = "Ошибка",
                    Icon = MsBox.Avalonia.Enums.Icon.Error,
                    //ContentTitle = "Внимание",
                    MinWidth = 400,
                    MinHeight = 100,
                    CanResize = false,
                    ShowInCenter = true,
                    Width = 400,
                    Height = 100


                }, this);


              
                return;
            }



            if (d)
            {
                MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
                {
                    ContentMessage = "Удалено",
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentHeader = "Ошибка",
                    Icon = MsBox.Avalonia.Enums.Icon.Info,
                    //ContentTitle = "Внимание",
                    MinWidth = 400,
                    MinHeight = 100,
                    CanResize = false,
                    ShowInCenter = true,
                    Width = 400,
                    Height = 100


                }, this);


               // Refresh();
            }
            else
            {

                MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
                {
                    ContentMessage = "Ошибка выполнения в БД, подробности в файле лога приложения",
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentHeader = "Ошибка",
                    Icon = MsBox.Avalonia.Enums.Icon.Error,
                    //ContentTitle = "Внимание",
                    MinWidth = 400,
                    MinHeight = 100,
                    CanResize = false,
                    ShowInCenter = true,
                    Width = 400,
                    Height = 100


                }, this);

            }

        }


        private void DeleteSelected()
       // private void cntxMenuItem_Click(object sender, RoutedEventArgs e)
        {

            if (dtGridDeviceUpdateItems == null || dtGridDeviceUpdateItems.SelectedItems == null) return;


            List<DeviceUpdateItem> SelectedItemsList = dtGridDeviceUpdateItems.SelectedItems.Cast<DeviceUpdateItem>().ToList();

            var dels = dtGridDeviceUpdateItems.SelectedItems.Cast<DeviceUpdateItem>().Where(f => f.IsDel).Select(ff => ff.DuId).ToList();

            //((List<DeviceUpdateItem>)dtGridDeviceUpdateItems.SelectedItems).Where(f => f.IsDel);


            //if (MessageBox.Show("Хотите изменить стаус обновления?",
            //         "Подтвердите действие", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)

            //if (MessageBox.Show("Хотите удалить выбранные назначения?",
            //           "Подтвердите действие", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            //{
            //    DeleteRecordFromDevUpdates(dels);

            //}

        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            var r = ((Button)e.Source).DataContext as DeviceUpdateItem;

            var v = MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
            {
                ContentMessage = "Хотите удалить выбранные назначения?",
                ButtonDefinitions = ButtonEnum.YesNo,
                ContentHeader = "Подтвердите действие",
                Icon = MsBox.Avalonia.Enums.Icon.Question,
                //ContentTitle = "Внимание",
                MinWidth = 400,
                MinHeight = 100,
                CanResize = false,
                ShowInCenter = true,
                Width = 400,
                Height = 100


            }, this);


            if (v == ButtonResult.Yes)
            {
                DeleteRecordFromDevUpdates(new List<long> { r.DuId });
            }



            //if (MessageBox.Show("Удалить назначение?",
            //           "Подтвердите действие", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            //{
            //    DeleteRecordFromDevUpdates(new List<long> { r.DuId });

            //}

        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            //Refresh();
        }


    }
}

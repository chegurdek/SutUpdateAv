using Avalonia.Controls;
using Avalonia.Interactivity;
using SutUpdateAv.Models;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Data.Common;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
//using System.Windows.Threading;
using System.Threading.Tasks;
using System.Threading;
using ReactiveUI;
using Avalonia.Threading;

using SutUpdateAv.Common;




namespace SutUpdateAv
{
    public partial class MainWindow : Window
    {

        BusinessLayer bLayer = BusinessLayer.GetInstance();


        public MainWindow()
        {

            InitializeComponent();

            Load();
        }

        private void Load()
        {

            this.Title += UserSetting.Title;

            this.Closed += MainWindow_Closed;
            this.Closing += MainWindow_Closing;

 

          // refresh();
        }


        private  void MainWindow_Closing(object sender, CancelEventArgs e)
        {
          // var v = await MessageBoxHelper.ShowErrorBox("Завершить работу программы?", this);
           var v =  MessageBoxHelper.ShowMessageBoxNoAsync("Завершить работу программы?", this );

            if (v != ButtonResult.Yes)
            {

                e.Cancel = true;
                //  Environment.Exit(0);
            }
            else
            {
                
            }






        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {

            //foreach (Window w in App.Current.Windows)
            //    w.Close();


        }

        private void refresh()
        {
            List<UpdateItem> updtItems = null;

            try
            {
                updtItems = bLayer.getUpdateItems();
            }
            catch (Exception ex)
            {

                //  MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }



            if (updtItems != null)
            {
                if (chkBoxDisplayUnused.IsChecked.Value)
                {
                     dtGridUpdateItems.ItemsSource = updtItems;
                }
                else
                {
                
                   dtGridUpdateItems.ItemsSource= updtItems.Where(f => !f.IsDisabled);
                }

            }


            //
            // dtGridUpdateItems.Items.Refresh();

        }


        private void btnAddFact_Click(object sender, RoutedEventArgs e)
        {
            var r = ((Button)e.Source).DataContext as Models.UpdateItem;
            //if (r.IsAppointed == "Да") return; // сделать disabled

            ShowFactUpdate(r.Id, r.Name);

        }

        private void FactUpdate_AddFactEvent(object sender, EventArgs e)
        {
            refresh();
        }

        private void btnViewFact_Click(object sender, RoutedEventArgs e)
        {
            var r = ((Button)e.Source).DataContext as Models.UpdateItem;

            ShowDeviceUpdates(r.Id, r.Name);

        }


        //private void NewWindowHandler(object sender, RoutedEventArgs e)
        //{
        //    Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
        //    newWindowThread.SetApartmentState(ApartmentState.STA);
        //    newWindowThread.IsBackground = true;
        //    newWindowThread.Start();
        //}

        //private void ThreadStartingPoint()
        //{
        //    Window1 tempWindow = new Window1();
        //    tempWindow.Show();
        //    System.Windows.Threading.Dispatcher.Run();
        //}


        private void DeleteRecordFromUpdate(int UpdId)
        {

            try
            {
                var r = bLayer.deleteRecordFromUpdate(UpdId);
                refresh();
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowMessageBoxNoAsync(new MessageBoxStandardParams
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

        }



        private void ShowDeviceUpdates(int UpdId, string UpdName)
        {
            DeviceUpdateView devView = new DeviceUpdateView(UpdId, UpdName);
            devView.Closed += DevView_Closed;
            devView.Show(this);
       

        }

        private void DevView_Closed(object sender, EventArgs e)
        {
            refresh();
        }

        private void chkBoxIsDisabled_Checked(object sender, RoutedEventArgs e)
        {
            bool curChkState = ((CheckBox)sender).IsChecked.Value;


            var v = MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
            {
                ContentMessage = "Хотите изменить стаус обновления?",
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
                var r = ((CheckBox)e.Source).DataContext as UpdateItem;

                if (r != null)
                {

                    try
                    {
                        if (bLayer.updateDisabledField(r.Id, Convert.ToInt32(curChkState)))
                        {
                            refresh();
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBoxHelper.ShowMessageBoxNoAsync(new MessageBoxStandardParams
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
                }

            }
            else
            {
                ((CheckBox)e.Source).IsChecked = !curChkState;
            }

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dfdf = ((MenuItem)sender).Header;

            if (((MenuItem)sender).Name == "ExitMenuItem")
            {
                // Application.Current.Shutdown();


                //if (MessageBox.Show("Завершить работу программы?", "Подтвердите действие", MessageBoxButton.YesNo,
                //  MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                //{
                //}

            }

        }


        private void ShowFactUpdate(int idUpd, string nameUpd)
        {
            FactUpdate2 factUpdate = new FactUpdate2(idUpd, nameUpd);
            factUpdate.AddFactEvent += FactUpdate_AddFactEvent;



            try
            {
                factUpdate.Show(this);
            }
            catch { }

        }

        private void cntxMenuItem_Click(object sender, RoutedEventArgs e)
        {

            if (dtGridUpdateItems == null || dtGridUpdateItems.SelectedItem == null) return;

            var updateItem = (UpdateItem)dtGridUpdateItems.SelectedItem;

            if (((MenuItem)sender).Name == "addMenuItem")
            {
                ShowFactUpdate(updateItem.Id, updateItem.Name);
            }

            if (((MenuItem)sender).Name == "viewMenuItem")
            {
                ShowDeviceUpdates(updateItem.Id, updateItem.Name);
            }

            if (((MenuItem)sender).Name == "deleteMenuItem")
            {
                DeleteRecordFromUpdate(updateItem.Id);
            }


        }



        #region INotifyPropertyChanged implementation
        // Basically, the UI thread subscribes to this event and update the binding if the received Property Name correspond to the Binding Path element
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private void btnDelFact_Click(object sender, RoutedEventArgs e)
        {
            //var updateItem = (UpdateItem)dtGridUpdateItems.SelectedItem;
            var r = ((Button)e.Source).DataContext as Models.UpdateItem;
            DeleteRecordFromUpdate(r.Id);

        }



        //private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        //{
        //    if (dtGridUpdateItems == null || dtGridUpdateItems.SelectedItem == null)
        //    {
        //        gridUpdtCntxMenu.IsOpen = false;
        //    }
        //    else
        //    {
        //        var updateItem = (UpdateItem)dtGridUpdateItems.SelectedItem;
        //        // как получить по имени? 
        //        ((MenuItem)gridUpdtCntxMenu.Items[2]).IsEnabled = updateItem.IsVis;
        //    }

        //}

        private void btnAddUpdt_Click(object sender, RoutedEventArgs e)
        {
            CreateUpdate2 createUpdate = new CreateUpdate2();
            createUpdate.AddUpdtEvent += CreateUpdate_AddUpdtEvent;

            try
            {
                createUpdate.ShowDialog(this);
            }
            catch { }

        }

        private void CreateUpdate_AddUpdtEvent(object sender, EventArgs e)
        {
            refresh();
        }



        private void chkBoxDisplayUnused_Checked(object sender, RoutedEventArgs e)
        {
            refresh();
        }

        //private void dtGridUpdateItems_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    if (dtGridUpdateItems == null || dtGridUpdateItems.SelectedItem == null) return;

        //    var updateItem = (UpdateItem)dtGridUpdateItems.SelectedItem;

        //    ShowDeviceUpdates(updateItem.Id, updateItem.Name);
        //}

        ///////////////////////////////////////////////////////////////////////////////////////////
        ///
                // private async void MainWindow_Closing(object sender, CancelEventArgs e)







    }
}



using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ReactiveUI;
using SutUpdateAv.Models;
using System.Collections.ObjectModel;
using Avalonia.Controls.ApplicationLifetimes;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using SutUpdateAv.Common;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia;


namespace SutUpdateAv.ViewModels
{
    //public class Person
    //{
    //    public string FirstName { get; set; }
    //    public string LastName { get; set; }

    //    public Person(string firstName, string lastName)
    //    {
    //        FirstName = firstName;
    //        LastName = lastName;
    //    }
    //}
    // потом сюда переписать !!!!!!
    //а может и не надо
    public class MainWindowViewModel : ViewModelBase,
     INotifyPropertyChanged
       {

        // public ObservableCollection<Person> People { get; }

        Window? thisWindow;

        BusinessLayer bLayer = BusinessLayer.GetInstance();

        bool isDisplayUnused;


        public ICommand ShowDeviceUpdatesCommand { get; } //btnViewFact_Click

        public ICommand IsDisplayUnusedCommand { get; }

        public ICommand DeleteUpdateItemCommand { get; }

        public ICommand DeleteSelectedCommand { get; } //cntxMenuItem_Click

        public ICommand AddNewItemCommand { get; } //btnAddUpdt_Click

        public ICommand DisabledChangeUpdatesCommand { get; } //chkBoxIsDisabled_Checked

        public ICommand AddFactCommand { get; }





        public bool IsDisplayUnused
        {
            get
            {
                return isDisplayUnused;
            }
            set
            {
                if (isDisplayUnused != value)
                {
                    isDisplayUnused = value;
                }
            }
        }


         ObservableCollection<UpdateItem> updateItems;
        public ObservableCollection<UpdateItem> UpdateItems
        {
            get
            {
                return updateItems;
            }
            set
            {
                if (updateItems != value)
                {
                    updateItems = value;
                    //не обновляет без PropertyChanged 
                    RaisePropertyChanged("UpdateItems");
                }
               
            }
        }




        public MainWindowViewModel(Window? thisWindow)
        {


            this.thisWindow = thisWindow;

            //var people = new List<Person>
            //{
            //    new Person("Neil", "Armstrong"),
            //    new Person("Buzz", "Lightyear"),
            //    new Person("James", "Kirk")
            //};
            //People = new ObservableCollection<Person>(people);

            //var tst = new List<UpdateItem>
            //{ 
            //   new UpdateItem {  Id = 1,  Description = "gjgjgjjg",  FileName = "fhfhfh"}
            //};

            //UpdateItems = new ObservableCollection<UpdateItem>(tst);

            IsDisplayUnusedCommand = ReactiveCommand.Create(Refresh);

            DeleteSelectedCommand = ReactiveCommand.Create<object>(DeleteSelected);

            AddNewItemCommand = ReactiveCommand.Create(AddNewUpdate);

            ShowDeviceUpdatesCommand = ReactiveCommand.Create<UpdateItem>(ShowDeviceUpdates);

            DisabledChangeUpdatesCommand = ReactiveCommand.Create<UpdateItem>(DisabledChangeUpdates);


            AddFactCommand = ReactiveCommand.Create<UpdateItem>(AddFact);


            Refresh();



            // bLayer = new BusinessLayer(UserSetting.SutConnectionString, UserSetting.SutProvider);
            // bLayer = new BusinessLayer(UserSetting.SutConnectionString);
            //updtItems = bLayer.getUpdateItems();

            //bLayer = new BusinessLayer(UserSetting.SutConnectionString, UserSetting.SutProvider);
        }



        private void AddFact(UpdateItem updateItem)
        {
           //var r = ((Button)e.Source).DataContext as Models.UpdateItem;
            //if (r.IsAppointed == "Да") return; // сделать disabled

            ShowFactUpdate(updateItem.Id, updateItem.Name);

        }

        private void FactUpdate_AddFactEvent(object sender, EventArgs e)
        {
            Refresh();
        }

        private void ShowFactUpdate(int idUpd, string nameUpd)
        {
            FactUpdate2 factUpdate = new FactUpdate2(idUpd, nameUpd);
            factUpdate.AddFactEvent += FactUpdate_AddFactEvent;



            try
            {
                factUpdate.Show(thisWindow);
            }
            catch 
            {

            }

        }


        private void DisabledChangeUpdates(UpdateItem updateItem)
        {
            // bool curChkState = ((CheckBox)sender).IsChecked.Value;


            bool curChkState = updateItem.IsDisabled;

            var v = MessageBoxHelper.ShowMessageBoxNoAsync(new MessageBoxStandardParams
            {
                ContentMessage = "Хотите изменить стаус обновления?",
                ButtonDefinitions = ButtonEnum.YesNo,
                ContentHeader = "Подтвердите действие",
                Icon = Icon.Question,
                //ContentTitle = "Внимание",
                MinWidth = 400,
                MinHeight = 200,
                CanResize = false,
                ShowInCenter = true,
                Width = 400,
                Height = 200

            }, thisWindow);


            if (v == ButtonResult.Yes)
            {
             

                    try
                    {
                        if (bLayer.updateDisabledField(updateItem.Id, Convert.ToInt32(curChkState)))
                        {
                            Refresh();
                        }
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

                        }, thisWindow);

                        return;
                    }
                

            }
            else
            {
                //((CheckBox)e.Source).IsChecked = !curChkState;
                // найти в коллекции
            }

        }



        private void ShowDeviceUpdates(UpdateItem updateItem)
        {
            //DeviceUpdateView devView = new DeviceUpdateView { DataContext = new ViewModels.DeviceUpdateViewViewModel(UpdId, UpdName) };
            DeviceUpdateView devView = new DeviceUpdateView(updateItem.Id, updateItem.Name);
            devView.Closed += DevView_Closed;
            devView.Show(thisWindow);

        }

        private void DevView_Closed(object sender, EventArgs e)
        {
            Refresh();
        }

        private void AddNewUpdate()
        {
            CreateUpdate2 createUpdate = new CreateUpdate2 { DataContext = new SutUpdateAv.ViewModels.CreateUpdateViewModel() };
            createUpdate.AddUpdtEvent += CreateUpdate_AddUpdtEvent;

            try
            {
                createUpdate.ShowDialog(thisWindow);
            }
            catch { }

        }

        private void CreateUpdate_AddUpdtEvent(object sender, EventArgs e)
        {
            Refresh();
        }

        private void DeleteRecordFromUpdate(int UpdId)
        {

            try
            {
                var r = bLayer.deleteRecordFromUpdate(UpdId);
                Refresh();
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

                }, thisWindow);


                return;
            }

        }


        private void DeleteSelected(object slctd)
        {

          //  if (slctd == null) return;

            var updtItem = slctd as UpdateItem;

            if (updtItem == null) return;

          //  var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;


            var v = MessageBoxHelper.ShowMessageBoxNoAsync(new MessageBoxStandardParams
            {
                ContentMessage = "Хотите удалить выбранное обновление?",
                ButtonDefinitions = ButtonEnum.YesNo,
                ContentHeader = "Подтвердите действие",
                Icon = Icon.Question,
                //ContentTitle = "Внимание",
                MinWidth = 400,
                MinHeight = 200,
                CanResize = false,
                ShowInCenter = true,
                Width = 400,
                Height = 200

            }, thisWindow);



            if (v == ButtonResult.Yes)
            {
                DeleteRecordFromUpdate(updtItem.Id);
            }



        }
        private void Refresh()
        {
            var lst = IsDisplayUnused ? bLayer.getUpdateItems() : bLayer.getUpdateItems().Where(f => !f.IsDisabled).ToList();
   


            UpdateItems = new ObservableCollection<UpdateItem>(lst);
        


            //if (updtItems != null)
            //{
            //    if (chkBoxDisplayUnused.IsChecked.Value)
            //    {
            //        dtGridUpdateItems.Items = updtItems;
            //    }
            //    else
            //    {
            //        dtGridUpdateItems.Items = updtItems.Where(f => !f.IsDisabled);
            //    }

            //}

        }

        //public ICommand AddUpdateCommand { get; }


        //private void AddUpdate ()
        //{

        //}


        // our View about changes.
        public event PropertyChangedEventHandler? PropertyChanged;

        // For convinience we add a method which will raise the above event.
        private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}

using Avalonia.Controls;
using Avalonia.Interactivity;
using SutUpdateAv.Models;
using System.Collections.Generic;
using System;
using System.Text;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using SutUpdateAv.Common;
using System.Linq;
using System.Collections.ObjectModel;

namespace SutUpdateAv
{
    public partial class FactUpdate2 : Window
    {
        BusinessLayer bLayer = BusinessLayer.GetInstance();
        public event EventHandler AddFactEvent;

        int CodeUpdt;
        string NameUpdt;

        public FactUpdate2() 
        { 
        }


        public FactUpdate2(int codeUpdt, string nameUpdt)
        {
            InitializeComponent();
            this.Title += UserSetting.Title;
            CodeUpdt = codeUpdt;
            NameUpdt = nameUpdt;

            lblCodeUpdt.Content += codeUpdt.ToString();
            lblNameUpdt.Content += nameUpdt;


            ViewModels.FactUpdate2ViewModel vModel = new ViewModels.FactUpdate2ViewModel(codeUpdt, nameUpdt);

            this.DataContext = vModel;


        }





       // вернуть!!!
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            try //temp
            {

                var chkDevView = ((Item)cmdChekViewDev.SelectedItem)?.Id;

                var vestGrId = ((Item)cmbGroupVest.SelectedItem)?.Id;

                var lineId = ((Item)cmbLines.SelectedItem)?.Id;

                if (lineId == 0 && vestGrId == 0)
                {


                    // MessageBoxStandardParams в класс??
                    MessageBoxHelper.ShowMessageBoxNoAsync(new MessageBoxStandardParams
                    {
                        ContentMessage = "Не выбрана линия или группа!",
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentHeader = "Внимание",
                        Icon = MsBox.Avalonia.Enums.Icon.Info,
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

                var vestId = ((Item)cmbVest.SelectedItem)?.Id;


                List<int> listChkDevs = null;


                var obCollDevItems = dtGridCheckDevices.ItemsSource as ObservableCollection<DeviceItem>;



                if (obCollDevItems == null)
                {

                    MessageBoxHelper.ShowMessageBoxNoAsync(new MessageBoxStandardParams
                    {
                        ContentMessage = "Не выбрано ни одного устройства!",
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentHeader = "Внимание",
                        Icon = MsBox.Avalonia.Enums.Icon.Info,
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



                var dvids = obCollDevItems.Where(f => f.IsCheck).Select(ff => ff.DevId);

                if (dvids == null || dvids.Count() == 0)
                {
                    MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
                    {
                        ContentMessage = "Не выбрано ни одного устройства!",
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentHeader = "Внимание",
                        Icon = MsBox.Avalonia.Enums.Icon.Info,
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


                StringBuilder mesBuild = new StringBuilder();
                mesBuild.Append("Назначить обновление для");


                int? updType = null;

                //вернуть !!!!!
                if (chkSelectAll.IsChecked.Value)
                {
                    mesBuild.Append(" всех");
                }

                if (chkDevView == 2)
                {
                    mesBuild.Append(" назначенных");

                }
                else if (chkDevView == 1)
                {
                    mesBuild.Append(" не назначенных");
                }




                if (lineId > 0 && (vestId == 0 || vestId == null))
                {
                    mesBuild.Append($" устройств на линии \"{((Item)cmbLines.SelectedItem)?.Value}\"");

                }
                if (vestId > 0)
                {
                    mesBuild.Append($" устройств на вестибюле \"{((Item)cmbVest.SelectedItem)?.Value}\"");

                }
                if (vestGrId > 0)
                {
                    mesBuild.Append($" устройств в группе вестибюлей \"{((Item)cmbGroupVest.SelectedItem)?.Value}\"");

                }




                //if (chkSelectAll.IsChecked.Value)
                if ((chkSelectAll.IsChecked.Value) &&
                    obCollDevItems.Where(f => f.IsCheck).Count() ==
                    obCollDevItems.Count())
                {
                    if (lineId > 0 && vestId == 0)
                    {
                        updType = 8;
                    }
                    if (vestId > 0)
                    {
                        updType = 2;
                    }
                    if (vestGrId > 0)
                    {
                        updType = 16;
                    }

                }
                else
                {
                    var l = obCollDevItems.Where(f => f.IsCheck).Select(ff => ff.DevId);

                    if (l != null && l.Count() > 0)
                    {
                        listChkDevs = l.ToList();

                    }
                    else
                    {


                        MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
                        {
                            ContentMessage = "Не выбрано ни одного устройства!",
                            ButtonDefinitions = ButtonEnum.Ok,
                            ContentHeader = "Внимание",
                            Icon = MsBox.Avalonia.Enums.Icon.Info,
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

                mesBuild.Append($" в количестве {dvids.Count()} шт.?");
                //if (chkDevView == 2)
                //{
                //    mesBuild.Append("/r/nПри невыбранном флаге \"Принудительное обновление\" повторное назначение не произойдет");
                //}





                DateTime? fromDate = null;

                if (dtPickerAccessUpdate.SelectedDate != null)
                {

                    DateTime? fromTime = null;
                    fromDate = DateTime.Parse(dtPickerAccessUpdate.SelectedDate.ToString());

                    try
                    {

                        // добавить контрол  


                        if (mskTxtTime.Text != null && fromDate != null)
                        {
                            fromTime = DateTime.Parse(mskTxtTime.Text);

                            fromDate = new DateTime(fromDate.Value.Year, fromDate.Value.Month, fromDate.Value.Day,
                                fromTime.Value.Hour, fromTime.Value.Minute, fromTime.Value.Second);
                        }


                        if ((fromDate.Value.ToShortDateString() == DateTime.Now.ToShortDateString()) && (String.IsNullOrEmpty(mskTxtTime.Text)))
                        {
                            fromDate = DateTime.Now;
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
                        {
                            ContentMessage = "Некорректное время!",
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



                    //if (fromDate < DateTime.Now || fromDate > DateTime.Now.AddDays(7))
                    //{
                    //    MessageBox.Show($"Дата должна находится в диапазоне от {DateTime.Now} по {DateTime.Now.AddDays(7)}!",
                    //         "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    //    return;
                    //}

                }

                //var hghgh = dtPickerAccessUpdate.DisplayDate;




                int countAdded = 0;

                string errmess = null;


                #region проверка уже добавленных

                //if (!chkBoxIsForceRefr.IsChecked.Value)
                //{
                //    List<int> lstIsAdded = bLayer.checkIsAddedToFact(CodeUpdt, updType, vestId, null,
                //    lineId, vestGrId, chkDevs?.ToList());

                //    if (lstIsAdded?.Count() > 0)
                //    {
                //        var y = String.Join(", ", lstIsAdded);

                //        string mess = $"Для устройств {String.Join(", ", lstIsAdded)} повторно обновление назначено не будет." +
                //            " Если хотите продолжить нажмите \"Ok\". Если хотите повторно назначить указанным устройствам обновление, " +
                //            " выберите флаг \"Принудительное обновление\" ";

                //        MessageBoxResult messageBoxResult = MessageBox.Show(mess, "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

                //        if (messageBoxResult == MessageBoxResult.OK)
                //        {
                //            countAdded = bLayer.addUpdateFact(CodeUpdt, UserSetting.SutUser, updType, vestId, null,
                //                 lineId, vestGrId, fromDate, chkBoxIsForceRefr.IsChecked.Value, chkDevs?.ToList(), out errmess);
                //        }

                //    }
                //    else
                //    {
                //        countAdded = bLayer.addUpdateFact(CodeUpdt, UserSetting.SutUser, updType, vestId, null,
                //              lineId, vestGrId, fromDate, chkBoxIsForceRefr.IsChecked.Value, chkDevs?.ToList(), out errmess);
                //    }

                //}
                //else
                //{
                //    countAdded = bLayer.addUpdateFact(CodeUpdt, UserSetting.SutUser, updType, vestId, null,
                //    lineId, vestGrId, fromDate, chkBoxIsForceRefr.IsChecked.Value, chkDevs?.ToList(), out errmess);
                //}
                #endregion



                var v = MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
                {
                    ContentMessage = mesBuild.ToString(),
                    ButtonDefinitions = ButtonEnum.YesNo,
                    ContentHeader = "Подтвердите действие",
                    Icon = MsBox.Avalonia.Enums.Icon.Question,
                    //ContentTitle = "Внимание",
                    MinWidth = 400,
                    MinHeight = 100,
                    CanResize = false,
                    ShowInCenter = true,
                    Width = 400,
                    Height = 400


                }, this);

                if (v == ButtonResult.Yes)
                {

                    try
                    {
                        countAdded = bLayer.addUpdateFact(CodeUpdt, UserSetting.SutUser, updType, vestId, null,
                        lineId, vestGrId, fromDate, chkBoxIsForceRefr.IsChecked.Value,
                        //listChkDevs?.ToList(), 
                        listChkDevs,
                        out errmess);
                    }
                    catch (Exception ex)
                    {

                        MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
                        {
                            ContentMessage = ex.Message,
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



                    if (countAdded == 0 && !String.IsNullOrEmpty(errmess))
                    {

                        MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
                        {
                            ContentMessage = "Ошибка при записи в базу." + "/r/n" +
                            $"Дополнительная информация в логе",
                            ButtonDefinitions = ButtonEnum.Ok,
                            ContentHeader = "Ошибка",
                            Icon = MsBox.Avalonia.Enums.Icon.Error,
                            MinWidth = 400,
                            MinHeight = 100,
                            CanResize = false,
                            ShowInCenter = true,
                            Width = 400,
                            Height = 100


                        }, this);





                    }
                    else if (countAdded == 0 && String.IsNullOrEmpty(errmess))
                    {
                        MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
                        {
                            ContentMessage = $"Всем выбранным устройствам уже было назначено данное обновление.",
                            ButtonDefinitions = ButtonEnum.Ok,
                            ContentHeader = "Уведомление",
                            Icon = MsBox.Avalonia.Enums.Icon.Info,
                            MinWidth = 400,
                            MinHeight = 100,
                            CanResize = false,
                            ShowInCenter = true,
                            Width = 400,
                            Height = 100


                        }, this);


                    }
                    else
                    {

                        MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
                        {
                            ContentMessage = $"Обновление назначено для устройств в количестве {countAdded} шт. ",
                            ButtonDefinitions = ButtonEnum.Ok,
                            ContentHeader = "Уведомление",
                            Icon = MsBox.Avalonia.Enums.Icon.Success,
                            MinWidth = 400,
                            MinHeight = 100,
                            CanResize = false,
                            ShowInCenter = true,
                            Width = 400,
                            Height = 100


                        }, this);



                    }



                    var hndlAddFactEvent = AddFactEvent;
                    if (hndlAddFactEvent != null) hndlAddFactEvent(null, null);

                }
                else
                {
                    return;
                }

            }

            catch (Exception ex)
            {

                //Logger.Log.Error(ex.ToString());

                MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
                {
                    ContentMessage = "Ошибка",
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentHeader = "Ошибка",
                    Icon = MsBox.Avalonia.Enums.Icon.Success,
                    MinWidth = 400,
                    MinHeight = 100,
                    CanResize = false,
                    ShowInCenter = true,
                    Width = 400,
                    Height = 100


                }, this);


            }




            this.Close();

            return;


            //var b = bLayer.addUpdateFact(CodeUpdt, ConnectSetting.Login, updType, vestId == 0 ? null : vestId, null, 
            //    lineId, null, fromDate, chkBoxIsForceRefr.IsChecked.Value, chkDevs?.ToList());


            // возвращать количество записей добавленных в базу? 


        }



        private void chkDev_Unchecked(object sender, RoutedEventArgs e)
        {
            chkSelectAll.IsChecked = false;
        }

        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {

            if (dtGridCheckDevices == null) return;

            var sl = (List<DeviceItem>)dtGridCheckDevices.ItemsSource;

           // var sl = (List<DeviceItem>)dtGridCheckDevices.ItemsSource;

            if (sl == null)
            {
                chkSelectAll.IsChecked = false;
                return;
            }


            foreach (DeviceItem di in sl)
            {
                di.IsCheck = chkSelectAll.IsChecked.Value;
            }

        }




        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {

           // refresh();
        }


    }
}

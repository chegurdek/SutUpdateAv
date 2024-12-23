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


            Load();

        }



        private void Load()
        {
          
            
          // bLayer = new BusinessLayer(UserSetting.SutConnectionString, UserSetting.SutProvider);



            //var ty = bLayer.getComponets();
            //try
            //{
            //    cmbLines.DataContext = bLayer.getLines();
            //    cmbGroupVest.DataContext = bLayer.getGroupVests();

            //    //cmbLines.Items = bLayer.getLines(); 
            //    //cmbGroupVest.Items = bLayer.getGroupVests();
            //}
            //catch (Exception ex)
            //{
            //    //MessageBox.Show(ex.Message, "������", MessageBoxButton.OK, MessageBoxImage.Error);
            //    this.Close();

            //}

            //cmbLines.SelectionChanged += Cmb_SelectionChanged;
            //cmbVest.SelectionChanged += Cmb_SelectionChanged;
            //cmbGroupVest.SelectionChanged += Cmb_SelectionChanged;
            ////cmdChekViewDev.SelectionChanged += CmdChekViewDev_SelectionChanged;
            //cmdChekViewDev.SelectionChanged += Cmb_SelectionChanged;


            // cmdChekViewDev.Items = Common.Constants.ChoiceViewDevList;


            //cmdChekViewDev.DataContext = Common.Constants.ChoiceViewDevList;




            // dtPickerAccessUpdate.va = DateTime.Now;
            //dtPickerAccessUpdate.DisplayDateEnd = DateTime.Now.AddDays(7);

            // this.KeyDown += FactUpdate_KeyDown;


        }



        //�������!!!
        //private void btnAdd_Click(object sender, RoutedEventArgs e)
        //{

        //    try //temp
        //    {

        //        var chkDevView = ((Item)cmdChekViewDev.SelectedItem)?.Id;

        //        var vestGrId = ((Item)cmbGroupVest.SelectedItem)?.Id;

        //        var lineId = ((Item)cmbLines.SelectedItem)?.Id;

        //        if (lineId == 0 && vestGrId == 0)
        //        {


        //            // MessageBoxStandardParams � �����??
        //           MessageBoxHelper.ShowMessageBoxNoAsync(new MessageBoxStandardParams
        //            {
        //                ContentMessage = "�� ������� ����� ��� ������!",
        //                ButtonDefinitions = ButtonEnum.Ok,
        //                ContentHeader = "��������",
        //                Icon = MsBox.Avalonia.Enums.Icon.Info,
        //                //ContentTitle = "��������",
        //                MinWidth = 400,
        //                MinHeight = 100,
        //                CanResize = false,
        //                ShowInCenter = true,
        //                Width = 400,
        //                Height = 100


        //            }, this);


        //            return;
        //        }

        //        var vestId = ((Item)cmbVest.SelectedItem)?.Id;


        //        List<int> listChkDevs = null;

        //        if ((List<DeviceItem>)dtGridCheckDevices.ItemsSource == null)
        //        {

        //            MessageBoxHelper.ShowMessageBoxNoAsync(new MessageBoxStandardParams
        //            {
        //                ContentMessage = "�� ������� �� ������ ����������!",
        //                ButtonDefinitions = ButtonEnum.Ok,
        //                ContentHeader = "��������",
        //                Icon = MsBox.Avalonia.Enums.Icon.Info,
        //                //ContentTitle = "��������",
        //                MinWidth = 400,
        //                MinHeight = 100,
        //                CanResize = false,
        //                ShowInCenter = true,
        //                Width = 400,
        //                Height = 100


        //            }, this);



        //            return;
        //        }



        //        var dvids = ((List<DeviceItem>)dtGridCheckDevices.ItemsSource).Where(f => f.IsCheck).Select(ff => ff.DevId);

        //        if (dvids == null || dvids.Count() == 0)
        //        {
        //            MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
        //            {
        //                ContentMessage = "�� ������� �� ������ ����������!",
        //                ButtonDefinitions = ButtonEnum.Ok,
        //                ContentHeader = "��������",
        //                Icon = MsBox.Avalonia.Enums.Icon.Info,
        //                //ContentTitle = "��������",
        //                MinWidth = 400,
        //                MinHeight = 100,
        //                CanResize = false,
        //                ShowInCenter = true,
        //                Width = 400,
        //                Height = 100


        //            }, this);


        //            return;
        //        }


        //        StringBuilder mesBuild = new StringBuilder();
        //        mesBuild.Append("��������� ���������� ���");


        //        int? updType = null;

        //        //������� !!!!!
        //        if (chkSelectAll.IsChecked.Value)
        //        {
        //            mesBuild.Append(" ����");
        //        }

        //        if (chkDevView == 2)
        //        {
        //            mesBuild.Append(" �����������");

        //        }
        //        else if (chkDevView == 1)
        //        {
        //            mesBuild.Append(" �� �����������");
        //        }




        //        if (lineId > 0 && (vestId == 0 || vestId == null))
        //        {
        //            mesBuild.Append($" ��������� �� ����� \"{((Item)cmbLines.SelectedItem)?.Value}\"");

        //        }
        //        if (vestId > 0)
        //        {
        //            mesBuild.Append($" ��������� �� ��������� \"{((Item)cmbVest.SelectedItem)?.Value}\"");

        //        }
        //        if (vestGrId > 0)
        //        {
        //            mesBuild.Append($" ��������� � ������ ���������� \"{((Item)cmbGroupVest.SelectedItem)?.Value}\"");

        //        }




        //        //if (chkSelectAll.IsChecked.Value)
        //        if ((chkSelectAll.IsChecked.Value) &&
        //            ((List<DeviceItem>)dtGridCheckDevices.ItemsSource).Where(f => f.IsCheck).Count() ==
        //            ((List<DeviceItem>)dtGridCheckDevices.ItemsSource).Count())
        //        {
        //            if (lineId > 0 && vestId == 0)
        //            {
        //                updType = 8;
        //            }
        //            if (vestId > 0)
        //            {
        //                updType = 2;
        //            }
        //            if (vestGrId > 0)
        //            {
        //                updType = 16;
        //            }

        //        }
        //        else
        //        {
        //            var l = ((List<DeviceItem>)dtGridCheckDevices.ItemsSource).Where(f => f.IsCheck).Select(ff => ff.DevId);

        //            if (l != null && l.Count() > 0)
        //            {
        //                listChkDevs = l.ToList();

        //            }
        //            else
        //            {


        //                 MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
        //                {
        //                    ContentMessage = "�� ������� �� ������ ����������!",
        //                    ButtonDefinitions = ButtonEnum.Ok,
        //                    ContentHeader = "��������",
        //                    Icon = MsBox.Avalonia.Enums.Icon.Info,
        //                    //ContentTitle = "��������",
        //                    MinWidth = 400,
        //                    MinHeight = 100,
        //                    CanResize = false,
        //                    ShowInCenter = true,
        //                    Width = 400,
        //                    Height = 100


        //                }, this);


                    
          
        //                return;

        //            }

        //        }

        //        mesBuild.Append($" � ���������� {dvids.Count()} ��.?");
        //        //if (chkDevView == 2)
        //        //{
        //        //    mesBuild.Append("/r/n��� ����������� ����� \"�������������� ����������\" ��������� ���������� �� ����������");
        //        //}





        //        DateTime? fromDate = null;

        //        if (dtPickerAccessUpdate.SelectedDate != null)
        //        {

        //            DateTime? fromTime = null;
        //            fromDate = DateTime.Parse(dtPickerAccessUpdate.SelectedDate.ToString());

        //            try
        //            {

        //                // �������� �������  

                 
        //                if (mskTxtTime.Text != null && fromDate != null)
        //                {
        //                    fromTime = DateTime.Parse(mskTxtTime.Text);

        //                    fromDate = new DateTime(fromDate.Value.Year, fromDate.Value.Month, fromDate.Value.Day,
        //                        fromTime.Value.Hour, fromTime.Value.Minute, fromTime.Value.Second);
        //                }


        //                if ((fromDate.Value.ToShortDateString() == DateTime.Now.ToShortDateString()) && (String.IsNullOrEmpty(mskTxtTime.Text)))
        //                {
        //                    fromDate = DateTime.Now;
        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
        //                {
        //                    ContentMessage = "������������ �����!",
        //                    ButtonDefinitions = ButtonEnum.Ok,
        //                    ContentHeader = "������",
        //                    Icon = MsBox.Avalonia.Enums.Icon.Error,
        //                    //ContentTitle = "��������",
        //                    MinWidth = 400,
        //                    MinHeight = 100,
        //                    CanResize = false,
        //                    ShowInCenter = true,
        //                    Width = 400,
        //                    Height = 100


        //                }, this);


        //                return;
        //            }



        //            //if (fromDate < DateTime.Now || fromDate > DateTime.Now.AddDays(7))
        //            //{
        //            //    MessageBox.Show($"���� ������ ��������� � ��������� �� {DateTime.Now} �� {DateTime.Now.AddDays(7)}!",
        //            //         "��������", MessageBoxButton.OK, MessageBoxImage.Exclamation);

        //            //    return;
        //            //}

        //        }

        //        //var hghgh = dtPickerAccessUpdate.DisplayDate;




        //        int countAdded = 0;

        //        string errmess = null;


        //        #region �������� ��� �����������

        //        //if (!chkBoxIsForceRefr.IsChecked.Value)
        //        //{
        //        //    List<int> lstIsAdded = bLayer.checkIsAddedToFact(CodeUpdt, updType, vestId, null,
        //        //    lineId, vestGrId, chkDevs?.ToList());

        //        //    if (lstIsAdded?.Count() > 0)
        //        //    {
        //        //        var y = String.Join(", ", lstIsAdded);

        //        //        string mess = $"��� ��������� {String.Join(", ", lstIsAdded)} �������� ���������� ��������� �� �����." +
        //        //            " ���� ������ ���������� ������� \"Ok\". ���� ������ �������� ��������� ��������� ����������� ����������, " +
        //        //            " �������� ���� \"�������������� ����������\" ";

        //        //        MessageBoxResult messageBoxResult = MessageBox.Show(mess, "��������", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

        //        //        if (messageBoxResult == MessageBoxResult.OK)
        //        //        {
        //        //            countAdded = bLayer.addUpdateFact(CodeUpdt, UserSetting.SutUser, updType, vestId, null,
        //        //                 lineId, vestGrId, fromDate, chkBoxIsForceRefr.IsChecked.Value, chkDevs?.ToList(), out errmess);
        //        //        }

        //        //    }
        //        //    else
        //        //    {
        //        //        countAdded = bLayer.addUpdateFact(CodeUpdt, UserSetting.SutUser, updType, vestId, null,
        //        //              lineId, vestGrId, fromDate, chkBoxIsForceRefr.IsChecked.Value, chkDevs?.ToList(), out errmess);
        //        //    }

        //        //}
        //        //else
        //        //{
        //        //    countAdded = bLayer.addUpdateFact(CodeUpdt, UserSetting.SutUser, updType, vestId, null,
        //        //    lineId, vestGrId, fromDate, chkBoxIsForceRefr.IsChecked.Value, chkDevs?.ToList(), out errmess);
        //        //}
        //        #endregion



        //        var v = MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
        //        {
        //            ContentMessage = mesBuild.ToString(),
        //            ButtonDefinitions = ButtonEnum.YesNo,
        //            ContentHeader = "����������� ��������",
        //            Icon = MsBox.Avalonia.Enums.Icon.Question,
        //            //ContentTitle = "��������",
        //            MinWidth = 400,
        //            MinHeight = 100,
        //            CanResize = false,
        //            ShowInCenter = true,
        //            Width = 400,
        //            Height = 100


        //        }, this);

        //        if (v == ButtonResult.Yes)
        //        {

        //            try
        //            {
        //                countAdded = bLayer.addUpdateFact(CodeUpdt, UserSetting.SutUser, updType, vestId, null,
        //                lineId, vestGrId, fromDate, chkBoxIsForceRefr.IsChecked.Value,
        //                //listChkDevs?.ToList(), 
        //                listChkDevs,
        //                out errmess);
        //            }
        //            catch (Exception ex)
        //            {

        //                MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
        //                {
        //                    ContentMessage = ex.Message,
        //                    ButtonDefinitions = ButtonEnum.Ok,
        //                    ContentHeader = "������",
        //                    Icon = MsBox.Avalonia.Enums.Icon.Error,
        //                    //ContentTitle = "��������",
        //                    MinWidth = 400,
        //                    MinHeight = 100,
        //                    CanResize = false,
        //                    ShowInCenter = true,
        //                    Width = 400,
        //                    Height = 100


        //                }, this);

                     
        //                return;
        //            }



        //            if (countAdded == 0 && !String.IsNullOrEmpty(errmess))
        //            {

        //                MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
        //                {
        //                    ContentMessage = "������ ��� ������ � ����." + "/r/n" +
        //                    $"�������������� ���������� � ����",
        //                    ButtonDefinitions = ButtonEnum.Ok,
        //                    ContentHeader = "������",
        //                    Icon = MsBox.Avalonia.Enums.Icon.Error,
        //                    MinWidth = 400,
        //                    MinHeight = 100,
        //                    CanResize = false,
        //                    ShowInCenter = true,
        //                    Width = 400,
        //                    Height = 100


        //                }, this);



                

        //            }
        //            else if (countAdded == 0 && String.IsNullOrEmpty(errmess))
        //            {
        //                MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
        //                {
        //                    ContentMessage = $"���� ��������� ����������� ��� ���� ��������� ������ ����������.",
        //                    ButtonDefinitions = ButtonEnum.Ok,
        //                    ContentHeader = "�����������",
        //                    Icon = MsBox.Avalonia.Enums.Icon.Info,
        //                    MinWidth = 400,
        //                    MinHeight = 100,
        //                    CanResize = false,
        //                    ShowInCenter = true,
        //                    Width = 400,
        //                    Height = 100


        //                }, this);


        //            }
        //            else
        //            {

        //                MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
        //                {
        //                    ContentMessage = $"���������� ��������� ��� ��������� � ���������� {countAdded} ��. ",
        //                    ButtonDefinitions = ButtonEnum.Ok,
        //                    ContentHeader = "�����������",
        //                    Icon = MsBox.Avalonia.Enums.Icon.Success,
        //                    MinWidth = 400,
        //                    MinHeight = 100,
        //                    CanResize = false,
        //                    ShowInCenter = true,
        //                    Width = 400,
        //                    Height = 100


        //                }, this);


                     
        //            }



        //            var hndlAddFactEvent = AddFactEvent;
        //            if (hndlAddFactEvent != null) hndlAddFactEvent(null, null);

        //        }
        //        else
        //        {
        //            return;
        //        }

        //    }

        //    catch (Exception ex)
        //    {

        //        //Logger.Log.Error(ex.ToString());

        //        MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
        //        {
        //            ContentMessage = "������",
        //            ButtonDefinitions = ButtonEnum.Ok,
        //            ContentHeader = "������",
        //            Icon = MsBox.Avalonia.Enums.Icon.Success,
        //            MinWidth = 400,
        //            MinHeight = 100,
        //            CanResize = false,
        //            ShowInCenter = true,
        //            Width = 400,
        //            Height = 100


        //        }, this);


        //    }




        //    this.Close();

        //    return;


        //    //var b = bLayer.addUpdateFact(CodeUpdt, ConnectSetting.Login, updType, vestId == 0 ? null : vestId, null, 
        //    //    lineId, null, fromDate, chkBoxIsForceRefr.IsChecked.Value, chkDevs?.ToList());


        //    // ���������� ���������� ������� ����������� � ����? 


        //}

        //private void Cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    ComboBox cmbSend = (ComboBox)sender;

        //    if (cmbSend.Name == "cmbLines")
        //    {
        //        var lineId = ((Item)cmbLines.SelectedItem).Id;

        //        try
        //        {
        //            cmbVest.ItemsSource = bLayer.getVest(lineId);
        //        }
        //        catch (Exception ex)
        //        {
        //           // MessageBox.Show(ex.Message, "������", MessageBoxButton.OK, MessageBoxImage.Error);
        //            return;
        //        }

        //        if (cmbVest.Items != null)
        //        {
        //            cmbVest.SelectedIndex = 0;
        //        }

        //        if (cmbLines.SelectedIndex > 0)
        //        {
        //            cmbGroupVest.SelectedIndex = 0;
        //        }

        //    }

        //    if (cmbSend.Name == "cmbGroupVest")
        //    {
        //        if (cmbGroupVest.SelectedIndex > 0)
        //        {
        //            cmbLines.SelectedIndex = 0;
        //            cmbVest.SelectedIndex = 0;
        //        }

        //    }

        //    if (cmbSend.Name == "cmbVest")
        //    {
        //        if (cmbVest.SelectedIndex > 0)
        //        {

        //            cmbGroupVest.SelectedIndex = 0;
        //        }

        //    }


        //    refresh();

        //    //dtGridCheckDevices.ItemsSource = null;
        //    //chkSelectAll.IsChecked = true;



        //}

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


        //void refresh()
        //{
        //    var lineId = ((Item)cmbLines.SelectedItem)?.Id;
        //    var vestGrId = ((Item)cmbGroupVest.SelectedItem)?.Id;

        //    if (lineId == 0 && vestGrId == null) return;

        //    var vestId = ((Item)cmbVest.SelectedItem)?.Id;

        //    var isAllDev = ((Item)cmdChekViewDev.SelectedItem)?.Id;

        //    // var lst = bLayer.getDevItemsForUpdate(lineId, vestId.Value );

        //    List<DeviceItem> lst = null;

        //    try
        //    {
        //        //!!!!!
        //        lst = bLayer.getDevItemsForUpdate(lineId, vestId, vestGrId, new int?(CodeUpdt), isAllDev);



        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show(ex.Message, "������", MessageBoxButton.OK, MessageBoxImage.Error);
        //        //return;
        //    }



        //    dtGridCheckDevices.ItemsSource = lst;

        //   // chkSelectAll.IsChecked = false;

        //}

     

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

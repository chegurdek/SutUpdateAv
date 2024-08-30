using Avalonia.Controls;
using Avalonia.Interactivity;
using SutUpdateAv.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SutUpdateAv
{
    public partial class FactUpdate : Window
    {
        BusinessLayer bLayer;// = new BusinessLayer();
        int CodeUpdt;
        string NameUpdt;

        public event EventHandler AddFactEvent;


        public FactUpdate() { }


        public FactUpdate(int codeUpdt, string nameUpdt)
        {
            InitializeComponent();
            this.Title += UserSetting.Title;
            CodeUpdt = codeUpdt;
            NameUpdt = nameUpdt;

           // lblCodeUpdt.Content += codeUpdt.ToString();
           // lblNameUpdt.Content += nameUpdt;
           // Load();

        }

        private void chkDev_Unchecked(object sender, RoutedEventArgs e)
        {
            chkSelectAll.IsChecked = false;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {

            refresh();
        }

        void refresh()
        {
            var lineId = ((Item)cmbLines.SelectedItem)?.Id;
            var vestGrId = ((Item)cmbGroupVest.SelectedItem)?.Id;

            if (lineId == 0 && vestGrId == null) return;

            var vestId = ((Item)cmbVest.SelectedItem)?.Id;

            var isAllDev = ((Item)cmdChekViewDev.SelectedItem)?.Id;

           // var lst = bLayer.getDevItemsForUpdate(lineId, vestId.Value );

            List<DeviceItem> lst = null;

            try
            {
                //!!!!!
                lst = bLayer.getDevItemsForUpdate(lineId, vestId, vestGrId, new int?(CodeUpdt), isAllDev);



            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "������", MessageBoxButton.OK, MessageBoxImage.Error);
                //return;
            }



            dtGridCheckDevices.ItemsSource = lst;

            chkSelectAll.IsChecked = false;

        }

        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {

            var sl = (List<DeviceItem>)dtGridCheckDevices.ItemsSource;

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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            try //temp
            {

                var chkDevView = ((Item)cmdChekViewDev.SelectedItem)?.Id;

                var vestGrId = ((Item)cmbGroupVest.SelectedItem)?.Id;

                var lineId = ((Item)cmbLines.SelectedItem)?.Id;

                if (lineId == 0 && vestGrId == 0)
                {
                    //MessageBox.Show("�� ������� ����� ��� ������!",
                    //       "��������", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                var vestId = ((Item)cmbVest.SelectedItem)?.Id;


                List<int> listChkDevs = null;

                if ((List<DeviceItem>)dtGridCheckDevices.ItemsSource == null)
                {
                    //MessageBox.Show("�� ������� �� ������ ����������!",
                    //     "��������", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    return;
                }



                var dvids = ((List<DeviceItem>)dtGridCheckDevices.ItemsSource).Where(f => f.IsCheck).Select(ff => ff.DevId);

                if (dvids == null || dvids.Count() == 0)
                {
                    //MessageBox.Show("�� ������� �� ������ ����������!",
                    //        "��������", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    return;
                }


                StringBuilder mesBuild = new StringBuilder();
                mesBuild.Append("��������� ���������� ���");


                int? updType = null;

                if (chkSelectAll.IsChecked.Value)
                {
                    mesBuild.Append(" ����");
                }

                if (chkDevView == 2)
                {
                    mesBuild.Append(" �����������");

                }
                else if (chkDevView == 1)
                {
                    mesBuild.Append(" �� �����������");
                }




                if (lineId > 0 && (vestId == 0 || vestId == null))
                {
                    mesBuild.Append($" ��������� �� ����� \"{((Item)cmbLines.SelectedItem)?.Value}\"");

                }
                if (vestId > 0)
                {
                    mesBuild.Append($" ��������� �� ��������� \"{((Item)cmbVest.SelectedItem)?.Value}\"");

                }
                if (vestGrId > 0)
                {
                    mesBuild.Append($" ��������� � ������ ���������� \"{((Item)cmbGroupVest.SelectedItem)?.Value}\"");

                }




                //if (chkSelectAll.IsChecked.Value)
                if ((chkSelectAll.IsChecked.Value) &&
                    ((List<DeviceItem>)dtGridCheckDevices.ItemsSource).Where(f => f.IsCheck).Count() ==
                    ((List<DeviceItem>)dtGridCheckDevices.ItemsSource).Count())
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
                    var v = ((List<DeviceItem>)dtGridCheckDevices.ItemsSource).Where(f => f.IsCheck).Select(ff => ff.DevId);

                    if (v != null && v.Count() > 0)
                    {
                        listChkDevs = v.ToList();

                    }
                    else
                    {

                        //MessageBox.Show("�� ������� �� ������ ����������!",
                             //"��������", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                        return;

                    }

                }

                mesBuild.Append($" � ���������� {dvids.Count()} ��.?");
                //if (chkDevView == 2)
                //{
                //    mesBuild.Append("/r/n��� ����������� ����� \"�������������� ����������\" ��������� ���������� �� ����������");
                //}





                DateTime? fromDate = null;

                if (dtPickerAccessUpdate.SelectedDate != null)
                {

                    DateTime? fromTime = null;
                  //  fromDate = dtPickerAccessUpdate.SelectedDate;

                    try
                    {

                        //if (mskTxtTime.Value != null && fromDate != null)
                        //{
                        //    fromTime = DateTime.Parse(mskTxtTime.Value.ToString());

                        //    fromDate = new DateTime(fromDate.Value.Year, fromDate.Value.Month, fromDate.Value.Day,
                        //        fromTime.Value.Hour, fromTime.Value.Minute, fromTime.Value.Second);
                        //}
                      

                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show($"������������ �����!",
                        //    "������", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                        return;
                    }



                    //if (fromDate < DateTime.Now || fromDate > DateTime.Now.AddDays(7))
                    //{
                    //    MessageBox.Show($"���� ������ ��������� � ��������� �� {DateTime.Now} �� {DateTime.Now.AddDays(7)}!",
                    //         "��������", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                    //    return;
                    //}

                }

                //var hghgh = dtPickerAccessUpdate.DisplayDate;




                int countAdded = 0;

                string errmess = null;


                #region �������� ��� �����������

                //if (!chkBoxIsForceRefr.IsChecked.Value)
                //{
                //    List<int> lstIsAdded = bLayer.checkIsAddedToFact(CodeUpdt, updType, vestId, null,
                //    lineId, vestGrId, chkDevs?.ToList());

                //    if (lstIsAdded?.Count() > 0)
                //    {
                //        var y = String.Join(", ", lstIsAdded);

                //        string mess = $"��� ��������� {String.Join(", ", lstIsAdded)} �������� ���������� ��������� �� �����." +
                //            " ���� ������ ���������� ������� \"Ok\". ���� ������ �������� ��������� ��������� ����������� ����������, " +
                //            " �������� ���� \"�������������� ����������\" ";

                //        MessageBoxResult messageBoxResult = MessageBox.Show(mess, "��������", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

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

                if (true) //!!!

                //if (MessageBox.Show(mesBuild.ToString(), "����������� ��������", MessageBoxButton.YesNo,
                //    MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {


                    try
                    {
                        //!!!!
                        countAdded = bLayer.addUpdateFact(CodeUpdt, UserSetting.SutUser, updType, vestId, null,
                        lineId, vestGrId, fromDate, chkBoxIsForceRefr.IsChecked.Value,
                        //listChkDevs?.ToList(), 
                        listChkDevs,
                        out errmess);

                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message, "������", MessageBoxButton.OK, MessageBoxImage.Error);
                        //return;
                    }



                    if (countAdded == 0 && !String.IsNullOrEmpty(errmess))
                    {
                        //MessageBox.Show("������ ��� ������ � ����." + "/r/n" +
                        //    $"�������������� ���������� � ����", "������", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                    else if (countAdded == 0 && String.IsNullOrEmpty(errmess))
                    {

                        //MessageBox.Show($"���� ��������� ����������� ��� ���� ��������� ������ ����������.",
                        //     "�����������", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    else
                    {
                        //MessageBox.Show($"���������� ��������� ��� ��������� � ���������� {countAdded} ��. ",
                        //   "�����������", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                    //ConnectSetting.Login



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
                //MessageBox.Show("������",
                //       "������", MessageBoxButton.OK, MessageBoxImage.Error);
            }




            this.Close();

            return;


            //var b = bLayer.addUpdateFact(CodeUpdt, ConnectSetting.Login, updType, vestId == 0 ? null : vestId, null, 
            //    lineId, null, fromDate, chkBoxIsForceRefr.IsChecked.Value, chkDevs?.ToList());


            // ���������� ���������� ������� ����������� � ����? 


        }

    }
}

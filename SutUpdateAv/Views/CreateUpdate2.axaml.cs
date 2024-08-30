using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using SutUpdateAv.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Text.Encodings;
using MsBox.Avalonia.Enums;
using SutUpdateAv.Common;
using Serilog;

namespace SutUpdateAv
{
    public partial class CreateUpdate2 : Window
    {

        BusinessLayer bLayer = BusinessLayer.GetInstance();
        FileInfo fiOpenDialog = null;
        public event EventHandler AddUpdtEvent;
        public CreateUpdate2()
        {
            InitializeComponent();
            Load();
        }

        public async Task<string> GetPath()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            // dialog.Filters.Add(new FileDialogFilter() { Name = "Text", Extensions = { "txt" } });

            string[] result = await dialog.ShowAsync(this);

            // await GetPath();

            if (result != null)
            {
                return string.Join(" ", result);
            }

            return null;
        }


        public async void Browse_Clicked(object sender, RoutedEventArgs args)
        {
            string _path = await GetPath();

            // var context = this.DataContext as TxtViewModel;
            //context.Path = _path;
        }



        private void Load()
        {


           // bLayer = new BusinessLayer(UserSetting.SutConnectionString, UserSetting.SutProvider);

            //List<Item> items = null;

            //try
            //{
            //    //!!!!!!!
            //    items = bLayer.getSoftware();  // закоментировать для отображения дизайнера
            //    //!!!!!!!

            //    cmbPoCode.Items = items;

            //   // cmbPoCode.DataContext = items;

            //    if (items?.Count == 1)
            //    {
            //        cmbPoCode.SelectedIndex = 0;
            //    }

            //}
            //catch (Exception ex)
            //{
            //    //MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //    this.Close();

            //}

            this.KeyDown += CreateUpdate_KeyDown;
            cmbPoCode.DataContextChanged += CmbPoCode_DataContextChanged;

    


        }

        private void CmbPoCode_DataContextChanged(object? sender, EventArgs e)
        {
            //if (cmbPoCode.Items.Count > 0)
            //{
            //    cmbPoCode.SelectedIndex = 0;
            //}
        }

        private void CreateUpdate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private bool checkEmptyFileld()
        {


            if (fiOpenDialog == null)
            {

                var v = MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
                {
                     ContentMessage = "Не выбран файл!",
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

                //MessageBox.Show("Не выбран файл!",
                //  "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                return false;
            }

            //if (String.IsNullOrWhiteSpace(txtBoxNameUpdt.Text))
            //{
            //    //MessageBox.Show("Не заполнено поле \"Название обновления\"!",
            //       // "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            //    return false;

            //}
            //if (String.IsNullOrWhiteSpace(txtBoxDescr.Text))
            //{
            //    //MessageBox.Show("Не заполнено поле \"Описание обновления\"!",
            //    //    "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            //    return false;
            //}
            //if (cmbPoCode.SelectedIndex == -1)
            //{
            //    //MessageBox.Show("Не выбран \"Код ПО\"!",
            //    //  "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            //    return false;
            //}



            return true;

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void  btnOpenFile_Click(object sender, RoutedEventArgs e)
        {

            string fName = await GetPath();

            if (!String.IsNullOrEmpty(fName))
            {
                fiOpenDialog = new FileInfo(fName);

                lblNameFile.Content = fiOpenDialog.FullName;
                txtBoxNameUpdt.Text = fiOpenDialog.Name;

                string pathDescFile = Path.Combine(fiOpenDialog.DirectoryName, fiOpenDialog.Name + ".txt");

                if (File.Exists(pathDescFile))
                {
                   // string sDesc = File.ReadAllText(pathDescFile, GetEncoding("windows-1251"));
                   string sDesc = File.ReadAllText(pathDescFile);
                   txtBoxDescr.Text = sDesc;
                }

            }

            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //// openFileDialog.Filters.Add(new FileDialogFilter { "tar.gz files (*.tar.gz)|*.tar.gz|All files (*.*)|*.*" });
            ////openFileDialog.Filter = "tar.gz files (*.tar.gz)|*.tar.gz|All files (*.*)|*.*";


            //string[] result = openFileDialog.ShowAsync(this).Result;

            //if (result != null)
            //{
            //    string fName = string.Join(" ", result);


            //    fiOpenDialog = new FileInfo(fName);

            //    //  lblNameFile.Content = fiOpenDialog.FullName;

            //    //   txtBoxNameUpdt.Text = fiOpenDialog.Name;


            //    string pathDescFile = Path.Combine(fiOpenDialog.DirectoryName, fiOpenDialog.Name + ".txt");

            //    //if (File.Exists(pathDescFile))
            //    //{
            //    //    string sDesc = File.ReadAllText(pathDescFile, Encoding.GetEncoding("windows-1251"));
            //    //    txtBoxDescr.Text = sDesc;
            //    //}


            //}

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (!checkEmptyFileld()) return;

            UpdateItem updtItem = null;

            try
            {
                updtItem = new UpdateItem
                {
                    Name = txtBoxNameUpdt.Text,
                    Description = txtBoxDescr.Text,
                    Binary = File.ReadAllBytes(fiOpenDialog.FullName),
                    CreateDateByUser = UserSetting.SutUser,
                    FileName = fiOpenDialog.Name,

                    SoId = ((Item)cmbPoCode.SelectedItem).Id,
                    IsDisabled = false,
             
         
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }





            int r = 0;

            try
            {
                r = bLayer.addUpdateItem(updtItem);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }


            if (r > 0)
            {

                //clearControls();
                var hndlAddUpdtEvent = AddUpdtEvent;
                if (hndlAddUpdtEvent != null) hndlAddUpdtEvent(null, null);

                var v = MessageBoxHelper.ShowMessageBoxNoAsync(new MsBox.Avalonia.Dto.MessageBoxStandardParams
                {
                    ContentMessage = "Успешно добавлено",
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentHeader = "Информация",
                    Icon = MsBox.Avalonia.Enums.Icon.Info,
                    //ContentTitle = "Внимание",
                    MinWidth = 400,
                    MinHeight = 100,
                    CanResize = false,
                    ShowInCenter = true,
                    Width = 400,
                    Height = 100

                }, this);


                this.Close();
   
            }


            //refresh();
        }

    }
}

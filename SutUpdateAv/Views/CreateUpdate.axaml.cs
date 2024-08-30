using Avalonia.Controls;
using SutUpdateAv.Models;
using System.IO;
using System;
using System.Collections.Generic;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Serilog;



namespace SutUpdateAv
{
    public partial class CreateUpdate : Window
    {

        BusinessLayer bLayer = BusinessLayer.GetInstance();
        FileInfo fiOpenDialog = null;
        public event EventHandler AddUpdtEvent;

        public CreateUpdate()
        {
            InitializeComponent();
            Load();
        }

        private void Load()
        {


           // bLayer = new BusinessLayer(UserSetting.SutConnectionString, UserSetting.SutProvider);

            List<Item> items = null;

            try
            {
              //  items = bLayer.getSoftware();
                cmbPoCode.ItemsSource = items;

                if (items?.Count == 1)
                {
                   // cmbPoCode.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();

            }

            this.KeyDown += CreateUpdate_KeyDown;


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
                //MessageBox.Show("Не выбран файл!",
                //  "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                return false;
            }

            if (String.IsNullOrWhiteSpace(txtBoxNameUpdt.Text))
            {
                //MessageBox.Show("Не заполнено поле \"Название обновления\"!",
                // "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;

            }
            if (String.IsNullOrWhiteSpace(txtBoxDescr.Text))
            {
                //MessageBox.Show("Не заполнено поле \"Описание обновления\"!",
                //    "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                return false;
            }
            if (cmbPoCode.SelectedIndex == -1)
            {
                //MessageBox.Show("Не выбран \"Код ПО\"!",
                //  "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                return false;
            }



            return true;

        }
        private void clearControls()
        {


            lblNameFile.Content = null;
            fiOpenDialog = null;
            txtBoxDescr.Text = null;
            txtBoxNameUpdt.Text = null;
            cmbPoCode.SelectedIndex = -1;

            //var lstChecked = ((List<CheckItem>)lstViewCompnts.ItemsSource).Where(f => f.IsCheck).ToList();


            //foreach (CheckItem chkitem in lstChecked)
            //{
            //    chkitem.IsCheck = false;
            //}



            //lstViewCompnts.Items.Refresh();


        }

        public async Task<string> GetPath()
        {
            OpenFileDialog dialog = new OpenFileDialog();
           // dialog.Filters.Add(new FileDialogFilter() { Name = "Text", Extensions = { "txt" } });

            string[] result = await dialog.ShowAsync(this);

            if (result != null)
            {
                await GetPath();
            }

            return string.Join(" ", result);
        }


        public async void Browse_Clicked(object sender, RoutedEventArgs args)
        {
            string _path = await GetPath();

            // var context = this.DataContext as TxtViewModel;
            //context.Path = _path;
        }


        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {



            OpenFileDialog openFileDialog = new OpenFileDialog();
            // openFileDialog.Filters.Add(new FileDialogFilter { "tar.gz files (*.tar.gz)|*.tar.gz|All files (*.*)|*.*" });
            //openFileDialog.Filter = "tar.gz files (*.tar.gz)|*.tar.gz|All files (*.*)|*.*";


            string[] result = openFileDialog.ShowAsync(this).Result;

            if (result != null)
            {
                string fName =  string.Join(" ", result);


                //fiOpenDialog = new FileInfo(fName);

              //  lblNameFile.Content = fiOpenDialog.FullName;

             //   txtBoxNameUpdt.Text = fiOpenDialog.Name;


              //  string pathDescFile = Path.Combine(fiOpenDialog.DirectoryName, fiOpenDialog.Name + ".txt");

                //if (File.Exists(pathDescFile))
                //{
                //    string sDesc = File.ReadAllText(pathDescFile, Encoding.GetEncoding("windows-1251"));
                //    txtBoxDescr.Text = sDesc;
                //}


            }

        }





        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (!checkEmptyFileld()) return;

            UpdateItem updtItem = null;

            try
            {
                updtItem = new UpdateItem
                {


                    Binary = File.ReadAllBytes(fiOpenDialog.FullName),
                    //CreateDate = DateTime.Now.ToString(),
                    CreateDateByUser = UserSetting.SutUser,
                    // Description = txtBoxDescr.Text,
                    IsDisabled = false,
                    FileName = fiOpenDialog.Name,
                    // Name = txtBoxNameUpdt.Text,
                    // SoId = ((Item)cmbPoCode.SelectedItem).Id
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }





            int r = 0;

            try
            {
                //r = bLayer.addUpdateItem(updtItem);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }


            if (r > 0)
            {

                clearControls();
                var hndlAddUpdtEvent = AddUpdtEvent;
                if (hndlAddUpdtEvent != null) hndlAddUpdtEvent(null, null);

                //MessageBox.Show("Успешно добавлено",
                //       "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
                // tbControlUpdate.SelectedIndex = 1;

            }


            //refresh();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

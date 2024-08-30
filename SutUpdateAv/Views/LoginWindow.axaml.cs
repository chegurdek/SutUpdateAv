using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using SutUpdateAv.Models;
using MsBox.Avalonia;
using Newtonsoft.Json;
using System.Text;
//using log4net;
//using log4net.Config;
using System.Reflection;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Threading;
using MsBox.Avalonia.Enums;
using SutUpdateAv.Common;
using MsBox.Avalonia.Dto;
using Serilog;

namespace SutUpdateAv
{
    public partial class LoginWindow : Window
    {
        List<string> strProveders = new List<string> { "MSSQL", "NPGSQL" };

        public LoginWindow()
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //Logger.InitLogger();

            //XmlConfigurator.Configure(
            //  LogManager.GetRepository(Assembly.GetAssembly(typeof(LogManager))),
            //  new FileInfo("log4net.config"));


            // Logger.TryGet(LogEventLevel.Fatal, LogArea.Control)?.Log(this, "Avalonia Infrastructure");



            // System.Diagnostics.Debug.WriteLine(Assembly.GetAssembly(typeof(LogManager)).Location);


            //var msBox = MessageBox.Avalonia.MessageBoxManager
            //            .GetMessageBoxStandardWindow(
            //          new MessageBox.Avalonia.DTO.MessageBoxStandardParams
            //          {
            //              ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
            //              ContentTitle = "Внимание",
            //              ContentMessage = Assembly.GetAssembly(typeof(LogManager)).Location,
            //              Icon = MessageBox.Avalonia.Enums.Icon.Info,

            //              // Style =  Style.UbuntuLinux
            //          });
            //msBox.Show();



            //var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            //XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            // System.Diagnostics.Debug.WriteLine("run");

           // Log.Information("Tst");

            // Logger.Log.Info("run");

            InitializeComponent();

            cmbProv.ItemsSource = strProveders;
            cmbProv.SelectedIndex = 0;

            UserSetting userSetting = null;

            if (File.Exists(UserSetting.UserSettingSutUpdaterFile))
            {
              
                //var msBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                //                .GetMessageBoxStandardWindow(
                //              new MessageBox.Avalonia.DTO.MessageBoxStandardParams
                //              {
                //                  ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                //                  ContentTitle = "Внимание",
                //                  ContentMessage = UserSetting.UserSettingSutUpdaterFile,
                //                  Icon = MessageBox.Avalonia.Enums.Icon.Info,

                //                  // Style =  Style.UbuntuLinux
                //              });
                //msBoxStandardWindow.Show(this);


                userSetting = JsonConvert.DeserializeObject<UserSetting>(File.ReadAllText(UserSetting.UserSettingSutUpdaterFile));

                txtBoxServer.Text = userSetting.DataSource;
                txtBoxDb.Text = userSetting.InitialCatalog;
                txtBoxLogin.Text = userSetting.UserID;
                cmbProv.SelectedItem =  userSetting.Provider;

                txtBoxPass.Focus();
            }
            else
            {
                txtBoxServer.Focus();
            }
        }


        private void signin()
        {

            UserSetting userSetting = new UserSetting(txtBoxServer.Text, txtBoxDb.Text, txtBoxLogin.Text,
                txtBoxPass.Text, cmbProv.SelectedItem.ToString());



            BusinessLayer bl = new BusinessLayer();

            SqlConnectionStringBuilder sqlConnBuilder = new SqlConnectionStringBuilder
            {
                InitialCatalog = txtBoxDb.Text,
                DataSource = txtBoxServer.Text,
                UserID = txtBoxLogin.Text,
                Password = txtBoxPass.Text

            };


            Cursor tCursor = this.Cursor;


            this.Cursor = new Cursor(StandardCursorType.Wait);


         
            if (bl.CheckConnect(UserSetting.SutConnectionString, UserSetting.SutProvider))
            {
              

                try
                {
                    //var settFileName = Path.Combine(Environment.GetFolderPath(
                    //    Environment.SpecialFolder.ApplicationData), UserSetting.UserSettingSutUpdaterFile);

                    var settFileName = UserSetting.UserSettingSutUpdaterFile;



                    using (StreamWriter file = File.CreateText(settFileName))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(file, userSetting);
                    }

                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());

                    //Logger.Log.Error(ex.ToString());
                }



                this.Cursor = tCursor;

                MainWindow mainWindow = new MainWindow();
              //  MainWindow mainWindow = new MainWindow { DataContext = new ViewModels.MainWindowViewModel() };
                mainWindow.Show();
                this.Close();
            }

            else
            {
                this.Cursor = tCursor;


                MessageBoxHelper.ShowMessageBoxNoAsync(new MessageBoxStandardParams
                {
                    ContentMessage = "Неверные данные подключения или нет связи с БД!",
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentHeader = "Ошибка",
                    Icon = MsBox.Avalonia.Enums.Icon.Error,
                    //ContentTitle = "Внимание",
                    MinWidth = 400,
                    MinHeight = 200,
                    CanResize = false,
                    ShowInCenter = true,
                    Width = 400,
                    Height = 200


                }, this);



                // var v = await MessageBoxHelper.ShowErrorBox("Завершить работу программы?", this);
                //var v = MessageBoxHelper.ShowMessageBoxNoAsync("Завершить работу программы?", this);

                //if (v != ButtonResult.Yes)
                //{

                //    e.Cancel = true;
                //    //  Environment.Exit(0);
                //}
                //else
                //{

                //}




                //Mouse.OverrideCursor = null;


                //var messageBoxStandardWindow = MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(
                //    new MessageBox.Avalonia.DTO.MessageBoxStandardParams
                //    {
                //        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                //        ContentTitle = "Ошибка",
                //       // ContentHeader = "header",
                //        ContentMessage = "Неверные данные подключения или нет связи с БД!",
                //       // WindowIcon = new WindowIcon("icon-rider.png")
                //    });



                //MessageBox.Show("Неверные данные подключения или нет связи с БД!",
                //     "Ошибка", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }



        private bool checkEmptyFileld()
        {
           

      
            if (String.IsNullOrWhiteSpace(txtBoxServer.Text))
            {
                //var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                //     .GetMessageBoxStandardWindow("title", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed...");
                //messageBoxStandardWindow.Show();


                //var msBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                //        .GetMessageBoxStandardWindow(
                //      new MessageBox.Avalonia.DTO.MessageBoxStandardParams
                //        {
                //            ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                //            ContentTitle = "Внимание",
                //            ContentMessage = "Не заполнено поле \"Сервер\"!",
                //            Icon =   MessageBox.Avalonia.Enums.Icon.Info,
                            
                //           // Style =  Style.UbuntuLinux
                //        });
                //    msBoxStandardWindow.Show();



                //MessageBox.Show("Не заполнено поле \"Сервер\"!",
                //     "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                return false;
            }
            if (String.IsNullOrWhiteSpace(txtBoxDb.Text))
            {

                //var msBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                //     .GetMessageBoxStandardWindow(
                //   new MessageBox.Avalonia.DTO.MessageBoxStandardParams
                //   {
                //       ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                //       ContentTitle = "Внимание",
                //       ContentMessage = "Не заполнено поле \"База\"!",
                //       Icon = MessageBox.Avalonia.Enums.Icon.Info,

                //       // Style =  Style.UbuntuLinux
                //   });
                //msBoxStandardWindow.Show();


                //MessageBox.Show("Не заполнено поле \"База\"!",
                //     "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            if (String.IsNullOrWhiteSpace(txtBoxPass.Text))
            {

                //var msBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                //   .GetMessageBoxStandardWindow(
                // new MessageBox.Avalonia.DTO.MessageBoxStandardParams
                // {
                //     ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                //     ContentTitle = "Внимание",
                //     ContentMessage = "Не заполнено поле \"Пароль\"!",
                //     Icon = MessageBox.Avalonia.Enums.Icon.Info,

                //     // Style =  Style.UbuntuLinux
                // });
                //msBoxStandardWindow.Show();

                //MessageBox.Show("Не заполнено поле \"Пароль\"!",
                //     "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            if (String.IsNullOrWhiteSpace(txtBoxLogin.Text))
            {
                //MessageBox.Show("Не заполнено поле \"Логин\"!",
                //     "Внимание", MessageBoxButton.OK, MessageBoxImage.Exclamation);


               // var msBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
               //  .GetMessageBoxStandardWindow(
               //new MessageBox.Avalonia.DTO.MessageBoxStandardParams
               //{
               //    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
               //    ContentTitle = "Внимание",
               //    ContentMessage = "Не заполнено поле \"Логин\"!",
               //    Icon = MessageBox.Avalonia.Enums.Icon.Info,

               //    // Style =  Style.UbuntuLinux
               //});
               // msBoxStandardWindow.Show();
                return false;
            }

            return true;

        }


        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (!checkEmptyFileld()) return;

            signin();

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
            //System.Windows.Application.Current.Shutdown();
        }

        private void txtBoxPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                signin();
            }


        }


    }
}

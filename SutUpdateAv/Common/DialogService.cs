using Avalonia.Controls;
using Avalonia.Threading;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SutUpdateAv.Common
{
    public class DialogService //: IDialogService
    {
        public DialogService() { }

        public static async Task<ButtonResult> ShowBox(MessageBoxStandardParams MBSparams, Window owner = null)
        {
            if (owner == null)
            {
                owner = new MainWindow();
            }

            Func<Task<ButtonResult>> www = new Func<Task<ButtonResult>>(() => MessageBoxManager.GetMessageBoxStandard(MBSparams).ShowAsPopupAsync(owner));
            ButtonResult result = await Dispatcher.UIThread.InvokeAsync(www);

            var box = MessageBoxManager.GetMessageBoxStandard(MBSparams).ShowAsPopupAsync(owner);

            return result;
        }


        public static ButtonResult ShowBoxNoAsync(MessageBoxStandardParams MBSparams, Window owner = null)
        {
            if (owner == null)
            {
                owner = new MainWindow();
            }



            // var box = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(MBSparams);




            ButtonResult result = ButtonResult.None;



            using (var source = new CancellationTokenSource())
            {
                if (Dispatcher.UIThread.CheckAccess()) //Check if we are already on the UI thread
                {
                    MessageBoxManager.GetMessageBoxStandard(MBSparams).ShowAsPopupAsync(owner).ContinueWith
                    (
                        t =>
                        {
                            result = t.Result;
                            source.Cancel();
                        }
                    );
                    //var result =
                    //box.Show(owner).ContinueWith
                    //(
                    //    t =>
                    //    {
                    //        result = t.Result;
                    //        source.Cancel();
                    //    }
                    //);


                    //Dialog dialog = new Dialog(dialogViewModel);
                    //dialog.ShowDialog<MyDialogResult>(Parent).ContinueWith(t =>
                    //{
                    //    result = t.Result;
                    //    source.Cancel();
                    //});

                    Dispatcher.UIThread.MainLoop(source.Token);
                }
                else
                {
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        MessageBoxManager.GetMessageBoxStandard(MBSparams).ShowAsPopupAsync(owner).ContinueWith
                         (
                          t =>
                          {
                              result = t.Result;
                              source.Cancel();
                          }
                        );
                    });

                    while (!source.IsCancellationRequested) { } //Loop until dialog is closed

                }



                //var t =  box.ShowAsPopupAsync(this).ContinueWith(t => source.Cancel(), TaskScheduler.FromCurrentSynchronizationContext());
                //Dispatcher.UIThread.MainLoop(source.Token);

            }



            //ButtonResult result = ButtonResult.None;
            //var task = new Task(async () =>
            //{
            //    result = await MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(MBSparams).ShowDialog(owner);


            //    // Or with a class that inherits from window
            //    //result = await MessageBox.ShowAsync(this, "Hello world message", "Title", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            //    //textBlock.Text = result.ToString();

            //    //// Or with a custom window
            //    //var window = new Window();
            //    //result = await MessageBox.ShowAsync(window, "Hello world message", "Title", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            //    //textBlock.Text = result.ToString();
            //});
            //task.RunSynchronously();



            //Func<Task<ButtonResult>> www = new Func<Task<ButtonResult>>(() => MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(MBSparams).ShowDialog(owner));
            //ButtonResult result = Dispatcher.UIThread.InvokeAsync(www).Result;
            return result;
        }



        public static ButtonResult ShowMessageBoxNoAsync(string message, Window owner = null)
        {
            var MBSparams = new MessageBoxStandardParams
            {
                ButtonDefinitions = ButtonEnum.YesNo,
                ContentTitle = "Подтвердите действие", // в параметры
                ShowInCenter = true,
                ContentMessage = message,
                Icon = Icon.Error, // в параметры
                                   // Style = Style.Windows
            };

            return ShowBoxNoAsync(MBSparams, owner);

        }


        public static ButtonResult ShowMessageBoxNoAsync(MessageBoxStandardParams mbsParams, Window owner = null)
        {
            return ShowBoxNoAsync(mbsParams, owner);
        }


        public static async Task<ButtonResult> ShowErrorBox(string message, Window owner = null)
        {

            //var result = await MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow
            // (
            //     new MessageBox.Avalonia.DTO.MessageBoxStandardParams
            //     {
            //         ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.YesNo,
            //         ContentTitle = "Подтвердите действие",
            //         // ContentHeader = "header",
            //         ContentMessage = "Завершить работу программы?",
            //         Icon = MessageBox.Avalonia.Enums.Icon.Error,

            //         // WindowIcon = new WindowIcon("icon-rider.png")
            //     });



            var MBSparams = new MessageBoxStandardParams
            {
                ButtonDefinitions = ButtonEnum.YesNo,
                ContentTitle = "Подтвердите действие", // в параметры
                ShowInCenter = true,
                ContentMessage = message,
                Icon = MsBox.Avalonia.Enums.Icon.Error, // в параметры
                                                        // Style = Style.Windows
            };

            return await ShowBox(MBSparams, owner);
        }

    }
}

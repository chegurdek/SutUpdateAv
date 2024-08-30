using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SutUpdateAv.ViewModels
{
    public class LoginWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {

        string initialCatalog;
        string dataSource;
        string userID;
        string password;
        string provider;




        public event PropertyChangedEventHandler? PropertyChanged;

        // For convinience we add a method which will raise the above event.
        private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public string InitialCatalog
        {
            get
            {
                return initialCatalog;
            }
            set
            {
                // We only want to update the UI if the Name actually changed, so we check if the value is actually new
                if (initialCatalog != value)
                {
                    // 1. update our backing field
                    initialCatalog = value;

                    // 2. We call RaisePropertyChanged() to notify the UI about changes. 
                    // We can omit the property name here because [CallerMemberName] will provide it for us.  
                    RaisePropertyChanged();

                    // 3. Greeting also changed. So let's notify the UI about it. 
                   // RaisePropertyChanged(nameof(Greeting));
                }


               
            }

        }

        public string DataSource
        {
            get
            {
                return dataSource;
            }
            set
            {

          
                if (dataSource != value)
                {
                    dataSource = value;

                    RaisePropertyChanged();

                }

    
            }


        }

        public string UserID
        {
            get
            {
                return userID;
            }
            set
            {
                if (userID != value)
                {
                    userID = value;

                    RaisePropertyChanged();

                }

           
            }


        }


        public string Provider
        {
            get
            {
                return provider;
            }
            set
            {
                if (provider != value)
                {
                    provider = value;

                    RaisePropertyChanged();

                }

            }


        }
    }
}

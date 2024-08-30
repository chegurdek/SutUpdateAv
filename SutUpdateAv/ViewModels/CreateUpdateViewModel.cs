using SutUpdateAv.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace SutUpdateAv.ViewModels
{
    public class CreateUpdateViewModel :  ViewModelBase,   INotifyPropertyChanged
    {


        BusinessLayer bLayer = BusinessLayer.GetInstance();

        List<Item> poItems;
        public List<Item> PoItems
        {
            get
            {
                return poItems;
            }
            set
            {
                if (poItems != value)
                {
                    poItems = value;
                    //не обновляет без PropertyChanged 
                    RaisePropertyChanged("PoItems");
                }

            }
        }

        string selectedItem;

        public string SelectedItem 
        {
            // не работает 

            get => selectedItem;
            set
            {
                if (selectedItem != value) 
                {
                    selectedItem = value;
                    RaisePropertyChanged("SelectedItem");
                }
            }


        }


        public CreateUpdateViewModel()
        {


            try
            {
                //!!!!!!!
                poItems = bLayer.getSoftware();  // закоментировать для отображения дизайнера
                //!!!!!!!

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                // лог 

            }

        }



        public event PropertyChangedEventHandler? PropertyChanged;


        private void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

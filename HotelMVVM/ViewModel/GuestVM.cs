using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.UI.Xaml;
using HotelMVVM.Annotations;
using HotelMVVM.Common;
using HotelMVVM.Handler;
using HotelMVVM.Model;
using HotelMVVM.Persistency;
using ModelLibrary;

namespace HotelMVVM.ViewModel
{
    public class GuestVM : INotifyPropertyChanged
    {
        private GuestHandler _guestHandler;
        private Consumer<Guest> _consumerGuest;

        public Consumer<Guest> ConsumerGuest
        {
            get { return _consumerGuest; }
        }

        public CatalogSingleton<Guest> GuestCatalog { get; set; }

        public Visibility TableVisibility { get; set; }

        public Visibility OppositeTableVisibility
        {
            get { return TableVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible; }
        }

        private Guest _newGuest;
        public Guest NewGuest
        {
            get { return _newGuest; }
            set
            {
                _newGuest = value;
                OnPropertyChanged();
            }
        }

        public GuestVM()
        {
            _consumerGuest = ConsumerCatalog.GetConsumer<Guest>();
            _guestHandler = new GuestHandler(this);
            NewGuest = new Guest();
            _pressPostCommand = new RelayCommand(_guestHandler.PostNewGuest);
            _pressPutCommand = new RelayCommand(_guestHandler.PutNewGuest);
            _pressDeleteCommand = new RelayCommand(_guestHandler.DeleteGuest);
            _pressClearCommand = new RelayCommand(_guestHandler.Clear);

            TableVisibility = Visibility.Collapsed;
            GuestCatalog = CatalogSingleton<Guest>.Singleton;
            if (GuestCatalog.IsLoading)
            {
                TableVisibility = Visibility.Visible;
                GuestCatalog.Subscribe(() =>
                {
                    TableVisibility = Visibility.Collapsed;
                    OnPropertyChanged(nameof(TableVisibility));
                    OnPropertyChanged(nameof(OppositeTableVisibility));
                });
            }
        }

        
        private RelayCommand _pressPostCommand;

        public ICommand PressPostCommand
        {
            get { return _pressPostCommand; }
        }

        private RelayCommand _pressPutCommand;

        public ICommand PressPutCommand
        {
            get { return _pressPutCommand; }
        }

        private RelayCommand _pressDeleteCommand;

        public ICommand PressDeleteCommand
        {
            get { return _pressDeleteCommand; }
        }

        private RelayCommand _pressClearCommand;

        public ICommand PressClearCommand
        {
            get { return _pressClearCommand; }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

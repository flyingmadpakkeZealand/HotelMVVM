﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using HotelMVVM.Annotations;
using HotelMVVM.Common;
using HotelMVVM.Handler;
using HotelMVVM.Model;
using HotelMVVM.Persistency;
using ModelLibrary;

namespace HotelMVVM.ViewModel
{
    public class HotelVM : INotifyPropertyChanged
    {
        private HotelHandler _hotelHandler;
        private Consumer<Hotel> _consumerHotel;

        public Consumer<Hotel> ConsumerHotel
        {
            get { return _consumerHotel; }
        }

        public CatalogSingleton<Hotel> HotelCatalog { get; set; }

        public Visibility TableVisibility { get; set; }

        public Visibility OppositeTableVisibility
        {
            get { return TableVisibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible; }
        }

        public int SelectedIndex { get; set; } //Potential for auto selecting hotels depending on the typed HotelNo.

        public int RelayHotelNo
        {
            set
            {
                _newHotel.HotelNo = value;
                NotifyRelayHotelNo();
            }
            get { return _newHotel.HotelNo; }
        }

        private void NotifyRelayHotelNo()
        {
            OnPropertyChanged(nameof(RelayHotelNo));

            _hotelHandler.IdExistCompressQuery = true;
            _pressDeleteCommand.RaiseCanExecuteChanged();
            _pressPostCommand.RaiseCanExecuteChanged();
            _pressPutCommand.RaiseCanExecuteChanged();
            _hotelHandler.IdExistCompressQuery = false;
        }

        public Hotel SelectedHotel
        {
            set {
                if (value!=null)
                {
                    RelayHotelNo = value.HotelNo;
                    _newHotel.Name = value.Name;
                    _newHotel.Address = value.Address;
                    OnPropertyChanged(nameof(NewHotel));
                }
                else
                {
                    NewHotel = new Hotel();
                }
            }
        }

        private Hotel _newHotel;
        public Hotel NewHotel
        {
            get { return _newHotel; }
            set
            {
                _newHotel = value;
                OnPropertyChanged();
                NotifyRelayHotelNo();
            }
        }

        public HotelVM()
        {
            _consumerHotel = ConsumerCatalog.GetConsumer<Hotel>();
            _hotelHandler = new HotelHandler(this);
            _newHotel = new Hotel();
            _pressPostCommand = new RelayCommand(_hotelHandler.PostNewHotel, ()=> !_hotelHandler.IdExist());
            _pressPutCommand = new RelayCommand(_hotelHandler.PutNewHotel, _hotelHandler.IdExist);
            _pressDeleteCommand = new RelayCommand(_hotelHandler.DeleteHotel, _hotelHandler.IdExist);
            _pressClearCommand = new RelayCommand(_hotelHandler.Clear);

            TableVisibility = Visibility.Collapsed;
            HotelCatalog = CatalogSingleton<Hotel>.Singleton;
            if (HotelCatalog.IsLoading)
            {
                TableVisibility = Visibility.Visible;
                HotelCatalog.Subscribe(() =>
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

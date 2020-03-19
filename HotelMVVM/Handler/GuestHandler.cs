using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using HotelMVVM.Persistency;
using HotelMVVM.ViewModel;
using ModelLibrary;

namespace HotelMVVM.Handler
{
    public class GuestHandler
    {
        private GuestVM _guestVm;
        private Consumer<Guest> _consumer;

        public GuestHandler(GuestVM guestVm)
        {
            _guestVm = guestVm;
            _consumer = _guestVm.ConsumerGuest;
        }

        public async void PostNewGuest()
        {
            _guestVm.GuestCatalog.Add(_guestVm.NewGuest);
            bool ok = await _consumer.PostAsync(_guestVm.NewGuest);
        }

        public async void PutNewGuest()
        {
            for (int i = 0; i < _guestVm.GuestCatalog.Catalog.Count; i++)
            {
                if (_guestVm.GuestCatalog.Catalog[i].GuestNo == _guestVm.NewGuest.GuestNo)
                {
                    _guestVm.GuestCatalog.Delete(i);
                    _guestVm.GuestCatalog.Add(_guestVm.NewGuest);
                    bool ok = await _consumer.PutAsync(new[] { _guestVm.NewGuest.GuestNo }, _guestVm.NewGuest);
                    break;
                }
            }
        }

        public async void DeleteGuest()
        {
            for (int i = 0; i < _guestVm.GuestCatalog.Catalog.Count; i++)
            {
                if (_guestVm.GuestCatalog.Catalog[i].GuestNo == _guestVm.NewGuest.GuestNo)
                {
                    _guestVm.GuestCatalog.Delete(i);
                    bool ok = await _consumer.DeleteAsync(new[] { _guestVm.NewGuest.GuestNo });
                    return;
                }
            }
        }

        public void Clear()
        {
            _guestVm.NewGuest = new Guest();
        }
    }
}

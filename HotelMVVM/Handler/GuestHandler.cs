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

        private Guest CopyGuest(Guest originalGuest)
        {
            Guest copyGuest = new Guest(originalGuest.GuestNo, originalGuest.Name, originalGuest.Address);
            return copyGuest;
        }

        public async void PostNewGuest()
        {
            var query = from guest in _guestVm.GuestCatalog.Catalog
                where guest.GuestNo == _guestVm.NewGuest.GuestNo
                select guest;

            if (!query.Any())
            {
                Guest copyGuest = CopyGuest(_guestVm.NewGuest);
                _guestVm.GuestCatalog.Add(copyGuest);
                bool ok = await _consumer.PostAsync(copyGuest);
            }
        }

        public async void PutNewGuest()
        {
            for (int i = 0; i < _guestVm.GuestCatalog.Catalog.Count; i++)
            {
                if (_guestVm.GuestCatalog.Catalog[i].GuestNo == _guestVm.NewGuest.GuestNo)
                {
                    Guest copyGuest = CopyGuest(_guestVm.NewGuest);
                    _guestVm.GuestCatalog.Delete(i);
                    _guestVm.GuestCatalog.Add(copyGuest);
                    bool ok = await _consumer.PutAsync(new[] { copyGuest.GuestNo }, copyGuest);
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

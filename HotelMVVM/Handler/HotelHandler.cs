using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelMVVM.Persistency;
using HotelMVVM.ViewModel;
using ModelLibrary;

namespace HotelMVVM.Handler
{
    public class HotelHandler
    {
        private HotelVM _hotelVm;
        private Consumer<Hotel> _consumer;

        public HotelHandler(HotelVM hotelVm)
        {
            _hotelVm = hotelVm;
            _consumer = _hotelVm.ConsumerHotel;
        }

        public async void PostNewHotel()
        {
            _hotelVm.HotelCatalog.Add(_hotelVm.NewHotel);
            bool ok = await _consumer.PostAsync(_hotelVm.NewHotel);
        }

        public async void PutNewHotel()
        {
            for (int i = 0; i < _hotelVm.HotelCatalog.Catalog.Count; i++)
            {
                if (_hotelVm.HotelCatalog.Catalog[i].HotelNo == _hotelVm.NewHotel.HotelNo)
                {
                    _hotelVm.HotelCatalog.Delete(i);
                    _hotelVm.HotelCatalog.Add(_hotelVm.NewHotel);
                    bool ok = await _consumer.PutAsync(new[] {_hotelVm.NewHotel.HotelNo}, _hotelVm.NewHotel);
                    return;
                }
            }
        }

        public async void DeleteHotel()
        {
            for (int i = 0; i < _hotelVm.HotelCatalog.Catalog.Count; i++)
            {
                if (_hotelVm.HotelCatalog.Catalog[i].HotelNo == _hotelVm.NewHotel.HotelNo)
                {
                    _hotelVm.HotelCatalog.Delete(i);
                    bool ok = await _consumer.DeleteAsync(new[] {_hotelVm.NewHotel.HotelNo});
                    return;
                }
            }
        }

        public void Clear()
        {
            _hotelVm.NewHotel = new Hotel();
        }
    }
}

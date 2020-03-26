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

        private Hotel CopyHotel(Hotel originalHotel)
        {
            Hotel copyHotel = new Hotel(originalHotel.HotelNo, originalHotel.Name, originalHotel.Address);
            return copyHotel;
        }

        public async void PostNewHotel()
        {
            Hotel copyHotel = this.CopyHotel(_hotelVm.NewHotel);
            _hotelVm.HotelCatalog.Add(copyHotel);
            bool ok = await _consumer.PostAsync(copyHotel);
        }

        public async void PutNewHotel()
        {
            for (int i = 0; i < _hotelVm.HotelCatalog.Catalog.Count; i++)
            {
                if (_hotelVm.HotelCatalog.Catalog[i].HotelNo == _hotelVm.NewHotel.HotelNo)
                {
                    Hotel copyHotel = this.CopyHotel(_hotelVm.NewHotel);
                    _hotelVm.HotelCatalog.Delete(i);
                    _hotelVm.HotelCatalog.Add(copyHotel);
                    bool ok = await _consumer.PutAsync(new[] {copyHotel.HotelNo}, copyHotel);
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

        private bool? _queryResult;
        public bool IdExist()
        {
            if (_queryResult!=null)
            {
                return (bool)_queryResult;
            }

            var query = from hotel in _hotelVm.HotelCatalog.Catalog
                where hotel.HotelNo == _hotelVm.NewHotel.HotelNo
                select hotel;

            bool result = query.Any();

            if (_IdExistCompressQuery)
            {
                _queryResult = result;
            }

            return result;
        }

        private bool _IdExistCompressQuery;

        public bool IdExistCompressQuery
        {
            get { return _IdExistCompressQuery; }
            set
            {
                _IdExistCompressQuery = value;
                _queryResult = null;
            }
        }

    }
}

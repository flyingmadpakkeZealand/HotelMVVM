using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLibrary;

namespace HotelMVVM.Persistency
{
    public static class ConsumerCatalog
    {
		private static Dictionary<Type, string> _uriDictionary = new Dictionary<Type, string>()
        {
            {typeof(Guest), "http://localhost:56897/api/Guests"},
            {typeof(Hotel), "http://localhost:56897/api/Hotels"}
        };

        public static Consumer<T> GetConsumer<T>()
        {
            if (_uriDictionary.ContainsKey(typeof(T)))
            {
                Consumer<T> consumer = new Consumer<T>(_uriDictionary[typeof(T)]);
                return consumer;
            }

            return null;
        }
    }
}

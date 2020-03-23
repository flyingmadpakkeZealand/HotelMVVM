using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HotelMVVM.Persistency;
using HotelMVVM.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelLibrary;

namespace HotelMVVMTest
{
    [TestClass]
    public class HotelMVVMIOTest
    {
        [TestMethod]
        public async Task IOGetGuests_ResponseIsOk()
        {
            Consumer<Guest> consumer = ConsumerCatalog.GetConsumer<Guest>();

            List<Guest> guests = null;
            try
            {
                guests = await consumer.GetAsync();
            }
            catch (HttpRequestException re)
            {
                Assert.Fail(re.Message);
            }

            Assert.IsNotNull(guests);
        }
    }
}

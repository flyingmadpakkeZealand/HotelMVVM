using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ModelLibrary;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HotelMVVM.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GuestPage : Page
    {
        public GuestPage()
        {
            this.InitializeComponent();
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox guestNoBox = sender as TextBox; //Works as a prototype but should be implemented using trigger manage Nuget Package. Also searching for numbers should be optimized and it needs more wrong format handling.
            if (guestNoBox.Text != string.Empty)
            {
                int guestNoFromBox = Convert.ToInt32(guestNoBox.Text);
                var guests = GuestsList.Items;
                var query = from guest in guests.Cast<Guest>()
                    where guest.GuestNo == guestNoFromBox
                    select guest;

                if (query.Any())
                {
                    CreateButton.IsEnabled = false;
                }
                else
                {
                    CreateButton.IsEnabled = true;
                }
            }
        }
    }
}

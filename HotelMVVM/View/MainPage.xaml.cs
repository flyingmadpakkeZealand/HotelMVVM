using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HotelMVVM.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private bool RightSyncIsChecked
        {
            set
            {
                if (!_lock)
                {
                    _lock = true;
                    DropDownRight.IsEnabled = !value;
                    RightInvert.IsChecked = false;
                    _lock = false;
                }
            }
            get { return (bool) RightSync.IsChecked; }
        }

        private bool _lock = false;

        private bool RightInvertedIsChecked
        {
            set
            {
                if (!_lock)
                {
                    _lock = true;
                    DropDownRight.IsEnabled = !value;
                    RightSync.IsChecked = false;
                    _lock = false;
                }
            }
            get { return (bool) RightInvert.IsChecked; }
        }

        private void GuestPageLeft_OnClick(object sender, RoutedEventArgs e)
        {
            LeftFrame.Navigate(typeof(GuestPage));
            if (RightSyncIsChecked)
            {
                RightFrame.Navigate(typeof(GuestPage));
            }
            else if(RightInvertedIsChecked)
            {
                RightFrame.Navigate(typeof(HotelPage));
            }
        }

        private void HotelPageLeft_OnClick(object sender, RoutedEventArgs e)
        {
            LeftFrame.Navigate(typeof(HotelPage));
            if (RightSyncIsChecked)
            {
                RightFrame.Navigate(typeof(HotelPage));
            }
            else if (RightInvertedIsChecked)
            {
                RightFrame.Navigate(typeof(GuestPage));
            }
        }

        private void GuestPageRight_OnClick(object sender, RoutedEventArgs e)
        {
            RightFrame.Navigate(typeof(GuestPage));
        }

        private void HotelPageRight_OnClick(object sender, RoutedEventArgs e)
        {
            RightFrame.Navigate(typeof(HotelPage));
        }
    }
}

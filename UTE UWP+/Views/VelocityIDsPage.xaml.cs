using MicaForUWP.Media;
using UTE_UWP_.Helpers;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UTE_UWP_.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VelocityIDsPage : Page
    {
        public VelocityIDsPage()
        {
            this.InitializeComponent();

            if (BuildInfo.BeforeWin11)
            {
                if (App.Current.RequestedTheme == ApplicationTheme.Light)
                {
                    Application.Current.Resources["AppTitleBarBrush"] = new BackdropMicaBrush()
                    {
                        LuminosityOpacity = 0.8F,
                        TintOpacity = 0F,
                        BackgroundSource = BackgroundSource.WallpaperBackdrop,
                        Opacity = 1,
                        TintColor = Windows.UI.Color.FromArgb(255, 230, 230, 230),
                        FallbackColor = Windows.UI.Color.FromArgb(255, 230, 230, 230)
                    };
                    this.Background = (Brush)Application.Current.Resources["AppTitleBarBrush"];
                }
                else
                {
                    Application.Current.Resources["AppTitleBarBrush"] = new BackdropMicaBrush()
                    {
                        LuminosityOpacity = 0.8F,
                        TintOpacity = 0F,
                        BackgroundSource = BackgroundSource.WallpaperBackdrop,
                        Opacity = 1,
                        TintColor = Windows.UI.Color.FromArgb(255, 25, 25, 25),
                        FallbackColor = Windows.UI.Color.FromArgb(25, 25, 25, 25)
                    };
                    this.Background = (Brush)Application.Current.Resources["AppTitleBarBrush"];
                }

            }
            else
            {

            }

            var LocalSettings = ApplicationData.Current.LocalSettings;
            if (LocalSettings.Values["DialogsInRibbonVID"] != null)
            {
                if ((string)LocalSettings.Values["DialogsInRibbonVID"] == "On")
                {
                    dialogsonribbonvidToggle.IsOn = true;

                }
                if ((string)LocalSettings.Values["DialogsInRibbonVID"] == "Off")
                {
                    dialogsonribbonvidToggle.IsOn = false;
                }
            }
            else
            {
                LocalSettings.Values["DialogsInRibbonVID"] = "Off";
                dialogsonribbonvidToggle.IsOn = false;
            }
        
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Window.Current.Content is Frame rootFrame && rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private void dialogsonribbonvidToggle_Toggled(object sender, RoutedEventArgs e)
        {
            if (dialogsonribbonvidToggle.IsOn == true)
            {
                var LocalSettings = ApplicationData.Current.LocalSettings;
                if (LocalSettings.Values["DialogsInRibbonVID"] != null)
                {
                    LocalSettings.Values["DialogsInRibbonVID"] = "On";
                }
            }
            else
            {
                var LocalSettings = ApplicationData.Current.LocalSettings;
                if (LocalSettings.Values["DialogsInRibbonVID"] != null)
                {
                    LocalSettings.Values["DialogsInRibbonVID"] = "Off";
                }
            }
        }
    }
}

using MicaForUWP.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UTE_UWP_.Helpers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
            if (LocalSettings.Values["TabbedHideVID"] != null)
            {
                if ((string)LocalSettings.Values["TabbedHideVID"] == "On")
                {
                    tabbedhidevidToggle.IsOn = true;

                }
                if ((string)LocalSettings.Values["TabbedHideVID"] == "Off")
                {
                    tabbedhidevidToggle.IsOn = false;
                }
            }
            else
            {
                LocalSettings.Values["TabbedHideVID"] = "On";
                tabbedhidevidToggle.IsOn = false;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Window.Current.Content is Frame rootFrame && rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private void tabbedhidevidToggle_Toggled(object sender, RoutedEventArgs e)
        {
            if (tabbedhidevidToggle.IsOn == true)
            {
                var LocalSettings = ApplicationData.Current.LocalSettings;
                if (LocalSettings.Values["TabbedHideVID"] != null)
                {
                    LocalSettings.Values["TabbedHideVID"] = "On";
                }
            }
            else
            {
                var LocalSettings = ApplicationData.Current.LocalSettings;
                if (LocalSettings.Values["TabbedHideVID"] != null)
                {
                    LocalSettings.Values["TabbedHideVID"] = "Off";
                }
            }
        }
    }
}

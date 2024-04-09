﻿using MicaForUWP.Media;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using UltraTextEdit_UWP.Helpers;
using UltraTextEdit_UWP.Services;

using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UltraTextEdit_UWP.Views
{
    // TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/release/docs/UWP/pages/settings-codebehind.md
    // TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere
    public sealed partial class SettingsPage : Page, INotifyPropertyChanged
    {
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Set(ref _elementTheme, value); }
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
        }

        public SettingsPage()
        {
            InitializeComponent();
            if (BuildInfo.BeforeWin11)
            {
                Application.Current.Resources["AppTitleBarBrush"] = new BackdropMicaBrush()
                {
                    LuminosityOpacity = 0.8F,
                    TintOpacity = 0F,
                    BackgroundSource = BackgroundSource.WallpaperBackdrop,
                    Opacity = 1,
                    TintColor = Color.FromArgb(255, 230, 230, 230),
                    FallbackColor = Color.FromArgb(255, 230, 230, 230)
                };
                this.Background = (Brush)Application.Current.Resources["AppTitleBarBrush"];
            }
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (ElementSoundPlayer.State == ElementSoundPlayerState.On)
            {
                soundToggle.IsOn = true;
                localSettings.Values["sound"] = true;
            }
            else
            {
                soundToggle.IsOn = false;
                localSettings.Values["sound"] = false;
                localSettings.Values["spatialsound"] = false;
            }

            if (ElementSoundPlayer.SpatialAudioMode == ElementSpatialAudioMode.On)
            {
                spatialSoundBox.IsChecked = true;
                localSettings.Values["spatialsound"] = true;
            }

            if (Application.Current.FocusVisualKind == FocusVisualKind.HighVisibility)
            {
                HighVisibility.IsChecked = true;
            }
            else {
                RevealFocus.IsChecked = true;
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            VersionDescription = GetVersionDescription();
            await Task.CompletedTask;
        }

        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private async void ThemeChanged_CheckedAsync(object sender, RoutedEventArgs e)
        {
            var param = (sender as RadioButton)?.CommandParameter;

            if (param != null)
            {
                await ThemeSelectorService.SetThemeAsync((ElementTheme)param);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnBackRequested(object sender,  RoutedEventArgs e)
        {
            if (Window.Current.Content is Frame rootFrame && rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }
        private void spatialAudioBox_Checked(object sender, RoutedEventArgs e)
        {
            if (soundToggle.IsOn == true)
            {
                ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.On;
            }
        }

        private void soundToggle_Toggled(object sender, RoutedEventArgs e)
        {
            if (soundToggle.IsOn == true)
            {
                spatialSoundBox.IsEnabled = true;
                ElementSoundPlayer.State = ElementSoundPlayerState.On;
            }
            else
            {
                spatialSoundBox.IsEnabled = false;
                spatialSoundBox.IsChecked = false;

                ElementSoundPlayer.State = ElementSoundPlayerState.Off;
                ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.Off;
            }
        }
        private void spatialAudioBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (soundToggle.IsOn == true)
            {
                ElementSoundPlayer.SpatialAudioMode = ElementSpatialAudioMode.Off;
            }
        }
        private void HighVisibility_Checked(object sender, RoutedEventArgs e)
        {
            Application.Current.FocusVisualKind = FocusVisualKind.HighVisibility;
        }

        private void RevealFocus_Checked(object sender, RoutedEventArgs e)
        {
            Application.Current.FocusVisualKind = FocusVisualKind.Reveal;
        }
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void UTEUpdateNav(object sender, RoutedEventArgs e)
        {
            if (Window.Current.Content is Frame rootFrame)
            {
                rootFrame.Navigate(typeof(UTEUpdate));
            }
        }

        private void CopyVerInfo(object sender, RoutedEventArgs e)
        {
            var data = new DataPackage
            {
                RequestedOperation = DataPackageOperation.Copy
            };
            data.SetText(aboutblock.Header + " version " + aboutblock.Description);

            Clipboard.SetContentWithOptions(data, new ClipboardContentOptions() { IsAllowedInHistory = true, IsRoamable = true });
            Clipboard.Flush();
        }
    }
}

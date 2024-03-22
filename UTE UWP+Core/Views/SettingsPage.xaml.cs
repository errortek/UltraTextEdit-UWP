using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using UTE_UWP_.Helpers;
using UTE_UWP_.Services;
using UTE_UWP_.Views;

using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

namespace UTE_UWP_.Views
{
    // TODO: Add other settings as necessary. For help see https://github.com/microsoft/TemplateStudio/blob/main/docs/UWP/pages/settings-codebehind.md
    // TODO: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere
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
        }

        

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.Window.Content is Frame rootFrame && rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
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

        public List<string> Fonts;

        #region Appearance
        public int DocumentViewPadding;

        public string DefaultFont;

        // Modes:
        // 0. No wrap
        // 1. Wrap
        // 2. Wrap whole words

        public int TextWrapping;

        // Modes:
        // 0. Light
        // 1. Dark
        // 2. Default

        public int Theme;
        #endregion

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private async void GH_Navigate(object sender, RoutedEventArgs e)
        {
            // The URI to launch
            string uriToLaunch = @"https://github.com/jpbandroid/UTE-UWP-Plus";

            // Create a Uri object from a URI string 
            var uri = new Uri(uriToLaunch);

            // Launch the URI
            async void DefaultLaunch()
            {
                // Launch the URI
                var success = await Windows.System.Launcher.LaunchUriAsync(uri);

                if (success)
                {
                    // URI launched
                }
                else
                {
                    // URI launch failed
                }
            }
            DefaultLaunch();
        }
    }

}

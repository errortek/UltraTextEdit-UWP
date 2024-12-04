using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class UTEUpdate : Page
    {
        public UTEUpdate()
        {
            this.InitializeComponent();
        }

        private async void CheckForUpdates(object sender, RoutedEventArgs e)
        {
            updatecheckProgress.Visibility = Visibility.Visible;
            checkforupdateText.Text = "Checking for updates...";
            WebClient client = new WebClient();
            Stream stream = client.OpenRead("");
            StreamReader reader = new StreamReader(stream);
            var newVersion = new Version(await reader.ReadToEndAsync());
            Package package = Package.Current;
            PackageVersion packageVersion = package.Id.Version;
            var currentVersion = new Version(string.Format("{0}.{1}.{2}.{3}", packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision));

            //compare package versions
            if (newVersion.CompareTo(currentVersion) > 0)
            {
                checkforupdateText.Text = "An update is available";
            }
            else
            {
                checkforupdateText.Text = "No updates available";
            }
        }
    }
}

using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UTE_UWP_.Views
{
    public sealed partial class WhatsNewDialog : ContentDialog
    {

        private string changelog;

        public WhatsNewDialog()
        {
            // TODO: Update the contents of this dialog every time you release a new version of the app
            RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;
            InitializeComponent();
            changelog = File.ReadAllText("Assets/Changelogs/Latest.md");
        }
    }
}

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UTE_UWP_.Views
{
    public sealed partial class FirstRunDialog : ContentDialog
    {
        public FirstRunDialog()
        {
            RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;
            InitializeComponent();
        }
    }
}

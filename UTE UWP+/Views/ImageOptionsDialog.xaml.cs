using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UTE_UWP_.Views
{
    public sealed partial class ImageOptionsDialog : ContentDialog
    {
        public double DefaultWidth { get; set; }
        public double DefaultHeight { get; set; }
        public string Tag { get; private set; }

        public ImageOptionsDialog()
        {
            InitializeComponent();

            Loaded += ImageOptionsDialog_Loaded;
        }

        private void ImageOptionsDialog_Loaded(object sender, RoutedEventArgs e)
        {
            WidthBox.Value = DefaultWidth;
            HeightBox.Value = DefaultHeight;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            DefaultWidth = WidthBox.Value;
            DefaultHeight = HeightBox.Value;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}

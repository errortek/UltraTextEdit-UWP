using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

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
using Microsoft.Toolkit;
using Microsoft.Toolkit.Uwp.UI.Controls.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition.Interactions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UltraTextEdit_UWP.Dialogs
{
    public sealed partial class TableDialog : ContentDialog

    {
        public TableDialog()
        {
            this.InitializeComponent();
        }
            public int rows;
            public int columns;
    

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            variable_extraction();
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        public void variable_extraction()
        {
                rows = Int32.Parse(rowBox.Text);
                columns = Int32.Parse(columnBox.Text);
        }
    }
}

using System;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UTE_UWP_
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

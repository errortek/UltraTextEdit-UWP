using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace UltraTextEdit_UWP.Views
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        public MainPage()
        {
            InitializeComponent();
            string[] fonts = {
                "Arial",
                "Blackadder ITC",
                "Calibri",
                "Century Gothic",
                "Comic Sans MS",
                "Segoe UI",
                "Sitka Display",
                "Trebuchet MS",
                "Verdana"
            };
            fontbox.ItemsSource = fonts;
        }
        public List<double> FontSizes { get; } = new List<double>()
            {
                8,
                9,
                10,
                11,
                12,
                14,
                16,
                18,
                20,
                24,
                28,
                36,
                48,
                72 };

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

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void FindBoxHighlightMatches()
        {
            FindBoxRemoveHighlights();

            Color highlightBackgroundColor = (Color)App.Current.Resources["SystemColorHighlightColor"];
            Color highlightForegroundColor = (Color)App.Current.Resources["SystemColorHighlightTextColor"];

            string textToFind = Find.Text;
            if (textToFind != null)
            {
                ITextRange searchRange = box.Document.GetRange(0, 0);
                while (searchRange.FindText(textToFind, TextConstants.MaxUnitCount, FindOptions.None) > 0)
                {
                    searchRange.CharacterFormat.BackgroundColor = highlightBackgroundColor;
                    searchRange.CharacterFormat.ForegroundColor = highlightForegroundColor;
                }
            }
        }
        private void Combo3_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Combo3.SelectedIndex = 2;

            if ((ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7)))
            {
                Combo3.TextSubmitted += Combo3_TextSubmitted;
            }
        }

        private void Combo3_TextSubmitted(ComboBox sender, ComboBoxTextSubmittedEventArgs args)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                bool isDouble = double.TryParse(sender.Text, out double newValue);

                // Set the selected item if:
                // - The value successfully parsed to double AND
                // - The value is in the list of sizes OR is a custom value between 8 and 100
                if (isDouble && (FontSizes.Contains(newValue) || (newValue < 100 && newValue > 8)))
                {
                    // Update the SelectedItem to the new value. 
                    sender.SelectedItem = newValue;
                    box.Document.Selection.CharacterFormat.Size = (float)newValue;
                }
                else
                {
                    // If the item is invalid, reject it and revert the text. 
                    sender.Text = sender.SelectedValue.ToString();

                    var dialog = new ContentDialog
                    {
                        Content = "The font size must be a number between 8 and 100.",
                        CloseButtonText = "Close",
                        DefaultButton = ContentDialogButton.Close
                    };
                    var task = dialog.ShowAsync();
                }
            }

            // Mark the event as handled so the framework doesn’t update the selected item automatically. 
            args.Handled = true;
        }
        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Bold = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void ItalicButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Italic = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void UnderlineButton_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                if (charFormatting.Underline == Windows.UI.Text.UnderlineType.None)
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.Single;
                }
                else
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.None;
                }
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void FindBoxRemoveHighlights()
        {
            ITextRange documentRange = box.Document.GetRange(0, TextConstants.MaxUnitCount);
            SolidColorBrush defaultBackground = box.Background as SolidColorBrush;
            SolidColorBrush defaultForeground = box.Foreground as SolidColorBrush;

            documentRange.CharacterFormat.BackgroundColor = defaultBackground.Color;
            documentRange.CharacterFormat.ForegroundColor = defaultForeground.Color;
        }
        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            // Extract the color of the button that was clicked.
            Button clickedColor = (Button)sender;
            var rectangle = (Windows.UI.Xaml.Shapes.Rectangle)clickedColor.Content;
            var color = ((Windows.UI.Xaml.Media.SolidColorBrush)rectangle.Fill).Color;

            box.Document.Selection.CharacterFormat.ForegroundColor = color;

            fontColorButton.Flyout.Hide();
            box.Focus(Windows.UI.Xaml.FocusState.Keyboard);
        }
        private void ListStyleButton_IsCheckedChanged(Microsoft.UI.Xaml.Controls.ToggleSplitButton sender, Microsoft.UI.Xaml.Controls.ToggleSplitButtonIsCheckedChangedEventArgs args)
        {
            // Use the toggle button to turn the selected list style on or off.
            if (sender.IsChecked == true)
            {
                // On. Apply the list style selected in the drop down to the selected text.
                var listStyle = ((FrameworkElement)ListStylesListView.SelectedItem).Tag.ToString();
                ApplyListStyle(listStyle);
            }
            else
            {
                // Off. Make the selected text not a list,
                // but don't change the list style selected in the drop down.
                ApplyListStyle("none");
            }
        }

        private void ListStylesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listStyle = ((FrameworkElement)(e.AddedItems[0])).Tag.ToString();

            if (ListButton.IsChecked == true)
            {
                // Toggle button is on. Turn it off...
                if (listStyle == "none")
                {
                    ListButton.IsChecked = false;
                }
                else
                {
                    // or apply the new selection.
                    ApplyListStyle(listStyle);
                }
            }
            else
            {
                // Toggle button is off. Turn it on, which will apply the selection
                // in the IsCheckedChanged event handler.
                ListButton.IsChecked = true;
            }
        }

        private void ApplyListStyle(string listStyle)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                // Apply the list style to the selected text.
                var paragraphFormatting = selectedText.ParagraphFormat;
                if (listStyle == "none")
                {
                    paragraphFormatting.ListType = Windows.UI.Text.MarkerType.None;
                }
                else if (listStyle == "bullet")
                {
                    paragraphFormatting.ListType = Windows.UI.Text.MarkerType.Bullet;
                }
                else if (listStyle == "numeric")
                {
                    paragraphFormatting.ListType = Windows.UI.Text.MarkerType.Arabic;
                }
                else if (listStyle == "alpha")
                {
                    paragraphFormatting.ListType = Windows.UI.Text.MarkerType.UppercaseEnglishLetter;
                }
                selectedText.ParagraphFormat = paragraphFormatting;
            }
        }


        

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }
        private void ComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                // Get the instance of ComboBox
                ComboBox fontbox = sender as ComboBox;

                // Get the ComboBox selected item text
                string selectedItems = fontbox.SelectedItem.ToString();
                box.Document.Selection.CharacterFormat.Name = selectedItems;
            }
        }
        private void ListButton2_IsCheckedChanged(Microsoft.UI.Xaml.Controls.ToggleSplitButton sender, Microsoft.UI.Xaml.Controls.ToggleSplitButtonIsCheckedChangedEventArgs args)
        {
            // Use the toggle button to turn the selected list style on or off.
            if ((sender).IsChecked == true)
            {
                // On. Apply the list style selected in the drop down to the selected text.
                var listStyle2 = ((FrameworkElement)ListStylesListView2.SelectedItem).Tag.ToString();
                ApplyListStyle(listStyle2);
            }
            else
            {
                // Off. Make the selected text not a list,
                // but don't change the list style selected in the drop down.
                ApplyListStyle("none");
            }

        }


        private void ListStylesListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var listStyle2 = ((FrameworkElement)e.AddedItems[0]).Tag.ToString();

            if (ListButton2.IsChecked == true)
            {
                // Toggle button is on. Turn it off...
                if (listStyle2 == "none")
                {
                    ListButton2.IsChecked = false;
                }
                else
                {
                    // or apply the new selection.
                    ApplyListStyle2(listStyle2);
                }
            }
            else
            {
                // Toggle button is off. Turn it on, which will apply the selection
                // in the IsCheckedChanged event handler.
                ListButton2.IsChecked = true;
            }
        }
        private void ApplyListStyle2(string listStyle2)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                // Apply the list style to the selected text.
                var paragraphFormatting = selectedText.ParagraphFormat;
                if (listStyle2 == "left")
                {
                    paragraphFormatting.Alignment = Windows.UI.Text.ParagraphAlignment.Left;
                }
                else if (listStyle2 == "right")
                {
                    paragraphFormatting.Alignment = Windows.UI.Text.ParagraphAlignment.Right;
                }
                else if (listStyle2 == "center")
                {
                    paragraphFormatting.Alignment = Windows.UI.Text.ParagraphAlignment.Center;
                }
                selectedText.ParagraphFormat = paragraphFormatting;
            }
        }

        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            var dialog = new WhatsNewDialog();
            await dialog.ShowAsync();
        }
        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            // Open a text file.
            Windows.Storage.Pickers.FileOpenPicker open =
                new Windows.Storage.Pickers.FileOpenPicker();
            open.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            open.FileTypeFilter.Add(".rtf");

            Windows.Storage.StorageFile file = await open.PickSingleFileAsync();

            if (file != null)
            {
                using (Windows.Storage.Streams.IRandomAccessStream randAccStream =
                    await file.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    // Load the file into the Document property of the RichEditBox.
                    box.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.FormatRtf, randAccStream);
                }
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Rich Text", new List<string>() { ".rtf" });
            savePicker.FileTypeChoices.Add("Office Open XML Document", new List<string>() { ".docx" });
            savePicker.FileTypeChoices.Add("Plain Text File", new List<string>() { ".txt" });

            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "New Document";

            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until we
                // finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);
                // write to file
                using (Windows.Storage.Streams.IRandomAccessStream randAccStream =
                    await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
                {
                    box.Document.SaveToStream(Windows.UI.Text.TextGetOptions.FormatRtf, randAccStream);
                }

                // Let Windows know that we're finished changing the file so the
                // other app can update the remote version of the file.
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status != FileUpdateStatus.Complete)
                {
                    Windows.UI.Popups.MessageDialog errorBox =
                        new Windows.UI.Popups.MessageDialog("File " + file.Name + " couldn't be saved.");
                    await errorBox.ShowAsync();
                }
            }
        }

        private void BoldButton_Off(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextCharacterFormat selectedText = box.Document.Selection.CharacterFormat;
            if (selectedText != null)
            {
                selectedText.FontStyle = FontStyle.Normal;
            }
        }
        private void ItalicButton_Off(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextCharacterFormat selectedText = box.Document.Selection.CharacterFormat;
            if (selectedText != null)
            {
                selectedText.FontStyle = FontStyle.Normal;
            }
        }
        private void UnderlineButton_Off(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextCharacterFormat selectedText = box.Document.Selection.CharacterFormat;
            if (selectedText != null)
            {
                selectedText.FontStyle = FontStyle.Normal;
            }
        }

        private void SuperscriptCheck(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Superscript = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }

        private void SubscriptCheck(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = box.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Subscript = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }
    }
}

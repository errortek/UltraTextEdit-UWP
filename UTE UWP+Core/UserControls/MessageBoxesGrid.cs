using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.UI;
using UltraTextEdit_UWP.UserControls;

namespace UltraTextEdit_UWP.UserControls
{
    public sealed class MessageBoxesGrid : Grid
    {
        public MessageBoxesGrid()
        {
            SetBlockBorder();
        }

        public void SetBlockBorder()
        {
            PointerPressed -= BlockBorder_PointerPressed;
            PointerPressed += BlockBorder_PointerPressed;

            bool AnyElementOn = false;

            SetValue(MessageBoxValidation.IsMessageBoxProperty, true);
            foreach (MessageBox box in Children.Cast<MessageBox>())
            {
                if (box.IsOn == true && box.BlockUserInput == true)
                {
                    AnyElementOn = true;
                    break;
                }
            }

            if (AnyElementOn == true)
            {
                Background = new SolidColorBrush(Color.FromArgb(89, 0, 0, 0));
            }
            else
            {
                Background = null;
            }
        }

        private void BlockBorder_PointerPressed(object sender, PointerRoutedEventArgs e)
        {

        }

        public static readonly DependencyProperty IsMessageBoxProperty =
            DependencyProperty.RegisterAttached("IsMessageBox", typeof(bool), typeof(MessageBoxesGrid), new PropertyMetadata(true, OnIsMessageBoxChanged));

        public static bool GetIsMessageBox(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMessageBoxProperty);
        }

        public static void SetIsMessageBox(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMessageBoxProperty, value);
        }

        private static void OnIsMessageBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MessageBoxesGrid grid && !(bool)e.NewValue)
            {
                throw new ArgumentException("The IsMessageBox property of CustomMessageBoxGrid cannot be set to false.");
            }
        }
    }
}

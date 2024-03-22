using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace UltraTextEdit_UWP.UserControls
{
    public static class MessageBoxValidation
    {
        public static readonly DependencyProperty IsMessageBoxProperty =
            DependencyProperty.RegisterAttached("IsMessageBox", typeof(bool), typeof(MessageBoxValidation), new PropertyMetadata(false, OnIsMessageBoxChanged));

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
            if (d is Grid grid && (bool)e.NewValue)
            {
                grid.Loaded += Grid_Loaded;
            }
        }

        private static void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Grid grid)
            {
                foreach (UIElement child in grid.Children)
                {
                    if (!(child is MessageBox))
                    {
                        throw new ArgumentException("Only MessageBox elements are allowed as content in the grid.");
                    }
                }
            }
        }
    }
}

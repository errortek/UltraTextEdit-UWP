using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace UTE_UWP_.Helpers
{
    public class SystemAccentColorSetting : INotifyPropertyChanged
    {
        private SolidColorBrush systemAccentColor = new SolidColorBrush(Colors.Red);
        public SolidColorBrush SystemAccentColor
        {
            get
            {
                return systemAccentColor;
            }
            set
            {
                systemAccentColor = value; OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

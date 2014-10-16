using System;
using System.Net;
using System.Windows;
using System.ComponentModel;

namespace TaskyWin8 {
    public class ViewModelBase : INotifyPropertyChanged {

#region IPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
#endregion
    }
}

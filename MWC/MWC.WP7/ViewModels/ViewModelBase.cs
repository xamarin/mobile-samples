using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace MWC.WP7.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged (string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler (this, new PropertyChangedEventArgs (propertyName));
            }
        }

        protected string CleanupPlainTextDocument (string doc)
        {
            if (string.IsNullOrWhiteSpace (doc)) {
                return "";
            }

            //
            // We want to make sure that there are no more than 1
            // blank continuous lines
            //
            var oldLines = doc.Split ('\n');
            var newLines = new List<string> ();

            var prevWasBlank = true;
            foreach (var line in oldLines) {
                var blank = string.IsNullOrWhiteSpace (line);

                if (!blank || !prevWasBlank) {
                    newLines.Add (line.TrimEnd ());
                }

                prevWasBlank = blank;
            }

            return string.Join ("\r\n", newLines);
        }
    }
}

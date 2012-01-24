using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MWC.WP7.ViewModels
{
    public class NewsItemViewModel : ViewModelBase
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public DateTime Published { get; set; }
        public string Content { get; set; }
    }
}

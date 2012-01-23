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
using MWC.BL;

namespace MWC.WP7.ViewModels
{
    public class SpeakerDetailsViewModel : ViewModelBase
    {
        public int ID { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string ImageUrl { get; set; }
        public string Bio { get; set; }

        public void Update (Speaker speaker)
        {
            ID = speaker.ID;
            Key = speaker.Key;
            Name = speaker.Name;
            Title = speaker.Title;
            Company = speaker.Company;
            ImageUrl = speaker.ImageUrl;
            Bio = speaker.Bio;
        }
    }
}

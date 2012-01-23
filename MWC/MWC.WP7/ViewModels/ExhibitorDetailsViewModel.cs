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
    public class ExhibitorDetailsViewModel : ViewModelBase
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Locations { get; set; }
        public bool IsFeatured { get; set; }
        public string Overview { get; set; }
        public string Tags { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string ImageUrl { get; set; }

        public void Update (Exhibitor exhibitor)
        {
            ID = exhibitor.ID;
            Name = exhibitor.Name;
            City = exhibitor.City;
            Country = exhibitor.Country;
            Locations = exhibitor.Locations;
            IsFeatured = exhibitor.IsFeatured;
            Overview = exhibitor.Overview;
            Tags = exhibitor.Tags;
            Email = exhibitor.Email;
            Address = exhibitor.Address;
            Phone = exhibitor.Phone;
            Fax = exhibitor.Fax;
            ImageUrl = exhibitor.ImageUrl;

            if (string.IsNullOrWhiteSpace (Overview)) {
                Overview = "No background information available.";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using MWC;
using MWC.BL;
using Android.Util;

namespace MWC.Adapters {
    public class ExhibitorListAdapter : BaseAdapter<Exhibitor>, ISectionIndexer {
        protected Activity context = null;
        protected IList<Exhibitor> exhibitors = new List<Exhibitor>();

        string[] sections;
        Java.Lang.Object[] sectionsO;
        Dictionary<string, int> alphaIndexer;

        public ExhibitorListAdapter(Activity context, IList<Exhibitor> exhibitors)
            : base() {
            this.context = context;
            this.exhibitors = exhibitors;

            alphaIndexer = new Dictionary<string, int>();

            for (int i = 0; i < exhibitors.Count; i++) {
                var key = exhibitors[i].Index;
                if (alphaIndexer.ContainsKey(key)) {
                    alphaIndexer[key] = i;
                }
                else
                    alphaIndexer.Add(key, i);
            }
            sections = new string[alphaIndexer.Keys.Count];
            alphaIndexer.Keys.CopyTo(sections, 0);
            sectionsO = new Java.Lang.Object[sections.Length];
            for (int i = 0; i < sections.Length; i++) {
                sectionsO[i] = new Java.Lang.String(sections[i]);
            }
        }

        public override Exhibitor this[int position]
        {
            get { return exhibitors[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return exhibitors.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            // Get our object for this position
            var item = this.exhibitors[position];

            //Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
            // This gives us some performance gains by not always inflating a new view
            // This will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
            var view = (convertView ??
                    this.context.LayoutInflater.Inflate(
                    Resource.Layout.ExhibitorListItem,
                    parent,
                    false)) as LinearLayout;

            // Find references to each subview in the list item's view
            var nameTextView = view.FindViewById<TextView>(Resource.Id.NameTextView);
            var countryTextView = view.FindViewById<TextView>(Resource.Id.CountryTextView);
            var locationTextView = view.FindViewById<TextView>(Resource.Id.LocationTextView);
            var imageview = view.FindViewById<ImageView>(Resource.Id.ExhibitorImageView);

            //Assign this item's values to the various subviews
            nameTextView.SetText(this.exhibitors[position].Name, TextView.BufferType.Normal);
            countryTextView.SetText(this.exhibitors[position].City + ", " + this.exhibitors[position].Country, TextView.BufferType.Normal);
            locationTextView.SetText(this.exhibitors[position].Locations, TextView.BufferType.Normal);

            var uri = new Uri(this.exhibitors[position].ImageUrl);
            var iw = new AL.ImageWrapper(imageview, context);
            imageview.Tag = uri.ToString();

            try {
                var drawable = MonoTouch.Dialog.Utilities.ImageLoader.DefaultRequestImage(uri, iw);
                if (drawable != null)
                    imageview.SetImageDrawable(drawable);
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }

            //Finally return the view
            return view;
        }

        public int GetPositionForSection(int section)
        {
            return alphaIndexer[sections[section]];
        }

        public int GetSectionForPosition(int position)
        {
            return 1;
        }

        public Java.Lang.Object[] GetSections()
        {
            return sectionsO;
        }
    }
}


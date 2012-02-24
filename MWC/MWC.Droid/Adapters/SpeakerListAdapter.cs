using System;
using System.Collections.Generic;
using Android.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using MWC.BL;


namespace MWC.Adapters {
    public class SpeakerListAdapter : BaseAdapter<Speaker>, ISectionIndexer {
        protected Activity context = null;
        protected IList<Speaker> speakers = new List<Speaker>();

        string[] sections;
        Java.Lang.Object[] sectionsO;
        Dictionary<string, int> alphaIndexer;

        public SpeakerListAdapter(Activity context, IList<Speaker> speakers)
            : base()
        {
            this.context = context;
            this.speakers = speakers;


            alphaIndexer = new Dictionary<string, int>();

            for (int i = 0; i < speakers.Count; i++) {
                var key = speakers[i].Index;
                if (alphaIndexer.ContainsKey(key)) {
                    //alphaIndexer[key] = i;
               } else
                    alphaIndexer.Add(key, i);
            }
            sections = new string[alphaIndexer.Keys.Count];
            alphaIndexer.Keys.CopyTo(sections, 0);
            sectionsO = new Java.Lang.Object[sections.Length];
            for (int i = 0; i < sections.Length; i++) {
                sectionsO[i] = new Java.Lang.String(sections[i]);
            }
        }

        public override Speaker this[int position]
        {
            get { return speakers[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return speakers.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            // Get our object for this position
            var item = speakers[position];

            //Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
            // This gives us some performance gains by not always inflating a new view
            // This will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
            var view = (convertView ??
                    context.LayoutInflater.Inflate(
                    Resource.Layout.SpeakerListItem,
                    parent,
                    false)) as LinearLayout;

            // Find references to each subview in the list item's view
            var bigTextView = view.FindViewById<TextView>(Resource.Id.NameTextView);
            var smallTextView = view.FindViewById<TextView>(Resource.Id.CompanyTextView);
            var imageview = view.FindViewById<ImageView>(Resource.Id.SpeakerImageView);

            //Assign this item's values to the various subviews
            bigTextView.SetText(speakers[position].Name, TextView.BufferType.Normal);
            smallTextView.SetText(speakers[position].Title+", "+this.speakers[position].Company, TextView.BufferType.Normal);

            
            var uri = new Uri(speakers[position].ImageUrl);
            imageview.Tag = uri.ToString();
            var iw = new AL.ImageWrapper(imageview, context);
            
            try {
                var drawable = MonoTouch.Dialog.Utilities.ImageLoader.DefaultRequestImage(uri, iw);
                if (drawable == null)
                    imageview.SetImageResource(Resource.Drawable.Icon);
                else
                    imageview.SetImageDrawable(drawable);
            } catch (Exception ex) {
                MonoTouch.Dialog.Utilities.ImageLoader.Purge(); // have seen outofmemory here
                MWCApp.LogDebug("SPEAKERS " + ex.ToString());
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
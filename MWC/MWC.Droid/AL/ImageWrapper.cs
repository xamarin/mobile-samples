using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MWC.AL {
    /// <summary>
    /// Bit of a hack, need a way to reference ImageViews inside ListViews, so they can be 
    /// updated with async-loaded images. Could have created a custom image view class, but
    /// this was quicker.
    /// </summary>
    public class ImageWrapper : MonoTouch.Dialog.Utilities.IImageUpdated {
        ImageView imageView;
        Activity context;

        public ImageWrapper(ImageView imageView,  Activity context)
        {
            this.imageView = imageView;
            this.context = context;
        }
        //TODO: remove; this is just here for logging
        static string ImageName(string uri)
        {
            var start = uri.LastIndexOf("/") + 1;
            var end = uri.LastIndexOf(".");
            return uri.Substring(start, end - start);
        }
        public void UpdatedImage(Uri uri)
        {
            //// this is how the iOS version behaves, but on Android it seems to result in 'wrong images'
            //context.RunOnUiThread(() => {
            //    var drawable = MonoTouch.Dialog.Utilities.ImageLoader.DefaultRequestImage(uri, null);
            //    if (drawable == null) {
            //        //TODO: this shouldn't happen, if the UpdatedImage callback comes, it should ALWAYS be there
            //        MonoTouch.Dialog.Utilities.ImageLoader.LogDebug("            ???? " + ImageName(uri.ToString()));
            //    } else {
            //        imageView.SetImageDrawable(drawable);
            //    }
            //});
            //return;

            MonoTouch.Dialog.Utilities.ImageLoader.LogDebug("            updt " + ImageName(uri.ToString()));
            // imageViews are re-used (because the View/cells they live in are re-used),
            // so just check the 'current' tag for the imageView matches the Url we are 
            // updating it to, if not then the image has been cached but is not required right now.
            if (imageView.Tag.ToString() == uri.ToString()) {
                context.RunOnUiThread(() => {
                    var drawable = MonoTouch.Dialog.Utilities.ImageLoader.DefaultRequestImage(uri, null); 
                    if (drawable == null) {
                        //TODO: this shouldn't happen, if the UpdatedImage callback comes, it should ALWAYS be there
                        MonoTouch.Dialog.Utilities.ImageLoader.LogDebug("            xxxx " + ImageName(uri.ToString()));
                    } else {
                        imageView.SetImageDrawable(drawable);
                    }
                });
            } else {
                MonoTouch.Dialog.Utilities.ImageLoader.LogDebug(
                    String.Format("            miss w{0}, g{1}"   // wanted, but got
                    , ImageName(imageView.Tag.ToString())
                    , ImageName(uri.ToString())
                    )
                );
                // hit-and-hope, maybe the image we really want is already in the cache/filesystem
                var uri1 = new Uri(imageView.Tag.ToString());
                context.RunOnUiThread(() => {
                    var drawable = MonoTouch.Dialog.Utilities.ImageLoader.DefaultRequestImage(uri1, this);
                    if (drawable != null)
                        imageView.SetImageDrawable(drawable);
                });
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using XamarinFormsScreenShot.Droid;
using Android.Graphics;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(ScreenHelper))]
namespace XamarinFormsScreenShot.Droid
{
    public class ScreenHelper : IScreen
    {
        public async Task<byte[]> CaptureScreenAsync()
        {
            var activity = Xamarin.Forms.Forms.Context as MainActivity;
            if (activity == null)
            {
                return null;
            }
            var view = activity.Window.DecorView;
            view.DrawingCacheEnabled = true;
            Bitmap bitmap = view.GetDrawingCache(true);
            byte[] bitmapData;

            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                bitmapData = stream.ToArray();
            }

            return bitmapData;
        }
    }
}
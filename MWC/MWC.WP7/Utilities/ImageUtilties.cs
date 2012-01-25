using System;
using System.IO.IsolatedStorage;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MWC.WP7.Utilities
{
    public static class ImageUtilties
    {
        public static Uri SaveAsTile (this Image image, string key)
        {
            WriteableBitmap bmp = new WriteableBitmap ((BitmapSource)image.Source);

            var iso = IsolatedStorageFile.GetUserStoreForApplication ();
            if (!iso.DirectoryExists ("Shared")) {
                iso.CreateDirectory ("Shared");
            }

            var dir = "Shared\\ShellContent";
            if (!iso.DirectoryExists (dir)) {
                iso.CreateDirectory (dir);
            }

            var path = dir + "\\" + key + ".jpg";

            using (var stream = iso.CreateFile (path)) {
                bmp.SaveJpeg (stream, 173, 173, 0, 100);
            }

            return new Uri ("isostore:/Shared/ShellContent/" + key + ".jpg", UriKind.RelativeOrAbsolute);
        }
    }
}

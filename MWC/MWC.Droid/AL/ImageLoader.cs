// Copyright 2010-2011 Miguel de Icaza
//
// Based on the TweetStation specific ImageStore
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

//
// Minor changes (UIImage -> Drawable) required to get this running on Mono-for-Android
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

using System.Security.Cryptography;
using Android.Graphics.Drawables;
using MWC;
using Android.Util;

namespace MonoTouch.Dialog.Utilities
{
    /// <summary>
    ///    This interface needs to be implemented to be notified when an image
    ///    has been downloaded.   The notification will happen on the UI thread.
    ///    Upon notification, the code should call RequestImage again, this time
    ///    the image will be loaded from the on-disk cache or the in-memory cache.
    /// </summary>
    public interface IImageUpdated
    {
        /// <summary>
        /// On Android, you MUST do the operations in your implementation on the UiThread.
        /// Be sure to use RunOnUiThread()!!!
        /// </summary>
        void UpdatedImage(Uri uri);
    }

    /// <summary>
    ///   Network image loader, with local file system cache and in-memory cache
    /// </summary>
    /// <remarks>
    ///   By default, using the static public methods will use an in-memory cache
    ///   for 50 images and 4 megs total.   The behavior of the static methods 
    ///   can be modified by setting the public DefaultLoader property to a value
    ///   that the user configured.
    /// 
    ///   The instance methods can be used to create different imageloader with 
    ///   different properties.
    ///  
    ///   Keep in mind that the phone does not have a lot of memory, and using
    ///   the cache with the unlimited value (0) even with a number of items in
    ///   the cache can consume memory very quickly.
    /// 
    ///   Use the Purge method to release all the memory kept in the caches on
    ///   low memory conditions, or when the application is sent to the background.
    /// </remarks>

    public class ImageLoader
    {
        public readonly static string BaseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "..");
        const int MaxRequests = 6;
        static string PicDir;

        // Cache of recently used images
        LRUCache<Uri, Drawable /*UIImage*/> cache;

        // A list of requests that have been issues, with a list of objects to notify.
        static Dictionary<Uri, List<IImageUpdated>> pendingRequests;

        // A list of updates that have completed, we must notify the main thread about them.
        static HashSet<Uri> queuedUpdates;

        // A queue used to avoid flooding the network stack with HTTP requests
        static Stack<Uri> requestQueue;

        /*static NSString nsDispatcher = "x"; */

        static MD5CryptoServiceProvider checksum = new MD5CryptoServiceProvider();

        /// <summary>
        ///    This contains the default loader which is configured to be 50 images
        ///    up to 4 megs of memory.   Assigning to this property a new value will
        ///    change the behavior.   This property is lazyly computed, the first time
        ///    an image is requested.
        /// </summary>
        public static ImageLoader DefaultLoader;

        static ImageLoader()
        {
            PicDir = Path.Combine(BaseDir, "Library/Caches/Pictures.MonoTouch.Dialog/");

            if (!Directory.Exists(PicDir))
                Directory.CreateDirectory(PicDir);

            pendingRequests = new Dictionary<Uri, List<IImageUpdated>>();
            queuedUpdates = new HashSet<Uri>();
            requestQueue = new Stack<Uri>();
        }

        /// <summary>
        ///   Creates a new instance of the image loader
        /// </summary>
        /// <param name="cacheSize">
        /// The maximum number of entries in the LRU cache
        /// </param>
        /// <param name="memoryLimit">
        /// The maximum number of bytes to consume by the image loader cache.
        /// </param>
        public ImageLoader(int cacheSize, int memoryLimit)
        {
            cache = new LRUCache<Uri, Drawable /*UIImage*/>(cacheSize, memoryLimit, sizer);
        }

        static int sizer(Drawable /*UIImage*/ img)
        {
            /*var cg = img.CGImage;
            return cg.BytesPerRow * cg.Height;*/
            var pixels = img.IntrinsicHeight * img.IntrinsicWidth;
            return pixels * 3; //HACK: assume 3 bytes per pixel (24bit)
        }

        /// <summary>
        ///    Purges the contents of the DefaultLoader
        /// </summary>
        public static void Purge()
        {
            if (DefaultLoader != null)
                DefaultLoader.PurgeCache();
        }

        /// <summary>
        ///    Purges the cache of this instance of the ImageLoader, releasing 
        ///    all the memory used by the images in the caches.
        /// </summary>
        public void PurgeCache()
        {
            cache.Purge();
        }

        static int hex(int v)
        {
            if (v < 10)
                return '0' + v;
            return 'a' + v - 10;
        }

        static string md5(string input)
        {
            var bytes = checksum.ComputeHash(Encoding.UTF8.GetBytes(input));
            var ret = new char[32];
            for (int i = 0; i < 16; i++)
            {
                ret[i * 2] = (char)hex(bytes[i] >> 4);
                ret[i * 2 + 1] = (char)hex(bytes[i] & 0xf);
            }
            return new string(ret);
        }

        /// <summary>
        ///   Requests an image to be loaded using the default image loader
        /// </summary>
        /// <param name="uri">
        /// The URI for the image to load
        /// </param>
        /// <param name="notify">
        /// A class implementing the IImageUpdated interface that will be invoked when the image has been loaded
        /// </param>
        /// <returns>
        /// If the image has already been downloaded, or is in the cache, this will return the image as a Drawable.
        /// </returns>
        public static Drawable /*UIImage*/ DefaultRequestImage(Uri uri, IImageUpdated notify)
        {
            if (DefaultLoader == null)
                DefaultLoader = new ImageLoader(50, 4 * 1024 * 1024);
            return DefaultLoader.RequestImage(uri, notify);
        }

        /// <summary>
        ///   Requests an image to be loaded from the network
        /// </summary>
        /// <param name="uri">
        /// The URI for the image to load
        /// </param>
        /// <param name="notify">
        /// A class implementing the IImageUpdated interface that will be invoked when the image has been loaded
        /// </param>
        /// <returns>
        /// If the image has already been downloaded, or is in the cache, this will return the image as a Drawable.
        /// </returns>
        public Drawable /*UIImage*/ RequestImage(Uri uri, IImageUpdated notify)
        {

            LogDebug("..requ " + ImageName(uri.AbsoluteUri));
            Drawable /*UIImage*/ ret;

            lock (cache)
            {
                ret = cache[uri];
                if (ret != null)
                    return ret;
            }

            lock (requestQueue)
            {
                if (pendingRequests.ContainsKey(uri))
                    return null;
            }

            string picfile = uri.IsFile ? uri.LocalPath : PicDir + md5(uri.AbsoluteUri);
            if (File.Exists(picfile))
            {
                ret = Drawable.CreateFromPath(picfile); /* UIImage.FromFileUncached(picfile);*/
                if (ret != null)
                {
                    lock (cache)
                        cache[uri] = ret;
                    return ret;
                }
            }
            if (uri.IsFile) // so there is no point queueing a request for it :)
                return null;

            if (notify != null) // if notify is null, we won't bother retrieving again... assume this is a hit-and-hope query
                QueueRequest(uri, picfile, notify);
            return null;
        }

        static void QueueRequest(Uri uri, string target, IImageUpdated notify)
        {
            if (notify == null)
                throw new ArgumentNullException("notify");

            lock (requestQueue)
            {
                if (pendingRequests.ContainsKey(uri))
                {
                    LogDebug("-------- pendingRequest: added new listener for " + ImageName(uri.AbsoluteUri));
                    pendingRequests[uri].Add(notify);
                    return;
                }
                var slot = new List<IImageUpdated>(4);
                slot.Add(notify);
                pendingRequests[uri] = slot;

                if (picDownloaders >= MaxRequests) {
                    requestQueue.Push(uri);
                    LogDebug("----push " + ImageName(uri.AbsoluteUri));
                } else {
                    
                    ThreadPool.QueueUserWorkItem(delegate {
                        try {
                            LogDebug("----dwnl " + ImageName(uri.AbsoluteUri));
                            StartPicDownload(uri, target);
                        } catch (Exception e) {
                            LogDebug(e.Message);
                        }
                    });
                }
            }
        }
        //TODO: remove; this is just here for logging
        public static string ImageName(string uri)
        {
            var start = uri.LastIndexOf("/") + 1;
            var end = uri.LastIndexOf(".");
            return uri.Substring(start, end-start);
        }
        static bool Download(Uri uri, string target)
        {
            var buffer = new byte[4 * 1024];

            try
            {   // use Java.Net.URL instead of WebClient...
                var tmpfile = target + ".tmp";
                var imageUrl = new Java.Net.URL(uri.AbsoluteUri);
                var stream = imageUrl.OpenStream();
                LogDebug("====== open " + ImageName(uri.AbsoluteUri));
                using (var o = File.Open(tmpfile, FileMode.OpenOrCreate)) {
                    byte[] buf = new byte[1024];
                    int r;
                    while ((r = stream.Read(buf, 0, buf.Length)) > 0) {
                        o.Write(buf, 0, r);
                    }
                }

                //using (var file = new FileStream(tmpfile, FileMode.Create, FileAccess.Write, FileShare.Read))
                //{
                    //var req = WebRequest.Create(uri) as HttpWebRequest;

                    //using (var resp = req.GetResponse())
                    //{
                    //    using (var s = resp.GetResponseStream())
                    //    {
                    //        int n;
                    //        while ((n = s.Read(buffer, 0, buffer.Length)) > 0)
                    //        {
                    //            file.Write(buffer, 0, n);
                    //        }
                    //    }
                    //}
                //}
                if (!File.Exists(target))   // we're never updating images if they change, to reduce Exceptions and speed up
                    File.Move(tmpfile, target);
                return true;
            }
            catch (Exception e)
            {
                LogDebug(String.Format("Problem with {0} {1}", uri, e.Message));
                return false;
            }
        }

        static long picDownloaders;

        static void StartPicDownload(Uri uri, string target)
        {
            LogDebug("________star " + picDownloaders);
            Interlocked.Increment(ref picDownloaders);
            try
            {
                _StartPicDownload(uri, target);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("CRITICAL: should have never happened {0}", e);
            }
            //Util.Log ("Leaving StartPicDownload {0}", picDownloaders);
            LogDebug("________end  " + picDownloaders);
            Interlocked.Decrement(ref picDownloaders);
        }

        static void _StartPicDownload(Uri uri, string target)
        {
            do
            {
                bool downloaded = false;

                //System.Threading.Thread.Sleep (5000);
                downloaded = Download(uri, target);
                if (!downloaded)
                    LogDebug((String.Format("Error fetching picture for {0} to {1}", uri, target)));

                // Cluster all updates together
                bool doInvoke = false;

                lock (requestQueue)
                {
                    if (downloaded)
                    {
                        queuedUpdates.Add(uri);

                        // If this is the first queued update, must notify
                        if (queuedUpdates.Count == 1)
                            doInvoke = true;
                    }
                    else
                        pendingRequests.Remove(uri);

                    // Try to get more jobs.
                    if (requestQueue.Count > 0)
                    {
                        uri = requestQueue.Pop();
                        if (uri == null)
                        {
                            Console.Error.WriteLine("Dropping request {0} because url is null", uri);
                            pendingRequests.Remove(uri);
                            uri = null;
                        }
                    }
                    else
                    {
                        //Util.Log ("Leaving because requestQueue.Count = {0} NOTE: {1}", requestQueue.Count, pendingRequests.Count);
                        uri = null;
                    }
                }
                if (doInvoke)
                {
                    /*nsDispatcher.BeginInvokeOnMainThread(NotifyImageListeners);*/
                    // HACK: need a context to do RunOnUiThread on...
                    //RunOnUiThread(() =>
                    //{
                        NotifyImageListeners();
                    //});
                }
            } while (uri != null);
        }

        /// <summary>
        /// NEEDS TO run on the main thread. The iOS version does, but in Android
        /// we need access to a Context to get to the main thread, and I haven't
        /// figured out a non-hacky way to do that yet.
        /// </summary>
        static void NotifyImageListeners()
        {
            lock (requestQueue)
            {
                foreach (var quri in queuedUpdates)
                {
                    var list = pendingRequests[quri];
                    pendingRequests.Remove(quri);
                    foreach (var pr in list)
                    {
                        try
                        {
                            pr.UpdatedImage(quri); // this is the bit that should be on the UiThread
                        }
                        catch (Exception e)
                        {
                            LogDebug(e.Message);
                        }
                    }
                }

                queuedUpdates.Clear();
            }
        }

        /*
Use this to help with ADB watching in CMD 
"c:\Program Files (x86)\Android\android-sdk\platform-tools\adb" logcat -s MonoDroid:* mono:* MWC:* ActivityManager:*
*/
        public static void LogDebug(string message)
        {
            Console.WriteLine(message);
            Log.Debug("MWC", message);
        }
    }
}


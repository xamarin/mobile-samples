using System;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.IO.IsolatedStorage;
using System.IO;
using System.Windows.Resources;

namespace MWC.WP7 {
    public partial class AboutXamarin : PhoneApplicationPage {
        public AboutXamarin()
        {
            InitializeComponent();
            SaveFilesToIsoStore();

            Browser.Navigate(new Uri("Assets/About/index.html", UriKind.Relative));
            Browser.Navigating += Browser_Navigating;
        }
        
        private void Browser_Navigating(object sender, NavigatingEventArgs e)
        {
            if (e.Uri.IsAbsoluteUri) {
                e.Cancel = true;

                var task = new WebBrowserTask {
                    Uri = e.Uri,
                };
                task.Show();
            }
        }

        private void SaveFilesToIsoStore()
        {
            //These files must match what is included in the application package,
            //or BinaryStream.Dispose below will throw an exception.
            string[] files = {
                  "Assets/About/api.png"
                , "Assets/About/framework.png"
                , "Assets/About/hardware-320.png"
                , "Assets/About/header.png"
                , "Assets/About/ide.png"
                , "Assets/About/index.html"
                , "Assets/About/logo_facebook.png"
                , "Assets/About/logo_linkedin.png"
                , "Assets/About/logo_rss.png"
                , "Assets/About/logo_stackoverflow.png"
                , "Assets/About/logo_twitter.png"
                , "Assets/About/logo_youtube.png"
                , "Assets/About/multi.png"
                , "Assets/About/net.png"
                , "Assets/About/share.png"
                , "Assets/About/site.css"
            };

            IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();

            if (false == isoStore.FileExists(files[0])) {
                foreach (string f in files) {
                    StreamResourceInfo sr = Application.GetResourceStream(new Uri(f, UriKind.Relative));
                    using (BinaryReader br = new BinaryReader(sr.Stream)) {
                        byte[] data = br.ReadBytes((int)sr.Stream.Length);
                        SaveToIsoStore(f, data);
                    }
                }
            }
        }

        private void SaveToIsoStore(string fileName, byte[] data)
        {
            string strBaseDir = string.Empty;
            string delimStr = "/";
            char[] delimiter = delimStr.ToCharArray();
            string[] dirsPath = fileName.Split(delimiter);

            //Get the IsoStore.
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();

            //Re-create the directory structure.
            for (int i = 0; i < dirsPath.Length - 1; i++) {
                strBaseDir = System.IO.Path.Combine(strBaseDir, dirsPath[i]);
                isoStore.CreateDirectory(strBaseDir);
            }

            //Remove the existing file.
            if (isoStore.FileExists(fileName)) {
                isoStore.DeleteFile(fileName);
            }

            //Write the file.
            using (BinaryWriter bw = new BinaryWriter(isoStore.CreateFile(fileName))) {
                bw.Write(data);
                bw.Close();
            }
        }

    }
}

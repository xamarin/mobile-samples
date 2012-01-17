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
using System.IO;
using System.Text;

namespace MWC
{
    public static partial class FileHelper
    {
        public static string ReadAllText(string path)
        {
	        string result;
	        using (StreamReader streamReader = new StreamReader (path))
	        {
		        result = streamReader.ReadToEnd ();
	        }
	        return result;
        }
        public static void WriteAllText(string path, string contents, Encoding encoding)
        {
            using (StreamWriter streamWriter = new StreamWriter(path, false, encoding))
            {
                streamWriter.Write(contents);
            }
        }
        public static void WriteAllText(string path, string contents)
        {
            FileHelper.WriteAllText(path, contents, Encoding.UTF8);
        }


    }
}

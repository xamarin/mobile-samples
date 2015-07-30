using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Tasky.Portable;

namespace TaskyWinPhone
{
    public class XmlStorageImplementation : IXmlStorage
    {
        public XmlStorageImplementation()
        {
        }

        public List<Task> ReadXml(string filename)
        {
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();

            if (fileStorage.FileExists(filename))
            {
                var serializer = new XmlSerializer(typeof(List<Task>));

                using (var stream = new StreamReader(new IsolatedStorageFileStream(filename, FileMode.Open, fileStorage)))
                {
                    return (List<Task>)serializer.Deserialize(stream);
                }
            }
            return new List<Task>();
        }

        public void WriteXml(List<Task> tasks, string filename)
        {
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();

            var serializer = new XmlSerializer(typeof(List<Task>));
            using (var writer = new StreamWriter(new IsolatedStorageFileStream(filename, FileMode.OpenOrCreate, fileStorage)))
            {
                serializer.Serialize(writer, tasks);
            }
        }
    }
}

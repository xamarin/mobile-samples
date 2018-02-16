using System.Runtime.Serialization;

namespace HelloWorldService
{
    [DataContract]
    public class HelloWorldData
    {
        [DataMember]
        public bool SayHello { get; set; }

        [DataMember]
        public string Name { get; set; }

        public HelloWorldData()
        {
            Name = "Hello ";
            SayHello = false;
        }
    }
}

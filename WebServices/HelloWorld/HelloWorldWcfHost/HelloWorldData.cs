namespace HelloWorldWcfHost
{
    using System.Runtime.Serialization;

    [DataContract]
    public class HelloWorldData
    {
        public HelloWorldData()
        {
            Name = "Hello ";
            SayHello = false;
        }

        [DataMember]
        public bool SayHello { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
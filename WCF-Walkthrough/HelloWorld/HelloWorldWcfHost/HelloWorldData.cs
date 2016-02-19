using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace HelloWorldWcfHost
{
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
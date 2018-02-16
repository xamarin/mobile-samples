using System;

namespace HelloWorldService
{
    public class HelloWorldService : IHelloWorldService
    {
        public HelloWorldData GetHelloData(HelloWorldData helloWorldData)
        {
            if (helloWorldData == null)
                throw new ArgumentException("helloWorldData");

            if (helloWorldData.SayHello)
                helloWorldData.Name = "Hello World to {helloWorldData.Name}";

            return helloWorldData;
        }

        public string SayHelloTo(string name)
        {
            return "Hello World to you, {name}";
        }
    }
}

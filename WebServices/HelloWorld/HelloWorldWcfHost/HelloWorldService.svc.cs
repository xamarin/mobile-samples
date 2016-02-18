namespace HelloWorldWcfHost
{
    using System;

    public class HelloWorldService : IHelloWorldService
    {
        public HelloWorldData GetHelloData(HelloWorldData helloWorldData)
        {
            if (helloWorldData == null)
            {
                throw new ArgumentNullException("helloWorldData");
            }

            if (helloWorldData.SayHello)
            {
                helloWorldData.Name = String.Format("Hello World to {0}.", helloWorldData.Name);
            }
            return helloWorldData;
        }

        public string SayHelloTo(string name)
        {
            return string.Format("Hello World to you, {0}", name);
        }
    }
}

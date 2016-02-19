namespace HelloWorldWcfHost
{
    using System;

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class HelloWorldService : IHelloWorldService
    {
        public HelloWorldData GetHelloData(HelloWorldData composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.SayHello)
            {
                composite.Name += " Suffix ";
            }
            return composite;
        }

        public string SayHelloTo(string name)
        {
            return String.Format("Hello to you, {0}.", name);
        }
    }
}

namespace Xamarin.HelloWorldClient
{
    using System;
    using System.Diagnostics;

    using Xamarin.HelloWorldClient.ServiceReference1;

    internal class Program
    {
        private static void Main(string[] args)
        {
            using (HelloWorldServiceClient client = new HelloWorldServiceClient())
            {
                string result = client.SayHelloTo("Kilroy");
                Console.WriteLine(result);
                Debug.WriteLine(result);

                HelloWorldData data = new HelloWorldData
                                          {
                                              SayHello = true,
                                              Name = "Mr. Chad"
                                          };
                HelloWorldData newData = client.GetHelloData(data);
                Console.WriteLine(newData.Name);
                Debug.WriteLine(newData.Name);

                Console.ReadKey();
            }
        }
    }
}

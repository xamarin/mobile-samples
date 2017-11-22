namespace HelloWorldWcfHost
{
    using System.ServiceModel;

    [ServiceContract]
    public interface IHelloWorldService
    {
        [OperationContract]
        string SayHelloTo(string name);

        [OperationContract]
        HelloWorldData GetHelloData(HelloWorldData helloWorldData);
    }
}

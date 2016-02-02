using HelloWorldWcfHost;
using System;
using System.ServiceModel;
using UIKit;

namespace HelloWorld.iOS
{
    public partial class ViewController : UIViewController
    {
        public static readonly EndpointAddress EndPoint = new EndpointAddress("http://x.x.x.x:9608/HelloWorldService.svc");

        private HelloWorldServiceClient _client;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitializeHelloWorldServiceClient();

        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        private void InitializeHelloWorldServiceClient()
        {
            BasicHttpBinding binding = CreateBasicHttp();

            _client = new HelloWorldServiceClient(binding, EndPoint);
            _client.SayHelloToCompleted += ClientOnSayHelloToCompleted;
            _client.GetHelloDataCompleted += ClientOnGetHelloDataCompleted;

            getHelloWorldDataButton.TouchUpInside += GetHelloWorldDataButtonTouchUpInside;
            sayHelloWorldButton.TouchUpInside += SayHelloWorldDataButtonTouchUpInside;
        }

        private static BasicHttpBinding CreateBasicHttp()
        {
            BasicHttpBinding binding = new BasicHttpBinding
            {
                Name = "basicHttpBinding",
                MaxBufferSize = 2147483647,
                MaxReceivedMessageSize = 2147483647
            };
            TimeSpan timeout = new TimeSpan(0, 0, 30);
            binding.SendTimeout = timeout;
            binding.OpenTimeout = timeout;
            binding.ReceiveTimeout = timeout;
            return binding;
        }

        private void SayHelloWorldDataButtonTouchUpInside(object sender, EventArgs e)
        {
            sayHelloWorldText.Text = "Waiting for WCF...";
            _client.SayHelloToAsync("Kilroy");
        }

        private void GetHelloWorldDataButtonTouchUpInside(object sender, EventArgs e)
        {
            getHelloWorldDataText.Text = "Waiting WCF...";
            HelloWorldData data = new HelloWorldData { Name = "Mr. Chad", SayHello = true };
            _client.GetHelloDataAsync(data);
        }

        private void ClientOnGetHelloDataCompleted(object sender, GetHelloDataCompletedEventArgs e)
        {
            string msg = null;

            if (e.Error != null)
            {
                msg = e.Error.Message;
            }
            else if (e.Cancelled)
            {
                msg = "Request was cancelled.";
            }
            else
            {
                msg = e.Result.Name;
            }

            InvokeOnMainThread(() => getHelloWorldDataText.Text = msg);
        }

        private void ClientOnSayHelloToCompleted(object sender, SayHelloToCompletedEventArgs e)
        {
            string msg = null;

            if (e.Error != null)
            {
                msg = e.Error.Message;
            }
            else if (e.Cancelled)
            {
                msg = "Request was cancelled.";
            }
            else
            {
                msg = e.Result;
            }
            InvokeOnMainThread(() => sayHelloWorldText.Text = msg);
        }
    }
}
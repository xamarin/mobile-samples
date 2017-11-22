namespace Xamarin.iOS
{
    using System;
    using System.ServiceModel;

    using HelloWorldWcfHost;

    using MonoTouch.UIKit;

    public partial class Xamarin_iOSViewController : UIViewController
    {
        public static readonly EndpointAddress EndPoint = new EndpointAddress("http://192.168.100.143:9609/HelloWorldService.svc");

        private HelloWorldServiceClient _client;

        public Xamarin_iOSViewController() : base("Xamarin_iOSViewController", null)
        {
        }

        public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
        {
            // Return true for supported orientations
            return (toInterfaceOrientation == UIInterfaceOrientation.Portrait);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            InitializeHelloWorldServiceClient();
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

        private void GetHelloWorldDataButtonTouchUpInside(object sender, EventArgs e)
        {
            getHelloWorldDataText.Text = "Waiting WCF...";
            HelloWorldData data = new HelloWorldData { Name = "Mr. Chad", SayHello = true };
            _client.GetHelloDataAsync(data);
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

        private void SayHelloWorldDataButtonTouchUpInside(object sender, EventArgs e)
        {
            sayHelloWorldText.Text = "Waiting for WCF...";
            _client.SayHelloToAsync("Kilroy");
        }
    }
}

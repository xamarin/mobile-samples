using System;
using Android.App;
using Android.Widget;
using Android.OS;
using System.ServiceModel;
using HelloWorldWcfHost;
using Android;

namespace HelloWorld.Android
{
    [Activity(Label = "HelloWorld.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static readonly EndpointAddress EndPoint = new EndpointAddress("http://x.x.x.x:9608/HelloWorldService.svc");

        private HelloWorldServiceClient _client;
        private Button _getHelloWorldDataButton;
        private TextView _getHelloWorldDataTextView;
        private Button _sayHelloWorldButton;
        private TextView _sayHelloWorldTextView;

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

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            InitializeHelloWorldServiceClient();

            // This button will invoke the GetHelloWorldData - the method that takes a C# object as a parameter.
            _getHelloWorldDataButton = FindViewById<Button>(Resource.Id.getHelloWorldDataButton);
            _getHelloWorldDataButton.Click += GetHelloWorldDataButtonOnClick;
            _getHelloWorldDataTextView = FindViewById<TextView>(Resource.Id.getHelloWorldDataTextView);

            // This button will invoke SayHelloWorld - this method takes a simple string as a parameter.
            _sayHelloWorldButton = FindViewById<Button>(Resource.Id.sayHelloWorldButton);
            _sayHelloWorldButton.Click += SayHelloWorldButtonOnClick;
            _sayHelloWorldTextView = FindViewById<TextView>(Resource.Id.sayHelloWorldTextView);
        }

        private void InitializeHelloWorldServiceClient()
        {
            BasicHttpBinding binding = CreateBasicHttp();

            _client = new HelloWorldServiceClient(binding, EndPoint);
            _client.SayHelloToCompleted += ClientOnSayHelloToCompleted;
            _client.GetHelloDataCompleted += ClientOnGetHelloDataCompleted;
        }

        private void GetHelloWorldDataButtonOnClick(object sender, EventArgs eventArgs)
        {
            HelloWorldData data = new HelloWorldData { Name = "Mr. Chad", SayHello = true };
            _getHelloWorldDataTextView.Text = "Waiting for WCF...";
            _client.GetHelloDataAsync(data);
        }

        private void SayHelloWorldButtonOnClick(object sender, EventArgs eventArgs)
        {
            _sayHelloWorldTextView.Text = "Waiting for WCF...";
            _client.SayHelloToAsync("Kilroy");
        }

        private void ClientOnGetHelloDataCompleted(object sender, GetHelloDataCompletedEventArgs getHelloDataCompletedEventArgs)
        {
            string msg = null;

            if (getHelloDataCompletedEventArgs.Error != null)
            {
                msg = getHelloDataCompletedEventArgs.Error.Message;
            }
            else if (getHelloDataCompletedEventArgs.Cancelled)
            {
                msg = "Request was cancelled.";
            }
            else
            {
                msg = getHelloDataCompletedEventArgs.Result.Name;
            }
            RunOnUiThread(() => _getHelloWorldDataTextView.Text = msg);
        }

        private void ClientOnSayHelloToCompleted(object sender, SayHelloToCompletedEventArgs sayHelloToCompletedEventArgs)
        {
            string msg = null;

            if (sayHelloToCompletedEventArgs.Error != null)
            {
                msg = sayHelloToCompletedEventArgs.Error.Message;
            }
            else if (sayHelloToCompletedEventArgs.Cancelled)
            {
                msg = "Request was cancelled.";
            }
            else
            {
                msg = sayHelloToCompletedEventArgs.Result;
            }
            RunOnUiThread(() => _sayHelloWorldTextView.Text = msg);
        }
    }
}


using System;

namespace XamarinTodoQuickStart
{
    public static class Constants
    {
        // Azure app specific URL and key
		public const string ApplicationURL = @"https://YOUR_APP_NAME.azure-mobile.net/";
		public const string ApplicationKey = @"YOUR_APP_KEY";
		public const string SenderID = "YOUR_API_PROJ_NUMBER"; // Google API Project Number

        // Azure app specific connection string and hub path
		public const string ConnectionString = "Endpoint=sb://quickstarthub-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=YOUR_ACCESS_KEY";
        public const string NotificationHubPath = "QUICKSTARTHUB";
    }
}


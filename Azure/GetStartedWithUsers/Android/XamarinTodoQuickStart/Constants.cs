using System;

namespace XamarinTodoQuickStart
{
    public static class Constants
    {
		// Azure app specific URL and key
		public const string ApplicationURL = @"https://YOUR_APP_ID.azurewebsites.net/";

        // Scheme used to send web response to this app
		public const string AuthScheme = @"SCHEME_FROM_AUTH_PROVIDER"; // ALSO ENTER IN AndroidManifest.xml
    }
}

/*
EXAMPLE OF ANDROIDMANIFEST.XML ENTRY (for Microsoft authentication)

        <activity android:name="com.microsoft.windowsazure.mobileservices.authentication.RedirectUrlActivity"
                  android:launchMode="singleTop" android:noHistory="true">
          <intent-filter>
            <action android:name="android.intent.action.VIEW" />
            <category android:name="android.intent.category.DEFAULT" />
            <category android:name="android.intent.category.BROWSABLE" />
            <data android:scheme="msalGUIDFF-FFFF-FFFF-FFFF-0123456789ABCDEF" android:host="easyauth.callback" />
          </intent-filter>
        </activity>

*/
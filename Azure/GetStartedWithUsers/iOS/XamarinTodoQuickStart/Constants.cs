using System;

namespace XamarinTodoQuickStart
{
    public static class Constants
    {
        // Azure app specific URL and key
		public const string ApplicationURL = @"https://YOUR_APP_ID.azurewebsites.net/";

        // Scheme used to send web response to this app
		public const string AuthScheme = @"SCHEME_FROM_AUTH_PROVIDER"; // ALSO ENTER IN INFO.PLIST      

    }
}


/*
EXAMPLE OF INFO.PLIST ENTRY (for Microsoft authentication)

    <array>
        <dict>
            <key>CFBundleTypeRole</key>
            <string>Editor</string>
            <key>CFBundleURLName</key>
            <string>$(PRODUCT_BUNDLE_IDENTIFIER)</string>
            <key>CFBundleURLSchemes</key>
            <array>
                <string>msalGUIDFF-FFFF-FFFF-FFFF-0123456789ABCDEF</string>
            </array>
        </dict>
    </array>
*/
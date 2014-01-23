using System;
using System.Text;
using System.Net;
using System.IO;
using System.Json;
using System.Diagnostics;

namespace MWC
{
	public class TwitterAuthenticationResponse
	{
		public string TokenType { get; set; }

		public string AccessToken { get; set; }
	}

	public class TwitterAuthorizer
	{
		private string authHeaderFormat = "Basic {0}";
		private HttpWebRequest authRequest;

		public bool Authenticated { get; private set; }

		public TwitterAuthenticationResponse AuthenticationInfo { get; private set; }

		private string AuthHeader {
			get {
				byte[] authHeaderData = Encoding.UTF8.GetBytes (Uri.EscapeDataString (Constants.OAuthConsumerKey) + ":" +
				                        Uri.EscapeDataString (Constants.OAuthConsumerSecret));

				return string.Format (authHeaderFormat, Convert.ToBase64String (authHeaderData));
			}
		}

		private WebRequest AuthenticationRequest {
			get {
				if (authRequest == null) {
					authRequest = (HttpWebRequest)WebRequest.Create (Constants.OAuthUrl);
					authRequest.Headers.Add ("Authorization", AuthHeader);
					authRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
					authRequest.Method = "POST";
					authRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
					authRequest.Headers.Add ("Accept-Encoding", "gzip");

					using (var stream = authRequest.GetRequestStream ()) {
						byte[] content = ASCIIEncoding.ASCII.GetBytes ("grant_type=client_credentials");
						stream.Write (content, 0, content.Length);
					}
				} 

				return authRequest;
			}
		}

		public TwitterAuthorizer ()
		{
			Authorize ();
		}

		private void Authorize ()
		{
			try {
				var authResponse = AuthenticationRequest.GetResponse ();
				using (var reader = new StreamReader (authResponse.GetResponseStream ())) {
					var authenticationInfo = ParseAuthenticationResponse (reader.ReadToEnd ());
					if (!string.IsNullOrEmpty (authenticationInfo.AccessToken) &&
					    !string.IsNullOrEmpty (authenticationInfo.TokenType)) {
						Authenticated = true;
						AuthenticationInfo = authenticationInfo;
					} else {
						Debug.WriteLine ("Twitter authentication error occured");
					}
				}
			} catch (Exception e) {
				Debug.WriteLine ("Twitter authentication error: {0}", e.Message);
			}
		}

		private TwitterAuthenticationResponse ParseAuthenticationResponse (string tokenInfo)
		{
			if (string.IsNullOrEmpty (tokenInfo))
				throw new ArgumentNullException ("Twitter authentication error, token can't be null or emty string");

			var value = JsonValue.Parse (tokenInfo);
			return new TwitterAuthenticationResponse () {
				TokenType = value ["token_type"],
				AccessToken = value ["access_token"]
			};
		}
	}
}


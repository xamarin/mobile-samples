// 
//  Copyright 2011  abhatia
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using System.Linq;
using RestSharp;
//using ServiceStack.Text;
//using ServiceStack.Text.Common;
using MonoTouch.Foundation;
using System.Text;
using System.Json;

namespace WebServices.RxNormSample
{
	public class RxTermRestClient
	{
		public static bool IsInitialized = false;
		static RestClient Client;
		
		static Func<string, RxTerm> RxTermAction = RxTerm;
		
		public static bool Initialize()
		{
			Client = new RestClient(@"http://rxnav.nlm.nih.gov/REST/RxTerms/rxcui/");
			
			IsInitialized = true;
			
			return IsInitialized;
		}
		
		public static void GetRxTermAsync(string rxcui, Action<RxTerm> callback = null)
		{
			// This is how we call async methods
			RxTermAction.BeginInvoke(rxcui, (asyncResult) => {
				
				//This is the callback for the Action we defined we get the result then we fire the callback provided to us.
				var result = RxTermAction.EndInvoke(asyncResult);

				if(result != null && callback != null) {
					callback(result);
				}
				
			}, null);
		}
		
		private static RxTerm RxTerm(string rxcui)
		{
			var rxTerm = new RxTerm();
			
			try {
				
				var request = new RestRequest(string.Format("{0}/allinfo", rxcui));
				request.RequestFormat = DataFormat.Json;
				
				var response = Client.Execute(request);
				
				if(string.IsNullOrWhiteSpace(response.Content) || response.StatusCode != System.Net.HttpStatusCode.OK) {
					return null;
				}
				
				rxTerm = DeserializeRxTerm(response.Content);
				
			}
			catch (Exception ex) {
				Console.WriteLine("Problem getting data for concept: {0} -- Exception: {1}", rxcui, ex.Message + ex.StackTrace);
			}
			
			return rxTerm;
		}
		
		private static RxTerm RxTermViaNSURLConnectionAsync(string rxcui, Action<RxTerm> callback = null)
		{
			var rxTerm = new RxTerm();
			
			try {
				var request = new NSMutableUrlRequest(new NSUrl(string.Format("http://rxnav.nlm.nih.gov/REST/RxTerms/rxcui/{0}/allinfo", rxcui)), 
				NSUrlRequestCachePolicy.ReloadRevalidatingCacheData, 20);
				request["Accept"] = "application/json";
			
				var connectionDelegate = new RxTermNSURLConnectionDelegate();
				var connection = new NSUrlConnection(request, connectionDelegate);
				connection.Start();
				
				
			} catch (Exception ex) {
				Console.WriteLine("Problem getting data for concept: {0} -- Exception: {1}", rxcui, ex.Message + ex.StackTrace);
			}
			
			return rxTerm;
		}
		
//		private static RxTerm DeserializeRxTerm(string json)
//		{
//			return JsonObject.Parse(json).Object("rxtermsProperties")
//				.ConvertTo(x => new RxTerm {
//					BrandName = x.Get("brandName"),
//					DisplayName = x.Get("displayName"),
//					Synonym = x.Get("synonym"),
//					FullName = x.Get("fullName"),
//					FullGenericName = x.Get("fullGenericName"),
//					Strength = x.Get("strength"),
//					RxTermDoseForm = x.Get("rxtermsDoseForm"),
//					Route = x.Get("route"),
//					RxCUI = x.Get("rxcui"),
//					RxNormDoseForm = x.Get("rxnormDoseForm"),
//				});
//		}

        static RxTerm DeserializeRxTerm(string json)
        {
            var jsonValue = (JsonObject)JsonObject.Parse(json);

            var rxTerm = new RxTerm { 
                BrandName = jsonValue["rxtermsProperties"]["brandName"],
                DisplayName = jsonValue["rxtermsProperties"]["displayName"],
                Synonym = jsonValue["rxtermsProperties"]["synonym"],
                FullName =jsonValue["rxtermsProperties"]["fullName"],
                FullGenericName =jsonValue["rxtermsProperties"]["fullGenericName"],
                Strength =jsonValue["rxtermsProperties"]["strength"],
                RxTermDoseForm =jsonValue["rxtermsProperties"]["rxtermsDoseForm"],
                Route =jsonValue["rxtermsProperties"]["route"],
                RxCUI =jsonValue["rxtermsProperties"]["rxcui"],
                RxNormDoseForm =jsonValue["rxtermsProperties"]["rxnormDoseForm"]
            };

            return rxTerm;
        }
	}
		
	public class RxTermNSURLConnectionDelegate : NSUrlConnectionDelegate
	{
		StringBuilder _ResponseBuilder;
		
		public bool IsFinishedLoading { get; set; }
		public string ResponseContent { get; set; }
		
		public RxTermNSURLConnectionDelegate()
			: base()
		{
			_ResponseBuilder = new StringBuilder();
		}
		
		public override void ReceivedData(NSUrlConnection connection, NSData data)
		{
			if(data != null) {
				_ResponseBuilder.Append(data.ToString());
			}
		}
		public override void FinishedLoading(NSUrlConnection connection)
		{
			IsFinishedLoading = true;
			ResponseContent = _ResponseBuilder.ToString();
			Console.Out.WriteLine("Response Body: \r\n {0}", ResponseContent);
		}
	}
}


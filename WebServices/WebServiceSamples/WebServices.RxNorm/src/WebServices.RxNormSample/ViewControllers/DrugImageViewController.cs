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
using MonoTouch.UIKit;
using System.IO;
using MonoTouch.Foundation;
using MonoTouch.Dialog;
using System.Drawing;

namespace WebServices.RxNormSample
{
	public class DrugImageViewController : UIViewController
	{
		RxTerm _Term;
		UIScrollView _DrugScrollView;
		
		public DrugImageViewController(RxTerm term)
			: base()
		{
			_Term = term;
			this.Title = term.DisplayName;
			
		}
		
		public override void LoadView()
		{
			base.LoadView();
			
			_DrugScrollView = new UIScrollView();
			_DrugScrollView.ContentSize = new SizeF(500, 400);
			_DrugScrollView.ScrollEnabled = true;
			_DrugScrollView.UserInteractionEnabled = true;
			_DrugScrollView.MultipleTouchEnabled = true;
			
			if(PillboxClient.IsInitialized == false) {
				PillboxClient.Initialize();
			}
		}
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			_DrugScrollView.Frame = this.View.Bounds;
			
			this.View.AddSubview(_DrugScrollView);
		}
		
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			PillboxClient.DownloadPillboxImageAsync(_Term.RxCUI, FinishedDownloadingImage);
		}
		
		private void FinishedDownloadingImage(string path)
		{
			if(File.Exists(path)){
				using(var pool = new NSAutoreleasePool()) {
					pool.BeginInvokeOnMainThread(() => {
						var drugImageView = new UIImageView(UIImage.FromFile(path));
						drugImageView.Frame = new RectangleF(0, 0, 448, 320);
						_DrugScrollView.AddSubview(drugImageView);
					});
				}
			}
			else {
				using(var pool = new NSAutoreleasePool()) {
					pool.BeginInvokeOnMainThread(() => {
				
                        var alert = new UIAlertView ("", "No Image Found", null, "OK");
						alert.AlertViewStyle = UIAlertViewStyle.Default;
						
						alert.Dismissed += delegate {
							// do something, go back maybe.
						};
						alert.Show();
					});
				}
			}
		}
		
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
	}
}


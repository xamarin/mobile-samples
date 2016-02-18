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
using System.Drawing;

namespace WebServices.RxNormSample
{
	public class DrugSearchingView : UIView
	{
		UIButton _CancelButton;
		UILabel _LoadingLabel;
		UIView _TransparentBackground;
		
		Action SearchCancelledAction;
		
		public DrugSearchingView(Action searchCancelled)
			: base()
		{
			this.SearchCancelledAction = searchCancelled;
			InitializeView();
		}
		
		private void InitializeView()
		{
			_CancelButton = UIButton.FromType(UIButtonType.RoundedRect);
			_CancelButton.SetTitle("Cancel", UIControlState.Normal);
			_CancelButton.TouchUpInside += Handle_CancelButtonTouchUpInside;
			
			_LoadingLabel = new UILabel();
			_LoadingLabel.Text = "Loading...";
			_LoadingLabel.Font = UIFont.FromName("HelveticaNeue", 20);
			_LoadingLabel.TextColor = UIColor.Black;
			_LoadingLabel.ShadowColor = UIColor.LightGray;
			_LoadingLabel.BackgroundColor = UIColor.Clear;
			_LoadingLabel.TextAlignment = UITextAlignment.Center;
			
			_TransparentBackground = new UIView();
			_TransparentBackground.BackgroundColor = UIColor.DarkGray;
			_TransparentBackground.Layer.Opacity = 0.5f;
			
			this.AddSubview(_TransparentBackground);
			this.AddSubview(_LoadingLabel);
			this.AddSubview(_CancelButton);
			
			this.BackgroundColor = UIColor.Clear;
//			this.Layer.SublayersOpacity = 0.4f;
		}

		void Handle_CancelButtonTouchUpInside(object sender, EventArgs e)
		{
			if(SearchCancelledAction != null) {
				SearchCancelledAction();
			}
		}
		
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			_TransparentBackground.Frame = this.Bounds;
			_CancelButton.Frame = new RectangleF(new PointF(this.Center.X - 50, this.Center.Y - 22), new SizeF(100, 44));
			_LoadingLabel.Frame = new RectangleF(this.Frame.X, this.Frame.Top + 100, this.Frame.Width, 44);
		}
		
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			
			if(disposing) {
				
				
			}
		}
	}
}


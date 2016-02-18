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
using MonoTouch.Dialog;
using WebServices.RxNormSample.rxnav.nlm.nih.gov;
using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.Foundation;

namespace WebServices.RxNormSample
{
	public class DrugSearchViewController : DialogViewController
	{
		DBManagerService _Service;
		
		DrugSearchingView _SearchingView;
		UISearchBar _SearchBar;
		
		public bool IsSearching { get; set; }
		
		public DrugSearchViewController()
			: base(new RootElement(""), true)
		{
			this.Title = "Search";
		}
		
		public override void LoadView()
		{
			base.LoadView();
			this.IsSearching = false;
			
			PillboxClient.Initialize();
			_Service = new DBManagerService(@"http://mor.nlm.nih.gov/axis/services/RxNormDBService");
			
			_SearchBar = new UISearchBar();
			_SearchBar.Text = "sildenafil";
			_SearchBar.Frame = new RectangleF(0, -44, View.Frame.Width, 44);
			_SearchBar.SearchButtonClicked += delegate {
				Search(_SearchBar.Text);
			};
			_SearchBar.CancelButtonClicked += delegate {
				SearchCancelled();
			};
			
			_SearchingView = new DrugSearchingView(SearchCancelled);
			_SearchingView.Hidden = true;
			
			this.TableView.ContentInset = new UIEdgeInsets(44, 0, 0, 0);
			
			this.View.AddSubview(_SearchingView);
			this.View.AddSubview(_SearchBar);
		}		
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			_SearchingView.Frame = this.View.Bounds;
			SearchButtonClicked(this.SearchPlaceholder);
		}
		
		public void Search(string text)
		{
			IsSearching = true;
			_SearchingView.Hidden = false;
			_SearchBar.ResignFirstResponder();
			
			Console.WriteLine("Searching for {0} Async...", text);			
			
			_Service.getDrugsAsync(text);
			_Service.getDrugsCompleted += Handle_ServicegetDrugsCompleted;
					
			//What an asynchronous call looks like with an anymous delegate:
//			_Service.BegingetDrugs(text, (ar) => {
//				
//				var result = _Service.EndgetDrugs(ar);
//				PopulateResults(result);
//				
//			}, null);
			
			//What a synchronous call looks like:
			
//			var conceptGroup = _Service.getDrugs(text);
//			PopulateResults(conceptGroup);
			
		}
		
		private void Handle_ServicegetDrugsCompleted(object sender, getDrugsCompletedEventArgs args)
		{
			if(args.Result == null)
				return;
			
			PopulateResults(args.Result);
		}
		
		private void PopulateResults(RxConceptGroup[] conceptGroup)
		{
			var section = new Section();
			
			if(conceptGroup.Any()) {
				foreach(var concept in conceptGroup.SelectMany(x=>x.rxConcept)) {
					var element = new DrugSearchResultElement(concept, PushDrugViewController);
					section.Add(element);
				}
			}
			else {
				section.Add(new StringElement("No Results Found."));
			}
				
			Root.Clear();
			Root.Add(section);
			
			this.ReloadData();
			
			HideSearchingView();
		}
		
		public void PushDrugViewController(RxConcept concept)
		{
			var drugViewController = new DrugViewController(concept);
			this.NavigationController.PushViewController(drugViewController, true);
		}
		
		public void SearchCancelled()
		{
			HideSearchingView();
			_Service.getDrugsCompleted -= Handle_ServicegetDrugsCompleted;
		}
		
		public void HideSearchingView()
		{
			using(var pool = new NSAutoreleasePool()) {
				pool.BeginInvokeOnMainThread(() => {
					IsSearching = false;
					_SearchingView.Hidden = true;
				});
			}
		}
		
		public override void ViewWillLayoutSubviews()
		{
			base.ViewWillLayoutSubviews();
		}
		
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			this.NavigationItem.Title = "Search by Ingredient";
		}
		
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
		}
		
		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			return base.ShouldAutorotateToInterfaceOrientation(toInterfaceOrientation);
		}
		
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			
			if(disposing) { 
				_Service.getDrugsCompleted -= Handle_ServicegetDrugsCompleted;
			}
		}
	}
}


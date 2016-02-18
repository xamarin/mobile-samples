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
using MonoTouch.Dialog;
using WebServices.RxNormSample.rxnav.nlm.nih.gov;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace WebServices.RxNormSample
{
	public class DrugViewController : DialogViewController
	{
		RxConcept _Concept;
		
		public DrugViewController(RxConcept concept)
			: base(new RootElement(""), true)
		{
			_Concept = concept;
			this.Title = "Drug Info";
		}
		
		public override void LoadView()
		{
			base.LoadView();
			
			if(RxTermRestClient.IsInitialized == false) {
				RxTermRestClient.Initialize();	
			}
		}
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var section = new Section();
			section.Add(new LoadMoreElement("Failed...Click to Retry.", "Getting Drug Info...", (element) =>  { }) { Animating = true });
			this.Root.Clear();
			this.Root.Add(section);
			
			this.TableView.ReloadData();
		}
		
		public override void ViewWillLayoutSubviews()
		{
			base.ViewWillLayoutSubviews();
		}
		
		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
		}
		
		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			
			//Make Call to web service async here
			RxTermRestClient.GetRxTermAsync(_Concept.RXCUI, FinishedGettingDrugConcept);
		}
		
		
		private void FinishedGettingDrugConcept(RxTerm term)
		{
			var section = new Section();
			
			section.Add(new StringElement("Brand Name: ", term.BrandName));
			section.Add(new StringElement("Display Name: ", term.DisplayName));
			section.Add(new StringElement("Synonym: ", term.Synonym));
			section.Add(new StringElement("Full Name: ", term.FullName));
			section.Add(new StringElement("Full Generic Name: ", term.FullGenericName));
			section.Add(new StringElement("View Image!", () => { PushDrugImageView(term); }));
			
			using(var pool = new NSAutoreleasePool()) {
				pool.BeginInvokeOnMainThread(()=>{
					this.Root.Clear();
					this.Root.Add(section);
					
					this.TableView.ReloadData();
				});
			}
		}
		
		private void PushDrugImageView(RxTerm term)
		{
			var drugImageViewer = new DrugImageViewController(term);
			this.NavigationController.PushViewController(drugImageViewer, true);
			this.NavigationItem.BackBarButtonItem = new UIBarButtonItem("Back", UIBarButtonItemStyle.Done, null);
		}
		
		
		
		
		
		
		
		
		
	}
}


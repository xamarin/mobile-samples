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

namespace WebServices.RxNormSample
{
	public class DrugSearchResultElement : StringElement
	{
		public RxConcept Concept { get; private set; }
		public Action<RxConcept> ConceptSelectedAction { get; private set; }
		
		public DrugSearchResultElement(RxConcept concept, Action<RxConcept> conceptSelectedAction)
			: base(concept.STR)
		{
			this.Concept = concept;
			this.ConceptSelectedAction = conceptSelectedAction;
		}
		
		public override void Selected(DialogViewController dvc, MonoTouch.UIKit.UITableView tableView, MonoTouch.Foundation.NSIndexPath path)
		{
//			base.Selected(dvc, tableView, path);
			
			if(ConceptSelectedAction != null) {
					ConceptSelectedAction(this.Concept);
			}
			
			tableView.DeselectRow(path, true);
		}
		
		public override MonoTouch.UIKit.UITableViewCell GetCell(MonoTouch.UIKit.UITableView tv)
		{
			return base.GetCell(tv);
		}
	}
}


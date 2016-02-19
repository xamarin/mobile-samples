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
using System.Collections.Generic;
using ServiceStack.Text;

namespace WebServices.RxNormSample
{
	
	public class Pill
	{
		public string SplId { get; set; }
		public string RxCUI { get; set; }
		public string RxString { get; set; }
		public string Ingredients { get; set; }
		public string ImageId { get; set; }
		
		
	}
	
	public class Pills : List<Pill> { }
}


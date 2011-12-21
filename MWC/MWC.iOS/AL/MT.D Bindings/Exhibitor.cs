using System;
using MonoTouch.Dialog;

namespace MWC.iOS.AL
{
	public class Exhibitor : MWC.BL.Exhibitor
	{
		[Entry("poo")]
		public override string Name
		{
			get { return base.Name; }
			set { base.Name = value; }
		}
	}
}


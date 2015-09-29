using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace CoinTimeGame.ContentLoading.Animations
{
	[XmlType("AnimationChainArraySave")]
	public class AnimationChainListSave
	{
		#region Fields
		[XmlElementAttribute("AnimationChain")]
		public List<AnimationChainSave> AnimationChains;
		#endregion
	}


}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MWC.SAL.Helpers
{
	public static class HtmlDocHelper
	{
		public static string GetInnerText(HtmlAgilityPack.HtmlDocument doc, string xpath)
		{
			return GetInnerText(doc.DocumentNode, xpath);
		}

		public static string GetAttribute(HtmlAgilityPack.HtmlDocument doc, string xpath, string attributeName)
		{
			return GetAttribute(doc.DocumentNode, xpath, attributeName);
		}

		public static string GetInnerText(HtmlAgilityPack.HtmlNode doc, string xpath)
		{
			string result = string.Empty;

			var node = doc.SelectSingleNode(xpath);
			if(node != null)
			{ result = node.InnerText.Trim(); }

			return result;
		}

		public static string GetAttribute(HtmlAgilityPack.HtmlNode doc, string xpath, string attributeName)
		{
			string result = string.Empty;

			var node = doc.SelectSingleNode(xpath);
			if(node != null)
			{ result = node.Attributes[attributeName].Value; }

			return result;
		}
	}
}

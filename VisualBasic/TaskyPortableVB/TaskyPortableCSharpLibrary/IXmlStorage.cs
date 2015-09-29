using System;
using System.Collections.Generic;

namespace Tasky.Portable
{
	public interface IXmlStorage
	{
		List<Task> ReadXml (string filename);

		void WriteXml (List<Task> tasks, string filename);

	}
}


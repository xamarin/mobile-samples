using System;
using Newtonsoft.Json;

namespace XamarinTodoQuickStart
{
	public class TodoItem
	{
		// Tables created prior to November 2013 should use 'int' type. Check your configuration.
		public string Id { get; set; }

		[JsonProperty(PropertyName = "text")]
		public string Text { get; set; }

		[JsonProperty(PropertyName = "complete")]
		public bool Complete { get; set; }

		[JsonProperty(PropertyName = "deviceToken")]
        public string DeviceToken { get; set; }
	}
}


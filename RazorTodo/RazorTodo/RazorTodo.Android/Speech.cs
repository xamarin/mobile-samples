using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Speech.Tts;
using Java.Lang;

namespace RazorTodo
{
	public class Speech : Object, TextToSpeech.IOnInitListener
	{
		TextToSpeech speaker;
		string toSpeak;
		public Speech ()
		{
		}

		public void Speak (Context context, string text)
		{
			toSpeak = text;
			if (speaker == null)
				speaker = new TextToSpeech (context, this);
			else
			{
				var p = new Dictionary<string,string> ();
				speaker.Speak (toSpeak, QueueMode.Flush, p);
			}
		}


		#region IOnInitListener implementation
		public void OnInit (OperationResult status)
		{
			if (status.Equals (OperationResult.Success)) {
				System.Diagnostics.Debug.WriteLine ("spoke");
				var p = new Dictionary<string,string> ();
				speaker.Speak (toSpeak, QueueMode.Flush, p);
			}
			else
				System.Diagnostics.Debug.WriteLine ("was quiet");
		}
		#endregion
	}
}


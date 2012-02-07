using System.Xml.Serialization;

namespace MWC.BL {
	public partial class SessionSpeaker : Contracts.BusinessEntityBase {
		
		public string SessionKey { get; set; }
		
		public string SpeakerKey { get; set; }
		
		public SessionSpeaker ()
		{
		}

		public SessionSpeaker (string sessionKey, string speakerKey)
		{
			SessionKey = sessionKey;
			SpeakerKey = speakerKey;
		}
	}
}


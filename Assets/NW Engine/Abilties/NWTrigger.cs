using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;

namespace NetherWars
{
	[Flags]
	public enum NWTriggerType
	{
		None,
		EnterZone,
		DrawCard,
		StartOfTurn,
	}


	[XmlRoot("Trigger")]
	public class NWTrigger  {

		#region XML Fields
		
		[XmlElement("TriggertType")]
		public NWTriggerType Type;

		[XmlElement("FromZone")]
		public eZoneType FromZone;

		[XmlElement("ToZone")]
		public eZoneType ToZone;

		[XmlElement("Target")]
		public NWTarget Target;

		#endregion



	}
}

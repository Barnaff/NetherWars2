using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;

namespace NetherWars
{
	[Flags]
	public enum NWCountType
	{
		Fixedvalue,
		AllValidTargets,
		AllMana,
		FreeMana,
		CardsInPlayerHand,
		CardsInOpponentHand,

	}

	[XmlRoot("Count")]
	public class NWCount  {

		#region XML Fields
		
		[XmlElement("Type")]
		public NWCountType Type;

		[XmlElement("Value")]
		public int Value;

		[XmlElement("Target")]
		public NWTarget Target;
		
		#endregion
	}
}

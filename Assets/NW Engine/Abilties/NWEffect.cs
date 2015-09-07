using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System;

namespace NetherWars
{
	[Flags]
	public enum NWEffectType
	{
		None,
		DrawCards,
		Heal,
		PumpHealth,
		PumpAndHeal,
		PumpAttack,
		PumpAttackAndHealth,
		DealDamage,
		
		
	}
	
	[XmlRoot("Effect")]
	public class NWEffect  {
		
		#region XML Fields
		
		[XmlElement("EffectType")]
		public NWEffectType Type;
		
		[XmlElement("Target")]
		public NWTarget Target;
		
		[XmlElement("Count")]
		public NWCount Count;
		
		[XmlElement("InfoText")]
		public string InfoText;
		
		#endregion
	}

}

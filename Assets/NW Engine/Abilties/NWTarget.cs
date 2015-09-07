using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System;

namespace NetherWars
{
	[Flags]
	public enum NWTargetType
	{
		Self,
		AnyCard,
		Opponent,
		Controller,
		AnyEnemy,
		AnyFriendly,
		AnyOtherFriendly,
		
	}
	
	[XmlRoot("Target")]
	public class NWTarget  {
		
		#region XML Fields
		
		[XmlElement("TriggertType")]
		public NWTargetType Type;
		
		#endregion
		
		
		#region Public Helpers
		
		public bool IsCardMatchTarget(NWCard card, NWCard target)
		{
			bool isMatched = false;
			switch (Type)
			{
			case NWTargetType.Self:
			{
				if (card == target)
				{
					isMatched = true;
				}
				break;
			}
			}
			
			return isMatched;
		}
		
		#endregion
		
	}

}

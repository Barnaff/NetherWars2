using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;

namespace NetherWars
{
	public delegate void AbilityActivatedDelegate(NWAbility ability);
	
	[Flags]
	public enum NWAbilityType
	{
		None,
		Triggered,
		Static,
		Activated,
	}
	
	[XmlRoot("Ability")]
	public class NWAbility  {
		
		#region XML Fields
		
		[XmlElement("Type")]
		public NWAbilityType Type;
		
		[XmlElement("Trigger")]
		public NWTrigger Trigger;
		
		[XmlArray("Effects")]
		[XmlArrayItem("Effect")]
		public List<NWEffect> Effects;
		
		#endregion
		
		
		#region Private Properties
		
		private AbilityActivatedDelegate OnActivateAbility;
		private NWCard _parentCard;
		
		#endregion
		
		
		#region Public
		
		public void RegisterAbility(NWCard parentCard, IEventDispatcher eventDispatcher, AbilityActivatedDelegate activatedCallBack)
		{
			_parentCard = parentCard;
			if (Type == NWAbilityType.Triggered)
			{
				switch (Trigger.Type)
				{
				case NWTriggerType.DrawCard:
				{
					break;
				}
				case NWTriggerType.EnterZone:
				{
					eventDispatcher.OnCardChangeZone += CardChangeZoneHandler;
					break;
				}
				case NWTriggerType.StartOfTurn:
				{
					break;
				}
				case NWTriggerType.None:
				default:
				{
					break;
				}
				}
				
				OnActivateAbility += activatedCallBack;
			}
		}
		
		#endregion
		
		
		
		#region Event Handlers
		
		private void CardChangeZoneHandler(NWCard card, NWZone fromZone, NWZone toZone)
		{
			if (Type == NWAbilityType.Triggered)
			{
				switch (Trigger.Type)
				{
				case NWTriggerType.EnterZone:
				{
					if (Trigger.ToZone == toZone.Type && Trigger.Target.IsCardMatchTarget(_parentCard, card))
					{
						ResolveAbilityEvent();
					}
					break;
				}
				default:
				{
					break;
				}
				}
			}
		}
		
		#endregion
		
		
		#region Resolve Abilities
		
		private void ResolveAbilityEvent()
		{
			if (OnActivateAbility != null)
			{
				OnActivateAbility(this);
			}
		}
		
		#endregion
		
		
	}

}

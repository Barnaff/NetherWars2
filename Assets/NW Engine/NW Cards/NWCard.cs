using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System;


namespace NetherWars
{
	[Flags]
	public enum NWCardType
	{
		Creature,
		Spell,
		Legandary,
		Artifact,
	}
	
	[Flags]
	public enum NWColor
	{
		Colorless,
		Black,
		Blue,
		White,
		Green,
		Red,
		Purple,
	}
	
	public delegate void OnAbilityActivatedDelegate(NWCard card, NWAbility ability);
	
	[XmlRoot("Card")]
	public class NWCard  {
		
		#region XML Fields
		
		[XmlElement("CardName")]
		public string CardName;
		
		[XmlElement("CardId")]
		public int CardID;
		
		[XmlElement("CastingCost")]
		public int CastingCost;
		
		[XmlElement("Thrashold")]
		public string Thrashold;
		
		[XmlElement("ImageName")]
		public string ImageName;
		
		[XmlElement("InfoText")]
		public string InfoText;
		
		[XmlArray("CardTypes")]
		[XmlArrayItem("CardType")]
		public List<NWCardType> CardTypes;
		
		[XmlArray("CardColors")]
		[XmlArrayItem("CardColor")]
		public List<NWColor> CardColors;
		
		[XmlElement("Power")]
		public int Power;
		
		[XmlElement("Toughness")]
		public int Toughness;
		
		[XmlArray("ResourceGain")]
		[XmlArrayItem("ResourceColor")]
		public List<NWColor> ResourceGain;
		
		[XmlArray("Abilities")]
		[XmlArrayItem("Ability")]
		public List<NWAbility> Abilities;
		
		#endregion
		
		
		#region Dynamic Public Properties
		
		[XmlIgnore]
		public INWPlayer Controller;
		[XmlIgnore]
		public int CurrentPower;
		[XmlIgnore]
		public int CurrentToughness;
		[XmlIgnore]
		public OnAbilityActivatedDelegate OnAbilityActivated;
		[XmlIgnore]
		private int _cardUniqueID;


		#endregion


		#region Cards Cache

		public static Dictionary<int, NWCard> _cardsCache = new Dictionary<int, NWCard>();

		public static NWCard GetCard(int cardUniqueId)
		{
			if (NWCard._cardsCache.ContainsKey(cardUniqueId))
			{
				return _cardsCache[cardUniqueId];
			}
			return null;
		}

		public int CardUniqueID {
			get {
				return _cardUniqueID;
			}
			set {
				_cardUniqueID = value;
				NWCard._cardsCache.Add(_cardUniqueID, this);
			}
		}


		#endregion
		
		
		#region Public
		
		public void ActivateCard(IEventDispatcher eventDispatcher)
		{
			foreach (NWAbility ability in Abilities)
			{
				ability.RegisterAbility(this, eventDispatcher, OnAbilityActivatedHandler);
			}
		}
		
		public void InitCardForBattlefield()
		{
			CurrentPower = Power;
			CurrentToughness = Toughness;
		}
		
		public void SetController(INWPlayer controller)
		{
			Controller = controller;
		}
		
		#endregion
		
		
		#region Events
		
		private void OnAbilityActivatedHandler(NWAbility ability)
		{
			if (OnAbilityActivated != null)
			{
				OnAbilityActivated(this, ability);
			}
		}
		
		#endregion
		
	}

}


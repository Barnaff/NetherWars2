using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NetherWars
{
	public enum eZoneType
	{
		None,
		Hand,
		Library,
		Battlefield,
		Graveyard,
		ResourcePool,
		Exile,
	}
	
	public class NWZone : IZone
	{
		
		#region Protected Properties
		
		protected eZoneType _zoneType;

		protected int _zoneID;

		#endregion

		
		#region Private Properties
		
		protected List<NWCard> _cardsInZone = new List<NWCard>();

		private static Dictionary<int, NWZone> _cachedZones = new Dictionary<int, NWZone>();

		public static NWZone GetZone(int zoneId)
		{
			if (NWZone._cachedZones.ContainsKey(zoneId))
			{
				return NWZone._cachedZones[zoneId];
			}
			Debug.LogError("Could not find zone: " + zoneId);
			return null;
		}

		#endregion
		
		
		#region Constructors
		
		public NWZone()
		{
			_zoneID = NWZone._cachedZones.Count + 1;
			NWZone._cachedZones.Add(_zoneID, this);
		}
		
		public NWZone(eZoneType zone)
		{
			_zoneType = zone;
		}
		
		public NWZone(eZoneType zone, List<NWCard> cardsInZone)
		{
			_zoneType = zone;
			_cardsInZone = cardsInZone;
		}
		
		#endregion
		
		
		#region Public
		
		public eZoneType Type
		{
			get
			{
				return _zoneType;
			}
		}

		public int ZoneID
		{
			get
			{
				return _zoneID;
			}
			set
			{
				_zoneID = value;
				if (NWZone._cachedZones.ContainsKey(_zoneID))
				{
					Debug.Log("changing id for zone: " + _zoneID);
					NWZone._cachedZones[_zoneID] = this;
				}
				else
				{
					Debug.Log("creating new index for zone: " + _zoneID);
					NWZone._cachedZones.Add(_zoneID, this);
				}
			}
		}
		
		public virtual List<NWCard> Cards
		{
			get
			{
				return _cardsInZone;
			}
		}
		
		public virtual void AddCard(NWCard card)
		{
			if (!_cardsInZone.Contains(card))
			{
				_cardsInZone.Add(card);
				NWEventDispatcher.Instance().DispatchEvent(NWEvent.ZoneUpdated(this));
			}
		}
		
		public virtual void SetCardsList(List<NWCard> cards)
		{
			_cardsInZone = cards;
			string output = "";
			foreach (NWCard card in _cardsInZone)
			{
				output += card.CardUniqueID +",";
			}
			Debug.Log("set curds in zone: " + output);
		}
		
		public virtual void Shuffle()
		{
			System.Security.Cryptography.RNGCryptoServiceProvider provider = new System.Security.Cryptography.RNGCryptoServiceProvider();
			int n = _cardsInZone.Count;
			while (n > 1)
			{
				byte[] box = new byte[1];
				do provider.GetBytes(box);
				while (!(box[0] < n * (System.Byte.MaxValue / n)));
				int k = (box[0] % n);
				n--;
				NWCard value = _cardsInZone[k];
				_cardsInZone[k] = _cardsInZone[n];
				_cardsInZone[n] = value;
			}
		}
		
		public virtual NWCard DrawFromZone()
		{
			// TODO: lose if no more cards
			NWCard card = _cardsInZone[0];
			RemoveCardFromZone(card);
			return card;
		}
		
		public virtual void RemoveCardFromZone(NWCard i_Card)
		{
			this.RemoveCardsFromZone (new List<NWCard>(){i_Card});
		}
		
		public virtual void RemoveCardsFromZone(List<NWCard> i_cards)
		{
			if (i_cards != null)
			{
				foreach (NWCard card in i_cards)
				{
					if (this._cardsInZone.Contains(card))
					{
						this._cardsInZone.Remove(card);
					}
				} 
			}
		}
		
		public virtual NWCard RevealTopCard()
		{
			NWCard result = null;
			List<NWCard> topCard = this.RevealTopCards(1);
			if (topCard != null && topCard.Count > 0)
			{
				result = topCard[0];
			}
			return result;
		}
		
		public virtual List<NWCard> RevealTopCards(int i_NumberOfCardsToReveal)
		{
			List<NWCard> topCards = new List<NWCard>();
			for (int i = 1; i <= i_NumberOfCardsToReveal; i++)
			{
				if (this._cardsInZone.Count >= i)
				{
					topCards.Add(this._cardsInZone[i - 1]);
				}
			}
			return topCards;
		}
		
		#endregion
	}
}


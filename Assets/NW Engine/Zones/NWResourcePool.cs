using UnityEngine;
using System.Collections;

namespace NetherWars
{
	public class NWResourcePool : NWZone, IResourcePool {

		#region Private Properties

		private Hashtable _thrasholdCount;

		private int _totalMana;

		private int _manaUsedThisTurn;

		private int _numberOfResouecesPutThisTurn;

		#endregion

		public NWResourcePool()
		{
			_zoneType = eZoneType.ResourcePool;
			_thrasholdCount = new Hashtable();
		}


		#region NWZone 

		public override void AddCard (NWCard card)
		{
			base.AddCard (card);
			UpdateResourcePool();
			_totalMana++;
			_numberOfResouecesPutThisTurn++;
		}
		 
		public override void RemoveCardsFromZone (System.Collections.Generic.List<NWCard> i_cards)
		{
			base.RemoveCardsFromZone (i_cards);
			UpdateResourcePool();
		}

		#endregion


		#region IResourcePool Implementation

		public void ResetPool()
		{
			_totalMana = _cardsInZone.Count;
			_manaUsedThisTurn = 0;
			_numberOfResouecesPutThisTurn = 0;
		}
		
		public bool CanPayForCard(NWCard card)
		{
			if (_totalMana - _manaUsedThisTurn >= card.CastingCost)
			{
				return true;
			}
			return false;
		}
		
		public void PayForCard(NWCard card)
		{
			_manaUsedThisTurn += card.CastingCost;
			UpdateResourcePool();
		}

		public int ThrasholdForColor(NWColor color)
		{
			if (_thrasholdCount.Contains(color))
			{
				return (int)_thrasholdCount[color];
			}
			return 0;
		}

		public int CurrentMana
		{ 
			get
			{
				return _totalMana - _manaUsedThisTurn;
			}
		}

		public int NumberOfResourcesPutThisTurn
		{
			get
			{
				return _numberOfResouecesPutThisTurn;
			}
		}

		#endregion


		#region Private

		private void UpdateResourcePool()
		{
			_thrasholdCount.Clear();
			foreach (NWCard card in _cardsInZone)
			{
				foreach (NWColor color in card.CardColors)
				{
					if (_thrasholdCount.Contains(color))
					{
						_thrasholdCount[color] = (int)_thrasholdCount[color] + 1;
					}
					else
					{
						_thrasholdCount.Add(color, 1);
					}
				}
			}
		}

		#endregion
	}
}

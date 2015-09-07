using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NetherWars
{
	public interface IZone  {
		
		eZoneType Type { get; }
		
		List<NWCard> Cards { get; }
		
		void AddCard(NWCard card);
		
		void SetCardsList(List<NWCard> cards);
		
		void Shuffle();
		
		NWCard DrawFromZone();
		
		
		void RemoveCardFromZone(NWCard i_Card);
		
		void RemoveCardsFromZone(List<NWCard> i_cards);
		
		NWCard RevealTopCard();
		
		List<NWCard> RevealTopCards(int i_NumberOfCardsToReveal);
		
	}

}

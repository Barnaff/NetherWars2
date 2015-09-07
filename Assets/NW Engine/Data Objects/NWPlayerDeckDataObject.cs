using UnityEngine;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NetherWars
{
	public enum ePlayerDeckDataObjectKeys
	{
		PlayerId,
		DeckCards,
	}

	public class NWPlayerDeckDataObject : NWDataObjectAbstract {

		#region implemented abstract members of NWDataObjectAbstract
		
		public override Hashtable Encode ()
		{
			Hashtable hash = new Hashtable();
			Hashtable deckData = new Hashtable();
			foreach (NWCardDataObject card in Cards)
			{
				deckData.Add(card.CardUniqueId, card.CardId);
			}
			hash.Add(ePlayerDeckDataObjectKeys.DeckCards, deckData);
			hash.Add(ePlayerDeckDataObjectKeys.PlayerId, PlayerId);
			return hash;
		}
		
		public override void Decode (Hashtable data)
		{
			Cards = new List<NWCardDataObject>();
			PlayerId = int.Parse(data[ePlayerDeckDataObjectKeys.PlayerId].ToString());
			Hashtable deckData = (Hashtable)data[ePlayerDeckDataObjectKeys.DeckCards];
			foreach (int cardKey in deckData.Keys)
			{
				NWCardDataObject card = new NWCardDataObject(cardKey, int.Parse(deckData[cardKey].ToString()));
				Cards.Add(card);
			}
		}
		
		#endregion

		public int PlayerId;

		public List<NWCardDataObject> Cards; 
		
		public NWPlayerDeckDataObject(int playerId, List<NWCardDataObject> cards)
		{
			PlayerId = playerId;
			Cards = cards;
		}

		public NWPlayerDeckDataObject(int playerId, Hashtable deckData)
		{
			PlayerId = playerId;
			Cards = new List<NWCardDataObject>();
			foreach (int cardKey in deckData.Keys)
			{
				Debug.Log("cardKey: " + cardKey);
				NWCardDataObject card = new NWCardDataObject(cardKey, int.Parse(deckData[cardKey].ToString()));
				Cards.Add(card);
			}
		}
		
		public override string ToString ()
		{
			string output = "";
			foreach (NWCardDataObject card in Cards)
			{
				output += card.ToString() + ",";
			}
			return "{ PlayerId: " + PlayerId + ":" + output;
		}

		public void ShuffleDeck()
		{
			for (int i = Cards.Count - 1; i > 0; i--)
			{
				int randomIndex = Random.Range(0,i);
				NWCardDataObject tmp = Cards[i];
				Cards[i] = Cards[randomIndex];
				Cards[randomIndex] = tmp;
			}
		}

		public int[] GetDeckIndexes()
		{
			int[] deckIndexes = new int[Cards.Count];
			for (int i=0; i< Cards.Count; i++)
			{
				deckIndexes[i] = Cards[i].CardUniqueId;
			}
			return deckIndexes;
		}
		
	}
}


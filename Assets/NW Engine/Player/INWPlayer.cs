using System.Collections;
using System.Collections.Generic;

namespace NetherWars
{
	public enum NWPlayerFields
	{
		PlayerID, 
		PlayerName, 
		DeckCards, 
	}

	public interface INWPlayer 
	{

		int PlayerID { get; set; }
		
		string PlayerName {get; set; }
		
		int[] DeckCards { get; set; }

		void SetupPlayerDeck(List<NWCard> deckCards);

		void DrawCards(int amount);

		// zones

		NWLibrary Library { get; }

		NWBattlefield Battlefield { get; }
		 
		NWHand Hand { get; }

		NWResourcePool ResourcePool { get; }

	}


}


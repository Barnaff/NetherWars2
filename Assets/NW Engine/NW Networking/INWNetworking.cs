using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NetherWars
{
	public delegate void RecivedActionDelegate(NWServerAction serverAction);

	public delegate void JoinedGameDelegate();

	public delegate void OtherPlayerJoinedDelegate(INWPlayer player);

	public delegate void CardsDataUpdated(Hashtable cardsData);

	public interface INWNetworking  {
		
		event RecivedActionDelegate OnReciveAction;

		event JoinedGameDelegate OnJoinedGame;

		event OtherPlayerJoinedDelegate OnOtherPlayerJoined;

		event CardsDataUpdated OnCardsDataUpdated;
		
		bool IsServer { get; }

		//void SendGameplayEvent(

		void SendAction(NWServerAction serverAction);
		
		void SetPlayer(INWPlayer player);

		int NumberOfPlayersInRoom { get; }

		INWPlayer[] PlayersInRoom { get; }

		void SetPlayersCards(Hashtable cardsData);
	}
}


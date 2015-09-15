using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NetherWars
{
	[System.Serializable]
	public class NWPlayer : NetherWars.INWPlayer, NetherWars.INWNetworkObject 
	{

		[SerializeField]
		private int _playerId;

		[SerializeField]
		private string _playerName;

		[SerializeField]
		private int[] _deckCards;

		[SerializeField]
		private NWLibrary _library;

		[SerializeField]
		private NWHand _hand;

		[SerializeField]
		private NWBattlefield _battlefield;

		private static Dictionary<int, NWPlayer> _cachedPlayers = new Dictionary<int, NWPlayer>();

		public static NWPlayer GetPlayer(int playerId)
		{
			if (NWPlayer._cachedPlayers.ContainsKey(playerId))
			{
				return NWPlayer._cachedPlayers[playerId];
			}
			return null;
		}

		#region Initialize

		public NWPlayer()
		{
			_library = new NWLibrary();
			_hand = new NWHand();
			_battlefield = new NWBattlefield();


		}

		#endregion


		#region INWNetworkObject implementation
		
		public ExitGames.Client.Photon.Hashtable ToHash 
		{
			get
			{
				ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
				hash.Add(NetherWars.NWPlayerFields.PlayerID.ToString(), _playerId);
				hash.Add(NetherWars.NWPlayerFields.PlayerName.ToString(), _playerName);
				hash.Add(NetherWars.NWPlayerFields.DeckCards.ToString(), _deckCards);
				return hash;
			}
		}
		
		public void PopulateWithData(ExitGames.Client.Photon.Hashtable data)
		{
			_playerId = int.Parse(data[NetherWars.NWPlayerFields.PlayerID.ToString()].ToString());
			_playerName = data[NetherWars.NWPlayerFields.PlayerName.ToString()].ToString();
			_deckCards = (int[])data[NetherWars.NWPlayerFields.DeckCards.ToString()];

			if (NWPlayer._cachedPlayers.ContainsKey(_playerId))
			{
				NWPlayer._cachedPlayers[_playerId] = this;
			}
			else
			{
				NWPlayer._cachedPlayers.Add(_playerId, this);
			}
		}
		
		#endregion
		
		
		#region INWPlayer implementation
		
		public int PlayerID
		{
			get {
				return _playerId;
			}
			set {
				_playerId = value;
				if (!NWPlayer._cachedPlayers.ContainsKey(_playerId))
				{
					NWPlayer._cachedPlayers.Add(_playerId, this);
				}
			}
		}
		
		public string PlayerName
		{
			get {
				return _playerName;
			}
			set {
				_playerName = value;
			}
		}
		
		public int[] DeckCards
		{
			get {
				return _deckCards;
			}
			set {
				_deckCards = value;
			}
		}
		
		public NetherWars.NWLibrary Library {
			get {
				return _library;
			}
		}
		
		public NetherWars.NWBattlefield Battlefield {
			get {
				return _battlefield;
			}

		}
		
		public NetherWars.NWHand Hand {
			get {
				return _hand;
			}
			set
			{
				_hand = value;
			}
		}

		public void SetupPlayerDeck(List<NWCard> deckCards)
		{
			_library.SetCardsList(deckCards);
		}

		public void DrawCards(int amount)
		{
			for (int i=0; i< amount; i++)
			{
				NWCard card = _library.DrawFromZone();
				Hand.AddCard(card);
				NWEventDispatcher.Instance().DispatchEvent(NWEvent.CardChangeZone(card, _library, _hand));
			}

		}

		#endregion
	}
}




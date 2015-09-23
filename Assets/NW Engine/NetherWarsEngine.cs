using System.Collections;
using UnityEngine;
using System.Collections.Generic;

using Hashtable = ExitGames.Client.Photon.Hashtable;



namespace NetherWars
{
	public delegate void CardCreatedDelegate(NWCard card);

	public delegate void GameInitializedDelegate(List<INWPlayer> players);

	public class NetherWarsEngine  {

		private enum eInitPhaseType
		{
			Init, 
			LoadCards, 
			SetupCardsDictionary,
			ShufflePlayersDecks,
			SetupPlayersDecks, 
		}
		private eInitPhaseType _initPhaseType = eInitPhaseType.Init;

		private INWNetworking _networkManager;

		private NWEventDispatcher _eventDispatcher;

		private int _turnCount = 0;

		private INWPlayer _player;

		private INWPlayer _currentPlayer;

		private NWGameSetupDataObject _cardsDictionary;

		private List<INWPlayer> _players;

		// events
		public event CardCreatedDelegate OnCardCreated;

		public event GameInitializedDelegate OnGameInitialized;

		#region Initialization

		public NetherWarsEngine(INWNetworking networkManager, INWPlayer player)
		{
		
			_eventDispatcher = NWEventDispatcher.Instance();
			_eventDispatcher.OnDispatchEvent += HandleOnDispatchEvent;
			_eventDispatcher.OnEndTurn += HandleOnEndTurn;
			_eventDispatcher.OnStartTurn += HandleOnStartTurn;
			_eventDispatcher.OnPlayCard += HandleOnPlayCard;
			_eventDispatcher.OnPutCardInResources += HandleOnPutCardInResources;
			_networkManager = networkManager;
			networkManager.OnJoinedGame += OnJoinedGameHandler;
			networkManager.OnOtherPlayerJoined += OnOtherPlayerJoinedHandler;
			networkManager.OnCardsDataUpdated += OnCardsDataUpdatedHandler;
			networkManager.OnReciveAction += OnReciveActionHandler;
			_player = player;
		}



		#endregion


		#region Public

		public bool CanPlayCard(INWPlayer player, NWCard card)
		{
			if (card != null && player.ResourcePool.CanPayForCard(card))
			{
				return true;
			}
			return false;
		}

		public void PlayCard(INWPlayer player, NWCard card)
		{
			//_player.ResourcePool.PayForCard(card);
			//_eventDispatcher.DispatchEvent(NWEvent.CardChangeZone(card, player.Hand, player.Battlefield));
			_eventDispatcher.DispatchEvent(NWEvent.PlayCard((NWPlayer)player, card));
		}

		public bool CanPutCardInResources(INWPlayer player, NWCard card)
		{
			if (player.ResourcePool.NumberOfResourcesPutThisTurn < 1)
			{
				return true;
			}
			return false;
		}

		public void PutCardInResources(INWPlayer player, NWCard card)
		{
			//_eventDispatcher.DispatchEvent(NWEvent.CardChangeZone(card, player.Hand, player.ResourcePool));

			_eventDispatcher.DispatchEvent(NWEvent.PutCardInResources((NWPlayer)player, card));
		}

		public void EndPlayerTurn(INWPlayer player)
		{
			_eventDispatcher.DispatchEvent(NWEvent.EndTurn(player));
		}


		#endregion


		#region Private

		private List<NWCard> LoadCards(int[] cardsArray)
		{
			return NWCardsLoader.LoadCardList(cardsArray);
		}

		private void StartGame()
		{
			if (_initPhaseType == eInitPhaseType.Init)
			{
				Debug.Log("Start Game");
				
				// set players decks
				SetPlayersDecksData();
			}
		}

		private void SetPlayersDecksData()
		{
			_initPhaseType = eInitPhaseType.SetupCardsDictionary;
			_players = new List<INWPlayer>();
			List<NWPlayerDeckDataObject> playersDecks = new List<NWPlayerDeckDataObject>();
			List<NWZoneDataObject> playersZones = new List<NWZoneDataObject>();
			INWPlayer[] players = _networkManager.PlayersInRoom;
			for (int i=0 ; i< players.Length; i++)
			{
				INWPlayer player = players[i];
				_players.Add(player);
				List<NWCard> deckCards = LoadCards(player.DeckCards);
				List<NWCardDataObject> cardsList = new List<NWCardDataObject>();
				foreach (NWCard card in deckCards)
				{
					NWCardDataObject cardData = new NWCardDataObject(card.CardUniqueID, card.CardID);
					cardsList.Add(cardData);
					card.Controller = player;
					if (OnCardCreated != null)
					{
						OnCardCreated(card);
					}
				}

				NWPlayerDeckDataObject playerDeck = new NWPlayerDeckDataObject(player.PlayerID, cardsList);
				playersDecks.Add(playerDeck);

				NWZoneDataObject libraryZone = new NWZoneDataObject(player.PlayerID, player.Library.Type, player.Library.ZoneID);
				NWZoneDataObject handZone = new NWZoneDataObject(player.PlayerID, player.Hand.Type, player.Hand.ZoneID);
				NWZoneDataObject battlefieldZone = new NWZoneDataObject(player.PlayerID, player.Battlefield.Type, player.Battlefield.ZoneID);
				NWZoneDataObject resourcePoolZone = new NWZoneDataObject(player.PlayerID, player.ResourcePool.Type, player.ResourcePool.ZoneID);
				playersZones.Add(libraryZone);
				playersZones.Add(handZone);
				playersZones.Add(battlefieldZone);
				playersZones.Add(resourcePoolZone);
			}

			NWGameSetupDataObject cardsDictionary = new NWGameSetupDataObject(playersDecks, playersZones);

			_initPhaseType = eInitPhaseType.SetupPlayersDecks;
			Debug.Log("cardsDictionary: " + cardsDictionary.ToString());
			_networkManager.SetPlayersCards(cardsDictionary.Encode());


		}

		private void SetPlayersDecks(Hashtable decksData)
		{
			foreach (int playerIdKey in decksData.Keys)
			{
				foreach (NWPlayer player in _players)
				{
					if (player.PlayerID == playerIdKey)
					{
						List<NWCard> deckCards = new List<NWCard>();
						int[] cardsIndexes = (int[])decksData[playerIdKey];
						for (int i=0; i< cardsIndexes.Length ; i++)
						{
							NWCard card = NWCard.GetCard(cardsIndexes[i]);
							deckCards.Add(card);
						}
						player.SetupPlayerDeck(deckCards);
					}
				}
			}
		}

		private INWPlayer OpponentOfPlayer(INWPlayer player)
		{
			foreach (INWPlayer opponent in _players)
			{
				if (opponent != player)
				{
					return opponent;
				}
			}
			return null;
		}

		private INWPlayer PlayerForPlayerId(int playerId)
		{
			foreach (INWPlayer player in _players)
			{
				if (player.PlayerID == playerId)
				{
					return player;
				}
			}
			return null;
		}

		#endregion


		#region Game phases - Server

		private void StartGameplay()
		{
			_eventDispatcher.DispatchEvent(NWEvent.StartGame());
			// check if its the first turn - draw hands for the players
			if (_turnCount == 0)
			{
				StartTurn();

				foreach (INWPlayer player in _players)
				{
					player.DrawCards(7);
				}
			}
		}

		private void StartTurn(INWPlayer playerTurn = null)
		{
			if (_networkManager.IsServer)
			{
				if (playerTurn != null)
				{

				}
				else
				{
					if (_turnCount == 0)
					{
						playerTurn = _players[Random.Range(0,_players.Count)];
					}
				}
				_eventDispatcher.DispatchEvent(NWEvent.StartTurn(playerTurn));
			}
		}



		#endregion


		#region Event Handlers

		private void OnJoinedGameHandler()
		{
			if (PhotonNetwork.room.playerCount == 2 && _networkManager.IsServer)
			{
				StartGame();
			}
		}

		void OnOtherPlayerJoinedHandler (INWPlayer player)
		{
			if (PhotonNetwork.room.playerCount == 2 && _networkManager.IsServer)
			{
				StartGame();
			}
		}

		void OnCardsDataUpdatedHandler (Hashtable cardsData)
		{
				
			if (cardsData != null)
			{
				_cardsDictionary = new NWGameSetupDataObject();
				_cardsDictionary.Decode(cardsData);


			}

			if (_networkManager.IsServer)
			{
				if (_initPhaseType == eInitPhaseType.SetupPlayersDecks)
				{
					// shuffle the decks and post it


					Hashtable shuffledDecksData = _cardsDictionary.GetShuffledDecksIndexes();

					NWServerAction serverAction = new NWServerAction(ServerActionType.SetShuffledDecksData, shuffledDecksData);

					_networkManager.SendAction(serverAction);
				}
			}
			else
			{
				_players = new List<INWPlayer>();
				INWPlayer[] players = _networkManager.PlayersInRoom;
				for (int i=0 ; i< players.Length; i++)
				{
					INWPlayer player = players[i];
					_players.Add(player);
				}

				foreach (NWPlayerDeckDataObject deck in _cardsDictionary.PlayersDecks)
				{
					INWPlayer player = PlayerForPlayerId(deck.PlayerId);
					foreach (NWCardDataObject cardData in deck.Cards)
					{
						NWCard card = NWCardsLoader.LoadCard(cardData.CardId, cardData.CardUniqueId);
						card.Controller = player;
						if (OnCardCreated != null)
						{
							OnCardCreated(card);
						}
					}
				}
			}

			if (cardsData != null)
			{
				foreach (NWZoneDataObject zoneData in _cardsDictionary.PlayersZones)
				{
					NWPlayer player = NWPlayer.GetPlayer(zoneData.PlayerId);
					Debug.Log("set players zone: " + zoneData.ZoneType + " player: " + zoneData.PlayerId + " zoneId: " + zoneData.ZoneId);
					if (player != null)
					{
						if (zoneData.ZoneType == eZoneType.Library)
						{
							player.Library.ZoneID = zoneData.ZoneId;
						}
						if (zoneData.ZoneType == eZoneType.Hand)
						{
							player.Hand.ZoneID = zoneData.ZoneId;
						}
						if (zoneData.ZoneType == eZoneType.Battlefield)
						{
							player.Battlefield.ZoneID = zoneData.ZoneId;
						}
						if (zoneData.ZoneType == eZoneType.ResourcePool)
						{
							player.ResourcePool.ZoneID = zoneData.ZoneId;
						}
					}
					else
					{
						Debug.LogError("cant find player " + zoneData.PlayerId);
					}
				}
			}
		}

		private void OnReciveActionHandler(NWServerAction serverAction)
		{
			switch (serverAction.ActionType)
			{
			case ServerActionType.SetShuffledDecksData:
			{
				SetPlayersDecks(serverAction.ActionsParams);
				if (_networkManager.IsServer)
				{
					StartGameplay();
				}
				break;
			}
			case ServerActionType.GameplayEvent:
			{
				NWEvent gameplayEvent = new NWEvent(serverAction);
				HandleGameplayEvent(gameplayEvent);
				_eventDispatcher.ProcessEvent(gameplayEvent);
				break;
			}
			default:
			{
				Debug.LogError("ERROR - Unsupported action: " + serverAction.ActionType);
				break;
			}
			}
		}

		private void HandleGameplayEvent(NWEvent gameplayEvent)
		{
			switch (gameplayEvent.Type)
			{
			case NWEventType.StartGame:
			{
				if (OnGameInitialized != null)
				{
					OnGameInitialized(_players);
				}
//				if (_networkManager.IsServer)
//				{
//					StartTurn();
//				}
				break;
			}
			case NWEventType.StartTurn:
			{
				break;
			}
			case NWEventType.EndTurn:
			{
				break;
			}
			default:
			{
				break;
			}
			}
		}

		
		void HandleOnDispatchEvent (NWEvent eventObject)
		{
			_networkManager.SendAction(eventObject.ToServerAction());
		}

		void HandleOnStartTurn (INWPlayer player)
		{
			_turnCount++;
			if (_networkManager.IsServer)
			{
				player.DrawCards(1);
			}
		}

		void HandleOnEndTurn (INWPlayer player)
		{
			if (_networkManager.IsServer)
			{
				INWPlayer opponent = OpponentOfPlayer(player);
				StartTurn(opponent);
			}
		}

		void HandleOnPutCardInResources (INWPlayer player, NWCard card)
		{
			if (_networkManager.IsServer)
			{
				_eventDispatcher.DispatchEvent(NWEvent.CardChangeZone(card, player.Hand, player.ResourcePool));
			}
		}
		
		void HandleOnPlayCard (INWPlayer player, NWCard card)
		{
			if (_networkManager.IsServer)
			{
 				_eventDispatcher.DispatchEvent(NWEvent.PayForCard((NWPlayer)player, card));
				_eventDispatcher.DispatchEvent(NWEvent.CardChangeZone(card, player.Hand, player.Battlefield));
			}
		}

		#endregion


	}

}

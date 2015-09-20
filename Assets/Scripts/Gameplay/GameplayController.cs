using UnityEngine;
using System.Collections;
using NetherWars;
using System.Collections.Generic;

public class GameplayController : MonoBehaviour {

	[SerializeField]
	private CardController _cardControllerPrefab;

	private NetherWarsEngine _netherWarsEngine;

	[SerializeField]
	private PlayerController _player1Controller;

	[SerializeField]
	private PlayerController _player2Controller;

	private Dictionary<NWCard, CardController> _cardsInGame = new Dictionary<NWCard, CardController>();

	private Dictionary<NWZone, ZoneControllerAbstract> _zonesIngames = new Dictionary<NWZone, ZoneControllerAbstract>();

	[SerializeField]
	private NWPlayer _currentPlayer;

	// Use this for initialization
	void Start () {
	
		StartCoroutine(StartGame());

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator StartGame()
	{
		while (!PhotonNetwork.connected)
		{
			yield return null;
		}

		// init the network manager
		INWNetworking networkManager = this.gameObject.GetComponent<INWNetworking>();
		Debug.Log("connected");
		
		// create the local player
		_currentPlayer = new NWPlayer();
		_currentPlayer.PlayerName = "Test Player";
		_currentPlayer.PlayerID = (int)Random.Range(1,1000);
		_currentPlayer.DeckCards = new int[30]{1,1,1,2,2,2,3,3,3,4,1,1,1,2,2,2,3,3,3,4,1,1,1,2,2,2,3,3,3,4};

		// set the player in the server
		networkManager.SetPlayer(_currentPlayer);

		// start the game engine
		_netherWarsEngine = new NetherWarsEngine(networkManager, _currentPlayer);

		_netherWarsEngine.OnCardCreated += HandleOnCardCreated;
		_netherWarsEngine.OnGameInitialized += HandleOnGameInitialized;
		NWEventDispatcher.Instance().OnCardChangeZone += HandleOnCardChangeZone;
		NWEventDispatcher.Instance().OnStartTurn += HandleOnStartTurn;

	}

	#region Events

	private bool HandleOnCanPlayCard(PlayerController playerController, CardController card)
	{
		return _netherWarsEngine.CanPlayCard(null, null);
	}

	private void HandlePlayCard(PlayerController playerController, CardController cardController)
	{
		_netherWarsEngine.PlayCard(playerController.Player, cardController.Card);
	}

	private void HandleOnPutCardInResource(PlayerController playerController, CardController cardController)
	{
		_netherWarsEngine.PutCardInResources(playerController.Player, cardController.Card);
	}

	private void HandleEndTurn(PlayerController player)
	{
		_netherWarsEngine.EndPlayerTurn(player.Player);
	}
	

	void HandleOnGameInitialized (List<INWPlayer> players)
	{

		if (players[0].PlayerID > players[1].PlayerID)
		{
			_player1Controller.SetPlayer(players[0]);
			_player2Controller.SetPlayer(players[1]);
		}
		else
		{
			_player2Controller.SetPlayer(players[0]);
			_player1Controller.SetPlayer(players[1]);
		}

		for (int i=0 ; i< players.Count; i++)
		{
			INWPlayer player = players[i];
			PlayerController playerController = PlayerControllerForPlayer(player);

			playerController.SetPlayer(player);
			playerController.SetActivePlayer(false);
			if (player.PlayerID == _currentPlayer.PlayerID)
			{
				_currentPlayer = (NWPlayer)player;
				playerController.SetActivePlayer(true);
				playerController.OnCanPlayCard += HandleOnCanPlayCard;
				playerController.OnPlayCard += HandlePlayCard;
				playerController.OnPutCardInResource += HandleOnPutCardInResource;
				playerController.OnEndTurn += HandleEndTurn;
			}

			foreach (NWCard cardData in player.Library.Cards)
			{
				if (_cardsInGame.ContainsKey(cardData))
				{
					CardController card = _cardsInGame[cardData];
					playerController.AddCard(card);
				}
				else
				{
					Debug.LogError("Could not get card: " + cardData.CardUniqueID + " for player: " + player.PlayerID);
				}
			}
		}

		ZoneControllerAbstract[] zones = FindObjectsOfType<ZoneControllerAbstract>();
		for (int i=0 ; i < zones.Length; i++)
		{
			if (zones[i].Zone != null)
			{
				_zonesIngames.Add(zones[i].Zone, zones[i]);
				Debug.Log("added zone: " + zones[i].Zone.Type + " id: " + zones[i].Zone.ZoneID);
			}
		}
	}


	void HandleOnCardCreated (NWCard card)
	{
		CardController newCardController = Instantiate(_cardControllerPrefab) as CardController;
		newCardController.SetCard(card);
		_cardsInGame.Add(card, newCardController);

	}
	
	void HandleOnCardChangeZone (NWCard card, NWZone fromZone, NWZone toZone)
	{
		ZoneControllerAbstract fromZoneController = _zonesIngames[fromZone];
		ZoneControllerAbstract toZoneController = _zonesIngames[toZone];
		CardController cardController = _cardsInGame[card];

		if (fromZoneController != null && toZoneController != null && card != null)
		{
			fromZoneController.RemoveCardFromZone(cardController);
			toZoneController.AddCardToZone(cardController);
		}
	
	}

	void HandleOnStartTurn (INWPlayer player)
	{
		Debug.Log("start turn for player: " + player.PlayerID);
		if (player == _player1Controller.Player)
		{
			_player2Controller.EndPlayerTurn();
			_player1Controller.StartPlayerTurn();
		}
		else if (player == _player2Controller.Player)
		{
			_player1Controller.EndPlayerTurn();
			_player2Controller.StartPlayerTurn();
		}
	}


	#endregion


	#region Utils

	private PlayerController PlayerControllerForPlayer(INWPlayer player)
	{
		if (_player1Controller.Player == player)
		{
			return _player1Controller;
		}

		if (_player2Controller.Player == player)
		{
			return _player2Controller;
		}
		return null;
	}

	#endregion

}

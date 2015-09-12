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

	private INWPlayer _currentPlayer;

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
		_currentPlayer.DeckCards = new int[10]{1,1,1,2,2,2,3,3,3,4};

		// set the player in the server
		networkManager.SetPlayer(_currentPlayer);

		// start the game engine
		_netherWarsEngine = new NetherWarsEngine(networkManager, _currentPlayer);

		_netherWarsEngine.OnCardCreated += HandleOnCardCreated;
		_netherWarsEngine.OnGameInitialized += HandleOnGameInitialized;
		NWEventDispatcher.Instance().OnCardChangeZone += HandleOnCardChangeZone;
		NWEventDispatcher.Instance().OnStartTurn += HandleOnStartTurn;
		_player1Controller.SetPlayer(_currentPlayer);

		_player1Controller.OnCanPlayCard += HandleOnCanPlayCard;
		_player1Controller.OnPlayCard += HandlePlayCard;
	}

	#region Events

	private bool HandleOnCanPlayCard(PlayerController playerController, CardController card)
	{
		return _netherWarsEngine.CanPlayCard(null, null);
	}

	private void HandlePlayCard(PlayerController playerController, CardController card)
	{
		_netherWarsEngine.PlayCard(null, null);
	}

	void HandleOnGameInitialized (List<INWPlayer> players)
	{
		for (int i=0 ; i< players.Count; i++)
		{
			INWPlayer player = players[i];
			PlayerController playerController = null;
			if (i == 0)
			{
				playerController = _player1Controller;
			}
			else
			{
				playerController = _player2Controller;
			}

			playerController.SetPlayer(player);
			playerController.SetActivePlayer(false);
			if (player.PlayerID == _currentPlayer.PlayerID)
			{
				_currentPlayer = player;
				playerController.SetActivePlayer(true);
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
		
	}


	#endregion

}

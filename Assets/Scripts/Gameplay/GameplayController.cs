using UnityEngine;
using System.Collections;
using NetherWars;

public class GameplayController : MonoBehaviour {

	[SerializeField]
	private CardController _cardControllerPrefab;

	private NetherWarsEngine _netherWarsEngine;

	[SerializeField]
	private PlayerController _player1Controller;

	[SerializeField]
	private PlayerController _player2Controller;


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
		NWEventDispatcher.Instance().OnCardChangeZone += HandleOnCardChangeZone;
		NWEventDispatcher.Instance().OnStartTurn += HandleOnStartTurn;
		_player1Controller.SetPlayer(_currentPlayer);
	}





	#region Events

	void HandleOnCardCreated (NWCard card)
	{
		CardController newCardController = Instantiate(_cardControllerPrefab) as CardController;
		newCardController.SetCard(card);


		_player1Controller.AddCard(newCardController);

	}
	
	void HandleOnCardChangeZone (NWCard card, NWZone fromZone, NWZone toZone)
	{
		
	}

	void HandleOnStartTurn (INWPlayer player)
	{
		
	}


	#endregion
}

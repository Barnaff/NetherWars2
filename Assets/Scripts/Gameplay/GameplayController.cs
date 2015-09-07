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
		INWPlayer player = new NWPlayer();
		player.PlayerName = "Test Player";
		player.PlayerID = (int)Random.Range(1,1000);
		player.DeckCards = new int[10]{1,1,1,2,2,2,3,3,3,4};

		// set the player in the server
		networkManager.SetPlayer(player);

		// start the game engine
		_netherWarsEngine = new NetherWarsEngine(networkManager, player);

		_netherWarsEngine.OnCardCreated += HandleOnCardCreated;

		_player1Controller.SetPlayer(player);
	}

	#region Events

	void HandleOnCardCreated (NWCard card)
	{
		CardController newCardController = Instantiate(_cardControllerPrefab) as CardController;
		newCardController.SetCard(card);

		_player1Controller.AddCard(newCardController);

	}

	#endregion
}

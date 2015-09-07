using UnityEngine;
using System.Collections;
using NetherWars;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : Photon.MonoBehaviour, NetherWars.INWNetworking
{

	/// <summary>Connect automatically? If false you can set this to true later on or call ConnectUsingSettings in your own scripts.</summary>
	public bool AutoConnect = true;
	
	public byte Version = 1;
	
	/// <summary>if we don't want to connect in Start(), we have to "remember" if we called ConnectUsingSettings()</summary>
	private bool ConnectInUpdate = true;


	private List<NWServerAction> _queuedActions = new List<NWServerAction>();

	public virtual void Start()
	{
		PhotonNetwork.autoJoinLobby = false;    // we join randomly. always. no need to join a lobby to get the list of rooms.
		PhotonNetwork.OnEventCall += this.OnEventHandler;
	}
	
	public virtual void Update()
	{
		if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
		{
			Debug.Log("Update() was called by Unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings();");
			
			ConnectInUpdate = false;
			PhotonNetwork.ConnectUsingSettings(Version + "."+Application.loadedLevel);
		}
	}
	
	// to react to events "connected" and (expected) error "failed to join random room", we implement some methods. PhotonNetworkingMessage lists all available methods!
	
	public virtual void OnConnectedToMaster()
	{
		if (PhotonNetwork.networkingPeer.AvailableRegions != null) Debug.LogWarning("List of available regions counts " + PhotonNetwork.networkingPeer.AvailableRegions.Count + ". First: " + PhotonNetwork.networkingPeer.AvailableRegions[0] + " \t Current Region: " + PhotonNetwork.networkingPeer.CloudRegion);
		Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
		PhotonNetwork.JoinRandomRoom();
	}
	
	public virtual void OnPhotonRandomJoinFailed()
	{
		Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. " +
			"Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
		PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = 2 }, null);
	}
	
	// the following methods are implemented to give you some context. re-implement them as needed.
	
	public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.LogError("Cause: " + cause);
	}
	
	public void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room. From here on, your game would be running." +
			"For reference, all callbacks are listed in enum: PhotonNetworkingMessage");
		if (OnJoinedGame != null)
		{
			OnJoinedGame();
		}
	}
	
	public void OnJoinedLobby()
	{
		Debug.Log("OnJoinedLobby(). Use a GUI to show existing rooms available in PhotonNetwork.GetRoomList().");
	}

	public virtual void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
	{
		Debug.Log("OnPhotonCustomRoomPropertiesChanged() : " + Utils.HashToString(propertiesThatChanged));

		if (OnCardsDataUpdated != null)
		{
			OnCardsDataUpdated(propertiesThatChanged);
		}
	}

	public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		if (OnOtherPlayerJoined != null)
		{
			NWPlayer otherPlayer = new NWPlayer();
			otherPlayer.PopulateWithData(newPlayer.customProperties);
			OnOtherPlayerJoined(otherPlayer);
		}
	}


	void OnEventHandler (byte eventCode, object content, int senderId)
	{
		Debug.Log("got event: " + eventCode + " content: " + content + " senderId: " + senderId);

		if (OnReciveAction != null)
		{
			Hashtable[] actionsList = (Hashtable[])content;

			for (int i=0; i < actionsList.Length; i++)
			{
				Hashtable actionHash = (Hashtable)actionsList[i];
				NWServerAction serverAction = new NWServerAction(actionHash);
				OnReciveAction(serverAction);
			}
		}
	}


	#region INWNetworking implementation

	public event RecivedActionDelegate OnReciveAction;

	public event JoinedGameDelegate OnJoinedGame;

	public event CardsDataUpdated OnCardsDataUpdated;

	public event OtherPlayerJoinedDelegate OnOtherPlayerJoined;


	void INWNetworking.SendAction (NWServerAction serverAction)
	{

		_queuedActions.Add(serverAction);
	}

	void INWNetworking.SetPlayer (INWPlayer player)
	{
		ExitGames.Client.Photon.Hashtable playerData = ((INWNetworkObject)player).ToHash;
		PhotonNetwork.SetPlayerCustomProperties(playerData);
	}

	bool INWNetworking.IsServer {
		get {
			return PhotonNetwork.isMasterClient;
		}
	}

	public int NumberOfPlayersInRoom {
		get {
			return PhotonNetwork.room.playerCount;
		}
	}
	public INWPlayer[] PlayersInRoom {
		get {
			PhotonPlayer[] playersInRoom = PhotonNetwork.playerList;
			INWPlayer[] playersList = new INWPlayer[playersInRoom.Length];
			for (int i=0; i< playersInRoom.Length; i++)
			{
				NWPlayer player = new NWPlayer();
				player.PopulateWithData(playersInRoom[i].customProperties);
				playersList[i] = player;
			}

			return playersList;
		}
	}

	public void SetPlayersCards (ExitGames.Client.Photon.Hashtable cardsData)
	{
		Debug.Log("setting cards data: " + cardsData);
		PhotonNetwork.room.SetCustomProperties(cardsData);
	}



	void LateUpdate()
	{
		if (_queuedActions.Count > 0)
		{
			Hashtable[] actionsToSend = new Hashtable[_queuedActions.Count];
			for (int i=0; i< _queuedActions.Count ; i++)
			{
				actionsToSend[i] = _queuedActions[i].Encode();
			}


			RaiseEventOptions options = new RaiseEventOptions();
			options.Receivers = ExitGames.Client.Photon.ReceiverGroup.All;
			PhotonNetwork.RaiseEvent((byte)87, actionsToSend, true, options);
			_queuedActions.Clear();
		}

	}

	#endregion
}


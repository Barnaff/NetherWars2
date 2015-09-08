using UnityEngine;
using System.Collections;
using NetherWars;
using System.Collections.Generic;


public class PlayerController : MonoBehaviour {

	[SerializeField]
	private GameObject _battlefieldContainer;

	[SerializeField]
	private GameObject _resourceZoneContainer;

	[SerializeField]
	private GameObject _libraryContainer;

	[SerializeField]
	private GameObject _handContainer;

	[SerializeField]
	private Camera _playerCamera;


	[SerializeField]
	private int _playerId;

	private INWPlayer _player;

	[SerializeField]
	private Dictionary<int, CardController> _playerCardsDictionary = new Dictionary<int, CardController>();

	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {
	
	}


	#region Public

	public void SetPlayer(INWPlayer player)
	{
		_player = player;
	}

	public void SetActivePlayer(bool isActivePlayer)
	{
		_playerCamera.gameObject.SetActive(isActivePlayer);
	}

	public void AddCard(CardController card, eZoneType zone = eZoneType.Library)
	{
		_playerCardsDictionary.Add(card.UniqueId, card);

		if (zone == eZoneType.Library)
		{
			PlaceCardInLibrary(card);
		}
		else
		{
			Debug.LogError("Unsupported zone to add card!");
		}

		card.transform.SetParent(this.gameObject.transform);
	}

	#endregion


	#region private

	private void PlaceCardInLibrary(CardController card, bool animated = false)
	{
		card.IsFlipped = true;
		Vector3 position = _libraryContainer.transform.position;
		position.y += _playerCardsDictionary.Count * 0.01f;
		card.transform.position = position;
	}

	#endregion
}

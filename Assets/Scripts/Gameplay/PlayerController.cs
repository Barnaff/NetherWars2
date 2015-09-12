using UnityEngine;
using System.Collections;
using NetherWars;
using System.Collections.Generic;


public class PlayerController : MonoBehaviour {

	[SerializeField]
	private ZoneControllerAbstract _battlefieldContainer;

	[SerializeField]
	private ZoneControllerAbstract _resourceZoneContainer;

	[SerializeField]
	private ZoneControllerAbstract _libraryContainer;

	[SerializeField]
	private ZoneControllerAbstract _handContainer;

	[SerializeField]
	private Camera _playerCamera;


	[SerializeField]
	private int _playerId;

	private INWPlayer _player;

	[SerializeField]
	private Dictionary<int, CardController> _playerCardsDictionary = new Dictionary<int, CardController>();


	private CardController _mouseOverCard = null;

	private CardController _draggedCard = null;

	private Vector3 _screenPoint;
	private Vector3 _offset;

	// Use this for initialization
	void Start () {
	

	}
	
	// Update is called once per frame
	void Update () {
	
		if (_playerCamera.isActiveAndEnabled)
		{
			Ray ray;
			RaycastHit hit;
			
			ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit))
			{
				CardController cardController = hit.collider.gameObject.GetComponent<CardController>();

				if (_mouseOverCard != null && cardController != _mouseOverCard)
				{
					_mouseOverCard.CardEndHover();
					_mouseOverCard = null;
					
				}

				if (cardController != null && cardController != _mouseOverCard)
				{
					cardController.CardHover();
					_mouseOverCard = cardController; 
				}


			}

			if (hit.collider == null && _mouseOverCard != null)
			{
				_mouseOverCard.CardEndHover();
				_mouseOverCard = null;
			}

			if (Input.GetMouseButtonDown(0))
			{
				// mouse down
				if (_mouseOverCard != null)
				{
					_screenPoint = Camera.main.WorldToScreenPoint(_mouseOverCard.transform.position);
					_offset = _mouseOverCard.transform.position - _playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));
					_mouseOverCard.CardStartDraging(_offset);
				}
			}

			if (Input.GetMouseButtonUp(0))
			{
				// mouse up
				Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
				Vector3 curPosition = _playerCamera.ScreenToWorldPoint(curScreenPoint);
				_mouseOverCard.CardEndDraging(curPosition);
			}

			if (Input.GetMouseButton(0))
			{
				// mouse drag

				if (_mouseOverCard != null)
				{
					Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
					Vector3 curPosition = _playerCamera.ScreenToWorldPoint(curScreenPoint);
					_mouseOverCard.CardDragged(curPosition);
				}
			}
		}
	
	}


	#region Public

	public void SetPlayer(INWPlayer player)
	{
		_player = player;

		_battlefieldContainer.SetZone(_player.Battlefield, _player);
		_libraryContainer.SetZone(_player.Library, _player);
		_handContainer.SetZone(_player.Hand, _player);

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
		Vector3 position = _libraryContainer.gameObject.transform.position;
		position.y += _playerCardsDictionary.Count * 0.01f;
		card.transform.position = position;
	}

	#endregion
}

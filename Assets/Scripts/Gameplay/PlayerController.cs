using UnityEngine;
using System.Collections;
using NetherWars;
using System.Collections.Generic;


public delegate bool CanPlayCardDelegate(PlayerController playerController, CardController card);
public delegate void PlayCardDelegate(PlayerController playerController, CardController card);


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

	[SerializeField]
	private bool _isActivePlayer;

	private INWPlayer _player;

	[SerializeField]
	private Dictionary<int, CardController> _playerCardsDictionary = new Dictionary<int, CardController>();


	private CardController _selectedCard = null;

	private CardController _draggedCard = null;

	private Vector3 _screenPoint;
	private Vector3 _offset;

	#region Events

	public CanPlayCardDelegate OnCanPlayCard;
	public PlayCardDelegate OnPlayCard; 

	#endregion

	// Use this for initialization
	void Start () {
	
		if (!_isActivePlayer)
		{
			Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();
			for (int i=0; i< colliders.Length; i++)
			{
				colliders[i].enabled = false;
			}
		}

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
				//Debug.Log("hit: " + hit.collider.name);

				CardController cardController = hit.collider.gameObject.GetComponent<CardController>();

				if (_selectedCard != null && cardController != _selectedCard)
				{
					_selectedCard.CardEndHover();
					_selectedCard = null;
					
				}

				if (cardController != null && cardController != _selectedCard)
				{
					cardController.CardHover();
					_selectedCard = cardController; 
				}


			}

			if (hit.collider == null && _selectedCard != null)
			{
				_selectedCard.CardEndHover();
				_selectedCard = null;
			}

			if (Input.GetMouseButtonDown(0))
			{
				// mouse down
				if (_selectedCard != null)
				{
					_screenPoint = Camera.main.WorldToScreenPoint(_selectedCard.transform.position);
					_offset = _selectedCard.transform.position - _playerCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));
					_selectedCard.CardStartDraging(_offset);
				}
			}

			if (Input.GetMouseButtonUp(0))
			{
				// mouse up
				Debug.Log("mouse up");
				if (_selectedCard != null)
				{
					ZoneControllerAbstract releasedInZone = null;
					if(Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Zones")))
					{
						releasedInZone = hit.collider.gameObject.GetComponent<ZoneControllerAbstract>();
					}

					if (releasedInZone != null)
					{
						CardAttemtToMoveToZone(_selectedCard, releasedInZone);
					}
					else
					{
						Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
						Vector3 curPosition = _playerCamera.ScreenToWorldPoint(curScreenPoint);
						_selectedCard.CardEndDraging(curPosition);

					}

					_selectedCard = null;
				}
			}

			if (Input.GetMouseButton(0))
			{
				// mouse drag

				if (_selectedCard != null)
				{
					Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
					Vector3 curPosition = _playerCamera.ScreenToWorldPoint(curScreenPoint);
					_selectedCard.CardDragged(curPosition);
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
			_libraryContainer.AddCardToZone(card);
		}
		else
		{
			Debug.LogError("Unsupported zone to add card!");
		}

		card.transform.SetParent(this.gameObject.transform);
	}

	#endregion


	#region User Interactions

	private void CardAttemtToMoveToZone(CardController card, ZoneControllerAbstract zone)
	{
		switch (zone.ZoneType)
		{
		case eZoneType.Battlefield:
		{
			ZoneControllerAbstract lastZone = CurrentZoneForCard(card);
			if (lastZone != zone)
			{
				lastZone.RemoveCardFromZone(card);
			}
			zone.AddCardToZone(card);
			break;
		}
		case eZoneType.ResourcePool:
		{
			ZoneControllerAbstract lastZone = CurrentZoneForCard(card);
			if (lastZone != zone)
			{
				lastZone.RemoveCardFromZone(card);
			}
			zone.AddCardToZone(card);
			break;
		}
		default:
		{
			break;
		}
		}
	}

	#endregion
	

	private ZoneControllerAbstract CurrentZoneForCard(CardController card)
	{
		if (_handContainer.IsCardInZone(card))
		{
			return _handContainer;
		}

		if (_battlefieldContainer.IsCardInZone(card))
		{
			return _battlefieldContainer;
		}

		if (_resourceZoneContainer.IsCardInZone(card))
		{
			return _resourceZoneContainer;
		}

		if (_libraryContainer.IsCardInZone(card))
		{
			return _libraryContainer;
		}

		return null;

	}


	private bool CanPlayCard(CardController card)
	{
		if (OnCanPlayCard != null)
		{
			return OnCanPlayCard(this, card);
		}
		return false;
	}
		  
	private void PlayCard(CardController card)
	{
		if (OnPlayCard != null)
		{
			OnPlayCard(this, card);
		}
	}

}

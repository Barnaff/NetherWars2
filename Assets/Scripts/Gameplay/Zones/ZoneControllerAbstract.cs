using UnityEngine;
using System.Collections;
using NetherWars;
using System.Collections.Generic;

public abstract class ZoneControllerAbstract : MonoBehaviour {

	[SerializeField]
	protected NWZone _zoneData;

	[SerializeField]
	protected eZoneType _zoneType;

	[SerializeField]
	protected INWPlayer _player;

	[SerializeField]
	protected List<CardController> _cardsInZone;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void SetZone(NWZone zone, INWPlayer player)
	{
		_cardsInZone = new List<CardController>();
		_zoneData = zone;
		_zoneType = zone.Type;
		_player = player;
	}

	public virtual void AddCardToZone(CardController card, bool animated = true)
	{
		_cardsInZone.Add(card);

		this.PlaceCardInZone(card, animated);
		RegisterToCardEvents(card);
	}

	public virtual void RemoveCardFromZone(CardController card)
	{
		if (_cardsInZone.Contains(card))
		{
			_cardsInZone.Remove(card);
		}
		UnregisterFromCardEvents(card);
	}
	
	public NWZone Zone
	{
		get
		{
			return _zoneData;
		}
	}

	protected abstract void PlaceCardInZone(CardController cardController, bool animated = true);

	protected abstract void SortCardInZone(bool animated);


	#region Register to cards events

	protected virtual void RegisterToCardEvents(CardController card)
	{
		card.OnCardClicked += HandleOnCardClicked;
		card.OnCardIsDragged += HandleOnCardDragged;
		card.OnCardHover += HandleOnCardHover;
		card.OnCardEndHover += HandleOnCardEndHover;
		card.OnCardStartDraging += HandleOnCardStartDraging;
		card.OnCardEndDraging += HandleOnCardEndDraging;
	}


	protected virtual void UnregisterFromCardEvents(CardController card)
	{
		card.OnCardClicked -= HandleOnCardClicked;
		card.OnCardIsDragged -= HandleOnCardDragged;
		card.OnCardHover -= HandleOnCardHover;
		card.OnCardEndHover -= HandleOnCardEndHover;
		card.OnCardStartDraging -= HandleOnCardStartDraging;
		card.OnCardEndDraging -= HandleOnCardEndDraging;
	}

	#endregion


	#region Handle Cards Events

	protected virtual void HandleOnCardEndHover (CardController card)
	{
		
	}
	
	protected virtual void HandleOnCardHover (CardController card)
	{
		
	}

	protected virtual void HandleOnCardClicked (CardController card)
	{

	}

	protected virtual void HandleOnCardDragged (CardController card, Vector3 mousePosition)
	{
		card.transform.localRotation = Quaternion.Euler(card.transform.localRotation.eulerAngles.x,0,card.transform.localRotation.eulerAngles.z);
		card.transform.position = mousePosition;
	}

	protected virtual void HandleOnCardEndDraging (CardController card, Vector3 mousePosition)
	{
		this.SortCardInZone(true);
	}
	
	protected virtual void HandleOnCardStartDraging (CardController card, Vector3 mousePosition)
	{

	}



	#endregion

}

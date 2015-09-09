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
	}

	public virtual void RemoveCardFromZone(CardController card)
	{
		if (_cardsInZone.Contains(card))
		{
			_cardsInZone.Remove(card);
		}
	}

	public virtual void SortCardInZone(bool animated)
	{

	}

	public NWZone Zone
	{
		get
		{
			return _zoneData;
		}
	}

	protected abstract void PlaceCardInZone(CardController cardController, bool animated = true);



}

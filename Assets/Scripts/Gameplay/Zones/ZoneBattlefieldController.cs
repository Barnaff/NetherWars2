using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneBattlefieldController : ZoneControllerAbstract {


	#region implemented abstract members of ZoneControllerAbstract

	protected override void InitializeZoneController ()
	{

	}

	protected override void PlaceCardInZone (CardController cardController, bool animated = true)
	{
		cardController.transform.SetParent(this.transform);
		
		SortCardInZone(true);
	}

	public override void SortCardInZone (bool animated)
	{
		float cardWidth = 2.3f;
		float posX = _cardsInZone.Count * 0.5f * -cardWidth + (-cardWidth * 0.5f);
		for (int i=0; i< _cardsInZone.Count; i++)
		{
			CardController card = _cardsInZone[i];
			posX += 2.5f;
			Vector3 position = new Vector3(posX, 1.0f, 0);

			card.transform.localPosition = position;

			card.transform.localRotation = Quaternion.AngleAxis(0, Vector3.up);

			card.IsFlipped = !_canSeeCardsInZone;
		}
	}

	#endregion

	void Start()
	{
		List<CardController> tmpCardsList = new List<CardController>();
		tmpCardsList.AddRange(_cardsInZone);
		_cardsInZone.Clear();
		
		foreach (CardController card in tmpCardsList)
		{
			AddCardToZone(card);
		}
		
		SortCardInZone(true);
	}

	#region Handle cards events
	
	protected override void HandleOnCardHover (CardController card)
	{
		base.HandleOnCardHover (card);
	}
	
	protected override void HandleOnCardEndHover (CardController card)
	{
		base.HandleOnCardEndHover (card);
	}

	protected override void HandleOnCardStartDraging (CardController card, Vector3 mousePosition)
	{
		base.HandleOnCardStartDraging (card, mousePosition);

		LineRenderer lineRenderer = card.gameObject.AddComponent<LineRenderer>();
		lineRenderer.SetPosition(0, card.transform.position);
		lineRenderer.SetWidth(0.5f, 0.5f);
	}
	
	protected override void HandleOnCardDragged (CardController card, Vector3 mousePosition)
	{
		base.HandleOnCardDragged(card, mousePosition);

		LineRenderer lineRenderer = card.gameObject.GetComponent<LineRenderer>();
		lineRenderer.SetPosition(1, mousePosition);

	}
	
	protected override void HandleOnCardEndDraging (CardController card, Vector3 mousePosition)
	{
		Destroy(card.gameObject.GetComponent<LineRenderer>());
		this.SortCardInZone(true);
	}
	
	#endregion


}

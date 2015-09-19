using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneHandController : ZoneControllerAbstract {

	#region implemented abstract members of ZoneControllerAbstract

	protected override void PlaceCardInZone (CardController cardController, bool animated = true)
	{
		cardController.IsFlipped = false;
		cardController.transform.SetParent(this.transform);

		SortCardInZone(true);

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

	float _distanceBtweenCards = 1.5f;

	protected override void SortCardInZone (bool animated)
	{

		float radius = _cardsInZone.Count * _distanceBtweenCards;
		Vector3 center = this.transform.localPosition;
		center.z -= radius - 0.5f;

		float angleDistance = 10.0f - (_cardsInZone.Count * 0.5f);
		float angle = 90.0f + (_cardsInZone.Count * angleDistance * 0.5f) - (angleDistance * 0.5f);

		for (int i=0; i< _cardsInZone.Count; i++)
		{
			CardController cardController = _cardsInZone[i];
			if (cardController != null)
			{
				float posX = center.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
				float posZ = center.z + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
				
				Vector3 position = new Vector3(posX, 0, posZ);
				cardController.transform.localPosition = position;
				cardController.transform.localRotation = Quaternion.AngleAxis(90.0f - angle, Vector3.up);
				angle -= angleDistance;

			}
		}
	}


	#region Handle cards events

	protected override void HandleOnCardHover (CardController card)
	{
		base.HandleOnCardHover (card);
		card.transform.localScale = new Vector3(1.2f,1.2f,1.2f);
		card.transform.localPosition = new Vector3(card.transform.localPosition.x, 0.1f, card.transform.localPosition.z);
	}

	protected override void HandleOnCardEndHover (CardController card)
	{
		base.HandleOnCardEndHover (card);
		card.transform.localScale = new Vector3(1f,1f,1f);
		card.transform.localPosition = new Vector3(card.transform.localPosition.x, 0, card.transform.localPosition.z);
	}

	protected override void HandleOnCardDragged (CardController card, Vector3 mousePosition)
	{
		card.transform.localRotation = Quaternion.Euler(card.transform.localRotation.eulerAngles.x,0,card.transform.localRotation.eulerAngles.z);
		card.transform.position = mousePosition;
	}
	
	protected override void HandleOnCardEndDraging (CardController card, Vector3 mousePosition)
	{
		this.SortCardInZone(true);
	}

	#endregion

}

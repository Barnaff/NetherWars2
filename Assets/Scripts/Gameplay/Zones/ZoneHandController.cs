using UnityEngine;
using System.Collections;

public class ZoneHandController : ZoneControllerAbstract {

	#region implemented abstract members of ZoneControllerAbstract

	protected override void PlaceCardInZone (CardController cardController, bool animated = true)
	{
		cardController.IsFlipped = false;

		SortCardsPositions();

	}

	#endregion

	void Start()
	{
		SortCardsPositions();
	}

	float _distanceBtweenCards = 1.5f;

	private void SortCardsPositions()
	{

		float leftX = _cardsInZone.Count * _distanceBtweenCards * 0.5f * -1.0f;

		for (int i=0; i< _cardsInZone.Count; i++)
		{
			CardController cardController = _cardsInZone[i];

			Vector3 position = new Vector3(leftX + (i * _distanceBtweenCards), this.transform.position.y, this.transform.position.z);
			cardController.transform.position = position;

		}
	}



}

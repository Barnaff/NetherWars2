using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZoneLibraryController : ZoneControllerAbstract {

	#region implemented abstract members of ZoneControllerAbstract

	protected override void PlaceCardInZone (CardController cardController, bool animated = true)
	{
		//cardController.IsFlipped = true;
		cardController.transform.SetParent(this.transform);

		SortCardInZone(true);
	}


	protected override void SortCardInZone (bool animated)
	{
		for (int i = _cardsInZone.Count - 1 ; i >= 0 ; i--)
		{
			CardController card = _cardsInZone[i];
			Vector3 position = Vector3.zero;
			position.y += i * 0.05f;
			card.transform.localPosition = position;
			//card.transform.localRotation = Quaternion.Euler(card.transform.localRotation.eulerAngles.x,0,card.transform.localRotation.eulerAngles.z);
			card.IsFlipped = true;
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


}

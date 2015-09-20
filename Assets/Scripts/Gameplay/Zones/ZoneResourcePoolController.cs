using UnityEngine;
using System.Collections;

public class ZoneResourcePoolController : ZoneControllerAbstract {

	#region implemented abstract members of ZoneControllerAbstract

	protected override void PlaceCardInZone (CardController cardController, bool animated = true)
	{
		cardController.transform.SetParent(this.transform);
		
		SortCardInZone(true);
	}

	protected override void SortCardInZone (bool animated)
	{
		foreach (CardController card in _cardsInZone)
		{
			card.gameObject.transform.localPosition = new Vector3(0, 1.0f, 0);
			card.transform.localRotation = Quaternion.AngleAxis(0, Vector3.up);


			card.IsFlipped = !_canSeeCardsInZone;

		}
	}

	#endregion

}

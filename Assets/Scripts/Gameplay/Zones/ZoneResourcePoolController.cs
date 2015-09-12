using UnityEngine;
using System.Collections;

public class ZoneResourcePoolController : ZoneControllerAbstract {

	#region implemented abstract members of ZoneControllerAbstract

	protected override void PlaceCardInZone (CardController cardController, bool animated = true)
	{
		cardController.IsFlipped = false;
		cardController.transform.SetParent(this.transform);
		
		SortCardInZone(true);
	}

	protected override void SortCardInZone (bool animated)
	{
		foreach (CardController card in _cardsInZone)
		{
			card.gameObject.transform.localPosition = Vector3.zero;
		}
	}

	#endregion

}

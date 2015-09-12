using UnityEngine;
using System.Collections;

public class ZoneBattlefieldController : ZoneControllerAbstract {


	#region implemented abstract members of ZoneControllerAbstract
	protected override void PlaceCardInZone (CardController cardController, bool animated = true)
	{
		cardController.IsFlipped = false;
		cardController.transform.SetParent(this.transform);
		
		SortCardInZone(true);
	}

	protected override void SortCardInZone (bool animated)
	{
		float cardWidth = 2.3f;
		float posX = _cardsInZone.Count * 0.5f * -cardWidth + (-cardWidth * 0.5f);
		for (int i=0; i< _cardsInZone.Count; i++)
		{
			CardController card = _cardsInZone[i];
			posX += 2.5f;
			Vector3 position = new Vector3(posX, 0, -0.5f);

			card.transform.localPosition = position;
		}
	}
	#endregion

}

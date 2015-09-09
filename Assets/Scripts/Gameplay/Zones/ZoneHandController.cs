using UnityEngine;
using System.Collections;

public class ZoneHandController : ZoneControllerAbstract {

	#region implemented abstract members of ZoneControllerAbstract

	protected override void PlaceCardInZone (CardController cardController, bool animated = true)
	{
		cardController.IsFlipped = false;
		cardController.transform.SetParent(this.transform);

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
		float radius = _cardsInZone.Count * _distanceBtweenCards;
		Vector3 center = this.transform.localPosition;
		center.z -= radius - 0.5f;

		float angleDistance = 10.0f - (_cardsInZone.Count * 0.5f);
		float angle = 90.0f + (_cardsInZone.Count * angleDistance * 0.5f) - (angleDistance * 0.5f);

		for (int i=0; i< _cardsInZone.Count; i++)
		{
			CardController cardController = _cardsInZone[i];
			cardController.transform.SetParent(this.transform);
			if (cardController != null)
			{
				float posX = center.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
				float posZ = center.z + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
				
				Vector3 position = new Vector3(posX, this.transform.localPosition.y, posZ);
				cardController.transform.localPosition = position;
				
				Vector3 rotation = cardController.transform.rotation.eulerAngles;
				rotation.z = angle - 90.0f;
				cardController.transform.localRotation = Quaternion.Euler(rotation);
				
				angle -= angleDistance;
			}
		


		}
	}



}

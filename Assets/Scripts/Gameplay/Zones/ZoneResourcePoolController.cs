using UnityEngine;
using System.Collections;

public class ZoneResourcePoolController : ZoneControllerAbstract {

	#region implemented abstract members of ZoneControllerAbstract
	protected override void PlaceCardInZone (CardController cardController, bool animated = true)
	{
		throw new System.NotImplementedException ();
	}
	protected override void SortCardInZone (bool animated)
	{
		throw new System.NotImplementedException ();
	}
	#endregion

}

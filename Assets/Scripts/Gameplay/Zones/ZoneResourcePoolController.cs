using UnityEngine;
using System.Collections;

public class ZoneResourcePoolController : ZoneControllerAbstract {


	[SerializeField]
	private TextMesh _currentManaCoutnLabel;
	[SerializeField]
	private TextMesh _blackThrasholdLabel;
	[SerializeField]
	private TextMesh _blueThrasholdLabel;
	[SerializeField]
	private TextMesh _greenThrasholdLabel;
	[SerializeField]
	private TextMesh _purpleThrasholdLabel;
	[SerializeField]
	private TextMesh _redThrasholdLabel;
	[SerializeField]
	private TextMesh _whiteThrasholdLabel;


	#region implemented abstract members of ZoneControllerAbstract

	protected override void InitializeZoneController ()
	{
		NetherWars.IEventDispatcher eventDispatcher = NetherWars.NWEventDispatcher.Instance();
		eventDispatcher.OnZoneUpdated += HandleOnZoneUpdated;

	}

	protected override void PlaceCardInZone (CardController cardController, bool animated = true)
	{
		cardController.transform.SetParent(this.transform);
		
		SortCardInZone(true);
	}

	public override void SortCardInZone (bool animated)
	{
		foreach (CardController card in _cardsInZone)
		{
			card.gameObject.transform.localPosition = new Vector3(0, 1.0f, 0);
			card.transform.localRotation = Quaternion.AngleAxis(0, Vector3.up);


			card.IsFlipped = !_canSeeCardsInZone;

		}
	}

	#endregion


	#region Handle Events

	
	void HandleOnZoneUpdated (NetherWars.NWZone zone)
	{
		if (zone == this._zoneData)
		{
			NetherWars.IResourcePool resourcePool = (NetherWars.IResourcePool)zone;
			string manaCountString = resourcePool.CurrentMana.ToString() + "/" + resourcePool.TotalMana.ToString();
			_currentManaCoutnLabel.text = manaCountString;
			
			_blackThrasholdLabel.text = resourcePool.ThrasholdForColor(NetherWars.NWColor.Black).ToString();
			_blueThrasholdLabel.text = resourcePool.ThrasholdForColor(NetherWars.NWColor.Blue).ToString();
			_greenThrasholdLabel.text = resourcePool.ThrasholdForColor(NetherWars.NWColor.Green).ToString();
			_purpleThrasholdLabel.text = resourcePool.ThrasholdForColor(NetherWars.NWColor.Purple).ToString();
			_redThrasholdLabel.text = resourcePool.ThrasholdForColor(NetherWars.NWColor.Red).ToString();
			_whiteThrasholdLabel.text = resourcePool.ThrasholdForColor(NetherWars.NWColor.White).ToString();
		}
	}

	#endregion

}

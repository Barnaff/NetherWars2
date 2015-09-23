using UnityEngine;
using System.Collections;
using System;

namespace NetherWars
{
	#region Delegates

	public delegate void DispatchEventDelegate(NWEvent eventObject);

	public delegate void PlayCardDelegate(INWPlayer player, NWCard card);
	public delegate void PutCardInResourcesDelegate(INWPlayer player, NWCard card);
	public delegate void CardDrawDelegate(INWPlayer player, NWCard card);
	public delegate void CardChangeZoneDelegate(NWCard card, NWZone fromZone, NWZone toZone);
	public delegate void StartTurnDelegate(INWPlayer player);
	public delegate void StartGameDelegate();
	public delegate void EndTurnDelegate(INWPlayer player);
	public delegate void ZoneUpdatedDelegate(NWZone zone);
	public delegate void PayForCardDelegate(INWPlayer player, NWCard card);

	#endregion

	public interface IEventDispatcher  {

		#region Events

		event DispatchEventDelegate OnDispatchEvent;

		event PlayCardDelegate OnPlayCard;
		event PutCardInResourcesDelegate OnPutCardInResources;
		event CardDrawDelegate OnCardDraw;
		event CardChangeZoneDelegate OnCardChangeZone;
		event StartTurnDelegate OnStartTurn;
		event StartGameDelegate OnStartGame;
		event EndTurnDelegate OnEndTurn;
		event ZoneUpdatedDelegate OnZoneUpdated;
		event PayForCardDelegate OnPayForCard;

		#endregion

		void ProcessEvent(NWEvent eventObject);
	}

}
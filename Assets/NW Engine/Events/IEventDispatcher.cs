using UnityEngine;
using System.Collections;
using System;

namespace NetherWars
{
	#region Delegates

	public delegate void DispatchEventDelegate(NWEvent eventObject);

	public delegate void PlayCardDelegate(INWPlayer player, NWCard card, bool playAsResource = false);
	public delegate void CardDrawDelegate(INWPlayer player, NWCard card);
	public delegate void CardChangeZoneDelegate(NWCard card, NWZone fromZone, NWZone toZone);
	public delegate void StartTurnDelegate(INWPlayer player);

	#endregion

	public interface IEventDispatcher  {

		#region Events

		event DispatchEventDelegate OnDispatchEvent;

		event PlayCardDelegate OnPlayCard;
		event CardDrawDelegate OnCardDraw;
		event CardChangeZoneDelegate OnCardChangeZone;
		event StartTurnDelegate OnStartTurn;

		#endregion

		void ProcessEvent(NWEvent eventObject);
	}

}
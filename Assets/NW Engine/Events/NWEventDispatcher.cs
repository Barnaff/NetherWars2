using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NetherWars
{
	public class NWEventDispatcher : IEventDispatcher  {

		#region Events

		public event DispatchEventDelegate OnDispatchEvent;

		public event PlayCardDelegate OnPlayCard;
		public event PutCardInResourcesDelegate OnPutCardInResources;
		public event CardDrawDelegate OnCardDraw;
		public event CardChangeZoneDelegate OnCardChangeZone;
		public event StartTurnDelegate OnStartTurn;
		public event StartGameDelegate OnStartGame;
		public event EndTurnDelegate OnEndTurn;
		public event ZoneUpdatedDelegate OnZoneUpdated;
		public event PayForCardDelegate OnPayForCard;

		#endregion

		#region Private Properties

		private static NWEventDispatcher _instance;

		#endregion


		#region Shared Instance

		/// <summary>
		/// Singleton instance
		/// </summary>
		public static NWEventDispatcher Instance()
		{
			if (_instance == null)
			{
				_instance = new NWEventDispatcher();
			}
			return _instance;
		}

		#endregion


		#region Event Dispatching

		/// <summary>
		/// Dispatchs the event.
		/// </summary>
		/// <param name="eventObject">Event object.</param>
		public void DispatchEvent(NWEvent eventObject)
		{
			if (OnDispatchEvent != null)
			{
				OnDispatchEvent(eventObject);
			}
		}

		public void ProcessEvent(NWEvent eventObject)
		{
			switch (eventObject.Type)
			{
			case NWEventType.PlayCard:
			{
				NWPlayer player = NWPlayer.GetPlayer((int)eventObject.Data[(int)eEventField.Player]);
				NWCard card = NWCard.GetCard((int)eventObject.Data[(int)eEventField.Card]);
				PlayCard(player, card);
				break;
			}
			case NWEventType.PutCardInResource:
			{
				NWPlayer player = NWPlayer.GetPlayer((int)eventObject.Data[(int)eEventField.Player]);
				NWCard card = NWCard.GetCard((int)eventObject.Data[(int)eEventField.Card]);
				PutCardInResources(player, card);
				break;
			}
			case NWEventType.DrawCard:
			{
				NWPlayer player = NWPlayer.GetPlayer((int)eventObject.Data[(int)eEventField.Player]);
				NWCard card = NWCard.GetCard((int)eventObject.Data[(int)eEventField.Card]);
				CardDraw(player, card);
				break;
			}
			case NWEventType.CardChangeZone:
			{
				NWZone fromZone = NWZone.GetZone((int)eventObject.Data[(int)eEventField.FromZone]);
				NWZone toZone = NWZone.GetZone((int)eventObject.Data[(int)eEventField.ToZone]);
				NWCard card = NWCard.GetCard((int)eventObject.Data[(int)eEventField.Card]);
				CardChangeZone(card, fromZone, toZone);
				break;
			}
			case NWEventType.StartTurn:
			{
				NWPlayer player = NWPlayer.GetPlayer((int)eventObject.Data[(int)eEventField.Player]);
				StartTurn(player);
				break;
			}
			case NWEventType.StartGame:
			{
				StartGame();
				break;
			}
			case NWEventType.EndTurn:
			{
				NWPlayer player = NWPlayer.GetPlayer((int)eventObject.Data[(int)eEventField.Player]);
				EndTurn(player);
				break;
			}
			case NWEventType.ZoneUpdated:
			{
				NWZone zone = NWZone.GetZone((int)eventObject.Data[(int)eEventField.Zone]);
				ZoneUpdated(zone);
				break;
			}
			case NWEventType.PayForCard:
			{
				NWPlayer player = NWPlayer.GetPlayer((int)eventObject.Data[(int)eEventField.Player]);
				NWCard card = NWCard.GetCard((int)eventObject.Data[(int)eEventField.Card]);
				PayForCard(player, card);
				break;
			}
			default:
			{
				Debug.LogError("ERROR - Unrecognized Event Type!");
				break;
			}
			}
			
			string eventString = "[" + eventObject.Type.ToString() + "] ";
			if (eventObject.Data != null)
			{
				foreach (int key in eventObject.Data.Keys)
				{
					eventString += key + ": " + eventObject.Data[key].ToString() + ", ";
				}
			}
			Debug.Log(eventString);
		}

		#endregion


		#region event handlers

		private void PlayCard(NWPlayer player, NWCard card)
		{
			if (OnPlayCard != null)
			{
				OnPlayCard(player, card);
			}
		}

		private void PutCardInResources(NWPlayer player, NWCard card)
		{
			if (OnPutCardInResources != null)
			{
				OnPutCardInResources(player, card);
			}
		}

		private void CardDraw(NWPlayer player, NWCard card)
		{
			if (OnCardDraw != null)
			{
				OnCardDraw(player, card);
			}
		}

		private void CardChangeZone(NWCard card, NWZone fromZone, NWZone toZone)
		{
			if (OnCardChangeZone != null)
			{
				OnCardChangeZone(card, fromZone, toZone);
			}
		}

		private void StartTurn(NWPlayer player)
		{
			if (OnStartTurn != null)
			{
				OnStartTurn(player);
			}
		}

		private void StartGame()
		{
			if (OnStartGame != null)
			{
				OnStartGame();
			}
		}

		private void EndTurn(INWPlayer player)
		{
			if (OnEndTurn != null)
			{
				OnEndTurn(player);
			}
		}

		private void ZoneUpdated(NWZone zone)
		{
			if (OnZoneUpdated != null)
			{
				OnZoneUpdated(zone);
			}
		}

		private void PayForCard(NWPlayer player, NWCard card)
		{
			if (OnPayForCard != null)
			{
				OnPayForCard(player, card);
			}
		}

	    #endregion
	}
}

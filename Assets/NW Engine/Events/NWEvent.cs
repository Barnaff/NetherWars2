using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NetherWars
{
	public enum NWEventType
	{
	    None,
	    PlayCard,
	    DrawCard,
	    CardChangeZone,
		StartGame,
	    StartTurn,
	    CardAttemptToChangeZone
	}

	#region Events Keys
	
	/*  Event keys for the data Hashtable  */
	public enum eEventField
	{
		Card,
		NumberOfCards,
		FromZone,
		ToZone,
		Player,
		AsResource,
	}
	
	
	#endregion


	public enum eEventKeys
	{
		Type,
		Data,
	}

	public class NWEvent  {

		#region Public Fields

		public NWEventType Type;
		public Hashtable Data;

		#endregion

	

		#region Initialize Event

		public NWEvent()
		{
			Debug.LogError("ERROR - Cannot create event witout type!");
		}

		public NWEvent(NWEventType type)
		{
			Type = type;
		}

		public NWEvent(NWEventType type, Hashtable data)
		{
			Type = type;
			Data = data;
		}

		public NWEvent(NWServerAction serverAction)
		{
			if (serverAction.ActionsParams.ContainsKey((int)eEventKeys.Type))
			{
				Type = (NWEventType)serverAction.ActionsParams[(int)eEventKeys.Type];
			}

			if (serverAction.ActionsParams.ContainsKey((int)eEventKeys.Data))
			{
				Data = (Hashtable)serverAction.ActionsParams[(int)eEventKeys.Data];
			}
		}

		public NWServerAction ToServerAction()
		{
			Hashtable hash = new Hashtable();
			hash.Add((int)eEventKeys.Type, Type);
			if (Data != null)
			{
				hash.Add((int)eEventKeys.Data, Data);
			}
			NWServerAction serverAction = new NWServerAction(ServerActionType.GameplayEvent, hash);
			return serverAction;
		}


		#endregion

		#region Static Events Constructors

		public static NWEvent Draw(INWPlayer player, NWCard card)
		{
			Hashtable data = new Hashtable();
			data.Add((int)eEventField.Card, card.CardUniqueID);
			data.Add((int)eEventField.Player, player.PlayerID);
			NWEvent eventObject = new NWEvent(NWEventType.DrawCard, data);
			return eventObject;
		}

		public static NWEvent CardChangeZone(NWCard card, NWZone fromZone, NWZone toZone)
		{
			Hashtable data = new Hashtable();
			data.Add((int)eEventField.Card , card.CardUniqueID);
			data.Add((int)eEventField.FromZone, fromZone.ZoneID);
			data.Add((int)eEventField.ToZone, toZone.ZoneID);
			NWEvent eventObject = new NWEvent(NWEventType.CardChangeZone, data);
			return eventObject;
		}

		public static NWEvent StartTurn(INWPlayer player)
		{
			Hashtable data = new Hashtable();
			data.Add((int)eEventField.Player, player.PlayerID);
			NWEvent eventObject = new NWEvent(NWEventType.StartTurn, data);
			return eventObject;
		}

		public static NWEvent StartGame()
		{
			NWEvent eventObject = new NWEvent(NWEventType.StartGame);
			return eventObject;
		}


		#endregion
	}
}

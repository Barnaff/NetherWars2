using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NetherWars
{
	public enum ServerActionType
	{
		SetShuffledDecksData 	= 100,
		StartGame 				= 101,
		StartTurn				= 102,
		EndTurn					= 103,
		GameplayEvent			= 104,
	}
	
	public class NWServerAction  {

		public enum eServerActionKey
		{
			ActionType,
			ActionParams,
		}

		public ServerActionType ActionType { set; get; }
		
		public Hashtable ActionsParams { set; get; }

		public NWServerAction(ServerActionType actionType, Hashtable actionParams)
		{
			ActionType = actionType;
			ActionsParams = actionParams;
		}

		public NWServerAction(Hashtable data)
		{
			Decode(data);
		}


		public Hashtable Encode()
		{
			Hashtable hash = new Hashtable();
			hash.Add((int)eServerActionKey.ActionType, ActionType);
			hash.Add((int)eServerActionKey.ActionParams, ActionsParams);
			return hash;
		}
		
		public void Decode(Hashtable data)
		{
			ActionType = (ServerActionType)data[(int)eServerActionKey.ActionType];
			ActionsParams = (Hashtable)data[(int)eServerActionKey.ActionParams];
		}
	}



	
	
}

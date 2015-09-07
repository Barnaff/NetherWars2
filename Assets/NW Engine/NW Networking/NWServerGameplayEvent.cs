using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NetherWars
{
	public enum eServerGameplayEventType
	{
		StartGame, 
		ShuffleDecks,
		StartTurn,
		EndTurn,
	}
	
	public class NWServerGameplayEvent  {
		
		public eServerGameplayEventType Type;
		
		public Hashtable EventParams;

		public NWServerGameplayEvent(eServerGameplayEventType eventType, Hashtable eventParams)
		{
			Type = eventType;
			EventParams = eventParams;
		}
		
	}
	
}

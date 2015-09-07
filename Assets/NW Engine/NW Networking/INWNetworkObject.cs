using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NetherWars
{
	public interface INWNetworkObject  
	{
		Hashtable ToHash { get; }
		
		void PopulateWithData(ExitGames.Client.Photon.Hashtable data);
	}
}


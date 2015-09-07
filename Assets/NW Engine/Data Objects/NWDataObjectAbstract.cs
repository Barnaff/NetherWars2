using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NetherWars
{
	public abstract class NWDataObjectAbstract  {
		
		public abstract Hashtable Encode();

		public abstract void Decode(Hashtable data);
	}
}


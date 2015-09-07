using UnityEngine;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NetherWars
{
	public class NWCardDataObject : NWDataObjectAbstract {
		
		#region implemented abstract members of NWDataObjectAbstract
		
		public override Hashtable Encode ()
		{
			Hashtable hash = new Hashtable();
			hash.Add(CardUniqueId, CardId);
			return hash;
		}
		
		public override void Decode (Hashtable data)
		{

		}
		
		#endregion
	
		public int CardUniqueId;

		public int CardId;

		public NWCardDataObject(int cardUniqueId, int cardId)
		{
			CardUniqueId = cardUniqueId;
			CardId = cardId;
		}

		public NWCardDataObject(Hashtable data)
		{
			Decode(data);
		}

		public override string ToString ()
		{
			return "{" + CardUniqueId.ToString() + ":" + CardId.ToString() + "}";
		}
	}
}



using UnityEngine;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NetherWars
{
	public class NWZoneDataObject : NWDataObjectAbstract {

		private enum eZoneDataKeys
		{
			PlayerId,
			ZoneType,
			ZoneId,
		}

		#region implemented abstract members of NWDataObjectAbstract

		public override Hashtable Encode ()
		{
			Hashtable hash = new Hashtable();
			hash.Add((int)eZoneDataKeys.PlayerId, PlayerId);
			hash.Add((int)eZoneDataKeys.ZoneType, (int)ZoneType);
			hash.Add((int)eZoneDataKeys.ZoneId, ZoneId);
			return hash;
		}

		public override void Decode (Hashtable data)
		{
			PlayerId = (int)data[(int)eZoneDataKeys.PlayerId];
			ZoneType = (eZoneType)data[(int)eZoneDataKeys.ZoneType];
			ZoneId = (int)data[(int)eZoneDataKeys.ZoneId];
		}

		#endregion

		public int PlayerId;

		public eZoneType ZoneType;

		public int ZoneId;

		public NWZoneDataObject(int playerId, eZoneType zoneType, int zoneId)
		{
			PlayerId = playerId;
			ZoneType = zoneType;
			ZoneId = zoneId;
		}

		public NWZoneDataObject(Hashtable data)
		{
			Decode(data);
		}

	}
}

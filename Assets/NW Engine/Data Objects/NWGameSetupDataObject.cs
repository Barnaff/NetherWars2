using UnityEngine;
using System.Collections.Generic;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace NetherWars
{
	public class NWGameSetupDataObject : NWDataObjectAbstract {

		public enum eGameSetupKeys
		{
			Decks,
			PlayersZones,
		}

		#region implemented abstract members of NWDataObjectAbstract

		public override Hashtable Encode ()
		{
			Hashtable hash = new Hashtable();

			Hashtable decksHash = new Hashtable();
			foreach (NWPlayerDeckDataObject deck in PlayersDecks)
			{
				Hashtable deckData = deck.Encode();
				string playerId = deckData[ePlayerDeckDataObjectKeys.PlayerId].ToString();
				decksHash.Add(playerId, (Hashtable)deckData[ePlayerDeckDataObjectKeys.DeckCards]);

			}

			Hashtable[] zonesHash = new Hashtable[PlayersZones.Count];
			for (int i = 0; i< PlayersZones.Count; i++)
			{
				zonesHash[i] = PlayersZones[i].Encode();
			}

			hash.Add(eGameSetupKeys.Decks.ToString(), decksHash);
			hash.Add(eGameSetupKeys.PlayersZones.ToString(), zonesHash);


			Debug.Log( ">>>> " + Utils.HashToString(hash));
			return hash;
		}

		public override void Decode (Hashtable data)
		{
			Debug.Log(Utils.HashToString(data));
			Hashtable decksData = (Hashtable)data[eGameSetupKeys.Decks.ToString()];

			PlayersDecks = new List<NWPlayerDeckDataObject>();
			foreach (string playerIdKey in decksData.Keys)
			{
				NWPlayerDeckDataObject deck = new NWPlayerDeckDataObject(int.Parse(playerIdKey), (Hashtable)decksData[playerIdKey]);
				PlayersDecks.Add(deck);
			}

			PlayersZones = new List<NWZoneDataObject>();
			Hashtable[] zonesData = (Hashtable[])data[eGameSetupKeys.PlayersZones.ToString()];
			for (int i=0; i< zonesData.Length; i++)
			{
				NWZoneDataObject zone = new NWZoneDataObject(zonesData[i]);
				PlayersZones.Add(zone);
			}
		}

		#endregion

		public List<NWPlayerDeckDataObject> PlayersDecks;

		public List<NWZoneDataObject> PlayersZones;

		public NWGameSetupDataObject()
		{

		}

		public NWGameSetupDataObject(List<NWPlayerDeckDataObject> playersDecks, List<NWZoneDataObject> playersZones)
		{
			PlayersDecks = playersDecks;
			PlayersZones = playersZones;
		}

		public override string ToString ()
		{
			string output = "";
			foreach (NWPlayerDeckDataObject deck in PlayersDecks)
			{
				output += deck.ToString();
			}
			return "[NWCardsDictionaryDataObject]: " + output;
		}

		public void ShuffleDecks()
		{
			foreach (NWPlayerDeckDataObject deck in PlayersDecks)
			{
				deck.ShuffleDeck();
			}
		}

		public Hashtable GetShuffledDecksIndexes()
		{
			Hashtable hash = new Hashtable();

			foreach (NWPlayerDeckDataObject deck in PlayersDecks)
			{
				deck.ShuffleDeck();
				int[] shuffleDeck = deck.GetDeckIndexes();
				hash.Add(deck.PlayerId, shuffleDeck);
			}

			return hash;
		}


	}
}

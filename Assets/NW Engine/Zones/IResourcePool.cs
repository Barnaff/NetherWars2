using UnityEngine;
using System.Collections;

namespace NetherWars
{
	public interface IResourcePool  {

		void ResetPool();

		bool CanPayForCard(NWCard card);

		void PayForCard(NWCard card);
			
		int ThrasholdForColor(NWColor color);

		int CurrentMana{ get; }

		int NumberOfResourcesPutThisTurn{ get; }

	}
}

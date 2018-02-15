using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace PanicPinnacle.Matches {

	/// <summary>
	/// A data structure that can be passed into the initialization process of a match to configure it properly.
	/// </summary>
	[System.Serializable]
	public class MatchSettings {

		#region FIELDS - ROUNDS
		/// <summary>
		/// The queue of rounds that should be run for this match.
		/// </summary>
		[TabGroup("Match Settings", "Rounds"), PropertyTooltip("The queue of rounds that should be run for this match."), SerializeField]
		private Queue<RoundSettings> roundSettings = new Queue<RoundSettings>();
		#endregion

		#region ROUND MANAGEMENT
		/// <summary>
		/// Pops and returns the next RoundSettings in line for this match.
		/// </summary>
		/// <returns>The RoundSettings that define the next round for the match.</returns>
		public RoundSettings PopNextRound() {
			Debug.Log("Popping next RoundSettings from the MatchSettings queue.");
			return this.roundSettings.Dequeue();
		}
		/// <summary>
		/// Peeks at the next RoundSettings in line for this match.
		/// </summary>
		/// <returns>The RoundSettings that define the next round for the match.</returns>
		public RoundSettings PeekNextRound() {
			return this.roundSettings.Peek();
		}
		#endregion


	}


}
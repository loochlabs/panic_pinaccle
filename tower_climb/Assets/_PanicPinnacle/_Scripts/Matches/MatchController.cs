using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Combatants;
using System.Linq;

namespace PanicPinnacle.Matches {

	/// <summary>
	/// Takes care of the progression of the match throughout the multiple rounds.
	/// </summary>
	public class MatchController : MonoBehaviour {

		public static MatchController instance;

		#region FIELDS - ROUND SETTINGS
		/// <summary>
		/// The queue of rounds that should be run for this match.
		/// </summary>
		private Queue<RoundSettings> roundSettings = new Queue<RoundSettings>();
		#endregion

		#region UNITY FUNCTIONS
		private void Awake() {
			if (instance == null) {
				instance = this;
			}
		}
		#endregion

		#region PREPARATION
		/// <summary>
		/// Preps the match by initializing the controller with a collection of settings.
		/// </summary>
		/// <param name="matchSettings"></param>
		public void StartMatch(MatchSettings matchSettings) {
			Debug.Log("PREPARING MATCH");
			// Copy over the queue of round settings.
			this.roundSettings = new Queue<RoundSettings>(matchSettings.roundSettings);
			// Start up the first round by dequeuing it from the RoundSettings queue.
			RoundController.instance.PrepareRound(matchSettings: matchSettings, roundSettings: this.DequeueNextRound());
		}
		#endregion

		#region ROUND MANAGEMENT
		/// <summary>
		/// Pops and returns the next RoundSettings in line for this match.
		/// </summary>
		/// <returns>The RoundSettings that define the next round for the match.</returns>
		private RoundSettings DequeueNextRound() {
			Debug.Log("Popping next RoundSettings from the MatchSettings queue.");
			return this.roundSettings.Dequeue();
		}
		/// <summary>
		/// Peeks at the next RoundSettings in line for this match.
		/// </summary>
		/// <returns>The RoundSettings that define the next round for the match.</returns>
		private RoundSettings PeekNextRound() {
			return this.roundSettings.Peek();
		}
		#endregion

	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Combatants;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace PanicPinnacle.Matches {

	/// <summary>
	/// Takes care of the progression of the match throughout the multiple rounds.
	/// </summary>
	public class MatchController : SerializedMonoBehaviour {

		public static MatchController instance;

		#region FIELDS - SETTINGS
		//STEVE : no need for round settings here. This info can be held in currentMatchSettings.
		/// The settings that define this current match.
		/// Needs to be serialized because I'm a fucking idiot.
		/// </summary>
		[OdinSerialize, HideInInspector]
		private MatchSettings currentMatchSettings;
		/// <summary>
		/// The settings that define this current match.
		/// </summary>
		public MatchSettings CurrentMatchSettings {
			get {
				if (this.currentMatchSettings == null) {
					Debug.LogError("CurrentMatchSettings is null!");
				}
				return this.currentMatchSettings;
			}
		}
		#endregion

		

		#region RUNTIME FIELDS
		/// <summary>
		/// Current phase of this match.
		/// </summary>
		private MatchPhase currentPhase;
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
		/// Prepare match with all of match template settings.
		/// </summary>
		/// <param name="matchTemplate"></param>
		public void PrepareMatch(MatchTemplate matchTemplate) {
			Debug.Log("PREPARING MATCH");

			//@TODO need a clone of the matchTemplate
			currentMatchSettings = new MatchSettings(matchTemplate);
		}
		/// <summary>
		/// Starts the match by initializing the controller with the collection of settings.
		/// </summary>
		/// <param name="matchSettings"></param>
		public void StartMatch() {
			Debug.Log("STARTING NEW ROUND");
			GotoNextPhase();
		}
		#endregion

		#region ROUND MANAGEMENT

		/// <summary>
		/// Single call for easily going to the next round/phase of the match.
		/// @TODO: control flow from ROUND > TALLY > ROUND > COMPLETE_TALLY
		/// </summary>
		private void GotoNextPhase() {
			//Check for end of match
			if (currentMatchSettings.roundSettings.Count == 0) {
				Debug.Log("MATCH COMPLETE : going to final Match Tally!");
				SceneController.instance.LoadScene(currentMatchSettings.MatchTemplate.MatchTallySceneName);
			}

			//go to next round
			int currentRoundIndex = currentMatchSettings.MatchTemplate.RoundCount - currentMatchSettings.roundSettings.Count + 1;
			Debug.Log("Going to Round " + currentRoundIndex);
			RoundController.instance.PrepareRound(roundSettings: DequeueNextRound());
		}

		/// <summary>
		/// Pops and returns the next RoundSettings in line for this match.
		/// </summary>
		/// <returns>The RoundSettings that define the next round for the match.</returns>
		private RoundSettings DequeueNextRound() {
			Debug.Log("Popping next RoundSettings from the MatchSettings queue.");
			return currentMatchSettings.roundSettings.Dequeue();
		}
		/// <summary>
		/// Peeks at the next RoundSettings in line for this match.
		/// </summary>
		/// <returns>The RoundSettings that define the next round for the match.</returns>
		private RoundSettings PeekNextRound() {
			return currentMatchSettings.roundSettings.Peek();
		}
		#endregion

	}

	/// <summary>
	/// Phases for state of a match.
	/// </summary>
	public enum MatchPhase {
		pregame,
		round,
		tally,
		endtally,
		debug
	}


}
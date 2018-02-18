using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Combatants;
using System.Linq;
using Sirenix.OdinInspector;

namespace PanicPinnacle.Matches {

	/// <summary>
	/// Takes care of the progression of the match throughout the multiple rounds.
	/// </summary>
	public class MatchController : SerializedMonoBehaviour {

		public static MatchController instance;

		#region FIELDS - SETTINGS
		/// <summary>
		/// The queue of rounds that should be run for this match.
		/// </summary>
		private Queue<RoundSettings> roundSettings = new Queue<RoundSettings>();
		/// <summary>
		/// The settings that define this current match.
		/// </summary>
		private MatchSettings currentMatchSettings;
		/// <summary>
		/// The settings that define this current match.
		/// </summary>
		public MatchSettings CurrentMatchSettings {
			get {
				return this.currentMatchSettings;
			}
		}
		#endregion

		#region FIELDS - DEBUG
		/// <summary>
		/// Should the game run in debug mode? 
		/// DELETE THIS LATER WHEN IM DONE WITH IT. THAT OR HAVE IT ONLY WORK IN EDITOR MODE
		/// </summary>
		[TabGroup("Debug", "Debug"), SerializeField]
		private bool debugMode = false;
		/// <summary>
		/// Some debug match settings to use if.. in debug mode.
		/// </summary>
		[TabGroup("Debug", "Debug"), ShowIf("debugMode"), SerializeField]
		private MatchSettings debugMatchSettings = new MatchSettings();
		#endregion

		#region UNITY FUNCTIONS
		private void Awake() {
			if (instance == null) {
				instance = this;
			}
		}
		private void Start() {
			// If in debug mode, immediately prepare the match with the debug setings.
			if (this.debugMode == true) {
				this.StartDebugMatch(this.debugMatchSettings);
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
			// Save the match settings, as they'll be needed.
			this.currentMatchSettings = matchSettings;
			// Copy over the queue of round settings.
			// IS THIS NEEDED IF I'M SAVING THE MATCH SETINGS ANYWAY? CHANGE LATER IF IT CREATES PROBLEMS
			this.roundSettings = new Queue<RoundSettings>(matchSettings.roundSettings);
			// Start up the first round by dequeuing it from the RoundSettings queue.
			RoundController.instance.PrepareRound(matchSettings: matchSettings, roundSettings: this.DequeueNextRound());
		}
		/// <summary>
		/// Starts the match in debug mode. FIX THIS LATER I DONT LIKE IT
		/// </summary>
		/// <param name="matchSettings"></param>
		private void StartDebugMatch(MatchSettings matchSettings) {
			// Save the match settings, as they'll be needed.
			this.currentMatchSettings = matchSettings;
			// Copy over the queue of round settings.
			// IS THIS NEEDED IF I'M SAVING THE MATCH SETINGS ANYWAY? CHANGE LATER IF IT CREATES PROBLEMS
			this.roundSettings = new Queue<RoundSettings>(matchSettings.roundSettings);
			// Start up the first round by dequeuing it from the RoundSettings queue.
			RoundController.instance.PrepareDebugRound(matchSettings: matchSettings, roundSettings: this.DequeueNextRound());
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
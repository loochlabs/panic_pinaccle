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
					Debug.LogError("CurrentMatchSettings is null! Using the DebugMatchSettings.");
					this.currentMatchSettings = this.DebugMatchSettings;
				}
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
		/// Some debug match settings. Good if I just need to have something ready to go.
		/// </summary>
		[TabGroup("Debug", "Debug"), OdinSerialize]
		private MatchSettings debugMatchSettings;
		/// <summary>
		/// Some debug match settings. Good if I just need to have something ready to go.
		/// </summary>
		public MatchSettings DebugMatchSettings {
			get {
				return this.debugMatchSettings;
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
		public void StartMatch(MatchTemplate matchTemplate) {
			Debug.Log("PREPARING MATCH");

            //@TODO need a clone of the matchTemplate
            currentMatchSettings = new MatchSettings(matchTemplate);

            Debug.Log("STARTING NEW ROUND");
            GotoNextPhase();
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
			//this.roundSettings = new Queue<RoundSettings>(matchSettings.roundSettings);
			// Start up the first round by dequeuing it from the RoundSettings queue.
			//RoundController.instance.PrepareDebugRound(matchSettings: matchSettings, roundSettings: this.DequeueNextRound());
		}
		#endregion

		#region ROUND MANAGEMENT

        /// <summary>
        /// Single call for easily going to the next round/phase of the match.
        /// @TODO: control flow from ROUND > TALLY > ROUND > COMPLETE_TALLY
        /// </summary>
        private void GotoNextPhase()
        {
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
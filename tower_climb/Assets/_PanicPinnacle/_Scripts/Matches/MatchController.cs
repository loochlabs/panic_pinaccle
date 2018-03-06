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
			Debug.Log("STARTING NEW MATCH");
			NextPhase(MatchPhase.pregame);
		}
		#endregion

		#region ROUND MANAGEMENT

		/// <summary>
		/// Single call for easily going to the next round/phase of the match.
		/// </summary>
		public void NextPhase(MatchPhase phase) {
            switch (phase)
            {
                case MatchPhase.pregame:
                    currentPhase = phase;
                    SceneController.instance.LoadScene(
                        sceneName: currentMatchSettings.MatchTemplate.PregameSceneName, 
                        showLoadingText: true);
                    break;

                case MatchPhase.round:
                    currentPhase = phase;
                    
                    //go to next round
                    int currentRoundIndex = currentMatchSettings.MatchTemplate.RoundCount - currentMatchSettings.roundSettings.Count + 1;
                    Debug.Log("Going to Round " + currentRoundIndex);
                    RoundController.instance.PrepareRound(roundSettings: DequeueNextRound());
                    // Use the scene controller to load up the next scene.
                    SceneController.instance.LoadScene(roundSettings: RoundController.instance.CurrentRoundSettings);
                    break;

                case MatchPhase.tally:
                    currentPhase = phase;
                    SceneController.instance.LoadScene(
                        sceneName: currentMatchSettings.MatchTemplate.MatchTallySceneName, 
                        showLoadingText: true);

                    break;
                case MatchPhase.endtally:
                    currentPhase = phase;
                    //TODO: temp until there is an endtally
                    NextPhase();
                    break;
                case MatchPhase.debug:
                    break;
            }
            
		}

        /// <summary>
        /// Go to next phase of Match.
        /// This is a control wrapper to simplify the call from the various phases.
        /// </summary>
        public void NextPhase()
        {
            switch (currentPhase) {
                case MatchPhase.pregame:
                    NextPhase(MatchPhase.round);
                    break;
                case MatchPhase.round:
                    NextPhase(MatchPhase.tally);
                    break;
                case MatchPhase.tally:
                    if(currentMatchSettings.roundSettings.Count == 0)
                    {
                        NextPhase(MatchPhase.endtally);
                    }
                    else
                    {
                        NextPhase(MatchPhase.round);
                    }
                    break;
                case MatchPhase.endtally:
                    //Match is complete, go back to Title Screen
                    SceneController.instance.LoadScene(
                        sceneName: currentMatchSettings.MatchTemplate.TitleScreenSceneName, 
                        showLoadingText: true, 
                        collectGarbageOnTransition: true);
                    break;
                case MatchPhase.debug:
                    break;
            }

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
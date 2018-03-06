using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PanicPinnacle.Matches;

namespace PanicPinnacle {

	/// <summary>
	/// Controls the game from a big picture perspective and persists through all scenes. 
	/// </summary>
	public class GameController : MonoBehaviour {

		public static GameController instance;

        #region MATCH TEMPLATE
        /// <summary>
		/// Predifined match settings if starting from TitleScreen.
		/// </summary>
		[TabGroup("Match Info"), PropertyTooltip("Predifined match template."), SerializeField]
        private MatchTemplate matchTemplate;
        #endregion

        #region DEBUG
        [TabGroup("Debug", "Debug"), SerializeField]
		private bool debugMode = false;
		/// <summary>
		/// The number of players to start the match with in debug mode, since pregame is being skipped.
		/// </summary>
		[TabGroup("Debug", "Debug"), SerializeField, ShowIf("debugMode"), PropertyTooltip("The number of players to start the match with, since pregame is being skipped.")]
		private int debugPlayerCount = 4;
		/// <summary>
		/// Predifined match settings.
		/// </summary>
		[TabGroup("Debug", "Debug"), PropertyTooltip("Predifined match settings."), SerializeField, ShowIf("debugMode")]
		private MatchTemplate debugMatchTemplate;
		
		#endregion

		#region UNITY FUNCTIONS
		private void Awake() {
            if (instance == null) {
				instance = this;
				DontDestroyOnLoad(this);
			} else {
				// Destorying this object if the instance is already set will also destroy new instances of its children.
				// This is intentional.
				Destroy(this.gameObject);
			}

		}
		private void Start() {
			//@TEMP debug info
			if (debugMode) {
				DebugStart();
			}
		}
		#endregion

		/// <summary>
		/// Prepare new match with MatchTemplate.
		/// This is ideally called from TileScreen > "New Game"
		/// Breaking up PREP and START for more control with Debug Settings
		/// </summary>
		public void PrepareMatch() {
			Debug.Log("Preparing match with template stored in the GameController.");
			MatchController.instance.PrepareMatch(debugMode ? debugMatchTemplate : matchTemplate);
		}
		/// <summary>
		/// Top level call for starting a new match. This should be the single entry point for the entire Match/Game.
		/// </summary>
		public void StartMatch() {
            MatchController.instance.PrepareMatch(debugMode ? debugMatchTemplate : matchTemplate);
			MatchController.instance.StartMatch();
		}

        /// <summary>
		/// Predifined match settings.
		/// </summary>
		public MatchTemplate MatchTemplate
        {
            get {
                if (debugMode)
                {
                    return debugMatchTemplate;
                }
                else
                {
                    return matchTemplate;
                }
            }
        }

        #region DEBUG
        public void DebugStart() {
			PrepareMatch();

			//debug players for this match to bypass pregame setup
			for (int i = 0; i < debugPlayerCount; i++) {
				MatchController.instance.CurrentMatchSettings.AddCombatant(i);
			}

			StartMatch();
		}
		#endregion

	}

}
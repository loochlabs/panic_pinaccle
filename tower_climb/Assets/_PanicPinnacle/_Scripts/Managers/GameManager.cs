using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PanicPinnacle.Matches;

namespace PanicPinnacle {

	/// <summary>
	/// Controls the game from a big picture perspective and persists through all scenes. 
	/// </summary>
	public class GameManager : MonoBehaviour {

		public static GameManager instance;

        #region PREDIFINED TEMPLATE
        /// <summary>
        /// Predifined match settings.
        /// </summary>
        [TabGroup("Match", "Match Template"), PropertyTooltip("Predifined match settings."), SerializeField]
        private MatchTemplate matchTemplate;
        /// <summary>
        /// Predifined match settings.
        /// </summary>
        public MatchTemplate MatchTemplate
        {
            get { return matchTemplate; }
        }
        #endregion

        #region DEBUG
        [TabGroup("Debug", "Debug"), SerializeField]
        private bool debugMode = false;
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

        private void Start()
        {
            //@TEMP debug info
            if (debugMode)
            {
                StartMatch();
            }
        }
        
        #endregion

        /// <summary>
        /// Top level call for starting a new match. This should be the single entry point for the entire Match/Game.
        /// </summary>
        public void StartMatch()
        {
            MatchController.instance.StartMatch(matchTemplate);
        }

	}


}
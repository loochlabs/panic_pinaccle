using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PanicPinnacle.Combatants;
using Sirenix.Serialization;

namespace PanicPinnacle.Matches {

	/// <summary>
	/// A data structure that can be passed into the initialization process of a match to configure it properly.
	/// </summary>
    [System.Serializable]
    public class MatchSettings {

        #region TEMPLATE
        /// <summary>
        /// Reference to current match template.
        /// </summary>
        private MatchTemplate matchTemplate;

        /// <summary>
        /// Reference to current match template.
        /// </summary>
        public MatchTemplate MatchTemplate
        {
            get { return matchTemplate; }
        }

        #endregion

        #region FIELDS - COMBATANTS
        /// <summary>
        /// The templates to use for instansiating combatants with at the beginning of a match.
        /// </summary>
        [TabGroup("Match Settings", "Combatants"), PropertyTooltip("The templates to use for instansiating combatants with at the beginning of a match."), SerializeField]

		private List<CombatantTemplate> combatantTemplates = new List<CombatantTemplate>();
		/// <summary>
		/// The templates to use for instansiating combatants with at the beginning of a match.
		/// </summary>
		public List<CombatantTemplate> CombatantTemplates {
			get {
				return this.combatantTemplates;
			}
		}
		#endregion

        #region FIELDS - COMBATANT SETTINGS
        [TabGroup("Match Settings", "Colors"), PropertyTooltip("Colors for the combatants during the match."), SerializeField]
        public List<Color> combatantColors = new List<Color>();
        #endregion

        #region FIELDS - ROUNDS
        /// <summary>
        /// The queue of rounds that should be run for this match.
        /// </summary>
        [TabGroup("Match Settings", "Rounds"), PropertyTooltip("The queue of rounds that should be run for this match."), SerializeField]
		public Queue<RoundSettings> roundSettings = new Queue<RoundSettings>();
        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="matchTemplate">Match Template provided before start of game.</param>
        public MatchSettings(MatchTemplate matchTemplate)
        {
            this.matchTemplate = matchTemplate;

            //Create our queue of rounds to be played this match.
            Queue<RoundSettings> roundSettings = new Queue<RoundSettings>();
            System.Random rng = new System.Random();
            for (int i = 0; i < matchTemplate.RoundCount; i++)
            {
                int index = rng.Next(matchTemplate.RoundSettings.Count);
                roundSettings.Enqueue(matchTemplate.RoundSettings[index]);
                //@TODO dont remove for now, but we may want to avoid duplicate round settings
                //matchTemplate.RoundSettings.RemoveAt(index);
            }

            this.combatantTemplates = matchTemplate.CombatantTemplates;
            this.combatantColors = matchTemplate.PlayerColors;
            this.roundSettings = roundSettings;
        }
        
        #endregion
    }
}
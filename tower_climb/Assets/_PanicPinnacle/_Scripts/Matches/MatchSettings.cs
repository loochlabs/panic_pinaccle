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
		public MatchTemplate MatchTemplate {
			get { return matchTemplate; }
		}
		#endregion

		#region FIELDS - COMBATANTS
		/// <summary>
		/// The templates to use for instansiating combatants with at the beginning of a match.
		/// </summary>
		[TabGroup("Match Settings", "Combatants"), PropertyTooltip("The templates to use for instansiating combatants with at the beginning of a match."), SerializeField]
		private Dictionary<int, CombatantTemplate> combatantTemplates = new Dictionary<int, CombatantTemplate>();
		/// <summary>
		/// The templates to use for instansiating combatants with at the beginning of a match.
		/// </summary>
		public Dictionary<int, CombatantTemplate> CombatantTemplates {
			get {
				return this.combatantTemplates;
			}
		}
		#endregion

		#region FIELDS - ROUNDS
		/// <summary>
		/// The queue of rounds that should be run for this match.
		/// </summary>
		[TabGroup("Match Settings", "Rounds"), PropertyTooltip("The queue of rounds that should be run for this match."), SerializeField]
		public Queue<RoundSettings> roundSettings = new Queue<RoundSettings>();
		#endregion

		#region PREGAME FUNCTIONS
		/// <summary>
		/// Add a combatant during the pregame.
		/// </summary>
		/// <param name="combatantId">ID of combatant user input</param>
		/// <param name="templateId">ID of combatant template. 0 is our default template</param>
		public bool AddCombatant(int combatantId, int templateId = 0) {
			if (combatantTemplates.Count >= matchTemplate.MaxPlayerCount) {
				Debug.Log("MAXIMUM Players have already joined.");
				return false;
			} else {
				combatantTemplates[combatantId] = matchTemplate.CombatantTemplates[templateId];
				return true;
			}
		}
		/// <summary>
		/// Remove a combatant during the pregame.
		/// </summary>
		/// <param name="combatantId">ID of combatant user input</param>
		public bool RemoveCombatant(int combatantId) {
			if (combatantTemplates.ContainsKey(combatantId)) {
				Debug.Log("Removing Player " + combatantId);
				combatantTemplates.Remove(combatantId);
				return true;
			} else {
				Debug.LogWarning("Trying to remove Player " + combatantId + " but does not exist.");
				return false;
			}
		}
		#endregion

		#region CONSTRUCTORS
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="matchTemplate">Match Template provided before start of game.</param>
		public MatchSettings(MatchTemplate matchTemplate) {
			this.matchTemplate = matchTemplate;

			//Create our queue of rounds to be played this match.
			Queue<RoundSettings> roundSettings = new Queue<RoundSettings>();
			System.Random rng = new System.Random();
			for (int i = 0; i < matchTemplate.RoundCount; i++) {
				int index = rng.Next(matchTemplate.RoundSettings.Count);
				roundSettings.Enqueue(matchTemplate.RoundSettings[index]);
				//@TODO dont remove for now, but we may want to avoid duplicate round settings
				//matchTemplate.RoundSettings.RemoveAt(index);
			}

			this.roundSettings = roundSettings;
		}
		#endregion
	}
}
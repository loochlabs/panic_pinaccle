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

		#region FIELDS - ROUNDS
		/// <summary>
		/// The queue of rounds that should be run for this match.
		/// </summary>
		[TabGroup("Match Settings", "Rounds"), PropertyTooltip("The queue of rounds that should be run for this match."), SerializeField]
		public List<RoundSettings> roundSettings = new List<RoundSettings>();
		#endregion

		/*
		#region CLONING
		public static MatchSettings Clone(MatchSettings matchSettings) {
			// Create a clone of
			MatchSettings clonedSettings = (MatchSettings)matchSettings.MemberwiseClone();
			for (int i = 0; i < clonedSettings.roundSettings.Count; i++) {
				clonedSettings.roundSettings[i] = RoundSettings.Clone(matchSettings.roundSettings[i]);
			}
			return clonedSettings;
		}
		#endregion*/

	}
}
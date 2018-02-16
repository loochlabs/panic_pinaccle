using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PanicPinnacle.Combatants;

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
		public List<CombatantTemplate> combatantTemplates = new List<CombatantTemplate>();
		#endregion

		#region FIELDS - ROUNDS
		/// <summary>
		/// The queue of rounds that should be run for this match.
		/// </summary>
		[TabGroup("Match Settings", "Rounds"), PropertyTooltip("The queue of rounds that should be run for this match."), SerializeField]
		public Queue<RoundSettings> roundSettings = new Queue<RoundSettings>();
		#endregion


	}
}
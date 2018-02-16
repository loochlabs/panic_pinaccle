using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Combatants;
using System.Linq;

namespace PanicPinnacle.Matches {

	/// <summary>
	/// Takes care of the progression of the match throughout the multiple rounds.
	/// </summary>
	public class MatchController : MonoBehaviour {

		public static MatchController instance;

		#region FIELDS - COMBATANTS
		/// <summary>
		/// A list of combatants currently in-game.
		/// </summary>
		private List<Combatant> combatants = new List<Combatant>();
		/// <summary>
		/// A list of combatants currently in-game.
		/// </summary>
		public List<Combatant> Combatants {
			get {
				return this.combatants;
			}
		}
		#endregion

		#region FIELDS - ROUND SETTINGS
		/// <summary>
		/// The queue of rounds that should be run for this match.
		/// </summary>
		private Queue<RoundSettings> roundSettings = new Queue<RoundSettings>();
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
		/// Preps the match by initializing the controller with a collection of settings.
		/// </summary>
		/// <param name="matchSettings"></param>
		public void PrepareMatch(MatchSettings matchSettings) {
			Debug.Log("PREPARING MATCH");
			// Prep the combatants with the information contained within the MatchSettings and save the list.
			this.combatants = this.PrepareCombatants(
				combatants: new List<Combatant>(GameObject.FindObjectsOfType<Combatant>()), 
				matchSettings: matchSettings);
		}
		/// <summary>
		/// Goes through the combatants listed and preps them with the information stored in the MatchSettings passed in.
		/// </summary>
		/// <param name="combatants">The combatants to prep for the match.</param>
		/// <param name="matchSettings">The settings of the match that also determine how these combatants should be prepared.</param>
		/// <returns></returns>
		private List<Combatant> PrepareCombatants(List<Combatant> combatants, MatchSettings matchSettings) {
			// Go through each of the combatants that were provided.
			for (int i = 0; i < combatants.Count; i++) {
				// If there are more combatants than there are templates, destroy the extra combatants within the scene.
				// This will happen in cases where there are less people playing than there are combatants defined within the scene.
				if (i >= matchSettings.combatantTemplates.Count) {
					Destroy(combatants[i]);
				} else {
					// If the index can be used in both the combatants list and the templates list, prep the combatant with that template.
					combatants[i].Prepare(combatantTemplate: matchSettings.combatantTemplates[i], combatantId: i);
				}
			}
			// When done, return the modified list of combatants.
			return combatants;
		}
		#endregion

		#region ROUND MANAGEMENT
		/// <summary>
		/// Pops and returns the next RoundSettings in line for this match.
		/// </summary>
		/// <returns>The RoundSettings that define the next round for the match.</returns>
		public RoundSettings PopNextRound() {
			Debug.Log("Popping next RoundSettings from the MatchSettings queue.");
			return this.roundSettings.Dequeue();
		}
		/// <summary>
		/// Peeks at the next RoundSettings in line for this match.
		/// </summary>
		/// <returns>The RoundSettings that define the next round for the match.</returns>
		public RoundSettings PeekNextRound() {
			return this.roundSettings.Peek();
		}
		#endregion

	}

}
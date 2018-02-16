using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Combatants;

namespace PanicPinnacle.Matches {

	/// <summary>
	/// Manages rounds at a more fine-grain level that the MatchController shouldn't have responsibility of.
	/// </summary>
	public class RoundController : MonoBehaviour {

		public static RoundController instance;

		#region FIELDS - SETTINGS
		/// <summary>
		/// The settings currently being used for this particular round.
		/// </summary>
		private RoundSettings roundSettings;
		#endregion

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

		#region UNITY FUNCTIONS
		private void Awake() {
			if (instance == null) {
				instance = this;
			}
		}
		#endregion

		#region PREPARATION
		/// <summary>
		/// Prepares and loads the round with the settings provided.
		/// </summary>
		/// <param name="matchSettings">The settings used for preparing the match, on the off chance they're needed.</param>
		/// <param name="roundSettings">The settings to use when preparing this round.</param>
		public void PrepareRound(MatchSettings matchSettings, RoundSettings roundSettings) {
			Debug.Log("STARTING ROUND");
			// Save the settings, in case they are needed.
			this.roundSettings = roundSettings;
			// Use the scene controller to load up the next scene.
			SceneController.instance.LoadScene(roundSettings: roundSettings);
		}
		/// <summary>
		/// Start the round once the scene has finally been loaded.
		/// </summary>
		/// <param name="matchSettings">The settings that define the match, if needed.</param>
		/// <param name="roundSettings">The settings for this round.</param>
		public void StartRound(MatchSettings matchSettings, RoundSettings roundSettings) {
			// Prep the combatants with the information contained within the MatchSettings and save the list.
			this.combatants = this.PrepareCombatants(
				combatants: new List<Combatant>(GameObject.FindObjectsOfType<Combatant>()),
				matchSettings: matchSettings,
				roundSettings: roundSettings);
		}
		/// <summary>
		/// Goes through the combatants listed and preps them with the information stored in the MatchSettings passed in.
		/// </summary>
		/// <param name="combatants">The combatants to prep for the match.</param>
		/// <param name="matchSettings">The settings of the match that also determine how these combatants should be prepared.</param>
		/// <returns></returns>
		private List<Combatant> PrepareCombatants(List<Combatant> combatants, MatchSettings matchSettings, RoundSettings roundSettings) {
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


	}


}
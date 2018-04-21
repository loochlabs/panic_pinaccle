using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PanicPinnacle.Matches;
using System.Linq;
using Sirenix.OdinInspector;

namespace PanicPinnacle.Menus {

	/// <summary>
	/// A simple class for managing the finer level of detail that is displayed for a combatant on the match tally screen.
	/// </summary>
	public class CombatantRoundTallyInfo : MonoBehaviour {

		#region FIELDS - ID
		/// <summary>
		/// The ID of the combatant that this tally info should build.
		/// </summary>
		[SerializeField, TabGroup("Settings", "Settings")]
		private int combatantId;
		#endregion

		#region FIELDS - SCENE REFERENCES
		[SerializeField, TabGroup("Settings", "Scene References")]
		private SuperTextMesh combatantNameTextMesh;
		[SerializeField, TabGroup("Settings", "Scene References")]
		private Image combatantPortrait;
		/// <summary>
		/// The text that shows the breakdown of scores for the given round.
		/// </summary>
		[SerializeField, TabGroup("Settings", "Scene References")]
		private SuperTextMesh roundTallyTextMesh;
		/// <summary>
		/// The text that shows the numerical sum of the breakdown of scores for the given round.
		/// </summary>
		[SerializeField, TabGroup("Settings", "Scene References")]
		private SuperTextMesh totalScoreTextMesh;
		#endregion

		/// <summary>
		/// If the tally info is being enabled, it probably means it's being enabled in a context where it needs to be rebuilt.
		/// Do that.
		/// </summary>
		private void OnEnable() {
			// Check if there is a tally for this info in the score keeper before proceeding.
			if (ScoreKeeper.ContainsCombatant(id: this.combatantId, tallyscreenType: RoundTallyScreen.tallyType)) {
				// If there is, build it.
				this.Rebuild();
			} else {
				// If there isn't, don't bother.
				this.gameObject.SetActive(false);
			}
		}
		/// <summary>
		/// Assembles the tally with the information that needs to be displayed on screen.
		/// </summary>
		private void Rebuild() {
			Debug.Log("Rebuilding tally for combatant with ID: " + this.combatantId);

			// Grab the scores for this combatant.
			// I'm kinda shoehorning this if/else statement, I might get rid of it later.
			List<ScoreType> scores;
			if (RoundTallyScreen.tallyType == TallyScreenType.Round) {
				scores = ScoreKeeper.GetRoundScores(combatantId: this.combatantId);
			} else {
				scores = ScoreKeeper.GetTotalScores(combatantId: this.combatantId);
			}
			// Just write out the player's ID for now.
			this.combatantNameTextMesh.Text = "Player " + this.combatantId;
			// Initialize a string for the tally text.
			string roundTallyText = "";

			try {
				/*// Generate the tally text. For right now I'm just going to convert the score types to strings and append them on new lines.
				roundTallyText = scores
					.Select(s => s.ToString())              // Convert the scores into strings.
					.Aggregate((i, j) => i + "\n" + j);     // Add them into a new string that has a newline as a delimiter.*/
				// Generate the tally text. For right now I'm just going to convert the score types to strings and append them on new lines.
				roundTallyText = ScoreKeeper
					.ConvertScoresToStrings(scores: scores) // Convert the scores into strings.
					.Aggregate((i, j) => i + "\n" + j);		// Add them into a new string that has a newline as a delimiter.
				
			} catch (System.InvalidOperationException e) {
				// This exception happens when there's no elements in the list.
				Debug.LogWarning("InvalidOperationException was caught when building round tally! Info:\n" + e.Message);
			}
		
			// Set the new text.
			this.roundTallyTextMesh.Text = roundTallyText;

			// Also get the numerical sum of those scores.
			this.totalScoreTextMesh.Text = "TOTAL: " + ScoreKeeper.GetScoresValue(scores: scores);

			// Tint the color appropriately. The player grabs their color from the same source (the match controller) so this should work.
			this.combatantPortrait.color = MatchController.instance.CurrentMatchSettings.MatchTemplate.PlayerColors[this.combatantId];

		}

	}

}
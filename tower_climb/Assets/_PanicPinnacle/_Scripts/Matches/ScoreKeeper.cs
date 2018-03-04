using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PanicPinnacle.Combatants;

namespace PanicPinnacle.Matches {

	/// <summary>
	/// A class to help keep track of the scores of the different combatants.
	/// </summary>
	public static class ScoreKeeper  {

		#region FIELDS - SCORES
		/// <summary>
		/// Maintains the total scores of the different combatants.
		/// Keys are Combatant IDs.
		/// </summary>
		private static Dictionary<int, List<ScoreType>> combatantTotalScores = new Dictionary<int, List<ScoreType>>();
		/// <summary>
		/// Maintains the scores of the different combatants for a round currently in session.
		/// Keys are combatant IDs.
		/// </summary>
		private static Dictionary<int, List<ScoreType>> combatantRoundScores = new Dictionary<int, List<ScoreType>>();
		#endregion

		#region CONSTRUCTOR
		static ScoreKeeper() {
			// Initialize the two dictionaries with lists for up to four combatants.
			for (int i = 0; i < 4; i++) {
				combatantTotalScores.Add(i, new List<ScoreType>());
				combatantRoundScores.Add(i, new List<ScoreType>());
			}

			// DEBUG WHOA. Uncomment this to populate the round scores with completely arbitrary data.
			// AddDebugScores();
		}
		#endregion

		#region SCORE MANAGEMENT
		/// <summary>
		/// Resets the scores of all combatants.
		/// </summary>
		public static void FlushScores() {
			// Go through each of the key value pairs that already exist within the dicts and clear out their lists.
			// This does not clear out the dictionaries themselves, which is good, because the lists can be reused.
			combatantRoundScores.ToList().ForEach(kvp => kvp.Value.Clear());
			combatantTotalScores.ToList().ForEach(kvp => kvp.Value.Clear());
		}
		/// <summary>
		/// Add points for this round to the specified combatant.
		/// </summary>
		/// <param name="combatantId">The ID of the combatant who is receiving the points.</param>
		/// <param name="scoreType">The type of points to add.</param>
		public static void AddPoints(int combatantId, ScoreType scoreType) {
			Debug.Log("Adding score of type " + scoreType + " to combatant with ID: " + combatantId);
			combatantRoundScores[combatantId].Add(scoreType);
		}
		/// <summary>
		/// Adds the scores of the combatant in the round to the total scores.
		/// </summary>
		public static void NextRound() {
			// Go through each of the combatants currently in the round.
			foreach (int id in RoundController.instance.Combatants.Select(c => c.CombatantID).ToList()) {
				// Add the entries in the round scores for this combatant to the total scores.
				combatantTotalScores[id].AddRange(combatantRoundScores[id]);
				// Clear out those round scores.
				combatantRoundScores[id].Clear();
			}
		}
		#endregion

		#region SCORE RETRIEVAL
		/// <summary>
		/// Returns a dictionary representing the scores of current round for the different combatants currently in game.
		/// </summary>
		/// <returns></returns>
		public static Dictionary<int, List<ScoreType>> GetAllRoundScores() {
			Debug.Log("Returning round scores for all combatants.");
			return GetScoresForCombatants(scoreDict: combatantRoundScores, combatants: RoundController.instance.Combatants);
		}
		/// <summary>
		/// Returns a dictionary representing the total scores of the different combatants currently in game.
		/// Does not include any points that may be attributed to a round currently in session.
		/// </summary>
		/// <returns></returns>
		public static Dictionary<int, List<ScoreType>> GetAllTotalScores() {
			Debug.Log("Returning round scores for all combatants.");
			return GetScoresForCombatants(scoreDict: combatantTotalScores, combatants: RoundController.instance.Combatants);
		}
		/// <summary>
		/// Compares the list of active combatants with the dictionaries of scores and returns a dictionary with relevant scores
		/// (I.e., no keys will be of combatants who aren't playing.)
		/// </summary>
		/// <param name="scoreDict">The score dict to use for retrieving scores.</param>
		/// <param name="combatants">The actual combatants that need their scores.</param>
		/// <returns></returns>
		private static Dictionary<int, List<ScoreType>> GetScoresForCombatants(Dictionary<int, List<ScoreType>> scoreDict, List<Combatant> combatants) {
			// I am. Experimenting.
			// This is probably overtly complicated but I wanted to see if I could leverage LINQ and also not have tons of ForEach loops.

			return scoreDict                                        // This is the dictionary that is being queried.
				.Where(kvp => combatants                            // Go through all of the combatants that were passed in.
					.Select(c => c.CombatantID)                     // Transform that list of combatants to a list of their IDs. 
					.ToList()                                       // (This line is what actually transforms it to a list.)
					.Contains(kvp.Key))                             // If this list of IDs contains a key from the key value pairs, it should be part of the new dict.
				.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);    // Need to do this line because LINQ.
		}
		/// <summary>
		/// Get the round scores for one combatant in particular.
		/// </summary>
		/// <param name="combatantId"></param>
		/// <returns></returns>
		public static List<ScoreType> GetRoundScores(int combatantId) {
			// Will probably remove this if statement if it never shows up but I'm a little worried so I wanna check for the time being.
			if (RoundController.instance.Combatants.Select(c => c.CombatantID).ToList().Contains(combatantId) == false) {
				throw new System.Exception("You're trying to get the round score of a combatant who's ID isn't actually in the round.");
			} else {
				return combatantRoundScores[combatantId];
			}
		}
		/// <summary>
		/// Gets the total scores for one combatant in particular.
		/// May not include the scores of a round that is currently in session.
		/// </summary>
		/// <param name="combatantId"></param>
		/// <returns></returns>
		public static List<ScoreType> GetTotalScores(int combatantId) {
			// Will explain later why I'm not doing a similar check like I am above but it's not for a reason I'm content with.
			return combatantTotalScores[combatantId];
		}
		#endregion

		#region DEBUG
		/// <summary>
		/// Adds some completely arbitrary scores for testing purposes.
		/// </summary>
		private static void AddDebugScores() {
			AddPoints(combatantId: 0, scoreType: ScoreType.Knockout);
			AddPoints(combatantId: 0, scoreType: ScoreType.Knockout);
			AddPoints(combatantId: 1, scoreType: ScoreType.Knockout);
			AddPoints(combatantId: 1, scoreType: ScoreType.Knockout);
			AddPoints(combatantId: 1, scoreType: ScoreType.Survival);
			AddPoints(combatantId: 2, scoreType: ScoreType.Knockout);
			AddPoints(combatantId: 2, scoreType: ScoreType.Survival);
			AddPoints(combatantId: 3, scoreType: ScoreType.Knockout);
		}
		#endregion

	}

	public enum ScoreType {
		Survival = 0,
		Knockout = 1,
	}
}
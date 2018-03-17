using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using PanicPinnacle.Combatants;
using PanicPinnacle.Menus;

namespace PanicPinnacle.Matches {

	/// <summary>
	/// A class to help keep track of the scores of the different combatants.
	/// </summary>
	public static class ScoreKeeper {

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

		#region SCORE MANAGEMENT
		/// <summary>
		/// Preps the ScoreKeeper to be used for the match with the IDs of the combatants it needs to keep track of.
		/// This means if there is no key, the combatant is not playing.
		/// </summary>
		/// <param name="combatantIds"></param>
		public static void Prepare(List<int> combatantIds) {
			Debug.Log("Preparing score keeper for use with combatant IDs " + combatantIds);
			// Completely flushout the dictionaries.
			combatantTotalScores = new Dictionary<int, List<ScoreType>>();
			combatantRoundScores = new Dictionary<int, List<ScoreType>>();
			// Make new lists for the IDs passed in. Note that only combatants  
			// who are actually in the game will have keys in the dictionaries.
			combatantIds.ForEach(id => combatantTotalScores.Add(key: id, value: new List<ScoreType>()));
			combatantIds.ForEach(id => combatantRoundScores.Add(key: id, value: new List<ScoreType>()));
		}

		/// <summary>
		/// Add points for this round to the specified combatant.
		/// </summary>
		/// <param name="combatantId">The ID of the combatant who is receiving the points.</param>
		/// <param name="scoreType">The type of points to add.</param>
		public static void AddPoints(int combatantId, ScoreType scoreType) {
			Debug.Log("Adding score of type " + scoreType + " to combatant with ID: " + combatantId);
            try
            {
                combatantRoundScores[combatantId].Add(scoreType);
            }
            catch (KeyNotFoundException ex)
            {
                Debug.LogError("CombatantID not found in ScoreKeeper!");
            }
			
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
		/// Checks for whether or not the combatant with the specified ID has an entry in the score.
		/// </summary>
		/// <param name="id">The ID of the combatant that may or may not need their scores.</param>
		/// <param name="tallyscreenType">The type of score to get.</param>
		/// <returns></returns>
		public static bool ContainsCombatant(int id, TallyScreenType tallyscreenType) {
			// Will probably refactor this later, but just check the respective dictionary
			// to see if it has a key with the ID. If there is no key, there is no combatant.
			switch (tallyscreenType) {
				case TallyScreenType.Match:
					return combatantTotalScores.ContainsKey(key: id);
				case TallyScreenType.Round:
					return combatantRoundScores.ContainsKey(key: id);
				default:
					return false;
			}
		}
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
			try {
				return combatantRoundScores[combatantId];
			} catch (System.Exception e) {
				Debug.LogError("Couldn't get scores for combatant with ID " + combatantId + "! Reason: " + e.ToString());
				// If something goes wrong, just return a blank score.
				return new List<ScoreType>();
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
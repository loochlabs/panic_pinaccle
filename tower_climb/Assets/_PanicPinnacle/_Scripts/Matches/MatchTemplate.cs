using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PanicPinnacle.Combatants;

namespace PanicPinnacle.Matches {
	/// <summary>
	/// A serialized data structure that contains the information that will be used to initialize a Mathc at runtime.
	/// </summary>
	[CreateAssetMenu(fileName = "New MatchTemplate", menuName = "Panic Pinnacle/Match Template", order = 1)]
	public class MatchTemplate : SerializedScriptableObject {

		#region FIELDS - METADATA
		/// <summary>
		/// The name of the Match settings.
		/// </summary>
		[TabGroup("Metadata", "Metadata"), PropertyTooltip("The name of the Match settings"), SerializeField]
		private string matchName = "";
		/// <summary>
		/// The name of the match.
		/// </summary>
		public string MatchName {
			get { return matchName; }
		}
		#endregion

		#region FIELDS - MATCH VALUES

		/// <summary>
		/// The name of the scene for the Match Tally
		/// </summary>
		[TabGroup("Match Tally"), PropertyTooltip("The name of the scene for the Match Tally."), SerializeField]
		private string matchTallySceneName = "";
		/// <summary>
		/// The name of the scene for the Match Tally.
		/// </summary>
		public string MatchTallySceneName {
			get {
				return matchTallySceneName;
			}
			set {
				matchTallySceneName = value;
			}
		}

		/// <summary>
		/// The number of rounds in this match.
		/// </summary>
		[TabGroup("Round", "Round"), PropertyTooltip("The number of rounds in this match."), SerializeField]
		private int roundCount = 3;
		/// <summary>
		/// The number of rounds in this match.
		/// </summary>
		public int RoundCount {
			get { return roundCount; }
		}


		/// <summary>
		/// Value of survival score bonus
		/// </summary>
		[TabGroup("Round", "Round"), PropertyTooltip("Value of survival score bonus"), SerializeField]
		private int survivaleScoreValue = 5;
		/// <summary>
		/// Survival score value.
		/// </summary>
		public int SurvivalScoreValue {
			get { return survivaleScoreValue; }
		}

		/// <summary>
		/// Value of knockout score bonus
		/// </summary>
		[TabGroup("Round", "Round"), PropertyTooltip("Value of knockout score bonus"), SerializeField]
		private int knockoutScoreValue = 2;
		/// <summary>
		/// Knockout score value.
		/// </summary>
		public int KnockoutScoreValue {
			get { return knockoutScoreValue; }
		}


		/// <summary>
		/// Player Prefab for entire match.
		/// </summary>
		[TabGroup("Player", "Player"), PropertyTooltip("Player Prefab for entire match.."), SerializeField]
		private GameObject playerPrefab;
		/// <summary>
		/// Player Prefab for entire match.
		/// </summary>
		public GameObject PlayerPrefab {
			get { return playerPrefab; }
		}

		/// <summary>
		/// Combatant templates.
		/// </summary>
		[TabGroup("Player", "Player"), PropertyTooltip("Combatant Templates."), SerializeField]
		private List<CombatantTemplate> combatantTemplates;
		/// <summary>
		/// Combatant templates.
		/// </summary>
		public List<CombatantTemplate> CombatantTemplates {
			get { return combatantTemplates; }
		}

		/// <summary>
		/// The default speed that this combatant should be able to run at.
		/// </summary>
		[TabGroup("Player", "Player"), PropertyTooltip("The maximum number of players in a match."), SerializeField]
		private int maxPlayerCount = 4;
		/// <summary>
		/// The default speed that this combatant should be able to run at.
		/// </summary>
		public float MaxPlayerCount {
			get { return maxPlayerCount; }
		}

		/// <summary>
		/// Player Colors
		/// </summary>
		[TabGroup("Player", "Player"), PropertyTooltip("The colors of players during a match."), SerializeField]
		private List<Color> playerColors = new List<Color>();
		/// <summary>
		/// The default speed that this combatant should be able to run at.
		/// </summary>
		public List<Color> PlayerColors {
			get { return playerColors; }
		}

		/// <summary>
		/// List of predifined Round Settings to pull from when creating queue of rounds for a match
		/// </summary>
		[TabGroup("Round", "Round"), PropertyTooltip("List of predifined Round Settings to pull from when creating queue of rounds for a match"), SerializeField]
		private List<RoundSettings> roundSettings = new List<RoundSettings>();
		/// <summary>
		/// List of predifined Round Settings to pull from when creating queue of rounds for a match
		/// </summary>
		public List<RoundSettings> RoundSettings {
			get { return roundSettings; }
		}

		#endregion

	}
}

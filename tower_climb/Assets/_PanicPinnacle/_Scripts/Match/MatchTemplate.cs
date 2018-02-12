using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace PanicPinnacle.Match
{
    /// <summary>
	/// A serialized data structure that contains the information that will be used to initialize a Mathc at runtime.
	/// </summary>
    [CreateAssetMenu(fileName = "New MatchTemplate", menuName = "Panic Pinnacle/Match Template", order = 1)]
	public class MatchTemplate : SerializedScriptableObject
    {

        #region FIELDS - METADATA
        /// <summary>
        /// The name of the Match settings.
        /// </summary>
        [TabGroup("Metadata", "Metadata"), PropertyTooltip("The name of the Match settings"), SerializeField]
        private string matchName = "";
        /// <summary>
        /// The name of the match.
        /// </summary>
        public string MatchTempalte
        {
            get
            {
                return matchName;
            }
        }
        #endregion

        #region FIELDS - MATCH VALUES

        /// <summary>
        /// Player Prefab for entire match.
        /// </summary>
        [TabGroup("Player", "Player"), PropertyTooltip("Player Prefab for entire match.."), SerializeField]
        private GameObject playerPrefab;

        /// <summary>
        /// Player Prefab for entire match.
        /// </summary>
        public GameObject PlayerPrefab
        {
            get
            {
                return playerPrefab;
            }
        }

        /// <summary>
        /// The default speed that this combatant should be able to run at.
        /// </summary>
        [TabGroup("Player", "Player"), PropertyTooltip("The maximum number of players in a match."), SerializeField]
        private int maxPlayerCount = 4;
        /// <summary>
        /// The default speed that this combatant should be able to run at.
        /// </summary>
        public float MaxPlayerCount
        {
            get
            {
                return maxPlayerCount;
            }
        }

        /// <summary>
        /// Player Colors
        /// </summary>
        [TabGroup("Player", "Player"), PropertyTooltip("The colors of players during a match."), SerializeField]
        private Color[] playerColors = new Color[4];
        /// <summary>
        /// The default speed that this combatant should be able to run at.
        /// </summary>
        public Color[] PlayerColors
        {
            get
            {
                return playerColors;
            }
        }

        #endregion

    }
}

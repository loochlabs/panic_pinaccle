using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeamUtility.IO;

namespace PanicPinnacle.Match { 

    public class MatchManager : MonoBehaviour {

        
        //MATCH SETTINGS
        //      -player colors
        //      -round scoring constants (win, survival bonus, knockouts value)
        [SerializeField]
        private MatchTemplate matchTemplate;


        //singleton global manager
        private static MatchManager _instance;

        #region LOCAL FIELDS

        /// <summary>
        /// Active player count in match.
        /// </summary>
        //@TODO need to account for controls unplugged/plugged in mid match
        private List<PlayerID> activePlayers = new List<PlayerID>();
        //private int activePlayerCount;

        /// <summary>
        /// Active scores for this currnet match.
        /// </summary>
        private Dictionary<PlayerID, List<ScoreType>> scores = new Dictionary<PlayerID, List<ScoreType>>();

        /// <summary>
        /// Current round manager.
        /// </summary>
        private RoundManager roundManager;
        #endregion


        #region UNITY FUNCTIONS

        private void Awake()
        {
            //NOTE: THIS SHOULD BE SETUP DURING PREGAME
            //Setup singleton manager
            if(_instance == null)
            {
                _instance = this;
            }

            //Setup initial players list
            _instance.activePlayers = new List<PlayerID>();
        }

        #endregion

        #region GLOBALS

        /// <summary>
        /// Preset template of Match settings.
        /// </summary>
        public static MatchTemplate MatchTemplate
        {
            get { return _instance.matchTemplate; }
        }

        /// <summary>
        /// Active players, decided during pre-game.
        /// </summary>
        public static List<PlayerID> ActivePlayers
        {
            get { return _instance.activePlayers; }
        }

        /// <summary>
        /// Active RoundManager in scene.
        /// </summary>
        public static RoundManager Round
        {
            get { return _instance.roundManager; }
            set { _instance.roundManager = value; }
        }

        /// <summary>
        /// Add new player to MatchManager.
        /// </summary>
        /// <param name="playerid">PlayerID recognized by controller.</param>
        public static void AddPlayer(PlayerID playerid)
        {
            _instance.activePlayers.Add(playerid);
            _instance.scores[playerid] = new List<ScoreType>();
        }

        /// <summary>
        /// Check if PlayerID exists in the current MatchManager.
        /// </summary>
        /// <param name="playerid">PlayerID to check.</param>
        public static bool HasPlayer(PlayerID playerid)
        {
            return _instance.scores.ContainsKey(playerid);
        }


        /// <summary>
        /// Current total score of the player in this match.
        /// </summary>
        /// <param name="playerid">PlayerID of combatant</param>
        /// <returns></returns>
        public static int Score(PlayerID playerid)
        {
            int score = 0;
            foreach(ScoreType st in _instance.scores[playerid])
            {
                switch (st) {
                    case ScoreType.survival:
                        score += _instance.matchTemplate.SurvivalScoreValue;
                        break;
                    case ScoreType.knockout:
                        score += _instance.matchTemplate.KnockoutScoreValue;
                        break;
                }
            }
            return score; 
        }
        

        /// <summary>
        /// Add to player score with given MatchManager.ScoreType
        /// </summary>
        /// <param name="playerid">PlayerID of combatant</param>
        /// <param name="ammount">ScoreType with defined value in MatchTemplate</param>
        /// <returns>New total score of player</returns>
        public static int AddScore(PlayerID playerid, ScoreType scoreType)
        {
            _instance.scores[playerid].Add(scoreType);
            return Score(playerid);
        }
        
        
        /// <summary>
        /// Destroy and cleanup the current MatchManager.
        /// </summary>
        public static void Destroy()
        {
            Destroy(_instance);
        }
        #endregion

    }
    
    public enum ScoreType
    {
        survival,
        knockout
    }
}

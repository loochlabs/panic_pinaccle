using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //@TODO might want to associate this with InputManager.player
        //      instead of raw int count
        private int activePlayerCount;
        #endregion


        #region UNITY FUNCTIONS

        private void Awake()
        {
            //Setup singleton manager
            if(_instance == null)
            {
                _instance = this;
            }

            //@TODO temp player count
            _instance.activePlayerCount = 4;
        }

        #endregion

        #region GLOBALS

        public static MatchTemplate MatchTemplate
        {
            get { return _instance.matchTemplate; }
        }

        public static int ActivePlayerCount
        {
            get { return _instance.activePlayerCount; }
        }
        #endregion

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeamUtility.IO;

namespace PanicPinnacle.Combatants {

	/// <summary>
	/// An actual, real life player participating in a match.
	/// </summary>
	public class Player : Combatant {

        #region FIELDS
        //@TODO might need a cleaner way of assigning nested objects to this
        [SerializeField]
        private SpriteRenderer playerBodySprite; 
        //round info
        private Color roundColor;
        //represents the player's position/rank this round 
        private int finalRoundPosition;
        #endregion


        #region GETTERS AND SETTERS
        public Color Color {
            get { return roundColor; }
        }
        
        public int FinalRoundPosition
        {
            get { return finalRoundPosition; }
            set { finalRoundPosition = value; }
        }
        #endregion

        #region FUNCTIONS
        public void Prepare(PlayerID playerid, Color color)
        {
            //setup fields
            this.Playerid = playerid;
            roundColor = color;

            //apply round properties to this
            playerBodySprite.color = color;
        }
        
        #endregion
    }

    

}
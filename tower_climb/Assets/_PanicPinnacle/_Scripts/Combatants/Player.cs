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

		#endregion


		#region GETTERS AND SETTERS
		public Color Color {
            get { return roundColor; }
        }
        
        #endregion

        #region FUNCTIONS
        public void Prepare(PlayerID playerid, Color color)
        {
			Debug.Log("Preparing Player with ID: " + playerid);
            //setup fields
            this.Playerid = playerid;
            roundColor = color;

            //apply round properties to this
            playerBodySprite.color = color;
        }
        
        #endregion
    }

    

}
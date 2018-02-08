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
        private PlayerState state = PlayerState.none;
        private int finalRoundPosition;
        #endregion


        #region GETTERS AND SETTERS
        public Color Color {
            get { return roundColor; }
        }
        
        public PlayerState State
        {
            get { return state; }
            set { state = value; }
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

        public void SetState(PlayerState state)
        {
            this.state = state;
            switch (state) {
                case PlayerState.none:
                    break;
                case PlayerState.intro:
                    break;
                case PlayerState.playing:
                    break;
                case PlayerState.dead:
                    break;
                case PlayerState.dazed:
                    break;
                case PlayerState.outro:
                    break;
            }

        }

        #endregion
    }

    #region ENUMS
    public enum PlayerState
    {
        none = 0,
        intro = 1,
        playing = 2,
        dead = 3,
        dazed = 4,
        outro = 5
    }
    #endregion

}
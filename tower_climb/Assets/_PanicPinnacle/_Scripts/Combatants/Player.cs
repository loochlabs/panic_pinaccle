using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Matches;
using cakeslice;

namespace PanicPinnacle.Combatants {

	/// <summary>
	/// An actual, real life player participating in a match.
	/// </summary>
	[RequireComponent(typeof(PlayerPhysicsBody)), RequireComponent(typeof(PlayerAnimator))]
	public class Player : Combatant {

        #region SCENE REFS

        //audio
        private AudioSource audio;

        #endregion


        #region PREPARATION
        /// <summary>
        /// Prepares this combatant with the information stored in a CombatantTemplate.
        /// </summary>
        /// <param name="combatantTemplate">The template to use for initialization.</param>
        /// <param name="combatantId">The ID that will be assigned to this combatant..</param>
        public override void Prepare(CombatantTemplate combatantTemplate, int combatantId) {
			// Call the base so that the default settings are in place.
			base.Prepare(combatantTemplate, combatantId);
			Debug.Log("Preparing Player with ID: " + combatantId);
			//setup fields

			
			Debug.LogWarning("SET THE BODY");
            
            //Prepare round properties of this player
			GetComponentInChildren<SpriteRenderer>().color =
                MatchController.instance.CurrentMatchSettings.MatchTemplate.PlayerColors[combatantId];
            GetComponentInChildren<Outline>().color = combatantId;
            //Outline ot = GetComponentInChildren<Outline>();
            //ot.color = combatantId;
            //ot.enabled = false;
            //ot.enabled = true;

            //sfx
            audio = gameObject.GetComponentInChildren<AudioSource>();
        }

        #endregion

        #region GAME EVENTS

        /// <summary>
        /// Knockout player if they touch environment bounds.
        /// </summary>
        public void Knockout()
        {
            Debug.Log("Player " + CombatantID + " knockedout!");

            this.SetState(CombatantStateType.dead);

            if (Aggressor)
            {
                ScoreKeeper.AddPoints(combatantId: Aggressor.CombatantID, scoreType: ScoreType.Knockout);
            }

            //sfx
            audio.PlayOneShot(DataController.instance.GetSFX(SFXType.BoundKnockout));

            //cleanup player
            combatantBody.Rigidbody.bodyType = RigidbodyType2D.Kinematic;
            this.enabled = false;
        }

        public void GoalTouch()
        {
            Debug.Log("Player " + CombatantID + " completed goal!");

            this.SetState(CombatantStateType.safe);

            //scoring
            ScoreKeeper.AddPoints(combatantId: CombatantID, scoreType: ScoreType.Survival);
            
            //sfx
            audio.PlayOneShot(DataController.instance.GetSFX(SFXType.PlayerTouchGoal));
            //TODO: play animation

            //cleanup player
            combatantBody.StopVertical();
            combatantBody.StopHorizontal();
            combatantBody.Rigidbody.bodyType = RigidbodyType2D.Kinematic;
            this.enabled = false;
        }

        #endregion
    }
}
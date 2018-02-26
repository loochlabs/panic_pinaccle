using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Matches;

namespace PanicPinnacle.Combatants {

	/// <summary>
	/// An actual, real life player participating in a match.
	/// </summary>
	[RequireComponent(typeof(PlayerPhysicsBody)), RequireComponent(typeof(PlayerAnimator))]
	public class Player : Combatant {


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

			Debug.LogWarning("PLEASE REMOVE THIS SWTICH CASE LATER WHEN I FIX UP THE ANIMATORS");
            //Prepare round properties of this player
			GetComponentInChildren<SpriteRenderer>().color =
                MatchController.instance.CurrentMatchSettings.MatchTemplate.PlayerColors[combatantId];
            
        }

		#endregion
	}



}
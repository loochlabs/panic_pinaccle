using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Combatants.Behaviors.Updates {

	/// <summary>
	/// The standard FixedUpdate behavior. Just checks CombatantInput and executes in response to that.
	/// </summary>
	[System.Serializable]
	public class StandardFixedUpdateBehavior : CombatantFixedUpdateBehavior {

		/// <summary>
		/// The standard FixedUpdate behavior. Just checks CombatantInput and executes in response to that.
		/// </summary>
		/// <param name="combatant">The combatant who owns this behavior.</param>
		public override void FixedUpdate(Combatant combatant) {
			// Apply a force to the comabtant body based on the movement input.
			combatant.CombatantBody.AddForce(
				direction: combatant.CombatantInput.GetMovementDirection(combatant: combatant),
				magnitude: combatant.CombatantTemplate.RunSpeed);

			// Also check if the combatant is trying to jump and if they're grounded.
			if (combatant.CombatantInput.GetJumpInput(combatant: combatant) == true && combatant.CombatantBody.IsGrounded == true) {
				Debug.Log("JUMP");
				// If they're able to jump, add that force amount.
				combatant.CombatantBody.AddForce(y: combatant.CombatantTemplate.JumpPower);
			}
		}
		
		#region INSPECTOR JUNK
		private static string behaviorDescription = "The standard FixedUpdate behavior. Just checks CombatantInput and executes in response to that.";
		protected override string InspectorDescription {
			get {
				return behaviorDescription;
			}
		}
		#endregion

	}
}
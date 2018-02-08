using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Combatants.Behaviors.Updates {

	/// <summary>
	/// The standard FixedUpdate behavior. Just checks CombatantInput and executes in response to that.
	/// </summary>
	[System.Serializable]
	public class StandardFixedUpdateBehavior : CombatantFixedUpdateBehavior {

        #region FIELDS

        private Vector3 moveDirection = Vector3.zero;

        #endregion

        /// <summary>
        /// So far nothing really needs to be prepared for the standard fixed update behavior.
        /// </summary>
        /// <param name="combatant"></param>
        public override void Prepare(Combatant combatant) {
		
		}
		/// <summary>
		/// The standard FixedUpdate behavior. Just checks CombatantInput and executes in response to that.
		/// </summary>
		/// <param name="combatant">The combatant who owns this behavior.</param>
		public override void FixedUpdate(Combatant combatant) {
            Debug.Log("input axis: " + combatant.CombatantInput.GetMovementDirection(combatant: combatant).ToString());
            //Horizontal Movement
            moveDirection = combatant.CombatantInput.GetMovementDirection(combatant: combatant);
            moveDirection.y = 0;
            moveDirection.z = 0;
            combatant.CombatantBody.AddForce(
                direction: this.moveDirection,
				magnitude: combatant.CombatantTemplate.RunSpeed);

            //Vertical Movement 
            //jump
            if(combatant.CombatantInput.GetMovementDirection(combatant: combatant).y < 0
                && combatant.CombatantBody.IsGrounded)
            {
                Debug.Log("JUMP");
                // If they're able to jump, add that force amount.
                combatant.CombatantBody.AddForce(y: combatant.CombatantTemplate.JumpPower);
            }

			// Also check if the combatant is trying to jump and if they're grounded.
			if (combatant.CombatantInput.GetJumpInput(combatant: combatant) && combatant.CombatantBody.IsGrounded) {
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
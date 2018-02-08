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

        //Movement direction of joystick inputs
        private Vector3 moveDirection = Vector3.zero;

        //Track if a jump is active
        //prevents user from holding UP and jumping repeatedly 
        private bool jumpActive;

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
            //Horizontal Movement
            moveDirection = combatant.CombatantInput.GetMovementDirection(combatant: combatant);
            moveDirection.y = 0;
            moveDirection.z = 0;
            if (moveDirection.magnitude > 0) {
                combatant.CombatantBody.AddForce(
                    direction: this.moveDirection,
                    magnitude: combatant.CombatantTemplate.RunSpeed);
            }
            else
            {
                //no player input -> hard stop horizontal
                combatant.CombatantBody.StopHorizontal();
            }

            //Vertical Movement 
            //reset vertical velocity
            if (combatant.CombatantBody.IsGrounded)
            {
                combatant.CombatantBody.StopVertical();

                //reset jump once player is grounded and releases joystick from UP
                if(jumpActive && combatant.CombatantInput.GetMovementDirection(combatant: combatant).y >= 0)
                {
                    jumpActive = false;
                }
            }

            //jump
            if (combatant.CombatantInput.GetMovementDirection(combatant: combatant).y < 0
                && combatant.CombatantBody.IsGrounded
                && !jumpActive)
            {
                // If they're able to jump, add that force amount.
                combatant.CombatantBody.AddForce(y: combatant.CombatantTemplate.JumpPower);
                jumpActive = true;
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Events;

namespace PanicPinnacle.Combatants.Behaviors {

	/// <summary>
	/// Defines how a combatant should move.
	/// </summary>
	[System.Serializable]
	public class MovementBehavior : CombatantBehavior, IUpdateEvent {

		#region FIELDS
		//Track if a jump is active
		//prevents user from holding UP and jumping repeatedly 
		private bool jumpActive;
		#endregion

		#region PREPARATION
		/// <summary>
		/// So far nothing really needs to be prepared for the standard fixed update behavior.
		/// </summary>
		/// <param name="combatant"></param>
		public override void Prepare(Combatant combatant) {

		}
		#endregion

		#region INTERFACE IMPLEMENTATION - IUPDATEEVENT
		public void Update(CombatantEventParams eventParams) {
			Combatant combatant = eventParams.combatant;

			//DAZED
			//@TODO add vfx feedback to dazed state
			if (combatant.State == CombatantStateType.dazed) { return; }
			if (combatant.State == CombatantStateType.punching) { return; }
			if (combatant.State == CombatantStateType.dead) { return; }

			//ORIENTATION
			//grab a reference here
			Vector3 inputDirection = combatant.CombatantInput.GetMovementDirection(combatant: combatant);
			if (inputDirection.x > 0) { combatant.Orientation = OrientationType.E; }
			if (inputDirection.x < 0) { combatant.Orientation = OrientationType.W; }
			if (inputDirection.y > 0) { combatant.Orientation = OrientationType.S; }
			if (inputDirection.y < 0) { combatant.Orientation = OrientationType.N; }

			//Horizontal Movement
			//calc all directional movement, AddForce after all calculations complete
			Vector3 moveDirection = Vector3.zero;

			//no player input -> hard stop horizontal
			if (inputDirection.x == 0) {
				combatant.CombatantBody.StopHorizontal();
			} else {
				moveDirection.x = inputDirection.x * combatant.CombatantTemplate.RunSpeed;
			}

			//Vertical Movement 
			//reset vertical velocity
			if (combatant.CombatantBody.IsGrounded) {
				combatant.CombatantBody.StopVertical();

				//reset jump once player is grounded and releases joystick from UP
				if (jumpActive && inputDirection.y >= 0) {
					jumpActive = false;
				}
			}

			//jump
			if (inputDirection.y < 0
				&& combatant.CombatantBody.IsGrounded
				&& !jumpActive) {
				// If they're able to jump, add that force amount.
				//combatant.CombatantBody.AddForce(y: combatant.CombatantTemplate.JumpPower);
				moveDirection.y = combatant.CombatantTemplate.JumpPower;
				jumpActive = true;
			}

			//add movement force with all conditions
			combatant.CombatantBody.AddForce(moveDirection);
		}
		#endregion

		#region INSPECTOR JUNK
		private static string inspectorDescription = "The behavior that allows the combatant to move around.";
		protected override string InspectorDescription {
			get {
				return inspectorDescription;
			}
		}
		#endregion

	}
}
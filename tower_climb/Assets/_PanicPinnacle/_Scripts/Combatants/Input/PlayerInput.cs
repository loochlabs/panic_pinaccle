using System.Collections;
using System.Collections.Generic;
using PanicPinnacle.Combatants;
using UnityEngine;
using TeamUtility.IO;

namespace PanicPinnacle.Input {

	/// <summary>
	/// The way in which a player's input will interface with something like. Idk, Rewired.
	/// I described the base class as "thinking" but since the thinking is being done by the irl player, it just grabs input.
	/// </summary>
	[System.Serializable]
	public class PlayerInput : CombatantInput {

        #region FIELDS
        /// <summary>
        /// Direction of joystick movement axis
        /// </summary>
        private Vector3 direction = Vector3.zero;
		/// <summary>
		/// PlayerID assigned to this for InputManager. Pull from a manager from Pregame setup.
		/// </summary>
		private PlayerInputID playerInputID;
		#endregion

		#region PREPARATION
		public override void Prepare(Combatant combatant) {
			// Save the ID of the player input. The enum actually begins at 1, so I need to add one, otherwise the player with the ID of 0 will throw errors.
			this.playerInputID = (PlayerInputID)(combatant.CombatantID + 1);
		}
		#endregion

		#region INPUT GETTERS
		/// <summary>
		/// Gets the direction of movement for this Player.
		/// </summary>
		/// <param name="combatant">The combatant requesting their movement direction.</param>
		/// <returns>The direction the Player is trying to move towards..</returns>
		public override Vector3 GetMovementDirection(Combatant combatant) {
			// Assemble those two axes into a new vector and return it.
			direction.x = InputManager.GetAxisRaw("Horizontal", this.playerInputID);
			direction.y = InputManager.GetAxisRaw("Vertical", this.playerInputID);
			return direction;
		}
		/// <summary>
		/// Grabs whether or not this combatant is trying to jump.
		/// </summary>
		/// <param name="combatant">The combatant that may or may not be trying to jump.</param>
		/// <returns>Whether or not this combatant is trying to jump.</returns>
		public override bool GetJumpInput(Combatant combatant) {
			// Just get the value of the jump button being pressed.
			//return InputManager.GetButtonDown("Jump", combatant.Playerid);
			return false; //@TEMP;
		}
		/// <summary>
		/// Grabs whether or not this combatant is trying to punch.
		/// </summary>
		/// <param name="combatant">The combatant that may or may not be trying to punch.</param>
		/// <returns>Whether or not this combatatant is trying to punch.</returns>
		public override bool GetPunchInput(Combatant combatant) {
			// Just get the value of the punch button being pressed.
			return InputManager.GetButtonDown("PrimaryAction", this.playerInputID);
		}
		#endregion



		#region FIELDS - INSPECTOR JUNK
		/// <summary>
		/// A static string for the description so that every OnInspectorGUI doesn't create and throw away a new string every frame or whatever.
		/// </summary>
		private static string inputDescription = "Produces the combatant's input by calling the input manager. All Players will need to use this.";
		/// <summary>
		/// The string that gets used in the info box that describes this CombatantInput.
		/// </summary>
		protected override string InspectorDescription {
			get {
				return inputDescription;
			}
		}
		#endregion
	}
}
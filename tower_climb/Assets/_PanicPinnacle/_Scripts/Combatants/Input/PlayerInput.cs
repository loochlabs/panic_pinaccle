using System.Collections;
using System.Collections.Generic;
using PanicPinnacle.Combatants;
using UnityEngine;

namespace PanicPinnacle.Input {

	/// <summary>
	/// The way in which a player's input will interface with something like. Idk, Rewired.
	/// I described the base class as "thinking" but since the thinking is being done by the irl player, it just grabs input.
	/// </summary>
	[System.Serializable]
	public class PlayerInput : CombatantInput {

		#region FIELDS

		/// <summary>
		/// The class provided by Rewired which grabs input from a given controller.
		/// </summary>
		private Rewired.Player rewiredPlayer;
		#endregion

		#region PREPARATION
		public override void Prepare(Combatant combatant) {
			Debug.Log("Grabbing Rewired Player for combatant with ID " + combatant.CombatantID);
			// Grab the rewired player associated with the combatant's ID.
			this.rewiredPlayer = Rewired.ReInput.players.GetPlayer(playerId: combatant.CombatantID);
		}
		#endregion

		#region INPUT GETTERS
		/// <summary>
		/// Gets the direction of movement for this Player.
		/// </summary>
		/// <param name="combatant">The combatant requesting their movement direction.</param>
		/// <returns>The direction the Player is trying to move towards..</returns>
		public override Vector3 GetMovementDirection(Combatant combatant) {
			// Ask rewired for the vector representing the movement direction.
			return this.rewiredPlayer.GetAxis2DRaw(xAxisActionName: "Move Horizontal", yAxisActionName: "Move Vertical");
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
			return this.rewiredPlayer.GetButtonDown("Punch");
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
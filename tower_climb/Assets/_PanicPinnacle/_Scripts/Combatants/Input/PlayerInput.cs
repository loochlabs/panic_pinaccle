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

		/// <summary>
		/// Gets the direction of movement for this Player.
		/// </summary>
		/// <param name="combatant">The combatant requesting their movement direction.</param>
		/// <returns>The direction the Player is trying to move towards..</returns>
		public override Vector3 GetMovementDirection(Combatant combatant) {
			// TODO: Replace this with something that doesn't call UnityEngine.Input. It's. It's not very good.
			float horizontalInput = UnityEngine.Input.GetAxisRaw("Horizontal");
			float verticalInput = UnityEngine.Input.GetAxisRaw("Vertical");
			// Assemble those two axes into a new vector and return it.
			return new Vector3(x: horizontalInput, y: verticalInput, z: 0f);
		}
	}


}
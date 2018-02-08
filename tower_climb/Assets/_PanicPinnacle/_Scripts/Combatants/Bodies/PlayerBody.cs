using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace PanicPinnacle.Combatants {

	/// <summary>
	/// The component that takes care of forces and interactions with things in the environment for the Player.
	/// Might not be a whole lot different.
	/// </summary>
	public class PlayerBody : CombatantBody {

		#region FIELDS - FLAGS
		/// <summary>
		/// Is the combatant grounded?
		/// </summary>
		public override bool IsGrounded {
			get {
				// Go through each of the transforms that exist to check if theyre next to the ground.
				foreach (Transform groundTransform in this.groundTransforms) {
					// Figure out if a linecast hits a ground collider.
					bool linecastHit = Physics2D.Linecast(
						start: this.transform.position,
						end: groundTransform.position, 
						layerMask: 1 << LayerMask.NameToLayer("Ground"));
					// If it hits just one, the body is grounded.
					if (linecastHit) {
						return true;
					}
				}
				// If nothing worked, that means the body is not grounded. Return false.
				return false;
			}
		}
		#endregion

		#region FIELDS - SCENE REFERENCES
		/// <summary>
		/// The transforms used to check if the PlayerBody is on the ground or in the air.
		/// </summary>
		[TabGroup("Physics", "Physics"), PropertyTooltip("The transforms used to check if the PlayerBody is on the ground or in the air."), SerializeField]
		private List<Transform> groundTransforms = new List<Transform>();
		#endregion

		#region FIELDS - INSPECTOR JUNK
		/// <summary>
		/// A static string for the description so that every OnInspectorGUI doesn't create and throw away a new string every frame or whatever.
		/// </summary>
		private static string playerBodyDescription = "The body that handles a lot of the physics and collision events on the Player.";
		/// <summary>
		/// The string that gets used in the info box that describes this CombatantInput.
		/// </summary>
		protected override string InspectorDescription {
			get {
				return playerBodyDescription;
			}
		}
		#endregion
	}

}
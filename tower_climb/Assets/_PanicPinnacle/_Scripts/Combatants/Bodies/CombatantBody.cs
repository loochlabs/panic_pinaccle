using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Combatants {

	/// <summary>
	/// The component that takes care of forces and interactions with things in the environment for the Combatant.
	/// </summary>
	public abstract class CombatantBody : MonoBehaviour {

		#region FIELDS - SCENE REFERENCES
		/// <summary>
		/// The RigidBody2D component attached to this body.
		/// Needed so I can actually apply forces and whatnot.
		/// </summary>
		private Rigidbody2D rigidBody;
		#endregion

		private void Awake() {
			// Grab a reference to the rigidbody.
			this.rigidBody = GetComponent<Rigidbody2D>();
		}

		#region FORCES
		/// <summary>
		/// Applies a force to this CombatantBody.
		/// </summary>
		/// <param name="force">The vector of force to be applied to this body. Will not be normalized.</param>
		public void AddForce(Vector3 force) {
			// Just add it to the rigidbody.
			this.rigidBody.AddForce(force);
		}
		/// <summary>
		/// Applies a force to this CombatantBody.
		/// </summary>
		/// <param name="direction">The direction of force to be applied to this body. Will be normalized.</param>
		/// <param name="magnitude">The magnitude of force to be applied to this body.</param>
		public void AddForce(Vector3 direction, float magnitude) {
			// Normalize the direction and multiply it by the magnitude.
			this.rigidBody.AddForce(direction.normalized * magnitude);
		}
		#endregion

	}

}
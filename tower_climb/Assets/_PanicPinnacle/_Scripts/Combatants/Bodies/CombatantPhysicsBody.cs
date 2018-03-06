using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PanicPinnacle.Events;

#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif
namespace PanicPinnacle.Combatants {

	/// <summary>
	/// The component that takes care of forces and interactions with things in the environment for the Combatant.
	/// </summary>
	public abstract class CombatantPhysicsBody : MonoBehaviour {

		#region FIELDS - FLAGS
		/// <summary>
		/// Is the combatant grounded?
		/// </summary>
		public abstract bool IsGrounded { get; }
		#endregion

		#region FIELDS - PHYSICS
		/// <summary>
		/// The gravity scale for this combatant's body.
		/// </summary>
		public float GravityScale {
			get {
				return this.rigidBody.gravityScale;
			}
			set {
				this.rigidBody.gravityScale = value;
			}
		}
		#endregion

		#region FIELDS - SCENE REFERENCES
		/// <summary>
		/// The Combatant this body is attached to.
		/// </summary>
		protected Combatant combatant;
		/// <summary>
		/// The RigidBody2D component attached to this body.
		/// Needed so I can actually apply forces and whatnot.
		/// </summary>
		protected Rigidbody2D rigidBody;
		#endregion

		private void Awake() {
			// Grab a reference to the rigidbody.
			this.rigidBody = GetComponent<Rigidbody2D>();
			// Also get a reference to the Combatant.
			this.combatant = GetComponent<Combatant>();
		}

		#region FORCES - ADDING
		/// <summary>
		/// Applies a force to this CombatantBody.
		/// </summary>
		/// <param name="force">The vector of force to be applied to this body. Will not be normalized.</param>
		public void AddForce(Vector3 force) {
			// Just add it to the rigidbody.
			this.rigidBody.AddForce(force);
			// Clamp the rigidbody's velocity to ensure it doesn't go too fast.
			this.rigidBody.velocity = this.ClampBodyVelocity(combatant: this.combatant, currentVelocity: this.rigidBody.velocity);
		}
		/// <summary>
		/// Applies a force to this CombatantBody.
		/// </summary>
		/// <param name="direction">The direction of force to be applied to this body. Will be normalized.</param>
		/// <param name="magnitude">The magnitude of force to be applied to this body.</param>
		public void AddForce(Vector3 direction, float magnitude) {
			// Normalize the direction and multiply it by the magnitude.
			this.rigidBody.AddForce(direction.normalized * magnitude);
			// Clamp the rigidbody's velocity to ensure it doesn't go too fast.
			this.rigidBody.velocity = this.ClampBodyVelocity(combatant: this.combatant, currentVelocity: this.rigidBody.velocity);
		}
		/// <summary>
		/// Applies a force to this CombatantBody.
		/// </summary>
		/// <param name="x">The x-component of the force.</param>
		/// <param name="y">The y-component of the force.</param>
		/// <param name="z">The z-component of the force.</param>
		public void AddForce(float x = 0f, float y = 0f, float z = 0f) {
			this.AddForce(force: new Vector3(x, y, z));
		}
		/// <summary>
		/// Returns a new velocity vector if the body is going too fast.
		/// </summary>
		/// <param name="combatant">The combatant this body is attached to.</param>
		/// <param name="velocity">The current velocity of the body.</param>
		/// <returns></returns>
		private Vector3 ClampBodyVelocity(Combatant combatant, Vector3 currentVelocity) {
			// Get the values of the x/y values clamped.
			float x = Mathf.Clamp(value: currentVelocity.x, min: -combatant.CombatantTemplate.MaxVelocity.x, max: combatant.CombatantTemplate.MaxVelocity.x);
			float y = Mathf.Clamp(value: currentVelocity.y, min: -combatant.CombatantTemplate.MaxVelocity.y, max: combatant.CombatantTemplate.MaxVelocity.y);
			float z = 0;
			// Return it as a new vector.
			return new Vector3(x, y, z);
		}
		#endregion

		#region FORCES - STOPPING
		/// <summary>
		/// Hard stop this body's horizontal velocity. 
		/// </summary>
		public void StopHorizontal() {
			this.rigidBody.velocity = new Vector2(x: 0f, y: this.rigidBody.velocity.y);
		}
		/// <summary>
		/// Hard stop this body's horizontal velocity. 
		/// </summary>
		public void StopVertical() {
			rigidBody.velocity = new Vector2(x: this.rigidBody.velocity.x, y: 0f);
		}
		#endregion

		#region COLLISIONS
		public virtual void OnCollisionEnter2D(Collision2D collision) {
			//player knockout
			//@TODO: might want to make this a functional call when fleshing out Round Manager
			if (collision.gameObject.tag == "Bound") {
				Debug.Log("KNOCKOUT! " + combatant);
				this.combatant.SetState(CombatantStateType.dead);
				Debug.LogWarning("UPDATE ACTIVE PLAYER COUNT");
				// combatant.FinalRoundPosition = MatchManager.Round.PlayerActiveCount--; 
			}

			//goal check
			if (collision.gameObject.tag == "Goal") {
				Debug.Log("ROUND WIN: " + combatant);
				this.combatant.SetState(CombatantStateType.dead); //@TODO: dead for now, might be outro state instead
				Debug.LogWarning("UPDATE ACTIVE PLAYER COUNT");
				// combatant.FinalRoundPosition = ++MatchManager.Round.PlayerCompleteCount;
				// MatchManager.Round.PlayerActiveCount--;
			}
		}
		public virtual void OnCollisionExit2D(Collision2D collision) { }
		public virtual void OnCollisionStay2D(Collision2D collision) { }
		public virtual void OnTriggerEnter2D(Collider2D collision) {
			this.combatant.CallEvent<IOnTriggerEnter2DEvent>(new CombatantEventParams(combatant: this.combatant, item: null, collision: collision));
		}
		public virtual void OnTriggerExit2D(Collider2D collision) {
			this.combatant.CallEvent<IOnTriggerExit2DEvent>(new CombatantEventParams(combatant: this.combatant, item: null, collision: collision));
		}
		public virtual void OnTriggerStay2D(Collider2D collision) {
			this.combatant.CallEvent<IOnTriggerStay2DEvent>(new CombatantEventParams(combatant: this.combatant, item: null, collision: collision));
		}
		#endregion

		#region FIELDS - INSPECTOR JUNK
#if UNITY_EDITOR
		/// <summary>
		/// This is what I need to use for making sure info boxes appear in the inspector without actually having to assign a field to accompany it.
		/// </summary>
		[PropertyOrder(int.MinValue), OnInspectorGUI]
		private void DrawIntroInfoBox() {
			SirenixEditorGUI.InfoMessageBox(this.InspectorDescription);
		}
#endif
		/// <summary>
		/// The string that gets used in the info box that describes this CombatantInput.
		/// </summary>
		protected abstract string InspectorDescription { get; }
		#endregion

	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PanicPinnacle.Input;
using PanicPinnacle.Combatants.Behaviors.Updates;

namespace PanicPinnacle.Combatants {

	/// <summary>
	/// A representation of a combatant participating in a round.
	/// Used as an entry point for most other forms of interaction.
	/// </summary>
	public abstract class Combatant : MonoBehaviour {

		#region FIELDS - TEMPLATE
		/// <summary>
		/// The template that is used to prepare this combatant.
		/// May be null in non-debug builds.
		/// </summary>
		[SerializeField]
		private CombatantTemplate combatantTemplate;
		/// <summary>
		/// The template that is used to prepare this combatant.
		/// May be null in non-debug builds.
		/// </summary>
		public CombatantTemplate CombatantTemplate {
			get {
				return this.combatantTemplate;
			}
		}
		#endregion

		#region FIELDS - BEHAVIORS AND INPUT
		/// <summary>
		/// The implementation for how this combatant should handle FixedUpdate() calls.
		/// </summary>
		private CombatantFixedUpdateBehavior fixedUpdateBehavior;
		/// <summary>
		/// The class that provides the input for the combatant.
		/// </summary>
		private CombatantInput combatantInput;
		/// <summary>
		/// The class that provides the input for the combatant.
		/// </summary>
		public CombatantInput CombatantInput {
			get {
				return this.combatantInput;
			}
		}
		#endregion

		#region FIELDS - SCENE REFERENCES
		/// <summary>
		/// A reference to the body that handles the physics for this Combatant.
		/// </summary>
		private CombatantBody combatantBody;
		/// <summary>
		/// A reference to the body that handles the physics for this Combatant.
		/// </summary>
		public CombatantBody CombatantBody {
			get {
				return this.combatantBody;
			}
		}
		#endregion

		#region UNITY FUNCTIONS
		private void Awake() {
			// Find the CombatantBody attached to this Combatant.
			this.combatantBody = GetComponentInChildren<CombatantBody>();
		}
		private void Start() {
			// Prep the combatant with the information it needs from the template.
			this.Prepare(combatantTemplate: this.combatantTemplate);
		}
		private void FixedUpdate() {
			// Allow the CombatantFixedUpdateBehavior to handle implementation of the FixedUpdate call.
			this.fixedUpdateBehavior.FixedUpdate(combatant: this);
		}
		#endregion


		#region PREPARATION
		/// <summary>
		/// Prepares this combatant with the information stored in a CombatantTemplate.
		/// </summary>
		/// <param name="combatantTemplate">The template to use for initialization.</param>
		public void Prepare(CombatantTemplate combatantTemplate) {
			// Save a reference to the template, because it will be needed.
			this.combatantTemplate = combatantTemplate;
			// Grab the CombatantInput from the template. Remember that this returns as a clone.
			this.combatantInput = combatantTemplate.CombatantInput;
			// Also grab the CombatantFixedUpdateBehavior. This is also returned as a clone.
			this.fixedUpdateBehavior = combatantTemplate.FixedUpdateBehavior;
		}
		#endregion

	}

}
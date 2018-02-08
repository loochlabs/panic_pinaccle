using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace PanicPinnacle.Combatants.Behaviors.Updates {

	/// <summary>
	/// The behavior that allows a combatant to punch.
	/// NOTE: REFERENCES THINGS THAT MUST BE IN SCENE. PLEASE REFACTOR THIS LATER.
	/// </summary>
	[System.Serializable]
	public class PunchFixedUpdateBehavior : CombatantFixedUpdateBehavior {

		#region FIELDS - FLAGS
		/// <summary>
		/// Can this combatant punch right now?
		/// </summary>
		private bool CanPunch {
			get {
				// Just returning true for rn.
				// TODO: Calculate the conditions for punching and see if this combatant can actually punch.
				return true;
			}
		}
		#endregion

		#region FIELDS - POWER
		/// <summary>
		/// What is the magnitude of the force that will propel the combatant when they actually use the punch move?
		/// </summary>
		[TabGroup("Punch Behavior", "Attributes"), PropertyTooltip("What is the magnitude of the force that will propel the combatant when they actually use the punch move?"), SerializeField]
		private float propellentForceMagnitude = 10f;
		/// <summary>
		/// What is the magnitude of the force that this combatant will inflict on other combatants when they use the punch move?
		/// </summary>
		[TabGroup("Punch Behavior", "Attributes"), PropertyTooltip("What is the magnitude of the force that this combatant will inflict on other combatants when they use the punch move?"), SerializeField]
		private float impactForceMagnitude = 10f;
		/// <summary>
		/// How much should the CombatantBody's gravity be scaled by when the combatant punches?
		/// </summary>
		[TabGroup("Punch Behavior", "Attributes"), PropertyTooltip("How much should the CombatantBody's gravity be scaled by when the combatant punches?"), SerializeField]
		private float bodyGravityModifier = 0.5f;
		/// <summary>
		/// How long should this punch be active for?
		/// </summary>
		[TabGroup("Punch Behavior", "Attributes"), PropertyTooltip("How long should this punch be active for?"), SerializeField]
		private float punchDuration = 0.5f;
		#endregion

		#region FIELDS - SCENE REFERENCES
		/// <summary>
		/// A reference to the GameObject that contains the box used for when this combatant punches.
		/// </summary>
		private GameObject punchBoxGameObject;
		#endregion

		/// <summary>
		/// Make sure this behavior has access to the parts of the combatant that are related to the punching and whatnot.
		/// </summary>
		/// <param name="combatant">The combatant this behavior is being assigned to.</param>
		public override void Prepare(Combatant combatant) {
			Debug.LogWarning("NOTE: This will fail if the combatant does not have the proper objects as part of their children. See if this can be refactored.");

		}
		/// <summary>
		/// Checks whether or not this combatant wants to punch and, if they do, does so.
		/// </summary>
		/// <param name="combatant">The combatant who owns this behavior.</param>
		public override void FixedUpdate(Combatant combatant) {
			// First, see if the combatant is trying to punch and if they are allowed to.
			if (combatant.CombatantInput.GetPunchInput(combatant: combatant) == true && this.CanPunch == true) {

			}
		}

		#region INSPECTOR JUNK
		private static string behaviorDescription = "Allows the combatant to use the Punch move.";
		protected override string InspectorDescription {
			get {
				return behaviorDescription;
			}
		}
		#endregion

	}
}
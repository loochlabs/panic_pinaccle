using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Combatants.Behaviors.Updates {

	/// <summary>
	/// The behavior that allows a combatant to punch.
	/// NOTE: REFERENCES THINGS THAT MUST BE IN SCENE. PLEASE REFACTOR THIS LATER.
	/// </summary>
	[System.Serializable]
	public class PunchFixedUpdateBehavior : CombatantFixedUpdateBehavior {

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
			// Just making sure that punches can get called with the new setup.
			// Press G to pay respects.
			if (UnityEngine.Input.GetKeyDown(KeyCode.G)) {
				Debug.Log("OH yea.... punches???? they can get called..... :weary:");
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
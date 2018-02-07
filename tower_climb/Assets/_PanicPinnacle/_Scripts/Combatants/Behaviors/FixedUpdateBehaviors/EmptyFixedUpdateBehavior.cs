using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Combatants.Behaviors.Updates {

	/// <summary>
	/// An empty method that doesn't do anything. Good for if the combatant just needs to wait.
	/// </summary>
	[System.Serializable]
	public class EmptyFixedUpdateBehavior : CombatantFixedUpdateBehavior {

		#region INSPECTOR JUNK
		private static string behaviorDescription = "An empty method that doesn't do anything. Good for if the combatant just needs to wait.";
		protected override string InspectorDescription {
			get {
				return behaviorDescription;
			}
		}
		#endregion

		/// <summary>
		/// Does nothing for this combatant on FixedUpdate.
		/// </summary>
		/// <param name="combatant">The combatant who owns this behavior.</param>
		public override void FixedUpdate(Combatant combatant) {
			// https://www.youtube.com/watch?v=-RFunvF0mDw
		}
	}


}
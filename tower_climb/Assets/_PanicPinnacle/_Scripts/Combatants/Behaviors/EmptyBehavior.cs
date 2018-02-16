using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Combatants.Behaviors {

	/// <summary>
	/// A blank behavior that does nothing. Mostly to fill a void if an error ever occurs.
	/// </summary>
	[System.Serializable]
	public class EmptyBehavior : CombatantBehavior {
		
		public override void Prepare(Combatant combatant) {
			// asdf
		}

		#region INSPECTOR JUNK
		private static string inspectorDescription = "A blank behavior that does nothing. Mostly to fill a void if an error ever occurs.";
		protected override string InspectorDescription {
			get {
				return inspectorDescription;
			}
		}
		#endregion

	}
}
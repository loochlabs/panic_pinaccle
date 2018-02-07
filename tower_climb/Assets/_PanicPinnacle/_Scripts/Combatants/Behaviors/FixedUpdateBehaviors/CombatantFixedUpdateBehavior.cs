using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Combatants;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

namespace PanicPinnacle.Combatants.Behaviors.Updates {

	/// <summary>
	/// The behavior that should get run on a Combatant every FixedUpdate call.
	/// </summary>
	public abstract class CombatantFixedUpdateBehavior {

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

		#region CLONING
		/// <summary>
		/// Clones a CombatantFixedUpdateBehavior.
		/// </summary>
		/// <param name="fixedUpdateBehavior">The CombatantFixedUpdateBehavior to clone.</param>
		/// <returns>A cloned version of the passed in CombatantFixedUpdateBehavior.</returns>
		public static CombatantFixedUpdateBehavior Clone(CombatantFixedUpdateBehavior fixedUpdateBehavior) {
			return (CombatantFixedUpdateBehavior)fixedUpdateBehavior.MemberwiseClone();
		}
		#endregion

		/// <summary>
		/// The implementation of FixedUpdate for the combatant.
		/// </summary>
		/// <param name="combatant">The combatant who owns this FixedUpdate behavior.</param>
		public abstract void FixedUpdate(Combatant combatant);

	}


}
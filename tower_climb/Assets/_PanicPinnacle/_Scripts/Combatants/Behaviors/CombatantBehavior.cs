using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

namespace PanicPinnacle.Combatants.Behaviors {

	/// <summary>
	/// A generalized class that can be customized with interfaces that define events. Used for a sort of ad-hoc customization.
	/// (I realized having a class dedicated specifically to FixedUpdate wasn't going to cut it.)
	/// </summary>
	public abstract class CombatantBehavior {

		/// <summary>
		/// Preps this behavior with the default information it needs to get going.
		/// </summary>
		/// <param name="combatant">The combatant this behavior is being assigned to.</param>
		public abstract void Prepare(Combatant combatant);


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
		/// Clones a CombatantBehavior and prepares it for use by the specified combatant.
		/// </summary>
		/// <param name="behavior">The CombatantBehavior to clone.</param>
		/// <param name="combatant">The combatant this CombatantBehavior is going to be added to.</param>
		/// <returns>A cloned version of the passed in CombatantBehavior.</returns>
		public static CombatantBehavior CloneAndPrepare(CombatantBehavior behavior, Combatant combatant) {
			// Create a clone of the behavior.
			CombatantBehavior clone = (CombatantBehavior)behavior.MemberwiseClone();
			try {
				// Try to prepare the cloned behavior. There may be behaviors that this will just fail completely on depending on the context.
				clone.Prepare(combatant: combatant);
			} catch (System.Exception e) {
				// Log out the error that was just caught.
				Debug.LogError("Couldn't prepare CombatantBehavior for combatant " + combatant.CombatantTemplate.CombatantName + "! Reason: " + e);
				// Since the preparation failed, just make the clone a new instance of an empty behavior.
				// This means it will remain in the combatant's list, but its functionality will be empty
				clone = new EmptyBehavior();
				// Prepare it because why not.
				clone.Prepare(combatant: combatant);
			}

			// All set! Return it.
			return clone;
		}
		#endregion

	}


}
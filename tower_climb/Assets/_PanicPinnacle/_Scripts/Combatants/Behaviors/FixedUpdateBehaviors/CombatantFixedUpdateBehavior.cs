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
        
        /// <summary>
        /// Preps this behavior with the default information it needs to get going.
        /// </summary>
        /// <param name="combatant">The combatant this behavior is being assigned to.</param>
        public abstract void Prepare(Combatant combatant);
		/// <summary>
		/// The implementation of FixedUpdate for the combatant.
		/// </summary>
		/// <param name="combatant">The combatant who owns this FixedUpdate behavior.</param>
		public abstract void FixedUpdate(Combatant combatant);

        //Collision Wrappers
        public abstract void OnCollisionEnter2D(Combatant combatant, Collision2D collision);
        public abstract void OnCollisionExit2D(Combatant combatant, Collision2D collision);
        public abstract void OnCollisionStay2D(Combatant combatant, Collision2D collision);
        public abstract void OnTriggerEnter2D(Combatant combatant, Collider2D collision);
        public abstract void OnTriggerExit2D(Combatant combatant, Collider2D collision);
        public abstract void OnTriggerStay2D(Combatant combatant, Collider2D collision);

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
		/// Clones a CombatantFixedUpdateBehavior and prepares it for use by the specified combatant.
		/// </summary>
		/// <param name="fixedUpdateBehavior">The CombatantFixedUpdateBehavior to clone.</param>
		/// <param name="combatant">The combatant this FixedUpdateBehavior is going to be added to.</param>
		/// <returns>A cloned version of the passed in CombatantFixedUpdateBehavior.</returns>
		public static CombatantFixedUpdateBehavior CloneAndPrepare(CombatantFixedUpdateBehavior fixedUpdateBehavior, Combatant combatant) {
			// Create a clone of the behavior.
			CombatantFixedUpdateBehavior clone = (CombatantFixedUpdateBehavior)fixedUpdateBehavior.MemberwiseClone();
			try {
				// Try to prepare the cloned behavior. There may be behaviors that this will just fail completely on depending on the context.
				clone.Prepare(combatant: combatant);
			} catch (System.Exception e) {
				// Log out the error that was just caught.
				Debug.LogError("Couldn't prepare CombatantFixedUpdateBehavior for combatant " + combatant.CombatantTemplate.CombatantName + "! Reason: " + e);
				// Since the preparation failed, just make the clone a new instance of an empty behavior.
				// This means it will remain in the combatant's list, but its functionality will be empty.
				clone = new EmptyFixedUpdateBehavior();
				// Prepare it because why not.
				clone.Prepare(combatant: combatant);
			}
			
			// All set! Return it.
			return clone;
		}
		#endregion
	}
}
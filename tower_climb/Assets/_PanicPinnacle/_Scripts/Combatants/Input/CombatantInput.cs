﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Combatants;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif
namespace PanicPinnacle.Input {

	/// <summary>
	/// A class that serves as an interface for how a combatant will input their moves.
	/// Think of it as the class that actually thinks about how this combatant wants to move.
	/// </summary>
	public abstract class CombatantInput {

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
		/// Clones the CombatantInput so that it can be used without affecting its origin.
		/// (I.e., usually it is stored in a CombatantTemplate, but modifying it will also modify its state in the ScriptableObject.
		/// </summary>
		/// <param name="combatantInput">The CombatantInput to clone.</param>
		/// <returns>A clone of the CombatantInput.</returns>
		public static CombatantInput Clone(CombatantInput combatantInput) {
			return (CombatantInput)combatantInput.MemberwiseClone();
		}
		#endregion

		/// <summary>
		/// Grabs the direction of movement that this combatant is attempting to move towards.
		/// </summary>
		/// <param name="combatant">The combatant who is requesting their movement direction.</param>
		/// <returns>The direction of movement for this combatant.</returns>
		public abstract Vector3 GetMovementDirection(Combatant combatant);
		/// <summary>
		/// Grabs whether or not this combatant is trying to jump.
		/// </summary>
		/// <param name="combatant">The combatant that may or may not be trying to jump.</param>
		/// <returns>Whether or not this combatant is trying to jump.</returns>
		public abstract bool GetJumpInput(Combatant combatant);
	}


}
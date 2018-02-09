﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PanicPinnacle.Input;
using PanicPinnacle.Combatants.Behaviors.Updates;

namespace PanicPinnacle.Combatants {

	/// <summary>
	/// A serialized data structure that contains the information that will be used to initialize a Combatant at runtime.
	/// </summary>
	public abstract class CombatantTemplate : SerializedScriptableObject {

		#region FIELDS - METADATA
		/// <summary>
		/// The name of the combatant.
		/// </summary>
		[TabGroup("Metadata", "Metadata"), PropertyTooltip("The name of the combatant"), SerializeField]
		private string combatantName = "";
		/// <summary>
		/// The name of the combatant.
		/// </summary>
		public string CombatantName {
			get {
				return this.combatantName;
			}
		}
		#endregion

		#region FIELDS - BEHAVIOR AND INPUT
		/// <summary>
		/// The class to be used for this combatant's input.
		/// </summary>
		[TabGroup("Behavior", "Input"), PropertyTooltip("The class to be used for this combatant's input."), SerializeField]
		private CombatantInput combatantInput;
		/// <summary>
		/// The class to be used for this combatant's input.
		/// Returns as a clone of its state in the CombatantTemplate.
		/// </summary>
		public CombatantInput CombatantInput {
			get {
				Debug.Log("Cloning CombatantInput from template for " + this.CombatantName);
				return CombatantInput.Clone(combatantInput: this.combatantInput);
			}
		}
		/// <summary>
		/// The classes that should be called every FixedUpdate on the Combatant.
		/// Handles implementation for that FixedUpdate call.
		/// </summary>
		[TabGroup("Behavior", "Fixed Update"), PropertyTooltip("The classes that should be called every FixedUpdate on the Combatant."), SerializeField]
		private List<CombatantFixedUpdateBehavior> fixedUpdateBehaviors = new List<CombatantFixedUpdateBehavior>();
		/// <summary>
		/// Gets the list of behaviors that should be called every FixedUpdate on the combatant.
		/// Order matters.
		/// </summary>
		/// <param name="combatant">The combatant these FixedUpdateBehaviors will be applied to. Needed so that it can be prepared before setting.</param>
		/// <returns>The CombatantFixedUpdateBehaviors, fully prepared for use by the combatant.</returns>
		public List<CombatantFixedUpdateBehavior> GetFixedUpdateBehaviors(Combatant combatant) {
			Debug.Log("Cloning CombatantFixedUpdateBehaviors from template for " + this.CombatantName);
			// Create a new list to store the FixedUpdateBehaviors.
			List<CombatantFixedUpdateBehavior> clonedBehaviors = new List<CombatantFixedUpdateBehavior>();
			// Go through each behavior in this template and clone/prep it.
			foreach (CombatantFixedUpdateBehavior fixedUpdateBehavior in this.fixedUpdateBehaviors) {
				clonedBehaviors.Add(CombatantFixedUpdateBehavior.CloneAndPrepare(fixedUpdateBehavior: fixedUpdateBehavior, combatant: combatant));
			}
			// After all the behaviors are cloned and prepped, return the new list.
			return clonedBehaviors;
		}
		#endregion

		#region FIELDS - PHYSICS
		/// <summary>
		/// The default speed that this combatant should be able to run at.
		/// </summary>
		[TabGroup("Physics", "Physics"), PropertyTooltip("The default speed that this combatant should be able to run at."), SerializeField]
		private float runSpeed = 365f;
		/// <summary>
		/// The default speed that this combatant should be able to run at.
		/// </summary>
		public float RunSpeed {
			get {
				return this.runSpeed;
			}
		}
		/// <summary>
		/// The default power that this combatant can jump at.
		/// </summary>
		[TabGroup("Physics", "Physics"), PropertyTooltip("The default power that this combatant can jump at."), SerializeField]
		private float jumpPower = 1000f;
		/// <summary>
		/// The default power that this combatant can jump at.
		/// </summary>
		public float JumpPower {
			get {
				return this.jumpPower;
			}
		}
		/// <summary>
		/// The maximum velocity that the body is allowed to move. Contains definition for both axes.
		/// </summary>
		[TabGroup("Physics", "Physics"), PropertyTooltip("The maximum velocity that the body is allowed to move. Contains definition for both axes."), SerializeField]
		private Vector2 maxVelocity = new Vector2();
		/// <summary>
		/// The maximum velocity that the body is allowed to move. Contains definition for both axes.
		/// </summary>
		public Vector2 MaxVelocity {
			get {
				return this.maxVelocity;
			}
		}

        /// <summary>
        /// Duration of a dazed effect (after a player has been punched)
        /// </summary>
        [TabGroup("Physics", "Physics"), PropertyTooltip("Duration of a dazed effect (after a player has been punched)."), SerializeField]
        private float dazeDuration = 1f;

        public float DazeDuration
        {
            get { return dazeDuration; }
        }
        #endregion

    }

}

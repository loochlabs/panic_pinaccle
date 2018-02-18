using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PanicPinnacle.Input;
using PanicPinnacle.Combatants.Behaviors;

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
		public CombatantInput GetCombatantInput(Combatant combatant) {
			Debug.Log("Cloning CombatantInput from template for " + this.CombatantName);
			CombatantInput combatantInput = CombatantInput.Clone(combatantInput: this.combatantInput);
			combatantInput.Prepare(combatant: combatant);
			return combatantInput;
		}
		/// <summary>
		/// The behaviors that determine how this combatant acts.
		/// </summary>
		[TabGroup("Behavior", "Behavior"), PropertyTooltip("The behaviors that determine how this combatant acts."), SerializeField]
		private List<CombatantBehavior> combatantBehaviors = new List<CombatantBehavior>();
		/// <summary>
		/// Gets the list of behaviors that define how this combatant acts.
		/// </summary>
		/// <param name="combatant">The combatant these behaviors will be applied to. Needed so that it can be prepared before setting.</param>
		/// <returns>The CombatantBehaviors, fully prepared for use by the combatant.</returns>
		public List<CombatantBehavior> GetCombatantBehaviors(Combatant combatant) {
			Debug.Log("Cloning CombatantBehaviors from template for " + this.CombatantName);
			// Create a new list to store the CombatantBehaviors.
			List<CombatantBehavior> clonedBehaviors = new List<CombatantBehavior>();
			// Go through each behavior in this template and clone/prep it.
			foreach (CombatantBehavior combatantBehavior in this.combatantBehaviors) {
				clonedBehaviors.Add(CombatantBehavior.CloneAndPrepare(behavior: combatantBehavior, combatant: combatant));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PanicPinnacle.Input;
using PanicPinnacle.Combatants.Behaviors.Updates;
using TeamUtility.IO;

namespace PanicPinnacle.Combatants {

	/// <summary>
	/// A representation of a combatant participating in a round.
	/// Used as an entry point for most other forms of interaction.
	/// </summary>
	public abstract class Combatant : MonoBehaviour {

		#region FIELDS - TEMPLATE
		/// <summary>
		/// The template that is used to prepare this combatant.
		/// May be null in non-debug builds.
		/// </summary>
		[SerializeField]
		private CombatantTemplate combatantTemplate;
		/// <summary>
		/// The template that is used to prepare this combatant.
		/// May be null in non-debug builds.
		/// </summary>
		public CombatantTemplate CombatantTemplate {
			get {
				return this.combatantTemplate;
			}
		}
		#endregion

		#region FIELDS - BEHAVIORS AND INPUT
		/// <summary>
		/// A list of behaviors that this combatant should run every FixedUpdate.
		/// Order matters if one behavior depends on the results of another.
		/// </summary>
		private List<CombatantFixedUpdateBehavior> fixedUpdateBehaviors = new List<CombatantFixedUpdateBehavior>();
		/// <summary>
		/// The class that provides the input for the combatant.
		/// </summary>
		private CombatantInput combatantInput;
		/// <summary>
		/// The class that provides the input for the combatant.
		/// </summary>
		public CombatantInput CombatantInput {
			get {
				return this.combatantInput;
			}
		}
		#endregion

		#region FIELDS - SCENE REFERENCES
		/// <summary>
		/// A reference to the body that handles the physics for this Combatant.
		/// </summary>
		private CombatantBody combatantBody;
		/// <summary>
		/// A reference to the body that handles the physics for this Combatant.
		/// </summary>
		public CombatantBody CombatantBody {
			get {
				return this.combatantBody;
			}
		}

        /// <summary>
        /// PlayerID assigned to this for InputManager. Pull from a manager from Pregame setup.
        /// </summary>
        private PlayerID playerid;

        public PlayerID Playerid {
            get { return playerid; }
            set { playerid = value; }
        }

        /// <summary>
        /// Current orientation of the player.
        /// This will be the last known directional input.
        /// </summary>
        private OrientationType orientation;

        public OrientationType Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        /// <summary>
        /// All the different states a player can be in a given round.
        /// </summary>
        private CombatantState state = CombatantState.none;

        public CombatantState State
        {
            get { return state; }
        }
        #endregion

        #region UNITY FUNCTIONS
        private void Awake() {
			// Find the CombatantBody attached to this Combatant.
			this.combatantBody = GetComponentInChildren<CombatantBody>();
		}
		private void Start() {
			// Prep the combatant with the information it needs from the template.
			this.Prepare(combatantTemplate: this.combatantTemplate);
		}
		private void FixedUpdate() {
			// Go through each CombatantFixedUpdateBehavior and run it. (As of right now, there's only like. Two/Three.)
			foreach (CombatantFixedUpdateBehavior fixedUpdateBehavior in this.fixedUpdateBehaviors) {
				fixedUpdateBehavior.FixedUpdate(combatant: this);
			}
		}
        private void OnTriggerEnter2D(Collider2D collision)
        {
            foreach (CombatantFixedUpdateBehavior fixedUpdateBehavior in this.fixedUpdateBehaviors)
            {
                fixedUpdateBehavior.OnTriggerEnter2D(combatant: this, collision: collision);
            }
        }
        #endregion


        #region PREPARATION
        /// <summary>
        /// Prepares this combatant with the information stored in a CombatantTemplate.
        /// </summary>
        /// <param name="combatantTemplate">The template to use for initialization.</param>
        public void Prepare(CombatantTemplate combatantTemplate) {
			// Save a reference to the template, because it will be needed.
			this.combatantTemplate = combatantTemplate;
			// Grab the CombatantInput from the template. Remember that this returns as a clone.
			this.combatantInput = combatantTemplate.CombatantInput;
			// Also grab a list of the FixedUpdateBehaviors. This also preps them for use. Handy!
			this.fixedUpdateBehaviors = combatantTemplate.GetFixedUpdateBehaviors(combatant: this);
		}
        #endregion

        /// <summary>
        /// Control state here. Seperate function call for plans on extra functionallity when changing states.
        /// </summary>
        /// <param name="state"></param>
        public void SetState(CombatantState state)
        {
            this.state = state;
        }
    }


    #region ENUMS
    public enum CombatantState
    {
        none = 0,
        intro = 1,
        playing = 2,
        dead = 3,
        dazed = 4,
        outro = 5
    }
    #endregion
}
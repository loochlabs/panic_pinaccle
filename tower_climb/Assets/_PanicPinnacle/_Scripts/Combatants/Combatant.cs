using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PanicPinnacle.Input;
using PanicPinnacle.Combatants.Behaviors;
using System.Linq;
using PanicPinnacle.Events;
using PanicPinnacle.Items;
using PanicPinnacle.UI;
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
		[ShowInInspector, ReadOnly, HideInEditorMode]
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

		#region FIELDS - METADATA AND STATE
		/// <summary>
		/// The identifier for this particular combatant.
		/// </summary>
		private int combatantId = -1;
		/// <summary>
		/// The identifier for this particular combatant.
		/// </summary>
		public int CombatantID {
			get {
				return this.combatantId;
			}
		}
		/// <summary>
		/// Current orientation of the player.
		/// This will be the last known directional input.
		/// </summary>
		private OrientationType orientation;
		/// <summary>
		/// Current orientation of the player.
		/// This will be the last known directional input.
		/// </summary>
		public OrientationType Orientation {
			get { return orientation; }
			set { orientation = value; }
		}
		/// <summary>
		/// All the different states a player can be in a given round.
		/// </summary>
		private CombatantStateType state = CombatantStateType.none;
		/// <summary>
		/// All the different states a player can be in a given round.
		/// </summary>
		public CombatantStateType State {
			get { return state; }
		}

        /// <summary>
        /// Keep track of aggressor againt combatant for knockout score.
        /// </summary>
        private Combatant aggressor;

        /// <summary>
        /// Keep track of aggressor againt combatant for knockout score.
        /// </summary>
        public Combatant Aggressor {
            get { return aggressor; }
            set { aggressor = value; }
        }

		#endregion

		#region FIELDS - BEHAVIORS AND INPUT	
		/// <summary>
		/// A list of behaviors that define how this combatant should act.
		/// </summary>
		private List<CombatantBehavior> combatantBehaviors = new List<CombatantBehavior>();
		/// <summary>
		/// The list of items in this combatant's posession.
		/// </summary>
		private List<Item> items = new List<Item>();
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
		protected CombatantPhysicsBody combatantBody;
		/// <summary>
		/// A reference to the body that handles the physics for this Combatant.
		/// </summary>
		public CombatantPhysicsBody CombatantBody {
			get {
				return this.combatantBody;
			}
		}
		/// <summary>
		/// The class that handles the visuals/animations of the combatant.
		/// </summary>
		protected CombatantAnimator combatantAnimator;
		/// <summary>
		/// The class that handles the visuals/animations of the combatant.
		/// </summary>
		public CombatantAnimator CombatantAnimator {
			get {
				return this.combatantAnimator;
			}
		}
		#endregion

		#region FIELDS - STANDARD EVENT PARAMS
		/// <summary>
		/// A "standard" params object that exists so I don't need to create a new one every single frame.
		/// Only use this for events that don't require items or collisions.
		/// May refactor so this isn't needed, but I'll see.
		/// </summary>
		private CombatantEventParams standardCombatantEventParams;
		/// <summary>
		/// A "standard" params object that exists so I don't need to create a new one every single frame.
		/// Only use this for events that don't require items or collisions.
		/// May refactor so this isn't needed, but I'll see.
		/// </summary>
		public CombatantEventParams StandardCombatantEventParams {
			get {
				if (this.standardCombatantEventParams == null) {
					this.standardCombatantEventParams = new CombatantEventParams(combatant: this, item: null, collision: null);
				}
				return this.standardCombatantEventParams;
			}
		}
		#endregion

		#region FIELDS - UI
		/// <summary>
		/// The status to be used for this combatant's UI. May or may not get rid of this but I say that about everything don't I.
		/// </summary>
		private PlayerStatus combatantStatus;
		#endregion

		#region UNITY FUNCTIONS
		private void Awake() {
			// Find the CombatantBody attached to this Combatant.
			this.combatantBody = GetComponent<CombatantPhysicsBody>();
			// Also get the animator.
			this.combatantAnimator = GetComponent<CombatantAnimator>();
		}
		private void Update() {
			this.CallEvent<IUpdateEvent>(eventParams: this.StandardCombatantEventParams);
		}
		private void FixedUpdate() {
			this.CallEvent<IFixedUpdateEvent>(eventParams: this.StandardCombatantEventParams);
		}

        private void OnTriggerEnter2D(Collider2D collision)
        {
            CombatantEventParams cep = StandardCombatantEventParams;
            cep.collision = collision;
            CallEvent<IOnTriggerEnter2DEvent>(eventParams: cep);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            CombatantEventParams cep = StandardCombatantEventParams;
            cep.collision = collision;
            CallEvent<IOnTriggerExit2DEvent>(eventParams: cep);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            CombatantEventParams cep = StandardCombatantEventParams;
            cep.collision = collision;
            CallEvent<IOnTriggerStay2DEvent>(eventParams: cep);
        }
        #endregion



        #region PREPARATION
        /// <summary>
        /// Prepares this combatant with the information stored in a CombatantTemplate.
        /// </summary>
        /// <param name="combatantTemplate">The template to use for initialization.</param>
        /// <param name="combatantId">The ID that will be assigned to this combatant..</param>
        public virtual void Prepare(CombatantTemplate combatantTemplate, int combatantId) {
			// Save the combatant ID. It will be needed during the preparation process as well.
			this.combatantId = combatantId;
			// Save a reference to the template, because it will be needed.
			this.combatantTemplate = combatantTemplate;
			// Grab the CombatantInput from the template. Remember that this returns as a clone.
			this.combatantInput = combatantTemplate.GetCombatantInput(combatant: this);
			// Grab the behaviors from the template. This also preps them for use.
			this.combatantBehaviors = combatantTemplate.GetCombatantBehaviors(combatant: this);

			// Find the PlayerStatus that has the CombatantID that matches up with this CombatantID.
			List<PlayerStatus> matches = FindObjectsOfType<PlayerStatus>().Where(ps => ps.CombatantID == this.CombatantID).ToList();
			if (matches.Count > 0) {
				this.combatantStatus = matches[0];
				this.combatantStatus.Prepare(this);
			}
			
		}
		#endregion

		#region EVENTS
		/// <summary>
		/// Calls the specified event on the behaviors that define how this combatant should act, as well as its items.
		/// </summary>
		/// <typeparam name="T">The type of event that is being called.</typeparam>
		/// <param name="eventParams"></param>
		public void CallEvent<T>(CombatantEventParams eventParams) {
			// Go through each of the behaviors.
			foreach (CombatantBehavior combatantBehavior in this.combatantBehaviors) {
				// If the behavior implements the specified interface, call its first method.
				if (combatantBehavior is T) {
					System.Reflection.MethodInfo method = typeof(T).GetMethods()[0];
					method.Invoke(obj: combatantBehavior, parameters: new object[] { eventParams });
				}
			}
			// Also do this same process for the items.
			foreach (Item item in this.items) {
				item.CallEvent<T>(eventParams: eventParams);
			}
		}
		#endregion

		#region ITEMS
		/// <summary>
		/// Adds an item to the combatant's inventory.
		/// </summary>
		/// <param name="item">The item to add to the combatant's inventory.</param>
		public void AddItem(Item item) {
			this.items.Add(item);
			// Invoke the event on the item that tells it that it was just added to the combatant.
			item.CallEvent<IOnItemAddEvent>(new CombatantEventParams(combatant: this, item: item, collision: null));
		}
		/// <summary>
		/// Removes an item from the combatant's inventory.
		/// </summary>
		/// <param name="item">The item to remove.</param>
		/// <returns>Whether the item was removed or not. This happens if attempting to remove an item that isn't actually there.</returns>
		public bool RemoveItem(Item item) {
			// Attempt to remove the item.
			if (this.items.Remove(item) == true) {
				// If it was successfully removed, call the event that says it just did that.
				item.CallEvent<IOnItemRemoveEvent>(new CombatantEventParams(combatant: this, item: item, collision: null));
				return true;
			} else {
				// If it wasn't found, return false.
				return false;
			}
		}
		#endregion

		#region STATE
		/// <summary>
		/// Control state here. Seperate function call for plans on extra functionallity when changing states.
		/// </summary>
		/// <param name="state"></param>
		public void SetState(CombatantStateType state) {
			this.state = state;
		}
		#endregion
	}


	#region ENUMS
	public enum CombatantStateType {
		none = 0,
		intro = 1,
		playing = 2,
		dead = 3,
		dazed = 4,
		punching = 5,
		outro = 6
	}
	#endregion
}
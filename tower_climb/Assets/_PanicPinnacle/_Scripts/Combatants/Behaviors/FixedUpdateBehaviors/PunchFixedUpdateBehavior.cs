using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace PanicPinnacle.Combatants.Behaviors.Updates {

	/// <summary>
	/// The behavior that allows a combatant to punch.
	/// NOTE: REFERENCES THINGS THAT MUST BE IN SCENE. PLEASE REFACTOR THIS LATER.
	/// </summary>
	[System.Serializable]
	public class PunchFixedUpdateBehavior : CombatantFixedUpdateBehavior {

		#region FIELDS - ATTRIBUTES
		/// <summary>
		/// What is the magnitude of the force that will propel the combatant when they actually use the punch move?
		/// </summary>
		[TabGroup("Punch Behavior", "Attributes"), PropertyTooltip("What is the magnitude of the force that will propel the combatant when they actually use the punch move?"), SerializeField]
		private float propellentForceMagnitude = 10f;
		/// <summary>
		/// What is the magnitude of the force that this combatant will inflict on other combatants when they use the punch move?
		/// </summary>
		[TabGroup("Punch Behavior", "Attributes"), PropertyTooltip("What is the magnitude of the force that this combatant will inflict on other combatants when they use the punch move?"), SerializeField]
		private float impactForceMagnitude = 10f;
		/// <summary>
		/// How much should the CombatantBody's gravity be scaled by when the combatant punches?
		/// </summary>
		[TabGroup("Punch Behavior", "Attributes"), PropertyTooltip("How much should the CombatantBody's gravity be scaled by when the combatant punches?"), SerializeField]
		private float bodyGravityModifier = 0.5f;
		/// <summary>
		/// How long should this punch be active for?
		/// </summary>
		[TabGroup("Punch Behavior", "Attributes"), PropertyTooltip("How long should this punch be active for?"), SerializeField]
		private float punchDuration = 0.5f;
        #endregion

        #region FIELDS - FLAGS AND TIMERS
		/// <summary>
		/// Can this combatant punch right now?
		/// </summary>
		private bool canPunch = true;
		/// <summary>
		/// Can this combatant punch right now?
		/// </summary>
		private bool CanPunch {
			get {
				// Just returning canPunch for rn. I actually don't like using a flag like this.
				// TODO: Calculate the conditions for punching and see if this combatant can actually punch.
				return this.canPunch;
			}
		}
        
        #endregion

        #region FIELDS - SCENE REFERENCES
        /// <summary>
        /// A reference to the GameObject that contains the box used for when this combatant punches.
        /// </summary>
        private GameObject punchBoxGameObject;

        //collection of targets active in punch hitbox
        private List<Player> targetsToPunch = new List<Player>();
        #endregion

        /// <summary>
        /// Make sure this behavior has access to the parts of the combatant that are related to the punching and whatnot.
        /// </summary>
        /// <param name="combatant">The combatant this behavior is being assigned to.</param>
        public override void Prepare(Combatant combatant) {
			Debug.Log("NOTE: This will fail if the combatant does not have the proper objects as part of their children. See if this can be refactored.");
			// Look for the punch box in this combatant's children.
			this.punchBoxGameObject = combatant.gameObject.transform.Find("Punch Box").gameObject;
			// If it wasn't found, throw an error. Usually I'm calling Prepare() from something that will catch this.
			if (this.punchBoxGameObject == null) {
				throw new System.Exception("Couldn't find the Punch Box on this combatant! Is it named properly?");
			} else {
				// If it WAS found, turn it off. It might already be off but do it anyway. Thanks.
				this.punchBoxGameObject.SetActive(false);
			}
			// Reset the canPunch flag. Hopefully I won't need it later.
			this.canPunch = true;
		}
		/// <summary>
		/// Checks whether or not this combatant wants to punch and, if they do, does so.
		/// </summary>
		/// <param name="combatant">The combatant who owns this behavior.</param>
		public override void FixedUpdate(Combatant combatant) {
			// First, see if the combatant is trying to punch and if they are allowed to.
			if (combatant.CombatantInput.GetPunchInput(combatant: combatant) == true && this.CanPunch == true) {

                //set Orientation for punch box
                Vector3 punchboxRotation = Vector3.zero;
                switch (combatant.Orientation) {
                    case OrientationType.N:
                        punchboxRotation.z = 0;
                        break;
                    case OrientationType.W:
                        punchboxRotation.z = 90;
                        break;
                    case OrientationType.S:
                        punchboxRotation.z = 180;
                        break;
                    case OrientationType.E:
                        punchboxRotation.z = 270;
                        break;
                }
                punchBoxGameObject.transform.localEulerAngles = punchboxRotation;


				// Create a new sequence.
				// TODO: See if I can just make this in Prepare() and reuse it. If it's playing, it would mean CanPunch is false.
				Sequence seq = DOTween.Sequence();
				// Append a callback that starts up the punch.
				seq.AppendCallback(new TweenCallback(delegate {
					// Disable the canPunch flag.
					this.canPunch = false;
					// Turn on the punch box game object.
					this.punchBoxGameObject.SetActive(true);
					Debug.LogWarning("Need to implement ability to move/rotate punch box based on input.");
				}));
				// Wait for the punch duration.
				seq.AppendInterval(interval: this.punchDuration);
				// Wrap up the punch.
				seq.AppendCallback(new TweenCallback(delegate {
					// Make sure the combatant can punch again.
					this.canPunch = true;
					// Turn the punch box game object back off.
					this.punchBoxGameObject.SetActive(false);
				}));

				// Play the sequence of events I just wrote out.
				seq.Play();
			}
		}
        

        public override void OnTriggerEnter2D(Combatant combatant, Collider2D collision)
        {
            if (collision.tag == "Player" && collision.gameObject.GetComponent<Player>().Playerid != combatant.Playerid)
            {
                Debug.Log("PUNCH ENTER: " + combatant.Playerid + " into " + collision.gameObject.GetComponent<Player>().Playerid);
            }
        }

        public override void OnTriggerExit2D(Combatant combatant, Collider2D collision)
        {
            if (collision.tag == "Player" && collision.gameObject.GetComponent<Player>().Playerid != combatant.Playerid)
            {
                Debug.Log("PUNCH EXIT: " + combatant.Playerid + " into " + collision.gameObject.GetComponent<Player>().Playerid);
            }
        }



        #region UNUSED WRAPPERS

        public override void OnCollisionEnter2D(Combatant combatant, Collider2D collision)
        {
            throw new System.NotImplementedException();
        }

        public override void OnCollisionExit2D(Combatant combatant, Collider2D collision)
        {
            throw new System.NotImplementedException();
        }

        public override void OnCollisionStay2D(Combatant combatant, Collider2D collision)
        {
            throw new System.NotImplementedException();
        }

        public override void OnTriggerStay2D(Combatant combatant, Collider2D collision)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region INSPECTOR JUNK
        private static string behaviorDescription = "Allows the combatant to use the Punch move.";
		protected override string InspectorDescription {
			get {
				return behaviorDescription;
			}
		}
		#endregion

	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using PanicPinnacle.Events;

namespace PanicPinnacle.Combatants.Behaviors {

	/// <summary>
	/// The behavior that allows the combatant to punch.
	/// </summary>
	[System.Serializable]
	public class PunchBehavior : CombatantBehavior, IFixedUpdateEvent, IOnTriggerEnter2DEvent, IOnTriggerExit2DEvent {

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
		private float bodyGravityModifier = 0.05f;
		/// <summary>
		/// How long should this punch be active for?
		/// </summary>
		[TabGroup("Punch Behavior", "Attributes"), PropertyTooltip("How long should this punch be active for?"), SerializeField]
		private float punchDuration = 0.5f;
		#endregion

		#region FIELDS - SCENE REFERENCES
		/// <summary>
		/// A reference to the GameObject that contains the box used for when this combatant punches.
		/// </summary>
		private GameObject punchBoxGameObject;
		/// <summary>
		/// Collection of targets active in punch hitbox
		/// </summary>
		private List<Player> targetsToPunch;
		/// <summary>
		/// Original grav modifier at beginning of scene
		/// </summary>
		private float originalGravModifier;
		#endregion

		#region PREPARATION
		/// <summary>
		/// Preps this behavior for use.
		/// </summary>
		/// <param name="combatant">The combatant that is using this behavior.</param>
		public override void Prepare(Combatant combatant) {
			Debug.Log("NOTE: This will fail if the combatant does not have the proper objects as part of their children. See if this can be refactored.");
			// Look for the punch box in this combatant's children.
			this.punchBoxGameObject = combatant.gameObject.transform.Find("Punch Box").gameObject;
			// If it wasn't found, throw an error. Usually I'm calling Prepare() from something that will catch this.
			if (this.punchBoxGameObject == null) {
				throw new System.Exception("Couldn't find the Punch Box on this combatant! Is it named properly?");
			} else {
				// If it WAS found, turn it off. It might already be off but do it anyway. Thanks.
				//this.punchBoxGameObject.SetActive(false);
			}

			originalGravModifier = combatant.CombatantBody.GravityScale;
			targetsToPunch = new List<Player>();
		}
		#endregion

		#region PUNCHING
		/// <summary>
		/// Apply punch logic on target
		///  combatant --PUNCH--> target
		/// </summary>
		/// <param name="combatant"></param>
		/// <param name="target"></param>
		private void PunchCombatant(Combatant combatant, Combatant target) {
			//impact direction to send targets
			Vector2 impactDirection = Vector2.zero;
			target.SetState(CombatantStateType.dazed);

			//calculate trajectory of impact
			Vector3 dv = target.transform.position - combatant.transform.position;
			if (dv.x > 0) { impactDirection.x = 1; }
			if (dv.x < 0) { impactDirection.x = -1; }
			if (dv.y > 0) { impactDirection.y = 1; }
			if (dv.y < 0) { impactDirection.y = -1; }

			target.CombatantBody.AddForce(impactDirection, impactForceMagnitude);

			//Daze target for specified daze duration
			Sequence dazeSeq = DOTween.Sequence();
			dazeSeq.AppendCallback(new TweenCallback(delegate {
				target.SetState(CombatantStateType.dazed);
			}));
			dazeSeq.AppendInterval(target.CombatantTemplate.DazeDuration);
			dazeSeq.AppendCallback(new TweenCallback(delegate {
				target.SetState(CombatantStateType.playing);
			}));
			dazeSeq.Play();
		}
		#endregion

		#region INTERFACE IMPLEMENTATION - IUPDATEEVENT
		public void FixedUpdate(CombatantEventParams eventParams) {
			// Grabbing a reference so I don't need to rewrite everything.
			Combatant combatant = eventParams.combatant;

			if (combatant.State == CombatantStateType.dazed) { return; }
			if (combatant.State == CombatantStateType.punching) { return; }

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

			// First, see if the combatant is trying to punch and if they are allowed to.
			if (combatant.CombatantInput.GetPunchInput(combatant: combatant) == true) {

				foreach (Player target in targetsToPunch) {
					PunchCombatant(combatant: combatant, target: target);
				}

				//clear current targets
				targetsToPunch.Clear();

				// Set this players properties for punching, for punch duration
				// TODO: See if I can just make this in Prepare() and reuse it. If it's playing, it would mean CanPunch is false.
				Sequence seq = DOTween.Sequence();
				seq.AppendCallback(new TweenCallback(delegate {
					combatant.SetState(CombatantStateType.punching);
					combatant.CombatantBody.GravityScale = bodyGravityModifier;
					//this.punchBoxGameObject.SetActive(true);
				}));
				seq.AppendInterval(interval: this.punchDuration);
				seq.AppendCallback(new TweenCallback(delegate {
					combatant.SetState(CombatantStateType.playing);
					combatant.CombatantBody.GravityScale = originalGravModifier;
					//this.punchBoxGameObject.SetActive(false);
				}));
				seq.Play();

				//reset combatant body velocity
				combatant.CombatantBody.StopHorizontal();
				combatant.CombatantBody.StopVertical();

				//Determine punch propellent direction
				Vector2 propellDirection = Vector2.zero;
				switch (combatant.Orientation) {
					case OrientationType.N:
						propellDirection.y = 1;
						break;
					case OrientationType.W:
						propellDirection.x = -1;
						break;
					case OrientationType.S:
						propellDirection.y = -1;
						break;
					case OrientationType.E:
						propellDirection.x = 1;
						break;
				}
				combatant.CombatantBody.AddForce(direction: propellDirection, magnitude: propellentForceMagnitude);
			}
		}
		#endregion

		#region INTERFACE IMPLEMENTATION - IONTRIGGERENTER2DEVENT
		public void OnTriggerEnter2D(CombatantEventParams eventParams) {
			Combatant combatant = eventParams.combatant;
			Collider2D collision = eventParams.collision;

			// Debug.Log("ENTER: " + collision);

			if (collision.tag == "Player" && collision.gameObject.GetComponent<Combatant>() != combatant) {
				//If already state == punching, punch our new target
				if (combatant.State == CombatantStateType.punching) {
					PunchCombatant(combatant: combatant, target: collision.gameObject.GetComponent<Player>());
				}
				//add target to list for when we punch
				else {
					if (!targetsToPunch.Contains(collision.gameObject.GetComponent<Player>())) {
						targetsToPunch.Add(collision.gameObject.GetComponent<Player>());
					}
				}
			}
		}
		#endregion

		#region INTERFACE IMPLEMENTATION - IONTRIGGEREXIT2DEVENT
		public void OnTriggerExit2D(CombatantEventParams eventParams) {
			Combatant combatant = eventParams.combatant;
			Collider2D collision = eventParams.collision;

			// Debug.Log("EXIT: " + collision);

			if (collision.tag == "Player" && collision.gameObject.GetComponent<Player>() != combatant) {
				targetsToPunch.Remove(collision.gameObject.GetComponent<Player>());
			}
		}
		#endregion

		#region INSPECTOR JUNK
		private static string inspectorDescription = "The behavior that allows the combatant to punch.";
		protected override string InspectorDescription {
			get {
				return inspectorDescription;
			}
		}
		#endregion

	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace PanicPinnacle.Combatants.Behaviors.Legacy {

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

		//collection of targets active in punch hitbox
		private List<Player> targetsToPunch;

		//Original grav modifier at beginning of scene
		private float originalGravModifier;
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
				//this.punchBoxGameObject.SetActive(false);
			}

			originalGravModifier = combatant.CombatantBody.GravityScale;
			targetsToPunch = new List<Player>();
		}
		/// <summary>
		/// Checks whether or not this combatant wants to punch and, if they do, does so.
		/// </summary>
		/// <param name="combatant">The combatant who owns this behavior.</param>
		public override void FixedUpdate(Combatant combatant) {

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
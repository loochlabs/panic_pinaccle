using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;
using PanicPinnacle.Events;
using PanicPinnacle.Matches;
using System.Linq;

namespace PanicPinnacle.Combatants.Behaviors {

	/// <summary>
	/// The behavior that allows the combatant to punch. Adjusted from PunchBehavior to allow for punch in the direction specified on the face button.
	/// </summary>
	[System.Serializable]
	public class FourWayPunchBehavior : CombatantBehavior, IFixedUpdateEvent, IOnTriggerEnter2DEvent, IOnTriggerExit2DEvent {

		#region FIELDS - STATIC
		/// <summary>
		/// A list of possible directions the combatant can punch in. I'm just using this as an easy way to iterate through the different types.
		/// </summary>
		private static List<OrientationType> punchDirections = new List<OrientationType>() { OrientationType.N, OrientationType.E, OrientationType.S, OrientationType.W };
		#endregion

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
		/// <summary>
		/// How much should punching deplete the meter by?
		/// </summary>
		[TabGroup("Punch Behavior", "Meter"), PropertyTooltip("How much should punching deplete the meter by?"), SerializeField]
		private float meterDepletion = 5f;
		/// <summary>
		/// How much of the meter should be restored *per second* when the player is grounded?
		/// </summary>
		[TabGroup("Punch Behavior", "Meter"), PropertyTooltip("How much of the meter should be restored *per second* when the player is grounded?"), SerializeField]
		private float meterRestorationRate = 10f;
		/// <summary>
		/// How much should the meter store in total?
		/// </summary>
		[TabGroup("Punch Behavior", "Meter"), PropertyTooltip("How much should the meter store in total?"), SerializeField]
		private float meterMax = 20f;
		/// <summary>
		/// How much is currently stored in the meter?
		/// </summary>
		private float meterAmount = 0f;
		/// <summary>
		/// How much is currently stored in the meter?
		/// </summary>
		private float MeterAmount {
			get {
				return this.meterAmount;
			} set {
				// When setting the amount stored on this meter, make sure to clamp it so it doesn't exceed the maximum allowed amount.
				this.meterAmount = Mathf.Clamp(value: value, min: 0f, max: this.meterMax);
			}
		}
		#endregion

		#region FIELDS - SCENE REFERENCES
		/// <summary>
		/// A reference to the GameObject that contains the box used for when this combatant punches.
		/// </summary>
		private GameObject punchboxGameObject;
		/// <summary>
		/// Collection of targets active in punch hitbox
		/// </summary>
		private List<Player> targetsToPunch;
		/// <summary>
		/// Original grav modifier at beginning of scene
		/// </summary>
		private float originalGravModifier;

		/// <summary>
		/// Audio source reference
		/// </summary>
		private AudioSource audio;
		#endregion

		#region PREPARATION
		/// <summary>
		/// Preps this behavior for use.
		/// </summary>
		/// <param name="combatant">The combatant that is using this behavior.</param>
		public override void Prepare(Combatant combatant) {
			Debug.Log("NOTE: This will fail if the combatant does not have the proper objects as part of their children. See if this can be refactored.");
			// Look for the punch box in this combatant's children.
			this.punchboxGameObject = combatant.gameObject.transform.Find("Punch Box").gameObject;
			// If it wasn't found, throw an error. Usually I'm calling Prepare() from something that will catch this.
			if (this.punchboxGameObject == null) {
				throw new System.Exception("Couldn't find the Punch Box on this combatant! Is it named properly?");
			} else {
				// If it WAS found, turn it off. It might already be off but do it anyway. Thanks.
				punchboxGameObject.SetActive(false);
				punchboxGameObject.GetComponentInChildren<SpriteRenderer>().color =
					MatchController.instance.CurrentMatchSettings.MatchTemplate.PlayerColors[combatant.CombatantID];
			}

			// originalGravModifier = combatant.CombatantBody.GravityScale;
			originalGravModifier = combatant.CombatantBody.gravityScale;

			targetsToPunch = new List<Player>();

			//sfx
			audio = combatant.GetComponentInChildren<AudioSource>();
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
			target.Aggressor = combatant;

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

			// Restore part of the player's punch meter if they are grounded.
			if (combatant.CombatantBody.IsGrounded == true) {
				this.MeterAmount += (this.meterRestorationRate * Time.fixedDeltaTime);
			}

			// After restoring, double check to make sure the player has enough meter to punch. If not, back out.
			// TODO: Might wanna go "dark souls stamina" style on this and allow them to punch as long as its not zero,
			// but make meter restoration take very slightly longer if punching at a value in between zero and the stamina cost?
			if (this.MeterAmount < this.meterDepletion) { return; }

			//
			//
			// If code is executing after this point, it means the player potentially may be punching and is also allowed to do so.
			//
			//

			// Okay so here's how this works.
			FourWayPunchBehavior.punchDirections
				.Where(pd => combatant.CombatantInput.GetFourWayPunchInput(combatant: combatant, orientationType: pd) == true)	// Go through the different directions the player can punch and see if they are punching that direction.
				.Take(count: 1)																									// Only use the first element in case they're pressing several buttons. (It will be empty if nothing is pressed)
				.ToList()																										// Convert it to a list so I can iterate.
				.ForEach(pd => {																								// "For Each" is being used, but since I only have one/no elements, this code either runs once or not at all.

					// Start off by setting the rotation of the punch box to the orientation of the punch.
					Vector3 punchboxRotation = Vector3.zero;
					switch (pd) {
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
					punchboxGameObject.transform.localEulerAngles = punchboxRotation;


					//impact sfx
					Debug.Log("target count " + targetsToPunch.Count);
					if (targetsToPunch.Count != 0) {
						audio.PlayOneShot(DataController.instance.GetSFX(SFXType.PunchImpact), 0.7f);
					}

					// Punch anyone who was caught in the targets.
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
						// combatant.CombatantBody.GravityScale = bodyGravityModifier;
						combatant.CombatantBody.SetGravityScale(gravityScale: bodyGravityModifier);
						punchboxGameObject.SetActive(true);
					}));
					seq.AppendInterval(interval: this.punchDuration);
					seq.AppendCallback(new TweenCallback(delegate {
						combatant.SetState(CombatantStateType.playing);
						// combatant.CombatantBody.GravityScale = originalGravModifier;
						combatant.CombatantBody.SetGravityScale(gravityScale: originalGravModifier);
						punchboxGameObject.SetActive(false);
					}));
					seq.Play();

					//reset combatant body velocity
					combatant.CombatantBody.StopHorizontal();
					combatant.CombatantBody.StopVertical();

					//Determine punch propellent direction
					Vector2 propellDirection = Vector2.zero;
					// The propellent direction is going to be the direction of the punch; not the combatant's orientation as it was in the standard punch.
					switch (pd) {
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

					//sfx
					audio.PlayOneShot(DataController.instance.GetSFX(SFXType.Punch1));

					// Deplete the meter after the punch.
					this.MeterAmount -= this.meterDepletion;

				});

			
		}
		#endregion

		#region INTERFACE IMPLEMENTATION - IONTRIGGERENTER2DEVENT
		public void OnTriggerEnter2D(CombatantEventParams eventParams) {
			Combatant combatant = eventParams.combatant;
			Collider2D collision = eventParams.collision;

			if (collision.tag == "Player" && collision.gameObject.GetComponent<Player>() != combatant) {
				//If already state == punching, punch our new target
				if (combatant.State == CombatantStateType.punching) {
					PunchCombatant(combatant: combatant, target: collision.gameObject.GetComponent<Player>());
					audio.PlayOneShot(DataController.instance.GetSFX(SFXType.PunchImpact), 0.7f);
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
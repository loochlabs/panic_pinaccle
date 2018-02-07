using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TeamUtility.IO;

namespace PanicPinnacle.Legacy {

	public class PlayerControls : MonoBehaviour {

		//PUBLIC FIELDS
		public PlayerID playerid;
		public SpriteRenderer spriteRenderer;
		public float moveForce = 365f;
		public float jumpForce = 1000f;
		public float maxFallSpeed = -10f;
		public float maxMoveSpeed;
		public Transform[] groundChecks;
		//punch
		public bool punchEnabled;
		public GameObject punchbox;
		public SpriteRenderer punchboxSprite;
		public float punchGravityScale;
		public float punchDuration;
		public float punchOnThisForce;
		public float punchOnTargetForce;
		//blast
		public bool blastEnabled;
		[Range(1, 50)]
		public float blastRange = 5f;
		[Range(1, 200)]
		public float blastForce = 10f;
		[Range(0, 3)]
		public float blastChargeTime = 1f;
		[Range(0, 2f)]
		public float blastChargeRate = 0.01f;
		public GameObject blastParent;
		public GameObject blastObject;
		public GameObject blastImpactSprite;
		public Transform blastTip;
		//sfx
		public AudioClip jumpSfx;

		//PRIVATE FIELDS
		private PlayerState state = PlayerState.NONE;
		private int roundPosition;
		private float originalGravity;
		private bool grounded = false;
		private bool jump = false;
		private bool canjump = true;
		private Rigidbody2D body;
		private Vector3 bv = Vector3.zero;
		private Orientation moveOrientation = Orientation.NONE;
		//punch
		private List<PlayerControls> punchTargets = new List<PlayerControls>();
		private ForceProperties forceCurrent;
		private bool canpunch = true;
		//blast
		private Orientation blastOrientation = Orientation.NONE;
		private Vector3 blastEuler = Vector3.zero;
		private Vector3 blastScale = Vector3.one;
		private Vector3 blastScaleBase;
		private Vector2 bd_primary = Vector2.zero;
		private float blastTimeCurrent = 0;
		private bool canfire = true;
		private Color introColor = new Color(1, 1, 1, 0.25f);
		private Color playerColor = Color.white;

		[Flags]
		enum Orientation {
			NONE = 0,
			LEFT = 1,
			RIGHT = 2,
			UP = 4,
			DOWN = 8
		}

		public enum PlayerState {
			NONE,
			INTRO,
			PLAYING,
			PAUSED,
			DEAD,
			OUTRO
		}

		public struct ForceProperties {
			public Vector2 direction;
			public float duration;
			public ForceProperties(Vector2 dir, float d) {
				direction = dir;
				duration = d;
			}
		}


		//GETTERS AND SETTERS

		public PlayerState State {
			get { return state; }
		}

		public int RoundPosition {
			get { return roundPosition; }
		}

		public float BlastPct {
			get { return blastTimeCurrent / blastChargeTime; }
		}

		public Color Color {
			get { return playerColor; }
		}

		public Rigidbody2D Body {
			get { return body; }
		}

		public ForceProperties ForceCurrent {
			get { return forceCurrent; }
			set { forceCurrent = value; }
		}


		// Use this for initialization
		void Awake() {
			body = GetComponent<Rigidbody2D>();

			//blast properties
			blastScaleBase = blastParent.transform.localScale;
		}


		private void Start() {
			//player properties
			blastParent.SetActive(false);
			blastImpactSprite.SetActive(false);
			originalGravity = body.gravityScale;
		}


		void Update() {
			if (state != PlayerState.PLAYING) { return; }

			//ground checks
			grounded = false;
			foreach (Transform t in groundChecks) {
				if (Physics2D.Linecast(transform.position, t.position, 1 << LayerMask.NameToLayer("Ground"))) {
					grounded = true;
				}
			}
		}

		void FixedUpdate() {

			switch (state) {

				case PlayerState.INTRO:

					break;

				case PlayerState.PLAYING:

					float horz = InputManager.GetAxisRaw("Horizontal", playerid);
					float vert = InputManager.GetAxisRaw("Vertical", playerid);

					if (horz > 0) {
						moveOrientation |= Orientation.RIGHT;
						moveOrientation &= ~Orientation.LEFT;
						blastOrientation = Orientation.RIGHT;
					} else if (horz < 0) {
						moveOrientation |= Orientation.LEFT;
						moveOrientation &= ~Orientation.RIGHT;
						blastOrientation = Orientation.LEFT;
					}
					if (vert > 0) {
						moveOrientation |= Orientation.DOWN;
						moveOrientation &= ~Orientation.UP;
						blastOrientation = Orientation.DOWN;
					} else if (vert < 0) {
						moveOrientation |= Orientation.UP;
						moveOrientation &= ~Orientation.DOWN;
						blastOrientation = Orientation.UP;
					} else {
						moveOrientation &= ~Orientation.UP;
						moveOrientation &= ~Orientation.DOWN;
					}

					//horizontal movement
					if (horz != 0) {
						body.AddForce(Vector2.right * horz * moveForce * Time.deltaTime);
					}

					//vertical movement
					//cap vertical speed
					bv.y =
						body.velocity.y < bv.y && bv.y < -maxFallSpeed ? -maxFallSpeed
						: body.velocity.y > bv.y && bv.y > maxFallSpeed ? maxFallSpeed
						: body.velocity.y;

					body.velocity = bv;

					if (canjump && grounded && (moveOrientation & Orientation.UP) == Orientation.UP) {
						jump = true;
						canjump = false;
					}
					if (!canjump && grounded && (moveOrientation & Orientation.UP) != Orientation.UP) {
						canjump = true;
					}
					if (jump) {
						body.AddForce(new Vector2(0f, jumpForce * Time.deltaTime));
						jump = false;
						//sfx
						GetComponent<AudioSource>().PlayOneShot(jumpSfx);
					}

					//@MECHANIC: Punch
					if (punchEnabled) {

						if (forceCurrent.duration > 0) {
							Debug.Log("PUNCH: " + forceCurrent.direction.ToString() + "|" + forceCurrent.duration);
							punchboxSprite.enabled = true;
							forceCurrent.duration -= Time.deltaTime;
							body.AddForce(forceCurrent.direction * Time.deltaTime);
							body.gravityScale = originalGravity * punchGravityScale;
							canpunch = false;
						} else {
							punchboxSprite.enabled = false;
							body.gravityScale = originalGravity;
							canpunch = true;
						}


						if (canpunch && InputManager.GetButtonDown("Fire", playerid)) {
							Debug.Log("fire: " + playerid);
							canfire = false;
							punchboxSprite.enabled = true;
							Vector3 boxRotation = Vector3.zero;
							bd_primary = Vector2.zero;
							if ((blastOrientation & Orientation.UP) == Orientation.UP) {
								bd_primary.y = 1;
								boxRotation.z = 0;
							}
							if ((blastOrientation & Orientation.LEFT) == Orientation.LEFT) {
								bd_primary.x = -1;
								boxRotation.z = 90;
							}
							if ((blastOrientation & Orientation.DOWN) == Orientation.DOWN) {
								bd_primary.y = -1;
								boxRotation.z = 180;
							}
							if ((blastOrientation & Orientation.RIGHT) == Orientation.RIGHT) {
								bd_primary.x = 1;
								boxRotation.z = 270;
							}
							punchbox.transform.localEulerAngles = boxRotation;
							body.velocity = Vector3.zero;
							forceCurrent = new ForceProperties(bd_primary * punchOnThisForce, punchDuration);
							Debug.Log("diration " + bd_primary.ToString());
							//force on this
							//body.AddForce(bd_primary * punchOnThisForce, ForceMode2D.Impulse);

							//force on targets
							foreach (PlayerControls target in punchTargets) {
								PunchTarget(target);
							}
							punchTargets.Clear();
						}

						//if (InputManager.GetButtonUp("Fire", playerid))
						//{
						//canfire = true;
						//}
					}

					//@MECHANIC: Blast
					if (blastEnabled) {
						if (canfire && InputManager.GetButton("Fire", playerid)) {
							//blast direction
							bd_primary = Vector2.zero;
							if ((blastOrientation & Orientation.UP) == Orientation.UP) { bd_primary.y = 1; }
							if ((blastOrientation & Orientation.DOWN) == Orientation.DOWN) { bd_primary.y = -1; }
							if ((blastOrientation & Orientation.RIGHT) == Orientation.RIGHT) { bd_primary.x = 1; }
							if ((blastOrientation & Orientation.LEFT) == Orientation.LEFT) { bd_primary.x = -1; }
							bd_primary.Normalize();

							//blast shot update
							blastParent.SetActive(true);
							blastImpactSprite.SetActive(true);
							blastImpactSprite.transform.position = blastTip.position;
							blastEuler.z = Mathf.Atan2(bd_primary.y, bd_primary.x) * Mathf.Rad2Deg - 90;
							blastParent.transform.eulerAngles = blastEuler;

							int mask_player = 1 << 9;
							int mask_all = ~(mask_player);

							//scale blast line/';.
							Bounds b = blastObject.GetComponent<SpriteRenderer>().sprite.bounds;
							blastScale.x = 1 / (b.size.x);
							blastScale.z = b.size.z;
							RaycastHit2D hit = Physics2D.Raycast(transform.position, bd_primary, blastRange, mask_all);
							if (hit.collider) {
								blastScale.y = hit.distance / b.size.y;
							} else {
								blastScale.y = blastRange / b.size.y;
							}

							blastParent.transform.localScale = blastScale;

							//blast force
							body.AddForce(bd_primary * -1 * blastForce);

							//blast timer
							blastTimeCurrent += blastChargeRate * Time.deltaTime;
							if (blastTimeCurrent >= blastChargeTime) {
								canfire = false;
							}
						} else {
							blastParent.SetActive(false);
							blastImpactSprite.SetActive(false);

							//recharge blast
							if (blastTimeCurrent > 0) {
								blastTimeCurrent -= blastChargeRate * Time.deltaTime;
								if (blastTimeCurrent < 0) { blastTimeCurrent = 0; }
							}
						}
						if (InputManager.GetButtonUp("Fire", playerid)) {
							canfire = true;
						}
					}

					break;

				case PlayerState.OUTRO:
					break;

				case PlayerState.DEAD:
					break;
			}


			//@DEBUG
			//Hard Round Reset
			if (playerid == PlayerID.One && InputManager.GetButton("Start", playerid)) {
				SceneManager.LoadScene(0);
			}
		}


		private void OnCollisionEnter2D(Collision2D collision) {
			if (collision.gameObject.tag == "Bound") {
				Knockout();
			}
		}


		private void OnTriggerEnter2D(Collider2D collision) {
			if (collision.gameObject.tag == "Goal") {
				RoundComplete();
			}

			if (collision.gameObject.tag == "PlayerPunchbox") {
				//Debug.Log("ADD Target : " + collision.GetComponentInParent<PlayerControls>().playerid + " TO " + playerid);
				if (forceCurrent.duration > 0) {
					PunchTarget(collision.GetComponentInParent<PlayerControls>());
				} else {
					collision.GetComponentInParent<PlayerControls>().AddTarget(this);
				}
			}
		}

		private void OnTriggerExit2D(Collider2D collision) {
			if (collision.gameObject.tag == "PlayerPunchbox") {
				//Debug.Log("REMOVE Target : " + collision.GetComponentInParent<PlayerControls>().playerid + " TO " + playerid);
				collision.GetComponentInParent<PlayerControls>().RemoveTarget(this);
			}
		}


		public void PunchTarget(PlayerControls target) {
			//calculate direction between this and target
			Vector2 direction = new Vector2(
				target.transform.position.x - transform.position.x,
				target.transform.position.y - transform.position.y);
			direction.Normalize();
			target.ForceCurrent = new ForceProperties(direction * punchOnTargetForce, 1f);
		}

		public void AddTarget(PlayerControls enemy) {
			if (!punchTargets.Contains(enemy)) {
				punchTargets.Add(enemy);
			}
		}

		public void RemoveTarget(PlayerControls enemy) {
			punchTargets.Remove(enemy);
		}


		/// <summary>
		/// Knockout from colliding with environment bounds.
		/// </summary>
		private void Knockout() {
			Debug.Log("PLAYER " + playerid + " KNOCKOUT");
			state = PlayerState.DEAD;
			roundPosition = MainManager.playerAliveCount--;

			//@TODO: temp deactivate
			gameObject.SetActive(false);
		}


		private void RoundComplete() {
			Debug.Log("PLAYER " + playerid + " complete");
			state = PlayerState.DEAD;
			roundPosition = ++MainManager.playerCompleteCount;
			MainManager.playerAliveCount--;

			//@TODO: temp deactivate
			gameObject.SetActive(false);
		}

		public void Init(int playerIndex, PlayerState state, Color c) {
			switch (playerIndex) {
				case 1:
					playerid = PlayerID.One;
					break;
				case 2:
					playerid = PlayerID.Two;
					break;
				case 3:
					playerid = PlayerID.Three;
					break;
				case 4:
					playerid = PlayerID.Four;
					break;
			}

			//Set color
			playerColor = c;
			spriteRenderer.color = c;
			blastObject.GetComponent<SpriteRenderer>().color = c;
			blastImpactSprite.GetComponent<SpriteRenderer>().color = c;

			//set initial state
			SetState(state);
		}


		public void SetState(PlayerState ps) {
			state = ps;

			//gfx settigns
			switch (state) {
				case PlayerState.INTRO:
					GetComponent<SpriteRenderer>().color = introColor;
					break;

				case PlayerState.PLAYING:
					GetComponent<SpriteRenderer>().color = playerColor;
					break;

				case PlayerState.DEAD:
					break;

				case PlayerState.OUTRO:
					break;
			}
		}
	}

}
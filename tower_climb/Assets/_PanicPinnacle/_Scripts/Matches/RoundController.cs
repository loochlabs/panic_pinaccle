using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Combatants;
using DG.Tweening;
using PanicPinnacle.Legacy;
using PanicPinnacle.Matches.Legacy;

namespace PanicPinnacle.Matches {

	/// <summary>
	/// Manages rounds at a more fine-grain level that the MatchController shouldn't have responsibility of.
	/// </summary>
	public class RoundController : MonoBehaviour {

		public static RoundController instance;

		#region FIELDS - SETTINGS
		/// <summary>
		/// The settings currently being used for this particular round.
		/// </summary>
		private RoundSettings currentRoundSettings;

        /// <summary>
        /// The settings currently being used for this particular round.
        /// </summary>
        public RoundSettings CurrentRoundSettings
        {
            get { return currentRoundSettings; }
        }
		#endregion
        
		#region FIELDS - LEGACY FIELDS
		// Mostly just have these here so I can add in old code.
		// Likely will replace later but it isn't important enough that I need to do it right now immediately.
		private RoundState state;
		private GameObject mainCamera; //@TODO: move to LevelSettings
		[SerializeField]
		private LevelSettings level;
		private Vector3 boundsScale = new Vector3(1, 1, 0); //@TODO: move to LevelSEttings
		#endregion

		#region FIELDS - COMBATANTS
		/// <summary>
		/// A list of combatants currently in-game.
		/// </summary>
		private List<Combatant> combatants = new List<Combatant>();
		/// <summary>
		/// A list of combatants currently in-game.
		/// </summary>
		public List<Combatant> Combatants {
			get {
				return this.combatants;
			}
		}
        #endregion
        

        #region UNITY FUNCTIONS
        private void Awake() {
			if (instance == null) {
				instance = this;
			}
		}

		private void Start() {
		}

		private void Update() {
			// Call the old Update function. It's good enough to work with as of now.
			// However, it will probably need to be refactored in the future.
			this.LegacyUpdate();
		}
		#endregion

		#region PREPARATION
		/// <summary>
		/// Prepares and loads the round with the settings provided.
		/// </summary>
		/// <param name="roundSettings">The settings to use when preparing this round.</param>
		public void PrepareRound(RoundSettings roundSettings) {
			Debug.Log("PREPARING ROUND");
			// Save the settings, in case they are needed.
			this.currentRoundSettings = roundSettings;
		}
		/// <summary>
		/// Prepares the round in debug mode. WILL CHANGE THIS LATER PROBABLY.
		/// </summary>
		/// <param name="matchSettings"></param>
		/// <param name="roundSettings"></param>
		public void PrepareDebugRound(RoundSettings roundSettings) {
			// Save the settings, in case they are needed.
			this.currentRoundSettings = roundSettings;
			RoundController.instance.StartRound();
		}
		/// <summary>
		/// Goes through the combatants listed and preps them with the information stored in the MatchSettings passed in.
		/// </summary>
		/// <param name="combatants">The combatants to prep for the match.</param>
		/// <param name="matchSettings">The settings of the match that also determine how these combatants should be prepared.</param>
		/// <returns></returns>
		private List<Combatant> PrepareCombatants(List<Combatant> combatants, MatchSettings matchSettings, RoundSettings roundSettings) {
			// Go through each of the combatants that were provided.
			for (int i = 0; i < combatants.Count; i++) {
				Debug.Log("COUNT: " + matchSettings.CombatantTemplates.Count);
				// If there are more combatants than there are templates, destroy the extra combatants within the scene.
				// This will happen in cases where there are less people playing than there are combatants defined within the scene.
				if (i >= matchSettings.CombatantTemplates.Count) {
					Debug.Log("DESTROYING combatant: " + i);
					Destroy(combatants[i]);
				} else {
					Debug.Log("PREPARING combatant: " + i);
					// If the index can be used in both the combatants list and the templates list, prep the combatant with that template.
					combatants[i].Prepare(combatantTemplate: matchSettings.CombatantTemplates[i], combatantId: i);
				}
			}
			// When done, return the modified list of combatants.
			return combatants;
		}
		#endregion

		#region STARTING
		/// <summary>
		/// Start the round once the scene has finally been loaded.
		/// </summary>
		public void StartRound() {
			// Prep the combatants with the information contained within the MatchSettings and save the list.
			//this.combatants = this.PrepareCombatants(
			//combatants: new List<Combatant>(GameObject.FindObjectsOfType<Combatant>()),
			//matchSettings: MatchController.instance.CurrentMatchSettings,
			//roundSettings: this.currentRoundSettings);

			Debug.Log("STARTING ROUND");
			// Look for the level settings somewhere within this scene.
			this.level = GameObject.FindObjectOfType<LevelSettings>();
            
			//Instantiate and prepare our Combatants
			foreach (var ct in MatchController.instance.CurrentMatchSettings.CombatantTemplates) {
				GameObject player = Instantiate(
					original: MatchController.instance.CurrentMatchSettings.MatchTemplate.PlayerPrefab,
					parent: level.Spawns[ct.Key]);
				player.GetComponent<Player>().Prepare(ct.Value, ct.Key);
				combatants.Add(player.GetComponent<Player>());
			}

			// Call the legacy function that sets the state and starts the round.
			this.LegacySetState(state: RoundState.intro);
		}
		#endregion

		#region LEGACY
		/// <summary>
		/// Just using the old state method for the time being to speed things up. 
		/// I'll refactor it later if it's an issue.
		/// </summary>
		/// <param name="state"></param>
		private void LegacySetState(RoundState state) {
			this.state = state;

			//set and specific settings for this state
			switch (state) {
				case RoundState.intro:
					//focus camera on top at start

					// mainCamera.GetComponent<CameraControls>().focusTransform = level.End;
					CameraControls.instance.focusTransform = this.level.End;

					//intro cinematic, pan downwards from level.End to level.Start
					//@TEMP
					Sequence introSeq = DOTween.Sequence();
					introSeq.AppendInterval(interval: 1f);
					introSeq.AppendCallback(new TweenCallback(delegate {
						LegacySetState(RoundState.countdown);
					}));
					introSeq.Play();
					break;

				case RoundState.countdown:
					// mainCamera.GetComponent<CameraControls>().focusTransform = level.BoundsCenter;
					CameraControls.instance.focusTransform = this.level.BoundsCenter;
					//3,2,1,Panic
					//@TODO: display count on UI
					Sequence countdownSeq = DOTween.Sequence();
					countdownSeq.AppendCallback(new TweenCallback(delegate {
						Debug.Log("3...");
					}));
					countdownSeq.AppendInterval(interval: 1f);
					countdownSeq.AppendCallback(new TweenCallback(delegate {
						Debug.Log("2...");
					}));
					countdownSeq.AppendInterval(interval: 1f);
					countdownSeq.AppendCallback(new TweenCallback(delegate {
						Debug.Log("1...");
					}));
					countdownSeq.AppendInterval(interval: 1f);
					countdownSeq.AppendCallback(new TweenCallback(delegate {
						Debug.Log("PANIC!");
						LegacySetState(RoundState.playing);
					}));
					countdownSeq.Play();
					break;

				case RoundState.playing:
					break;

				case RoundState.pause:
					break;

				case RoundState.allfail:
					break;

				case RoundState.outro:
					Debug.Log("ROUND OUTRO");
					Sequence outroSeq = DOTween.Sequence();
					outroSeq.AppendInterval(interval: 1f);
					outroSeq.AppendCallback(new TweenCallback(delegate {
						//LegacySetState(RoundState.complete);
						// Display the tally.
						//Menus.RoundTallyScreen.instance.DisplayTally(type: Menus.TallyScreenType.Round);
					}));
					outroSeq.AppendInterval(5f);
					outroSeq.AppendCallback(new TweenCallback(delegate {
						// There's a bit of overcomplication with how the next round should be picked out.
						// Currently, GoToNextPhase() in MatchController takes care of that logic, but 
						// that function is private and gets called as a result of MatchController.StartMatch(),
						// which, in turn, is called by GameController.StartMatch(). These calls are the only things
						// that these two functions do, so it's worth remembering if this ever becomes an issue.
                        MatchController.instance.NextPhase();
					}));
					outroSeq.Play();
					break;

				case RoundState.complete:
					//@TODO goto Match Tally
					Debug.Log("ROUND COMPLETE");

					break;
			}
		}
		/// <summary>
		/// Using the old Update method for right now.
		/// </summary>
		private void LegacyUpdate() {
			switch (state) {
				case RoundState.playing:
					//move bounds and camera up through the level
					if (Vector3.Distance(level.BoundsCenter.position, level.End.position) > level.BoundsEndDistThreshold) {
						level.BoundsCenter.position = level.BoundsCenter.position + level.BoundsDirection * level.BoundsMoveRate * Time.deltaTime;
					} else {
						//bounds expansion at end of level
						level.BoundsCenter.position = level.End.position;
						foreach (GameObject go in level.BoundsObjects) {
							go.transform.localScale = go.transform.localScale + boundsScale * level.BoundsEndScaleRate;
							//check for end of bounds scaling
							//@TODO cleaner check than this magic number
							if (go.transform.localScale.magnitude > 150) {
								LegacySetState(RoundState.outro);
								break;
							}
						}
					}
					break;
			}
		}
		#endregion

		public enum RoundState {
			intro = 0,          //intro cinematic
			countdown = 1,      //3,2,1, panic!
			playing = 2,        //
			pause = 3,          //
			allfail = 4,        //all players have failed (knocked out of bounds)
			outro = 5,          //outro cinematic
			complete = 6        //setup and goto Match Tally scene
		}
	}
}
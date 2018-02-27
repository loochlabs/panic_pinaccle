using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Matches;

namespace PanicPinnacle.Menus {

	/// <summary>
	/// Stores the callbacks that get used by the buttons.
	/// </summary>
	public class PreGameMenu : MonoBehaviour {


		#region FIELDS - MEMORY
		/// <summary>
		/// A list of players that have pressed A on the pre-game menu and are ready to start.
		/// </summary>
		private List<bool> readyPlayers = new List<bool>();
		/// <summary>
		/// A toggle that signals when the pre-game text needs to be refreshed. Mostly so I don't have to rewrite the STM every frame and waste garbage making strings and throwing them away.
		/// </summary>
		private bool refreshPreGameTextToggle = false;
		#endregion

		#region FIELDS - SCENE REFERENES
		/// <summary>
		/// A reference to the STM that shows which players are ready.
		/// </summary>
		[SerializeField]
		private SuperTextMesh preGamePlayersText;
		#endregion

		private void Awake() {
			// Initialize the list of ready players so that everyone is not ready. 
			this.readyPlayers.AddRange(new bool[] { false, false, false, false });
		}

		private void Start() {
			this.StartCoroutine("WaitForPlayers");
		}

		/// <summary>
		/// A coroutine that will just seek out 
		/// </summary>
		/// <returns></returns>
		private IEnumerator WaitForPlayers() {
			while (true) {
				// Find which players are ready for the game. Ready To play some ball. Ready to punch. hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut hut 
				Debug.LogError("Get this working again.");
				/*for (int i = 0; i < this.readyPlayers.Count; i++) {
					this.readyPlayers[i] = this.readyPlayers[i] | InputManager.GetButtonDown("PrimaryAction", (PlayerInputID)(i + 1));
					this.refreshPreGameTextToggle = true;
				}*/

				// If the list of ready players changed, refresh the text.
				if (this.refreshPreGameTextToggle == true) {
					this.refreshPreGameTextToggle = false;

					this.preGamePlayersText.Text = "READY: ";
					// Go through the list and, if the player is ready, add their number.
					for (int i = 0; i < this.readyPlayers.Count; i++) {
						if (this.readyPlayers[i] == true) {
							this.preGamePlayersText.Text += i.ToString() + " ";
						}
					}
				}


				Debug.LogError("Get this working again.");
				/*// If Player 1 hit start, begin the match with the settings listed above.
				if (InputManager.GetButton("Start", PlayerInputID.One)) {
					//@TEMP while we rework the Pregame setup
					GameManager.instance.StartMatch();

					//MatchController.instance.StartMatch(matchSettings: MatchController.instance.DebugMatchSettings);
					break;
				}*/

				yield return new WaitForEndOfFrame();
			}
		}
	}
}
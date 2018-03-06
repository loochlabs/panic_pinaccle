using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Matches;
using PanicPinnacle.Combatants;

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

        private Combatant[] combatants = new Combatant[4];
        
		#endregion

		#region FIELDS - SCENE REFERENES
		/// <summary>
		/// A reference to the STM that shows which players are ready.
		/// </summary>
		[SerializeField]
		private SuperTextMesh preGamePlayersText;

        /// <summary>
        /// Array of transforms for spawn locations.
        /// </summary>
        [SerializeField]
        private Transform[] spawns = new Transform[4];

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
				
				for (int i = 0; i < this.readyPlayers.Count; i++) {
                    Rewired.Player player = Rewired.ReInput.players.GetPlayer(playerId: i);
                    if (player.GetButtonDown("Punch") && combatants[i] != null)
                    {
                        GameObject gameobj = Instantiate(
                            original: MatchController.instance.CurrentMatchSettings.MatchTemplate.PlayerPrefab, 
                            parent: spawns[i]);
                        combatants[i] = gameobj.GetComponent<Player>();
                        combatants[i].Prepare(
                            combatantTemplate: MatchController.instance.CurrentMatchSettings.MatchTemplate.CombatantTemplates[0],
                            combatantId: i);
                    }
                    //readyPlayers[i] = this.readyPlayers[i] | player.GetButtonDown("Punch"); 
					refreshPreGameTextToggle = true;
				}

				// If the list of ready players changed, refresh the text.
				if (refreshPreGameTextToggle) {
					refreshPreGameTextToggle = false;

					preGamePlayersText.Text = "READY: ";
					// Go through the list and, if the player is ready, add their number.
					for (int i = 0; i < this.readyPlayers.Count; i++) {
						if (readyPlayers[i]) {
							preGamePlayersText.Text += i.ToString() + " ";
						}
					}
				}

                
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

        #region PUBLIC FUNCTIONS
        /// <summary>
        /// Player has entered the ready box at top of Pregame Scene.
        /// </summary>
        /// <param name="combatant"></param>
        public void AddPlayer(Combatant combatant)
        {
            readyPlayers[combatant.CombatantID] = true;
        }

        /// <summary>
        /// Player has exited ready box at top of Pregame Scene.
        /// </summary>
        /// <param name="combatant"></param>
        public void RemovePlayer(Combatant combatant)
        {
            readyPlayers[combatant.CombatantID] = false;
        }
        #endregion
    }
}
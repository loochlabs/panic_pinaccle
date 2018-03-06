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

        /// <summary>
        /// Container for combatants who have joined this Pregame
        /// </summary>
        private Combatant[] combatants = new Combatant[4];

        /// <summary>
        /// Check for if all players are ready and the countdown to start is active.
        /// </summary>
        private bool countdownActive = false;

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
                    if (player.GetButtonDown("Punch") && combatants[i] == null)
                    {
                        Debug.Log("Creating new player with ID:" + i);
                        MatchController.instance.CurrentMatchSettings.AddCombatant(combatantId: i);
                        GameObject gameobj = Instantiate(
                            original: MatchController.instance.CurrentMatchSettings.MatchTemplate.PlayerPrefab, 
                            parent: spawns[i]);
                        combatants[i] = gameobj.GetComponent<Player>();
                        combatants[i].Prepare(
                            combatantTemplate: MatchController.instance.CurrentMatchSettings.CombatantTemplates[0],
                            combatantId: i);
                        refreshPreGameTextToggle = true;
                    }
				}
                
                //If all players are ready, start countdown timer
                bool readyToStart = false;
                //TODO: this is a complete brain fart and I cant think of a better way to check if there is atleast one active combatant
                for(int i = 0; i<combatants.Length; i++)
                {
                    if (combatants[i]) readyToStart = true;
                }
                for(int i = 0; i < combatants.Length; i++)
                {
                    if (combatants[i])
                    {
                        readyToStart = readyToStart & readyPlayers[i];
                    }
                }

                //Start a 3 second countdown if everyone is ready.
                //Cancel the countdown otherwise.
                if (readyToStart && !countdownActive)
                {
                    StartCoroutine("StartCountdown");
                }
                else if(!readyToStart && countdownActive)
                {
                    //TODO: not sure if this is the proper way to cancel this coroutine.
                    //      Might want some sort of check to see if its running. I just didnt find anything liek that.
                    StopCoroutine("StartCountdown");
                }


                //STEVE: I'll leave this here incase you want to still have UI info for the players
                // If the list of ready players changed, refresh the text.
                /*
				if (refreshPreGameTextToggle) {
					refreshPreGameTextToggle = false;

					preGamePlayersText.Text = "READY: ";
					// Go through the list and, if the player is ready, add their number.
					for (int i = 0; i < this.readyPlayers.Count; i++) {
						if (readyPlayers[i]) {
							preGamePlayersText.Text += i.ToString() + " ";
						}
					}
				}*/

                yield return new WaitForEndOfFrame();
			}
		}

        /// <summary>
        /// Start countdown to go to next round if all players are in the ready up box.
        /// </summary>
        /// <returns></returns>
        private IEnumerator StartCountdown()
        {
            Debug.Log("Countdown to Start is active!");
            countdownActive = true;
            yield return new WaitForSeconds(3f);
            MatchController.instance.NextPhase(MatchPhase.round);
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
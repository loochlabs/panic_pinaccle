using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PanicPinnacle.Legacy.Globals;
using PanicPinnacle.Combatants;
using TeamUtility.IO;

namespace PanicPinnacle.Legacy {

	public class MainManager : MonoBehaviour {

		enum RoundState {
			NONE,
			INTRO,
			PLAYING,
			PAUSED,
			OUTRO
		}

		public GameObject playerPrefab;
		public float roundLength_Intro = 4;
		public float roundLength_Active = 300;
		public float roundLength_Outro = 5;
		public Transform[] playerSpawns;
		public Color[] playerColors;
		public Transform startTransform;
		public Transform endTransform;
		public float endDistanceThresh;
		public Transform boundsTransform;
		public GameObject[] boundsObjects;
		public float boundsMoveRate = 0.5f;
        public float boundsScaleRate = 0.5f;
		//ui
		public GameObject[] playerInfoSlots;
		public Text alertText;
		//debug info
		public Text roundStateText;
		public Text roundTimeText;
		public Text playerBlastText;
		public bool roundInactive;

		//globals 
		public const int MAX_PLAYER_COUNT = 4;
		public static int PLAYER_COUNT = 4; //@TEMP, pull this count for player setup in prev scene
		public static int playerAliveCount = 0;
		public static int playerCompleteCount = 0;

		//private fields
		private GameObject[] players;
		private float currentRoundTime;
		private RoundState roundState;
		private Vector3 boundsScale = new Vector3(1, 1, 0);
		private Vector3 boundsDirection;

		// Use this for initialization
		void Start() {

			//timer settings
			currentRoundTime = roundLength_Intro;
			roundState = RoundState.INTRO;

			//bounds settings
			if (endTransform && startTransform) {
				boundsTransform.position = startTransform.position;
				boundsDirection = new Vector3(
					endTransform.position.x - startTransform.position.x,
					endTransform.position.y - startTransform.position.y,
					endTransform.position.z - startTransform.position.z);
				boundsDirection.Normalize();
			} else {
				roundInactive = true;
			}

			//player settings
			//ui
			players = new GameObject[4]; //@CLEANUP get global/round manager const
			playerAliveCount = 0;
			playerCompleteCount = 0;
			for (int i = 0; i < PLAYER_COUNT; i++) {
				players[i] = Instantiate(playerPrefab, playerSpawns[i]);
				playerAliveCount++;

                //@CLEANUP need to cast int to PlayerID
                PlayerID pid;
                switch (i + 1) {
                    case 1:
                        pid = PlayerID.One;
                        break;
                    case 2:
                        pid = PlayerID.Two;
                        break;
                    case 3:
                        pid = PlayerID.Three;
                        break;
                    case 4:
                        pid = PlayerID.Four;
                        break;
                    default:
                        pid = PlayerID.One;
                        break;
                }
                
				players[i].GetComponent<Player>().Prepare(pid, playerColors[i]);
                //start our player out in intro phase
                //@TODO might want to manager this better with a RoundManager.SetState(state)
                players[i].GetComponent<Player>().SetState(CombatantState.intro);
				//ui
				playerInfoSlots[i].SetActive(true);
				playerInfoSlots[i].GetComponentInChildren<Text>().color = players[i].GetComponent<Player>().Color;

			}
		}

		// Update is called once per frame
		void Update() {
			switch (roundState) {
				case RoundState.INTRO:
					currentRoundTime -= Time.deltaTime;
					if (currentRoundTime < 0) {
						roundState = RoundState.PLAYING;
						currentRoundTime = roundLength_Active;
						foreach (GameObject p in players) {
							if (p) { p.GetComponent<Player>().SetState(CombatantState.playing); }
						}
					}

					break;
				case RoundState.PLAYING:

					currentRoundTime -= Time.deltaTime;
					if (currentRoundTime < 0 || playerAliveCount <= 0) {
						roundState = RoundState.OUTRO;
						currentRoundTime = roundLength_Outro;

						//deactivate players
						foreach (GameObject p in players) {
							if (p) { p.GetComponent<Player>().SetState(CombatantState.outro); }
						}
					}

					//move level bounds
					if (roundInactive) { break; }
					if (Vector3.Distance(boundsTransform.position, endTransform.position) > endDistanceThresh) {
						boundsTransform.position = boundsTransform.position + boundsDirection * boundsMoveRate * Time.deltaTime;
					} else {
						boundsTransform.position = endTransform.position;
						foreach (GameObject go in boundsObjects) {
							go.transform.localScale = go.transform.localScale + boundsScale * boundsScaleRate;
						}
					}

					break;
				case RoundState.PAUSED:

					break;
				case RoundState.OUTRO:
					currentRoundTime -= Time.deltaTime;
					if (currentRoundTime < 0) {
						currentRoundTime = 0;
						//@GOTO next round
						Debug.Log("ROUND COMPELTE");
					}
					break;
			}

			UpdateUI();
		}

		private void UpdateUI() {
			roundTimeText.text = "Time Remaining : " + ((int)(currentRoundTime)).ToString();
			roundStateText.text = "Round State: " + roundState.ToString();

			if (roundState != RoundState.INTRO) alertText.enabled = false;
			switch (roundState) {
				case RoundState.INTRO:
					if (currentRoundTime > 3) {
						alertText.text = "3...";
					} else if (currentRoundTime > 2) {
						alertText.text = "2...";
					} else if (currentRoundTime > 1) {
						alertText.text = "1...";
					} else {
						alertText.text = "PANIC!";
					}
					break;
			}

			//debug info
			playerBlastText.text = "";

			//player info
			for (int i = 0; i < players.Length; i++) {
				if (!players[i]) continue;

				//Display player position for this round if knockedout
				if (players[i].GetComponent<Player>().State == CombatantState.dead
					|| players[i].GetComponent<Player>().State == CombatantState.outro) {
					switch (players[i].GetComponent<Player>().FinalRoundPosition) {
						case 1:
							playerInfoSlots[i].GetComponentInChildren<Text>().text = "1st";
							break;
						case 2:
							playerInfoSlots[i].GetComponentInChildren<Text>().text = "2nd";
							break;
						case 3:
							playerInfoSlots[i].GetComponentInChildren<Text>().text = "3rd";
							break;
						case 4:
							playerInfoSlots[i].GetComponentInChildren<Text>().text = "4th";
							break;
						default:
							playerInfoSlots[i].GetComponentInChildren<Text>().text = "ERR";
							break;
					}
				}

			}
		}
	}
    

}
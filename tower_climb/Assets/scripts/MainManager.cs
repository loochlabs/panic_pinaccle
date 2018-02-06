using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Globals;

public class MainManager : MonoBehaviour {

    enum RoundState
    {
        NONE,
        INTRO,
        PLAYING,
        PAUSED,
        OUTRO
    }

    public GameObject playerPrefab;
    public float roundLength_Intro;
    public float roundLength_Active;
    public float roundLength_Outro;
    public Transform[] playerSpawns;
    public Color[] playerColors;
    public Transform startTransform;
    public Transform endTransform;
    public float endDistanceThresh;
    public Transform boundsTransform;
    public GameObject[] boundsObjects;
    public float boundsMoveRate;
    public float boundsScaleRate;
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
	void Start () {

        //timer settings
        currentRoundTime = roundLength_Intro;
        roundState = RoundState.INTRO;

        //bounds settings
        if (endTransform && startTransform)
        {
            boundsTransform.position = startTransform.position;
            boundsDirection = new Vector3(
                endTransform.position.x - startTransform.position.x,
                endTransform.position.y - startTransform.position.y,
                endTransform.position.z - startTransform.position.z);
            boundsDirection.Normalize();
        }
        else
        {
            roundInactive = true;
        }

        //player settings
        //ui
        players = new GameObject[4];
        playerAliveCount = 0;
        playerCompleteCount = 0;
        for(int i = 0; i < PLAYER_COUNT; i++)
        {
            players[i] = Instantiate(playerPrefab, playerSpawns[i]);
            playerAliveCount++;
            players[i].GetComponent<PlayerControls>().Init(i + 1, PlayerControls.PlayerState.INTRO, playerColors[i]);
            //ui
            playerInfoSlots[i].SetActive(true);
            playerInfoSlots[i].GetComponentInChildren<Text>().color = players[i].GetComponent<PlayerControls>().Color;
            
        }
	}
	
	// Update is called once per frame
	void Update () {
        switch (roundState)
        {
            case RoundState.INTRO:
                currentRoundTime -= Time.deltaTime;
                if (currentRoundTime < 0)
                {
                    roundState = RoundState.PLAYING;
                    currentRoundTime = roundLength_Active;
                    foreach (GameObject p in players)
                    {
                        if (p) { p.GetComponent<PlayerControls>().SetState(PlayerControls.PlayerState.PLAYING); }
                    }
                }
                
                break;
            case RoundState.PLAYING:

                currentRoundTime -= Time.deltaTime;
                if (currentRoundTime < 0 || playerAliveCount <= 0)
                {
                    roundState = RoundState.OUTRO;
                    currentRoundTime = roundLength_Outro;
                    
                    //deactivate players
                    foreach(GameObject p in players)
                    {
                        if (p) { p.GetComponent<PlayerControls>().SetState(PlayerControls.PlayerState.OUTRO); }
                    }
                }

                //move level bounds
                if (roundInactive) { break; }
                if(Vector3.Distance(boundsTransform.position, endTransform.position) > endDistanceThresh) {
                    boundsTransform.position = boundsTransform.position + boundsDirection * boundsMoveRate * Time.deltaTime;
                }
                else
                {
                    boundsTransform.position = endTransform.position;
                    foreach(GameObject go in boundsObjects)
                    {
                        go.transform.localScale = go.transform.localScale + boundsScale * boundsScaleRate;
                    }
                }
                
                break;
            case RoundState.PAUSED:
                
                break;
            case RoundState.OUTRO:
                currentRoundTime -= Time.deltaTime;
                if (currentRoundTime < 0)
                {
                    currentRoundTime = 0;
                    //@GOTO next round
                    Debug.Log("ROUND COMPELTE");
                }
                break;
        }
        
        UpdateUI();
	}

    private void UpdateUI()
    {
        roundTimeText.text = "Time Remaining : " + ((int)(currentRoundTime)).ToString();
        roundStateText.text = "Round State: " + roundState.ToString();
        
        if (roundState != RoundState.INTRO) alertText.enabled = false;
        switch (roundState)
        {
            case RoundState.INTRO:
                if(currentRoundTime > 3)
                {
                    alertText.text = "3...";
                }
                else if(currentRoundTime > 2)
                {
                    alertText.text = "2...";
                }
                else if(currentRoundTime > 1)
                {
                    alertText.text = "1...";
                }
                else
                {
                    alertText.text = "PANIC!";
                }
                break;
        }

        //debug info
        playerBlastText.text = "";

        //player info
        for (int i=0; i < players.Length; i++)
        {
            if (!players[i]) continue;

            //Display player position for this round if knockedout
            if(players[i].GetComponent<PlayerControls>().State == PlayerControls.PlayerState.DEAD 
                || players[i].GetComponent<PlayerControls>().State == PlayerControls.PlayerState.OUTRO)
            {
                switch (players[i].GetComponent<PlayerControls>().RoundPosition)
                {
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

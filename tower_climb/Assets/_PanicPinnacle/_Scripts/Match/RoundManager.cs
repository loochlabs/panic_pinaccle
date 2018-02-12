using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Legacy;
using PanicPinnacle.Combatants;
using TeamUtility.IO;

namespace PanicPinnacle.Match
{
    public class RoundManager : MonoBehaviour
    {


        //ROUND SETTINGS
        //@TODO create round template for round settings
        //      -round lengths (intro, outro)
        [SerializeField]
        private RoundTemplate roundTemplate;

        
        //LEVEL SETTINGS
        //      -spawn locations
        //      -level start, end transform
        //      -bounds main transform
        //      -bounds individual gameobjects (N,S,E,W)
        //      -bounds move rate
        //      -bounds end distance threshold
        //@TODO select level from pool of levels
        [SerializeField]
        private GameObject levelPrefab;


        //SCENE FIELDS
        private RoundState state;
        private GameObject mainCamera;
        private LevelSettings level;
        private GameObject[] players = new GameObject[4];

        // Use this for initialization
        void Start()
        {
            Debug.Log("ROund Manager START");
            //Create initial round settings
            state = RoundState.intro;
            //Setup our level and all settings
            Instantiate(levelPrefab, transform);
            level = levelPrefab.GetComponent<LevelSettings>();
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            mainCamera.GetComponent<CameraControls>().boundsCenter = level.BoundsCenter;

            Debug.Log("Player Count: " + MatchManager.ActivePlayerCount);
            //create players from Match Manager settings
            for (int i=0; i<MatchManager.ActivePlayerCount; i++)
            {
                players[i] = Instantiate(MatchManager.MatchTemplate.PlayerPrefab, level.Spawns[i]);
                players[i].GetComponent<Player>().Prepare((PlayerID)i+1, MatchManager.MatchTemplate.PlayerColors[i]);
                players[i].GetComponent<Player>().SetState(CombatantState.intro);
                Debug.Log("Player created : " + players[i].ToString());
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    public enum RoundState
    {
        intro = 0,          //intro cinematic
        countdown = 1,      //3,2,1, panic!
        playing = 2,        //
        pause = 3,          //
        allfail = 4,        //all players have failed (knocked out of bounds)
        outro = 5,          //outro cinematic
        complete = 6        //setup and goto Match Tally scene
    }
}

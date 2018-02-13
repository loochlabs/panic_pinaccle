using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Legacy;
using PanicPinnacle.Combatants;
using TeamUtility.IO;
using DG.Tweening;

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
        private int playerActiveCount;
        private int playerCompleteCount;
        //bounds info for end scaling
        private Vector3 boundsScale = new Vector3(1, 1, 0);

        #region GETTERS AND SETTERS

        public int PlayerActiveCount
        {
            get { return playerActiveCount; }
            set { playerActiveCount = value; }
        }

        public int PlayerCompleteCount
        {
            get { return playerCompleteCount; }
            set { playerCompleteCount = value; }
        }
        #endregion

        

        // Use this for initialization
        void Start()
        {
            Debug.Log("ROund Manager START");
            //Setup Match Manager
            MatchManager.Round = this;
            
            //create level
            level = levelPrefab.GetComponent<LevelSettings>();
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

            //create players from Match Manager settings
            playerActiveCount = MatchManager.ActivePlayerCount;
            playerCompleteCount = 0;
            for (int i=0; i<playerActiveCount; i++)
            {
                players[i] = Instantiate(MatchManager.MatchTemplate.PlayerPrefab, level.Spawns[i]);
                players[i].GetComponent<Player>().Prepare((PlayerID)(i+1), MatchManager.MatchTemplate.PlayerColors[i]);
                players[i].GetComponent<Player>().SetState(CombatantState.intro);
                Debug.Log("Player created : " + players[i].ToString() + ", pid: " + players[i].GetComponent<Player>().Playerid);
            }
            
            //Create initial round settings
            SetState(RoundState.intro);
        }


        //@TODO might want to wrap this up with behaviors like Combatants
        private void Update()
        {
            switch (state)
            {
                case RoundState.playing:
                    //move bounds and camera up through the level
                    if (Vector3.Distance(level.BoundsCenter.position, level.End.position) > level.BoundsEndDistThreshold)
                    {
                        level.BoundsCenter.position = level.BoundsCenter.position + level.BoundsDirection * level.BoundsMoveRate * Time.deltaTime;
                    }
                    else
                    {
                        //bounds expansion at end of level
                        level.BoundsCenter.position = level.End.position;
                        foreach (GameObject go in level.BoundsObjects)
                        {
                            go.transform.localScale = go.transform.localScale + boundsScale * level.BoundsEndScaleRate;
                            //check for end of bounds scaling
                            //@TODO cleaner check than this magic number
                            if (go.transform.localScale.magnitude > 200)
                            {
                                SetState(RoundState.outro);
                                break;
                            }
                        }
                    }
                    break;
            }
        }


        private void SetState(RoundState state)
        {
            this.state = state;

            //set and specific settings for this state
            switch (state)
            {
                case RoundState.intro:
                    //focus camera on top at start
                    mainCamera.GetComponent<CameraControls>().focusTransform = level.End;
                    //intro cinematic, pan downwards from level.End to level.Start
                    //@TEMP
                    Sequence introSeq = DOTween.Sequence();
                    introSeq.AppendInterval(interval: 1f);
                    introSeq.AppendCallback(new TweenCallback(delegate {
                        SetState(RoundState.countdown);
                    }));
                    introSeq.Play();
                    break;

                case RoundState.countdown:
                    mainCamera.GetComponent<CameraControls>().focusTransform = level.BoundsCenter;
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
                        SetState(RoundState.playing);
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
                        SetState(RoundState.complete);
                    }));
                    outroSeq.Play();
                    break;

                case RoundState.complete:
                    //@TODO goto Match Tally
                    Debug.Log("ROUND COMPLETE");
                    
                    break;
            }
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

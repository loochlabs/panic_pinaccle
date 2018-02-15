﻿using PanicPinnacle.Combatants;
using PanicPinnacle.Combatants.Behaviors.Updates;
using PanicPinnacle.Legacy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Match;

namespace PanicPinnacle.Combatants.Behaviors.Updates
{
    /// <summary>
    /// Self contained behavior for various states of Player.
    /// </summary>
    public class StateFixedUpdateBehavior : CombatantFixedUpdateBehavior
    {
        #region TEMPLATE ASSETS
        [SerializeField]
        private GameObject knockoutSpriteObject;
        #endregion

        #region SCENE FIELDS
        //maintain state of combatant from previous frame
        private CombatantState prevConbatantState;
        //particle system in player prefab
        private GameObject dazedParticleObject;
        private GameObject punchingParticleObject;
        #endregion

        public override void Prepare(Combatant combatant)
        {
            //references for particle systems
            dazedParticleObject = combatant.gameObject.transform.Find("Dazed Particles").gameObject;
            dazedParticleObject.SetActive(false);
            punchingParticleObject = combatant.gameObject.transform.Find("Punching Particles").gameObject;
            punchingParticleObject.SetActive(false);
        }

        public override void FixedUpdate(Combatant combatant)
        {
            if (combatant.State != prevConbatantState) {
                UpdateState(combatant, combatant.State);
            }
            prevConbatantState = combatant.State;
        }

        
        public override void OnTriggerEnter2D(Combatant combatant, Collider2D collision) {
            //player knockout
            //@TODO: might want to make this a functional call when fleshing out Round Manager
            if (collision.gameObject.tag == "Bound")
            {
                Debug.Log("KNOCKOUT! " + combatant);
                combatant.SetState(CombatantState.dead);
                combatant.FinalRoundPosition = MatchManager.Round.PlayerActiveCount--;
                //keep track of knockout scoring
                if (combatant.RecentAggressor)
                {
                    //TODO calculate this is MatchManager.Round
                    //MatchManager.AddScore(combatant.RecentAggressor.Playerid, ScoreType.knockout);
                }
            }

            //goal check
            if (collision.gameObject.tag == "Goal")
            {
                Debug.Log("ROUND WIN: " + combatant);
                combatant.SetState(CombatantState.dead); //@TODO: dead for now, might be outro state instead
                combatant.FinalRoundPosition = ++MatchManager.Round.PlayerCompleteCount;
                MatchManager.Round.PlayerActiveCount--;
                //add score for this player
                //@TODO calculate this in MatchManager.Round
                //MatchManager.AddScore(combatant.Playerid, ScoreType.survival);
            }
        }



        /// <summary>
        /// Handle updating the current state of combatant.
        /// </summary>
        /// <param name="state">Current state of combatant.</param>
        private void UpdateState(Combatant combatant, CombatantState state)
        {
            switch (state)
            {
                case CombatantState.dazed:
                    //emit smoke particles during dazed state
                    dazedParticleObject.SetActive(true);
                    //disable all other particle systems
                    punchingParticleObject.SetActive(false);

                    break;

                case CombatantState.playing:
                    //disable any particles during played state
                    dazedParticleObject.SetActive(false); 
                    punchingParticleObject.SetActive(false);
                    break;

                case CombatantState.punching:
                    //emit smoke particles during punching state
                    punchingParticleObject.SetActive(true);
                    //disable all other particle systems
                    dazedParticleObject.SetActive(false);

                    break;

                case CombatantState.dead:
                    //create a knockout sprite and disable the player
                    GameObject.Instantiate(knockoutSpriteObject, combatant.transform.position, combatant.transform.localRotation, 
                        MatchManager.Round.transform);
                    combatant.gameObject.SetActive(false);
                    break;
            }
        }


        #region UNUSED WRAPPERS
        public override void OnCollisionEnter2D(Combatant combatant, Collision2D collision) { }
        public override void OnCollisionExit2D(Combatant combatant, Collision2D collision) { }
        public override void OnCollisionStay2D(Combatant combatant, Collision2D collision) { }
        public override void OnTriggerExit2D(Combatant combatant, Collider2D collision) { }
        public override void OnTriggerStay2D(Combatant combatant, Collider2D collision) { }
        #endregion



        #region INSPECTOR JUNK
        private static string behaviorDescription = "Manage the various states of Combatant.";
        protected override string InspectorDescription
        {
            get
            {
                return behaviorDescription;
            }
        }
        #endregion
    }

}
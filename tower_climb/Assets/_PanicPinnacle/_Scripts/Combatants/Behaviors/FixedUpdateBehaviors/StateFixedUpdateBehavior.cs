using PanicPinnacle.Combatants;
using PanicPinnacle.Combatants.Behaviors.Updates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Combatants.Behaviors.Updates
{
    /// <summary>
    /// Self contained behavior for various states of Player.
    /// </summary>
    public class StateFixedUpdateBehavior : CombatantFixedUpdateBehavior
    {

        //particle system in player prefab
        private GameObject dazedParticleObject;
        private GameObject punchingParticleObject;


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
            switch (combatant.State)
            {
                case CombatantState.dazed:
                    //emit smoke particles during dazed state
                    if (!dazedParticleObject.activeSelf) { dazedParticleObject.SetActive(true); }
                    
                    break;

                case CombatantState.playing:
                    //stop any particles during played state
                    //@TODO: might need a cleaner way of checking/disabling this
                    if (dazedParticleObject.activeSelf) { dazedParticleObject.SetActive(false); }
                    if (punchingParticleObject.activeSelf) { punchingParticleObject.SetActive(false); }

                    break;

                case CombatantState.punching:
                    //emit smoke particles during dazed state
                    if (!punchingParticleObject.activeSelf) { punchingParticleObject.SetActive(true); }

                    break;
            }
        }


        #region UNUSED WRAPPERS
        public override void OnCollisionEnter2D(Combatant combatant, Collision2D collision) { }
        public override void OnCollisionExit2D(Combatant combatant, Collision2D collision) { }
        public override void OnCollisionStay2D(Combatant combatant, Collision2D collision) { }
        public override void OnTriggerEnter2D(Combatant combatant, Collider2D collision) { }
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
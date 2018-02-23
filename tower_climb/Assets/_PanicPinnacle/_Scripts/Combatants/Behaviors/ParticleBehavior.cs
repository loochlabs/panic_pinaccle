using PanicPinnacle.Combatants;
using PanicPinnacle.Combatants.Behaviors;
using PanicPinnacle.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehavior : CombatantBehavior, IUpdateEvent
{

    private ParticleSystem dazedParticles;
    private ParticleSystem punchingParticles;
    private CombatantStateType prevState;

    public override void Prepare(Combatant combatant)
    {
        //grab the particle systems on our Combatant
        //@TODO: not sure if this is the best way but it keeps from having to slap references on the inspector.
        ParticleSystem[] particles = combatant.GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem ps in particles)
        {
            if (ps.gameObject.name == "Dazed Particles") { dazedParticles = ps; }
            if (ps.gameObject.name == "Punching Particles") { punchingParticles = ps; }
        }

        if (dazedParticles) {
            dazedParticles.Stop();
        }
        if (punchingParticles)
        {
            punchingParticles.Stop();
        }
    }

    public void Update(CombatantEventParams eventParams)
    {
        if(prevState != eventParams.combatant.State)
        {
            switch (eventParams.combatant.State) {
                case CombatantStateType.dazed:
                    dazedParticles.Play();
                    punchingParticles.Stop();
                    break;
                case CombatantStateType.punching:
                    punchingParticles.Play();
                    dazedParticles.Stop();
                    break;
                default:
                    punchingParticles.Stop();
                    dazedParticles.Stop();
                    break;
            }
        }
        prevState = eventParams.combatant.State;
    }


    #region INSPECTOR JUNK
    private static string inspectorDescription = "The behavior that controls combatant animations.";
    protected override string InspectorDescription
    {
        get
        {
            return inspectorDescription;
        }
    }
    #endregion
}

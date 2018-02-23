using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Events;
using PanicPinnacle.Combatants.Behaviors;
using PanicPinnacle.Combatants;

public class AnimationBehavior : CombatantBehavior, IUpdateEvent
{

    private Animator anim;
    private CombatantStateType prevState;

    #region INTERFACES METHODS
    public override void Prepare(Combatant combatant)
    {
        anim = combatant.GetComponentInChildren<Animator>();
    }

    public void Update(CombatantEventParams eventParams)
    {
        //player running
        Vector3 inputDirection =
            eventParams.combatant.CombatantInput.GetMovementDirection(combatant: eventParams.combatant);
        anim.SetBool("running", inputDirection.magnitude > 0);
        
        //state specific anims
        if (prevState != eventParams.combatant.State) {
            switch (eventParams.combatant.State)
            {
                case CombatantStateType.playing:
                    anim.SetBool("dazed", false);
                    anim.SetBool("punching", false);
                    break;
                case CombatantStateType.dazed:
                    anim.SetBool("dazed", true);
                    anim.SetBool("punching", false);
                    break;
                case CombatantStateType.punching:
                    anim.SetBool("punching", true);
                    anim.SetBool("dazed", false);
                    break;
            }
        }
        prevState = eventParams.combatant.State;
    }

    #endregion

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

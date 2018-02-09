using PanicPinnacle.Combatants;
using PanicPinnacle.Combatants.Behaviors.Updates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFixedUpdateBehavior : CombatantFixedUpdateBehavior
{
    

    public override void Prepare(Combatant combatant)
    {

    }

    public override void FixedUpdate(Combatant combatant)
    {

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

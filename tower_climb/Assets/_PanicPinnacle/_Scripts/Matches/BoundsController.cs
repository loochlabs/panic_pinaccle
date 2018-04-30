using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Combatants;

namespace PanicPinnacle.Matches
{
    public class BoundsController : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "Player")
            {
                //knockout player
                Player player = collision.gameObject.GetComponent<Player>();
                if(player.State == CombatantStateType.dead || player.State == CombatantStateType.safe) { return; }

                Debug.Log("PLAYER " + player.CombatantID + " touched bounds!");
                player.Knockout();
            }
        }
    }

}



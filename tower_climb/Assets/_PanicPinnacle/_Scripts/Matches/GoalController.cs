using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Combatants;

namespace PanicPinnacle.Matches
{
    public class GoalController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                //knockout player
                Player player = collision.gameObject.GetComponent<Player>();
                Debug.Log("PLAYER " + player.CombatantID + " touched goal!");
                player.GoalTouch();
            }
        }
    }
}

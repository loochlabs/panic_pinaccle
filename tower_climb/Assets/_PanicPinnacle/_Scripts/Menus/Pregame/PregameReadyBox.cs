using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Combatants;

namespace PanicPinnacle.Menus
{
    /// <summary>
    /// Script to control collisions for Pregame Ready up.
    /// </summary>
    public class PregameReadyBox : MonoBehaviour
    {
        /// <summary>
        /// Menu manager for the pregame
        /// </summary>
        [SerializeField]
        private PreGameMenu pregameMenu;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                pregameMenu.AddPlayer(collision.gameObject.GetComponent<Player>());
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                pregameMenu.RemovePlayer(collision.gameObject.GetComponent<Player>());
            }
        }

    }
}

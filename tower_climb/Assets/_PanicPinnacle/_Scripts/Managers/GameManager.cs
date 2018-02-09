using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle {

	/// <summary>
	/// Controls the game from a big picture perspective and persists through all scenes. 
	/// </summary>
	public class GameManager : MonoBehaviour {

		public static GameManager instance;

		private void Awake() {
			if (instance == null) {
				instance = this;
			} else {
				// Destorying this object if the instance is already set will also destroy new instances of its children.
				// This is intentional.
				Destroy(this.gameObject);
			}
		}

	}


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.UI.Events {

	/// <summary>
	/// A basic class to see if I can get UnityEvents running properly on the TitleScreen.
	/// </summary>
	public class TitleScreenMenuEvents : MonoBehaviour {

		/// <summary>
		/// A callback that gets run when Play is hit on the title screen.
		/// </summary>
		public void LoadMainMenu() {
			SceneController.instance.LoadScene(sceneName: "Main Menu", showLoadingText: true, collectGarbageOnTransition: true);
		}
		/// <summary>
		/// A callback that gets run when Exit is hit on the title screen.
		/// </summary>
		public void ExitGame() {
			Application.Quit();
		}

	}
}
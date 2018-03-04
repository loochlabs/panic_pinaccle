using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Debugging {

	/// <summary>
	/// A script I'm probably going to remove like tomorrow.
	/// </summary>
	public class DEBUGPlaceholderMatchTally : MonoBehaviour {

		/// <summary>
		/// The name of the scene to load after a few seconds of this screen.
		/// </summary>
		[SerializeField]
		private string sceneToLoadNext = "";

		private IEnumerator Start() {
			yield return new WaitForSeconds(5f);
			SceneController.instance.LoadScene(sceneName: sceneToLoadNext, showLoadingText: false, collectGarbageOnTransition: true);
		}
	}


}
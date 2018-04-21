using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Matches;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace PanicPinnacle.UI.Events {

	/// <summary>
	/// A basic class to see if I can get UnityEvents running properly on the TitleScreen.
	/// </summary>
	public class TitleScreenMenuEvents : MonoBehaviour {

        #region SCENE REFERENCES
        
        /// <summary>
        /// Play button on main menu.
        /// This is the first selected object on Start.
        /// </summary>
        [SerializeField]
        private GameObject playButton;
		/// <summary>
		/// Options button on the main menu. The only reason I need this is for consistency when moving back from the settings menu.
		/// </summary>
		[SerializeField]
		private GameObject optionsButton;
		/// <summary>
		/// The button that gets selected at first when the screen transitions over to the settings.
		/// </summary>
		[SerializeField]
		private GameObject okayButton;
		/// <summary>
		/// The RectTransform of the game object that encapsulates the entire settings menu.
		/// (I could just as easily have gotten the GameObject but with canvases its easier for me to use the RectTransform.)
		/// </summary>
		[SerializeField]
		private RectTransform entireMenuTransform;
		#endregion


		#region UNITY FUNCTIONS

		private void Start() {
			GameController.instance.InputEventSystem.firstSelectedGameObject = playButton;
			GameController.instance.InputEventSystem.SetSelectedGameObject(playButton);
		}

		#endregion


		/// <summary>
		/// A callback that gets run when Play is hit on the title screen.
		/// </summary>
		public void LoadMainMenu() {
            Debug.Log("STARTING NEW MATCH");
            GameController.instance.StartMatch();
		}
		/// <summary>
		/// A callback that gets run when Exit is hit on the title screen.
		/// </summary>
		public void ExitGame() {
			Application.Quit();
		}
		/// <summary>
		/// A callback that gets run when the Options button is hit on the title screen.
		/// </summary>
		public void TransitionToOptions() {
			// Use DOTween to transition the menu over.
			this.entireMenuTransform.DOAnchorPos(
				endValue: (this.entireMenuTransform.anchoredPosition + new Vector2(x: -1920f, y: 0f)),
				duration: 0.5f,
				snapping: true);
			// Also set the okay button as the new selection (this technically happens before the tween is completed but its a half second so whatever lmao)
			GameController.instance.InputEventSystem.SetSelectedGameObject(this.okayButton);
		}
		/// <summary>
		/// A callback that gets run when the Options button is hit on the settings screen.
		/// </summary>
		public void TransitionBackFromOptions() {
			// Use DOTween to transition the menu over.
			this.entireMenuTransform.DOAnchorPos(
				endValue: (this.entireMenuTransform.anchoredPosition + new Vector2(x: 1920f, y: 0f)),
				duration: 0.5f,
				snapping: true);
			// Set the play button back as the default option.
			GameController.instance.InputEventSystem.SetSelectedGameObject(this.optionsButton);
		}
		/// <summary>
		/// A callback that gets run when the Credits button is hit on the title screen.
		/// </summary>
		public void TransitionToCredits() {
			Debug.LogError("Not implemented yet.");
		}
	}
}
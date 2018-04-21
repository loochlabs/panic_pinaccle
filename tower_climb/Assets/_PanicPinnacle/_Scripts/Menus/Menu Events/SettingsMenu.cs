using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PanicPinnacle.UI {

	/// <summary>
	/// Provides access to ways to change certain settings for the game.
	/// </summary>
	public class SettingsMenu : MonoBehaviour {

		#region FIELDS - SETTINGS
		/// <summary>
		/// The list of resolutions to pick from.
		/// Note that I'm hard coding these as the values for the list, but the actual strings used for their labels in the drop down are not dependent on these.
		/// If you need to change them in the drop down, you'll need to manually change their labels.
		/// </summary>
		private List<Vector2Int> resolutionsList = new List<Vector2Int>() {
			{ new Vector2Int(x: 1920, y: 1090) },
			{ new Vector2Int(x: 1600, y: 900) },
			{ new Vector2Int(x: 1366, y: 768) },
			{ new Vector2Int(x: 1280, y: 720) }
		};
		#endregion

		#region FIELDS - SCENE REFERENCES
		/// <summary>
		/// The toggle that sets whether the game is running in fullscreen or not.
		/// I need this because it's initial state of being on or off is dependent on whehter the game was booted up in fullscreen in the first place.
		/// (E.x., it shouldn't default to saying the game is fullscreen when in reality it booted up in windowed mode.)
		/// </summary>
		[SerializeField]
		private Toggle fullScreenToggle;
		#endregion

		private void Start() {
			// Tell the full screen toggle the current state of the game. 
			this.fullScreenToggle.isOn = Screen.fullScreen;
		}


		#region UNITY EVENTS
		/// <summary>
		/// Handles the event that occurs when a selection is picked from the dropdown of resolutions.
		/// </summary>
		/// <param name="itemNumber">The index of the item from the drop down.</param>
		public void OnResolutionChangeEvent(int itemIndex) {
			Debug.Log("RESOLUTION CHANGE EVENT: " + itemIndex);
			// Grab the width and height from the appropriate element in the resolutions list.
			int width = this.resolutionsList[itemIndex].x;
			int height = this.resolutionsList[itemIndex].y;
			// Set the new resolution. Screen.fullscreen holds the current state of the fullscreen toggle, so passing it in won't change anything.
			Screen.SetResolution(width: width, height: height, fullscreen: Screen.fullScreen);
		}
		/// <summary>
		/// Handles the event that occurs when the volume slider is adjusted.
		/// </summary>
		/// <param name="value"></param>
		public void OnVolumeChangeEvent(float value) {
			Debug.Log("VOLUME CHANGE EVENT: " + value);
			// Pass this value along to the AudioController. It's the one that makes use of it.
			AudioController.VolumeMultiplier = value;
		}
		/// <summary>
		/// Handles the event that occurs when the full screen toggle is switched.
		/// </summary>
		/// <param name="toggle"></param>
		public void OnFullScreenChangeEvent(bool toggle) {
			Debug.Log("FULL SCREEN CHANGE EVENT: " + toggle);
			// Screen.fullscreen can also be set to toggle fullscreen automatically.
			Screen.fullScreen = toggle;
		}
		#endregion


	}


}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using PanicPinnacle.UI;
using PanicPinnacle.Matches;

namespace PanicPinnacle {

	/// <summary>
	/// Manages how scene transitions happen and works as an entry point to load scenes.
	/// I would call this SceneManager but that conflicts with a Unity defined class so I'm keeping it SceneController for now.
	/// </summary>
	public class SceneController : MonoBehaviour {

		public static SceneController instance;

		private void Awake() {
			if (instance == null) {
				instance = this;
			} else {
				// Do not add a Destroy(this) line to this else case. It's taken care of by GameController.
			}
		}

		#region SCENE LOADING
		/// <summary>
		/// Loads the scene specified by the given name.
		/// </summary>
		/// <param name="sceneName">The name of the scene to load.</param>
		/// <param name="showLoadingText">Should the Loading text be displayed to the players?</param>
		/// <param name="collectGarbageOnTransition">Should the garbage collector be run while this scene is loading?</param>
		public void LoadScene(string sceneName, bool showLoadingText = true, bool collectGarbageOnTransition = false) {

			// A quick float to modify how long the fade in/out transitions are.
			float fadetime = 0.5f;

			// Tell the AudioController to fade out. Unless we want songs to persist through scenes, I don't see any reason not to put this here.
			AudioController.instance.StopMusic(fadeTime: (fadetime-0.5f));

			// Create a new sequence.
			Sequence seq = DOTween.Sequence();
			// Fade out the screen.
			seq.AppendCallback(new TweenCallback(delegate {
				// Call the SceneFaderCanvasGroup and tell it to fade the screen. Also says whether the loading text should be shown.
				SceneFaderCanvasGroup.instance.FadeOut(color: Color.black, fadeOut: fadetime, showLoadingText: showLoadingText);
			}));
			// Wait slightly longer than the fade time.
			seq.AppendInterval(fadetime + 0.1f);
			seq.AppendCallback(new TweenCallback(delegate {
				// If garbage was specified to be collected, do so now, because the loading screen is a valid opprotunity.
				if (collectGarbageOnTransition == true) {
					GarbageCollectorHelper.CollectGarbage();
				}

				// Add a callback for once the scene is loaded before actually loading it up. (This callback fades the screen back in.)
				SceneManager.sceneLoaded += this.OnSceneLoaded;
				// Load the specified scene.
				SceneManager.LoadScene(sceneName: sceneName);
			}));
			// Play that junk.
			seq.Play();
		}
		/// <summary>
		/// Loads the scene that's required from the specified RoundSettings.
		/// </summary>
		/// <param name="roundSettings"></param>
		public void LoadScene(RoundSettings roundSettings) {
			// Add OnRoundStart to the callbacks that need to get run when the scene gets loaded.
			SceneManager.sceneLoaded += this.OnRoundStart;
			// For now, just load up the scene that's listed in the round settings.
			this.LoadScene(sceneName: roundSettings.SceneName, showLoadingText: true, collectGarbageOnTransition: true);
		}
		#endregion

		#region ONSCENELOADED CALLBACKS
		/// <summary>
		/// A callback to run when the scene is finally loaded.
		/// </summary>
		/// <param name="scene">The scene that was just loaded.</param>
		/// <param name="mode">The LoadSceneMode. I have no idea what this is but it's required. lol</param>
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
			Debug.Log("OnSceneLoaded: " + scene.name);
			// Fade in the screen.
			SceneFaderCanvasGroup.instance.FadeIn(fadeIn: 0.5f);
			// Remember to remove this callback once the scene has been loaded.
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}
		/// <summary>
		/// An extra callback that gets called when a round begins.
		/// </summary>
		/// <param name="scene"></param>
		/// <param name="mode"></param>
		private void OnRoundStart(Scene scene, LoadSceneMode mode) {
			// Debug.Break();
			/*Sequence seq = DOTween.Sequence();
			seq.AppendInterval(0.4f);
			seq.AppendCallback(new TweenCallback(delegate {
				// This is going to be run when a round is loaded. Call the RoundController and tell it to get moving.
				// The required info should have been passed to it already.
				RoundController.instance.StartRound();
				// Remove OnRoundStart from the callbacks.
				SceneManager.sceneLoaded -= this.OnRoundStart;
			}));*/


            //STEVE: call this from RoundController.Start bc we need to wait for instantiation of Level Prefab to properlly setup players here.
			// This is going to be run when a round is loaded. Call the RoundController and tell it to get moving.
			// The required info should have been passed to it already.
			RoundController.instance.StartRound();
			
            
            // Remove OnRoundStart from the callbacks.
			SceneManager.sceneLoaded -= this.OnRoundStart;
		}
		#endregion

	}


}
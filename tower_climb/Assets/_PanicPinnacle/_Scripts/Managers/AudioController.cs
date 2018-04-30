using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle {

	/// <summary>
	/// Used as an entry point for playing music/sfx.
	/// Please route any audio clips you want to play through here.
	/// </summary>
	public class AudioController : MonoBehaviour {

		public static AudioController instance;

		#region FIELDS - SETTINGS
		/// <summary>
		/// A setting that determines how much the volume should be multiplied by. 
		/// </summary>
		private static float volumeMultiplier = 1f;
		/// <summary>
		/// A setting that determines how much the volume should be multiplied by. 
		/// </summary>
		public static float VolumeMultiplier {
			get {
				return volumeMultiplier;
			}
			set {
				// Clamp it between 0 and 1 just to be on the safe side.
				volumeMultiplier = Mathf.Clamp01(value);
			}
		}
		#endregion

		#region FIELDS - SCENE REFERENCES
		/// <summary>
		/// A list of audio sources that are used for playing music/sfx.
		/// </summary>
		private List<AudioSource> audioSources = new List<AudioSource>();
		#endregion

		private void Awake() {
			if (instance == null) {
				instance = this;
				// Don't call Destroy even if the condition above fails.
				// AudioController will be a child of another script who will take care of it.
			}
			// Go through each of this GameObject's children and 
			foreach (AudioSource audioSource in this.gameObject.GetComponentsInChildren<AudioSource>()) {
				this.audioSources.Add(audioSource);
			}
		}

		#region SFX
		/// <summary>
		/// Plays an SFX.
		/// </summary>
		/// <param name="clip">The AudioClip of the SFX.</param>
		/// <param name="volumeScale">The volume to play this SFX at.</param>
		public void PlaySFX(AudioClip clip, float volumeScale = 1f) {
			// The volume scale will also get multiplied by the volumeMultiplier, which is set by the settings menu at the start of the game.
			this.audioSources[0].PlayOneShot(clip: clip, volumeScale: (volumeScale * volumeMultiplier));
		}
		/// <summary>
		/// Plays an SFX.
		/// </summary>
		/// <param name="type">The type of SFX to play.</param>
		/// <param name="volumeScale">The volume to play this SFX at.</param>
		public void PlaySFX(SFXType type, float volumeScale = 1f) {
			// The volume scale will also get multiplied by the volumeMultiplier, which is set by the settings menu at the start of the game.

			// When given a type, retreive its clip from the DataManager and play it as normal.
			this.PlaySFX(
				clip: DataController.instance.GetSFX(type: type),
				volumeScale: (volumeScale * volumeMultiplier));
		}
		#endregion

		#region MUSIC
		/// <summary>
		/// Plays an audio track.
		/// </summary>
		/// <param name="clip">The music clip to play.</param>
		/// <param name="volumeScale">The volume to play this music at.</param>
		public void PlayMusic(AudioClip clip, float volumeScale = 1f) {
			// Give it the clip to play.
			this.audioSources[1].clip = clip;
			// Reset the volume, because if the music was previously faded out, the volume needs to be toggled again.
			this.audioSources[1].volume = volumeScale * VolumeMultiplier;
			// Play it.
			this.audioSources[1].Play();
		}
		/// <summary>
		/// Fades the music out and stops it.
		/// </summary>
		public void StopMusic(float fadeTime = 0.5f) {
			// To stop the music, just call the FadeMusic routine.
			this.StartCoroutine(this.FadeMusic(fadeTime: fadeTime));
		}
		/// <summary>
		/// Fades the music to a specific volume.
		/// </summary>
		/// <param name="fadeTime">The amount of time to fade the volume.</param>
		/// <returns></returns>
		private IEnumerator FadeMusic(float fadeTime) {
			// Grab the volume level at the start.
			float startVolume = this.audioSources[1].volume;
			// While the audio is still higher than zero, continue to fade it out.
			while (this.audioSources[1].volume > 0) {
				this.audioSources[1].volume -= startVolume * (Time.deltaTime / fadeTime);
				yield return null;
			}
			// Stop it completely at this point.
			this.audioSources[1].Stop();
		}
		#endregion

	}

}
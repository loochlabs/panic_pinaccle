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
			this.audioSources[0].PlayOneShot(clip: clip, volumeScale: volumeScale);
		}
		/// <summary>
		/// Plays an SFX.
		/// </summary>
		/// <param name="type">The type of SFX to play.</param>
		/// <param name="volumeScale">The volume to play this SFX at.</param>
		public void PlaySFX(SFXType type, float volumeScale = 1f) {
			// When given a type, retreive its clip from the DataManager and play it as normal.
			this.PlaySFX(
				clip: DataController.instance.GetSFX(type: type),
				volumeScale: volumeScale);
		}
		#endregion

	}

}
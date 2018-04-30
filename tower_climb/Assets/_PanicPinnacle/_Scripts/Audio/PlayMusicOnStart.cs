using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle {

	/// <summary>
	/// A quick and dirty solution for playing music at the start of the level.
	/// </summary>
	public class PlayMusicOnStart : MonoBehaviour {

		#region FIELDS - ASSETS
		/// <summary>
		/// The music clip to play when this scene is loaded up.
		/// </summary>
		[SerializeField]
		private AudioClip musicClip;
		/// <summary>
		/// The volume scale to play the track at.
		/// </summary>
		[SerializeField]
		private float volumeScale = 0.7f;
		#endregion

		private void Start() {
			// On start, play the music.
			AudioController.instance.PlayMusic(clip: this.musicClip, volumeScale: this.volumeScale);
		}

	}


}
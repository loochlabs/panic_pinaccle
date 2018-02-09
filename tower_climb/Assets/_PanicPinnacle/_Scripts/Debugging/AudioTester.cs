using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace PanicPinnacle.Debugging {

	/// <summary>
	/// A sample class to make sure that the AudioManager is working as intended.
	/// </summary>
	public class AudioTester : SerializedMonoBehaviour {

		#region FIELDS
		/// <summary>
		/// The type of SFX to test out.
		/// </summary>
		[SerializeField]
		private SFXType sfxType = SFXType.MenuHover;
		[SerializeField]
		private float volumeScale = 1f;
		#endregion

		/// <summary>
		/// Test to make sure the AudioManager is working properly.
		/// </summary>
		[SerializeField]
		public void TestSFX() {
			AudioManager.instance.PlaySFX(type: this.sfxType, volumeScale: this.volumeScale);
		}

	}


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle {

	/// <summary>
	/// Serves as an interface for accessing data such as audio/images/etc.
	/// </summary>
	public class DataController : MonoBehaviour {

		public static DataController instance;

		#region FIELDS - GLOBAL ASSET DATA
		/// <summary>
		/// The ScriptableObject that contains a collection of assets to be used by the game.
		/// </summary>
		[SerializeField]
		private GlobalAssetData assetData;
		#endregion

		private void Awake() {
			if (instance == null) {
				instance = this;
			}
			// Don't call Destroy even if the condition above fails.
			// DataManager will be a child of another script who will take care of it.
		}

		#region AUDIO
		/// <summary>
		/// Gets the SFX of the specified type.
		/// </summary>
		/// <param name="type">The type of SFX being requested.</param>
		/// <returns>An AudioClip of the type provided.</returns>
		public AudioClip GetSFX(SFXType type) {
			try {
				// Attempt to retrieve the specified type.
				return this.assetData.SFXDict[type];
			} catch (System.Exception e) {
				// If something goes wrong, log out the error and return the ERROR sfx.
				Debug.LogError("Couldn't get SFX from GlobalAssetData! Reason: " + e);
				return this.assetData.SFXDict[SFXType.ERROR];
			}
		}
		#endregion

	}


}
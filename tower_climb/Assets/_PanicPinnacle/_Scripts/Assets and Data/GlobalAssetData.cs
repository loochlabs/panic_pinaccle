using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace PanicPinnacle {

	/// <summary>
	/// A ScriptableObject that contains the data that should be used universally throughout the entire project.
	/// </summary>
	[CreateAssetMenu(fileName = "New GlobalAssetData", menuName = "Panic Pinnacle/Global Asset Data")]
	public class GlobalAssetData : SerializedScriptableObject {

		#region FIELDS - AUDIO
		/// <summary>
		/// Contains the different kinds of sound effects that will be used in the game.
		/// </summary>
		[TabGroup("Audio", "SFX"), SerializeField]
		private Dictionary<SFXType, AudioClip> sfxDict = new Dictionary<SFXType, AudioClip>();
		/// <summary>
		/// Contains the different kinds of sound effects that will be used in the game.
		/// </summary>
		public Dictionary<SFXType, AudioClip> SFXDict {
			get {
				return this.sfxDict;
			}
		}
		#endregion
	}


}
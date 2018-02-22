using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace PanicPinnacle.Matches {

	/// <summary>
	/// A specific collection of settings that define any given round.
	/// </summary>
	[System.Serializable]
    public class RoundSettings {

		#region FIELDS - GENERAL SETTINGS
		
		/// <summary>
		/// The name of the scene where this round takes place.
		/// </summary>
		[TabGroup("Round Settings", "General"), PropertyTooltip("The name of the scene where this round takes place."), SerializeField]
		private string sceneName = "";
		/// <summary>
		/// The name of the scene where this round takes place.
		/// </summary>
		public string SceneName {
			get {
				return this.sceneName;
			} set {
				this.sceneName = value;
			}
		}
		
		/// <summary>
		/// The max amount of time this round should last, in seconds.
		/// </summary>
		[TabGroup("Round Settings", "General"), PropertyTooltip("The max amount of time this round should last, in seconds."), SerializeField]
		private float roundTimer = 120f;
		/// <summary>
		/// The max amount of time this round should last, in seconds.
		/// </summary>
		public float RoundTimer {
			get {
				return this.roundTimer;
			}
			set {
				this.roundTimer = value;
			}
		}
		#endregion

		/*#region CLONING
		public static RoundSettings Clone(RoundSettings roundSettings) {
			return (RoundSettings)roundSettings.MemberwiseClone();
		}
		#endregion*/

	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace PanicPinnacle.UI {

	/// <summary>
	/// Simple animation that should play once the round is over.
	/// </summary>
	public class RoundOutroAnimation : SerializedMonoBehaviour {

		public static RoundOutroAnimation instance;

		#region FIELDS - SCENE REFERENCES
		/// <summary>
		/// The object that gets turned on/off to show/hide the animation.
		/// </summary>
		[SerializeField]
		private GameObject animationObject;
		/// <summary>
		/// The "upper" part of the text. Says "ROUND"
		/// </summary>
		[SerializeField]
		private RectTransform upperText;
		/// <summary>
		/// The "lower" part of the text. Says "COMPLETE"
		/// </summary>
		[SerializeField]
		private RectTransform lowerText;
		#endregion

		private void Awake() {
			instance = this;
		}

		/// <summary>
		/// Play the round outtro animation.
		/// </summary>
		public void Play() {
			this.animationObject.SetActive(true);
			upperText.DOAnchorPosX(endValue: 0f, duration: 1f, snapping: true);
			lowerText.DOAnchorPosX(endValue: 0f, duration: 1f, snapping: true);
		}

	}


}
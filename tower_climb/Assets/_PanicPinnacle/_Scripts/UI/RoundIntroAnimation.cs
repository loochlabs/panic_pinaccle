using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;
using System.Linq;

namespace PanicPinnacle.UI {

	/// <summary>
	/// Handles the animation that plays at the start of the round.
	/// </summary>
	public class RoundIntroAnimation : SerializedMonoBehaviour {

		#region FIELDS - SETTINGS
		/// <summary>
		/// The amount of time this animation should take to play out.
		/// </summary>
		[SerializeField, TabGroup("Data", "Settings")]
		private float animationDuration = 3f;
		#endregion

		#region FIELDS - SCENE REFERENCES
		/// <summary>
		/// The GameObject that stores all of the animation assets itself. Good for when I just need to turn it on/off.
		/// </summary>
		[SerializeField, TabGroup("Data", "Scene References")]
		private GameObject animationGameObject;
		/// <summary>
		/// The label that shows the PANIC text.
		/// </summary>
		[SerializeField, TabGroup("Data", "Scene References")]
		private SuperTextMesh panicLabel;
		/// <summary>
		/// The images that shows the numbers counting down. 
		/// I'm using images instead of text because it's easier for me to fade images in and out.
		/// </summary>
		[SerializeField, TabGroup("Data", "Scene References")]
		private List<Image> numberImages = new List<Image>();
		/// <summary>
		/// The rect transforms of the number images. Just making this a computed property for convinence.
		/// </summary>
		private List<RectTransform> NumberRectTransforms {
			get {
				// linq is good, actually
				return this.numberImages.Select(i => i.GetComponent<RectTransform>()).ToList();
			}
		}
		/// <summary>
		/// The image that just decreases its fill amount.
		/// </summary>
		[SerializeField, TabGroup("Data", "Scene References")]
		private Image countdownFillImage;
		#endregion


		private void Start() {
			this.Play();
		}

		#region ANIMATION
		/// <summary>
		/// Plays the Round Intro Animation.
		/// </summary>
		[TabGroup("Prototyping", "Prototyping")]
		public void Play() {

			// Turn the animation object on, if it wasn't already.
			this.animationGameObject.SetActive(true);

			// Reset the start states of the different parts of the animation so they're in default positions.
			this.panicLabel.gameObject.SetActive(false);
			this.NumberRectTransforms.ForEach(rt => rt.anchoredPosition = new Vector2Int(x: 200, y: 0));
			this.numberImages.ForEach(i => i.canvasRenderer.SetAlpha(alpha: 0f));
			this.countdownFillImage.fillAmount = 1f;

			Sequence seq = DOTween.Sequence();
			seq.AppendCallback(new TweenCallback(delegate {
				this.countdownFillImage.DOFillAmount(endValue: 0f, duration: this.animationDuration + (this.animationDuration * 0.2f));
			}));


			//
			//
			// Okay I have NO idea why this was happening but DOTween was bugging out when I tried to do all of this in a for loop.
			// This is why I'm repeating the same segment of code three times.
			//
			//

			seq.AppendCallback(new TweenCallback(delegate {
				this.NumberRectTransforms[0].DOAnchorPosX(endValue: 0f, duration: (this.animationDuration / 6f), snapping: true);
				this.numberImages[0].CrossFadeAlpha(alpha: 1f, duration: (this.animationDuration / 6f), ignoreTimeScale: true);
				AudioController.instance.PlaySFX(type: SFXType.MenuSelect);
			}));
			seq.AppendInterval(interval: this.animationDuration / 3f);
			seq.AppendCallback(new TweenCallback(delegate {
				this.NumberRectTransforms[0].DOAnchorPosX(endValue: -200f, duration: (this.animationDuration / 6f), snapping: true);
				this.numberImages[0].CrossFadeAlpha(alpha: 0f, duration: (this.animationDuration / 6f), ignoreTimeScale: true);
			}));


			// AAAAAAAAAAAAAAA
			// AAAAAAAAAAAAAAA

			seq.AppendCallback(new TweenCallback(delegate {
				this.NumberRectTransforms[1].DOAnchorPosX(endValue: 0f, duration: (this.animationDuration / 6f), snapping: true);
				this.numberImages[1].CrossFadeAlpha(alpha: 1f, duration: (this.animationDuration / 6f), ignoreTimeScale: true);
				AudioController.instance.PlaySFX(type: SFXType.MenuSelect);
			}));
			seq.AppendInterval(interval: this.animationDuration / 3f);
			seq.AppendCallback(new TweenCallback(delegate {
				this.NumberRectTransforms[1].DOAnchorPosX(endValue: -200f, duration: (this.animationDuration / 6f), snapping: true);
				this.numberImages[1].CrossFadeAlpha(alpha: 0f, duration: (this.animationDuration / 6f), ignoreTimeScale: true);
			}));


			// AAAAAAAAAAAAAAA
			// AAAAAAAAAAAAAAA


			seq.AppendCallback(new TweenCallback(delegate {
				this.NumberRectTransforms[2].DOAnchorPosX(endValue: 0f, duration: (this.animationDuration / 6f), snapping: true);
				this.numberImages[2].CrossFadeAlpha(alpha: 1f, duration: (this.animationDuration / 6f), ignoreTimeScale: true);
				AudioController.instance.PlaySFX(type: SFXType.MenuSelect);
			}));
			seq.AppendInterval(interval: this.animationDuration / 3f);
			seq.AppendCallback(new TweenCallback(delegate {
				this.NumberRectTransforms[2].DOAnchorPosX(endValue: -200f, duration: (this.animationDuration / 6f), snapping: true);
				this.numberImages[2].CrossFadeAlpha(alpha: 0f, duration: (this.animationDuration / 6f), ignoreTimeScale: true);
			}));




			// Go through each of the images and slide them in one at a time.
			// This will fade them in, then fade them out as the next number comes in.
			// THANKS DOTWEEN FOR NOT WORKING LIKE YOURE SUPPOSED TO GREAT JOB
			/*for (int i = 0; i < 3; i++) {
				seq.AppendCallback(new TweenCallback(delegate {
					this.NumberRectTransforms[i].DOAnchorPosX(endValue: 0f, duration: (this.animationDuration / 6f), snapping: true);
					this.numberImages[i].CrossFadeAlpha(alpha: 1f, duration: (this.animationDuration / 6f), ignoreTimeScale: true);
				}));
				// Make sure to wait for a few moments. while it stays on screen.
				seq.AppendInterval(interval: this.animationDuration / 3f);
				// Now, fade it out.
				seq.AppendCallback(new TweenCallback(delegate {
					this.NumberRectTransforms[i].DOAnchorPosX(endValue: -200f, duration: (this.animationDuration / 6f), snapping: true);
					this.numberImages[i].CrossFadeAlpha(alpha: 0f, duration: (this.animationDuration / 6f), ignoreTimeScale: true);
				}));
			}*/


			// Turn the PANIC text on.
			seq.AppendCallback(new TweenCallback(delegate {
				this.panicLabel.gameObject.SetActive(true);
				AudioController.instance.PlaySFX(type: SFXType.BoundKnockout);
			}));

			// Wait a second.
			seq.AppendInterval(1f);

			// Turn the whole thing off.
			seq.AppendCallback(new TweenCallback(delegate {
				this.animationGameObject.SetActive(false);
			}));

			seq.Play();


		}

		#endregion

	}


}
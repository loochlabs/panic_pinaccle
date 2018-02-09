using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace PanicPinnacle.UI {

	/// <summary>
	/// Fades the screen to any given color over a period of time. Also can display the loading animation, if requested.
	/// </summary>
	public class SceneFaderCanvasGroup : SerializedMonoBehaviour {

		public static SceneFaderCanvasGroup instance;

		#region FIELDS - SCENE REFERENCES
		/// <summary>
		/// The GameObject that contains the canvas being rendered.
		/// </summary>
		[TabGroup("Scene References","Scene References"), SerializeField]
		private GameObject canvasObject;
		/// <summary>
		/// A reference to the image used to fade in/out of scenes. It's basically just a giant Image stretched over the entire screen.
		/// </summary>
		[TabGroup("Scene References", "Scene References"), SerializeField]
		private Image faderImage;
		/// <summary>
		/// A reference to the text used to alert the players that the scene is loading.
		/// </summary>
		[TabGroup("Scene References", "Scene References"), SerializeField]
		private SuperTextMesh loadingText;
		#endregion

		private void Awake() {
			if (instance == null) {
				instance = this;
				// this.faderImage = GetComponentInChildren<Image>();
				// this.loadingText = GetComponentInChildren<SuperTextMesh>();
			}
		}
		private void Start() {
			// Remember to set the CanvasRenderer color on the image to Clear.
			this.faderImage.canvasRenderer.SetColor(Color.clear);
			// Also disable the loading text by default.
			this.loadingText.gameObject.SetActive(false);
		}
		
		#region FADING
		/// <summary>
		/// Fades the screen out in over a specified period of time.
		/// </summary>
		/// <param name="color">The color to fade to.</param>
		/// <param name="fadeOut">The amount of time needed to fade out the image.</param>
		/// <param name="showLoadingText">Should the loading text be displayed?</param>
		public void FadeOut(Color color, float fadeOut = 0.5f, bool showLoadingText = false) {
			// Remember to set the CanvasRenderer color on the image to Clear.
			this.faderImage.canvasRenderer.SetColor(Color.clear);
			// Create a new sequence.
			Sequence seq = DOTween.Sequence();
			// Fade the image.
			seq.AppendCallback(new TweenCallback(delegate {
				// Turn on the canvas object.
				this.canvasObject.SetActive(true);
				// Crossfade the color.
				this.faderImage.CrossFadeColor(targetColor: color, duration: fadeOut, ignoreTimeScale: true, useAlpha: true);
			}));
			// Wait the specified amount of time.
			seq.AppendInterval(fadeOut);
			// Turn on the loading text, if specified.
			seq.AppendCallback(new TweenCallback(delegate {
				this.loadingText.gameObject.SetActive(showLoadingText);
			}));
			// Play the sequence.
			seq.Play();
		}
		/// <summary>
		/// Fades the screen in over a specified period of time.
		/// </summary>
		/// <param name="fadeIn">The amount of time needed to fade in the image.</param>
		public void FadeIn(float fadeIn = 0.5f) {
			// Create a new sequence.
			Sequence seq = DOTween.Sequence();
			// Fade the image.
			seq.AppendCallback(new TweenCallback(delegate {
				// Disable the loading text. There isn't a context where it will ever be on.
				this.loadingText.gameObject.SetActive(false);
				// Fade out the image. This will always be Color.clear.
				this.faderImage.CrossFadeColor(targetColor: Color.clear, duration: fadeIn, ignoreTimeScale: true, useAlpha: true);
			}));
			// Wait the specified amount of time.
			seq.AppendInterval(fadeIn + 0.1f);
			// Turn on the loading text, if specified.
			seq.AppendCallback(new TweenCallback(delegate {
				// Turn the canvas off.
				this.canvasObject.SetActive(false);
			}));
			// Play the sequence.
			seq.Play();
			
		}
		#endregion

		#region DEBUGGING
		[TabGroup("Debugging", "Debug"), SerializeField]
		private void TestFadeOut() {
			this.FadeOut(color: Color.black, showLoadingText: true);
		}
		[TabGroup("Debugging", "Debug"), SerializeField]
		private void TestFadeIn() {
			this.FadeIn();
		}
		#endregion

	}


}
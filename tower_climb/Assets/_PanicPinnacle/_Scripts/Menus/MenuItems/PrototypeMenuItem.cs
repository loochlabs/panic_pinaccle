using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using PanicPinnacle.Menus;

namespace PanicPinnacle.UI {
	/// <summary>
	/// A very, very, very basic kind of MenuItem. Mostly so I can debug and set up things. 
	/// Preferably shouldn't be used in final builds.
	/// </summary>
	public class PrototypeMenuItem : MenuItem {

		#region FIELDS - MEMORY
		/// <summary>
		/// The text that is used to describe this button.
		/// </summary>
		private string menuItemText = "";
		#endregion

		#region FIELDS - SCENE REFERENCES
		/// <summary>
		/// The image used for the backgroudn of this button.
		/// </summary>
		private Image menuItemBackground;
		/// <summary>
		/// The label describing this button. The visual representation of it, I mean. I'm very tired.
		/// </summary>
		private SuperTextMesh menuItemLabel;
		#endregion

		private void Awake() {
			// Grab references to the two objects that need them.
			this.menuItemBackground = GetComponent<Image>();
			this.menuItemLabel = GetComponentInChildren<SuperTextMesh>();
		}
		private void Start() {
			// Remember the text of the label.
			this.menuItemText = this.menuItemLabel.Text;
		}

		#region MENUITEM IMPLEMENTATION
		public override void OnCancel(BaseEventData eventData) {
			// Execute onCancelEvent
			this.onCancelEvent.Invoke();
		}
		public override void OnDeselect(BaseEventData eventData) {
			// When this button is deselected, make it white again.
			this.menuItemBackground.CrossFadeColor(targetColor: Color.white, duration: 0f, ignoreTimeScale: true, useAlpha: true);
			// Also revert the text.
			this.menuItemLabel.Text = "<c=black>" + this.menuItemText;
		}
		public override void OnSelect(BaseEventData eventData) {
			// When this button is selected, change its color.
			this.menuItemBackground.CrossFadeColor(targetColor: Color.red, duration: 0f, ignoreTimeScale: true, useAlpha: true);
			// Also change the text..
			this.menuItemLabel.Text = "<j=sample><c=white>" + this.menuItemText;
		}
		public override void OnSubmit(BaseEventData eventData) {
			// When this button is submited, make it white again.
			this.menuItemBackground.CrossFadeColor(targetColor: Color.white, duration: 0f, ignoreTimeScale: true, useAlpha: true);
			// Also revert the text.
			this.menuItemLabel.Text = "<c=black>" + this.menuItemText;
			// Execute onSubmitEvent
			this.onSubmitEvent.Invoke();
		}
		public override void OnPointerEnter(PointerEventData eventData) {
			// When this button is selected, change its color.
			this.menuItemBackground.CrossFadeColor(targetColor: Color.red, duration: 0f, ignoreTimeScale: true, useAlpha: true);
			// Also change the text..
			this.menuItemLabel.Text = "<j=sample><c=white>" + this.menuItemText;
		}
		public override void OnPointerExit(PointerEventData eventData) {
			// When this button is deselected, make it white again.
			this.menuItemBackground.CrossFadeColor(targetColor: Color.white, duration: 0f, ignoreTimeScale: true, useAlpha: true);
			// Also revert the text.
			this.menuItemLabel.Text = "<c=black>" + this.menuItemText;
		}
		#endregion
	}


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Combatants;
using UnityEngine.UI;
using PanicPinnacle.Items;
using PanicPinnacle.Matches;

namespace PanicPinnacle.UI {

	/// <summary>
	/// Displays information of the combatant in the round. 
	/// Contains info such as name/obtained powerups/placement/etc.
	/// </summary>
	public class PlayerStatus : MonoBehaviour {

		#region FIELDS - COMBATANT
		/// <summary>
		/// The ID of the combatant that is allowed to use this status.
		/// </summary>
		[SerializeField]
		private int combatantId = -1;
		/// <summary>
		/// The ID of the combatant that is allowed to use this status.
		/// </summary>
		public int CombatantID {
			get {
				return this.combatantId;
			}
		}
		/// <summary>
		/// The combatant that this PlayerStatus belongs to.
		/// </summary>
		private Combatant combatant;
		#endregion

		#region FIELDS - SCENE REFERENCES
		/// <summary>
		/// The GameObject that contains the visuals for the status.
		/// Enabling/Disabling this object will effectively show/hide it from view, which is good if a combatant is not participating.
		/// </summary>
		[SerializeField]
		private GameObject childObjects;
		/// <summary>
		/// The text mesh displaying the name for this combatant.
		/// </summary>
		[SerializeField]
		private SuperTextMesh combatantNameLabel;
		/// <summary>
		/// The text mesh showing where this combatant has placed in the round.
		/// </summary>
		[SerializeField]
		private SuperTextMesh placementLabel;
		/// <summary>
		/// The image that is shown behind the combatant bust up.
		/// </summary>
		[SerializeField]
		private Image backImage;
		/// <summary>
		/// The image displaying the bust up for the combatant.
		/// </summary>
		[SerializeField]
		private Image bustUpImage;
		/// <summary>
		/// The image that is shown in front of the combatant bust up.
		/// </summary>
		[SerializeField]
		private Image frontImage;
		/// <summary>
		/// A list of images displaying the powerups currently in effect on this combatant.
		/// </summary>
		[SerializeField]
		private List<Image> powerUpImages = new List<Image>();
		#endregion

		#region PREPARATION
		/// <summary>
		/// Preps this
		/// </summary>
		/// <param name="combatant"></param>
		public void Prepare(Combatant combatant) {
			// Save a reference to the combatant.
			this.combatant = combatant;
			// Reset the round placement label, in case that was previously set. If it was, it needs to be blank now.
			this.SetRoundPlacementLabel("");
			// Turn on the child objects. This makes sure everything can be visible.
			this.childObjects.SetActive(true);
			// Make sure the images are faded in/white.
			this.FadeImages(color: Color.white);
			// Refresh the status with info.
			this.RefreshStatus();
		}
		#endregion

		#region SHOW/HIDE
		/// <summary>
		/// Refreshes the status with updated information.
		/// </summary>
		public void RefreshStatus() {
			// Look into the match template for this current match and pick out the appropriate color.
			// this.backImage.canvasRenderer.SetColor(color: MatchController.instance.CurrentMatchSettings.MatchTemplate.PlayerColors[this.CombatantID]);

			//Debug.LogWarning("Set the bust up image.");
			// this.bustUpImage.sprite = ???;
			this.bustUpImage.color = MatchController.instance.CurrentMatchSettings.MatchTemplate.PlayerColors[this.CombatantID];


			// Set the combatant's name to "Player" and whatever id they have.
			this.combatantNameLabel.Text = "Player " + combatant.CombatantID.ToString();

			// Set the round placement string if they are out.
			if (this.combatant.State == CombatantStateType.dead) {
				Debug.LogWarning("Set the placement of the combatant.");
				// this.SetRoundPlacementLabel(combatant.SomeValueThatHasTheirPlacement);
			}

			// Set the images that correspond to the different powerups.
			this.SetPowerUps(powerUps: this.combatant.Items);

		}
		/// <summary>
		/// Hides the status for when it's not needed anymore.
		/// </summary>
		public void HideStatus() {
			// Null out the combatant, just for type safety purposes.
			this.combatant = null;
			// Turn off the child objects.
			this.childObjects.SetActive(false);
		}
		#endregion

		#region MORE INTRICATE EFFECTS
		/// <summary>
		/// Sets the string on the round placement label.
		/// </summary>
		/// <param name="placement">The placement of the combatant.</param>
		private void SetRoundPlacementLabel(int placement) {
			switch (placement) {
				case 1:
					this.SetRoundPlacementLabel("1st");
					break;
				case 2:
					this.SetRoundPlacementLabel("2nd");
					break;
				case 3:
					this.SetRoundPlacementLabel("3rd");
					break;
				case 4:
					this.SetRoundPlacementLabel("4th");
					break;
				default:
					Debug.LogError("Placement number is not a valid round placement number!");
					break;
			}
		}
		/// <summary>
		/// Sets the string on the round placement label.
		/// </summary>
		/// <param name="str">The exact string to set.</param>
		private void SetRoundPlacementLabel(string str) {
			this.placementLabel.Text = "<j=sample>" + str;
		}
		/// <summary>
		/// Fades the the bustup/front/back images to a specified color.
		/// Helpful for handling the effect when the combatant is knocked out.
		/// </summary>
		private void FadeImages(Color color) {
			// Go through all the available images and call CrossFadeColor.
			this.bustUpImage.CrossFadeColor(targetColor: color, duration: 0.5f, ignoreTimeScale: true, useAlpha: true);
			this.backImage.CrossFadeColor(targetColor: color, duration: 0.5f, ignoreTimeScale: true, useAlpha: true);
			this.frontImage.CrossFadeColor(targetColor: color, duration: 0.5f, ignoreTimeScale: true, useAlpha: true);
			this.powerUpImages.ForEach(i => i.CrossFadeColor(targetColor: color, duration: 0.5f, ignoreTimeScale: true, useAlpha: true));
		}
		/// <summary>
		/// Sets the power up images. Turns off any images not in use.
		/// </summary>
		/// <param name="powerUps"></param>
		private void SetPowerUps(List<Item> powerUps) {
			// Initialize an index value.
			int i = 0;
			// Iterate through the available power ups, and set the sprites that are provided to the images.
			for (i = 0; i < powerUps.Count; i++) {
				this.powerUpImages[i].color = Color.white;
				this.powerUpImages[i].sprite = powerUps[i].ItemTemplate.ItemSprite;
			}
			// There may potentailly be unused power up images. In that case, hide them.
			for (i = powerUps.Count; i < this.powerUpImages.Count; i++) {
				this.powerUpImages[i].color = Color.clear;
			}
		}
		#endregion

	}


}
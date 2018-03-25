using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using PanicPinnacle.Items.Effects;

namespace PanicPinnacle.Items {

	/// <summary>
	/// The template that gets used for defining how an item should look and behave.
	/// </summary>
	[CreateAssetMenu(fileName = "New ItemTemplate", menuName = "Panic Pinnacle/Item Template")]
	public class ItemTemplate : SerializedScriptableObject {

		#region FIELDS - METADATA
		/// <summary>
		/// The name of the item that will be created from this template.
		/// </summary>
		[TabGroup("Metadata", "Metadata"), SerializeField]
		private string itemName = "";
		/// <summary>
		/// The name of the item that will be created from this template.
		/// </summary>
		public string ItemName {
			get {
				return this.itemName;
			}
		}
		/// <summary>
		/// The sprite used to represent this power up in the game world.
		/// </summary>
		[TabGroup("Metadata", "Visuals"), SerializeField]
		private Sprite itemSprite;
		/// <summary>
		/// The sprite used to represent this power up in the game world.
		/// </summary>
		public Sprite ItemSprite {
			get {
				return this.itemSprite;
			}
		}
		/// <summary>
		/// The icon used to show this item in a combatant's status. May be the same as the item's world sprite for debugging purposes.
		/// </summary>
		[TabGroup("Metadata", "Visuals"), SerializeField]
		private Sprite itemIcon;
		/// <summary>
		/// The icon used to show this item in a combatant's status. May be the same as the item's world sprite for debugging purposes.
		/// </summary>
		public Sprite ItemIcon {
			get {
				return this.itemIcon;
			}
		}
		/// <summary>
		/// The audio clip that should be played when this pickup is grabbed.
		/// </summary>
		[TabGroup("Metadata", "Audio"), SerializeField]
		private AudioClip itemSfx;
		/// <summary>
		/// The audio clip that should be played when this pickup is grabbed.
		/// </summary>
		public AudioClip ItemSFX {
			get {
				return this.itemSfx;
			}
		}
		#endregion

		#region FIELDS - EFFECTS
		/// <summary>
		/// The effects that make up the behavior of this item.
		/// </summary>
		[TabGroup("Effects", "Effects"), SerializeField]
		private List<ItemEffect> effects = new List<ItemEffect>();
		/// <summary>
		/// The effects that make up the behavior of this item.
		/// </summary>
		public List<ItemEffect> Effects {
			get {
				// Create a new list to hold the effects.
				List<ItemEffect> effectClones = new List<ItemEffect>();
				// Go through each effect and make a clone so that the instances in the template are not modified.
				foreach (ItemEffect effect in this.effects) {
					effectClones.Add(ItemEffect.Clone(effect));
				}
				// Return the list. The effects in this list can be freely modified without affecting the template.
				return effectClones;
			}
		}
		#endregion

	}


}
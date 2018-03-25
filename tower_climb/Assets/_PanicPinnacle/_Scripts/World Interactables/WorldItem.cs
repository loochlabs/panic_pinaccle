using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Combatants;
using PanicPinnacle.Events;
using PanicPinnacle.Matches;

namespace PanicPinnacle.Items {

	/// <summary>
	/// The representation of an item in the game world. The item it contains will be added to a combatant upon contact.
	/// </summary>
	[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(BoxCollider2D))]
	public class WorldItem : MonoBehaviour {

		#region FIELDS
		/// <summary>
		/// The template that will be used to create the item when a combatant collides with this world item.
		/// </summary>
		[SerializeField]
		private ItemTemplate itemTemplate;
		#endregion

		private void Start() {
			// Upon start, find the sprite renderer and give it the sprite that's contained within the template.
			this.GetComponent<SpriteRenderer>().sprite = this.itemTemplate.ItemSprite;
			// Can maybe add some particle effects here later if you're feeling fancy.
		}

		#region INTERFACE IMPLEMENTATION - IONTRIGGERENTER2DEVENT
		public void OnTriggerEnter2D(Collider2D collision) {
			if (collision.GetComponent<Combatant>() != null) {
				// When the combatant collides with this box collider, create a new item and add it to their inventory.
				collision.GetComponent<Combatant>().AddItem(item: new Item(itemTemplate: this.itemTemplate));
				// Destroy this WorldItem, because it's not needed any more.
				Destroy(this.gameObject);
			}
		}
		
		#endregion

	}


}
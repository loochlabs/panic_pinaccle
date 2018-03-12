using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Combatants;
using PanicPinnacle.Items.Effects;
using System.Linq;
using PanicPinnacle.Events;

namespace PanicPinnacle.Items {

	/// <summary>
	/// Manages the functionality of items that the combatants can pick up.
	/// </summary>
	public class Item {

		#region FIELDS
		/// <summary>
		/// The template that stores un-modifiable data about this item.
		/// </summary>
		private ItemTemplate itemTemplate;
		/// <summary>
		/// The template that stores un-modifiable data about this item.
		/// </summary>
		public ItemTemplate ItemTemplate {
			get {
				return this.itemTemplate;
			}
		}
		/// <summary>
		/// A list of effects that determine the behavior of this item.
		/// </summary>
		private List<ItemEffect> effects = new List<ItemEffect>();
		#endregion

		/// <summary>
		/// Create a new item from the template specified.
		/// </summary>
		/// <param name="itemTemplate">The template that defines how this item should behave.</param>
		public Item(ItemTemplate itemTemplate) {
			// Save a reference to the template.
			this.itemTemplate = itemTemplate;
			// Grab the effects from the template.
			// The list provided by the template consists of clones of the effects, so the template will not be affected at runtime.
			this.effects = itemTemplate.Effects;
		}

		#region EVENTS
		/// <summary>
		/// Invokes the specified event on any ItemEffects that implement it.
		/// </summary>
		/// <typeparam name="T">The event to invoke.</typeparam>
		/// <param name="eventParams">The parameters that define the context in which this event happened.</param>
		public void CallEvent<T>(CombatantEventParams eventParams) {
			// Go through each effect.
			foreach (ItemEffect effect in this.effects) {
				// If the effect implements the given type, invoke it.
				if (effect is T) {

					// nvm disregard the text below lol
					System.Reflection.MethodInfo method = typeof(T).GetMethods()[0];
					method.Invoke(obj: effect, parameters: new object[] { eventParams });

					/*// Okay this one's pretty batshit insane but its very versitile so bear with me.
					// Use Reflection to find the first method of the type (in this case, the interface being used as an event) that accepts ItemEventParams.
					System.Reflection.MethodInfo method = typeof(T).GetMethods().First(m => m.GetParameters()[0].ParameterType == typeof(CombatantEventParams));
					try {
						// Invoke it with the data defining the context in which it was called.
						method.Invoke(obj: effect, parameters: new object[] { eventParams });
					} catch (System.Exception e) {
						Debug.LogWarning("Couldn't invoke event on item. Reason: " + e + "\n(Maybe this particular interface doesn't have a method that takes an ItemEventParams?\nConsider adding a method with that signature to the interface or remove this effect from the item's template entirely.)");
					}*/
				}
			}
		}
		#endregion

	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Items.Effects {

	/// <summary>
	/// A modular piece of code that can be appended to an item in order to create a variety of effects.
	/// </summary>
	public abstract class ItemEffect {

		/// <summary>
		/// A function that just helps clone the effect so it can be changed outside of the ScriptableObject template it initially comes from.
		/// </summary>
		/// <param name="effect">The effect to be cloned.</param>
		/// <returns>A clone of the effect.</returns>
		public static ItemEffect Clone(ItemEffect effect) {
			return (ItemEffect)effect.MemberwiseClone();
		}

	}
}
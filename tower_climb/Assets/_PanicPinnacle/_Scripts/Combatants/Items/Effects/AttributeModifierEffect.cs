using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Combatants;
using PanicPinnacle.Items.Effects;
using System.Linq;
using PanicPinnacle.Events;

namespace PanicPinnacle.Items.Effects {

	/// <summary>
	/// Modifies the attributes on a combatant as long as it remains in their posession.
	/// </summary>
	public class AttributeModifierEffect : ItemEffect, IOnItemAddEvent, IOnItemRemoveEvent {


		#region FIELDS - CONFIG
		/// <summary>
		/// How long should this item last until it gets removed?
		/// </summary>
		[SerializeField]
		private float timeUntilRemove = 5f;
		/// <summary>
		/// How much should be added to the gravity modifier when this item is picked up?
		/// </summary>
		[SerializeField]
		private float gravityModifier = 1f;
		#endregion

		#region INTERFACE IMPLEMENTATION - IONITEMADDEVENT
		public void OnItemAdd(CombatantEventParams eventParams) {
			Debug.Log("Attribute item added.");
			Combatant combatant = eventParams.combatant;
			// Add the speed/gravity modifications to the combatant.
			combatant.CombatantBody.AddGravityModifier(modifier: this.gravityModifier);

			// Wait a few seconds before removing the item.
			combatant.StartCoroutine(this.Wait(eventParams));
		
		}
		#endregion

		private IEnumerator Wait(CombatantEventParams eventParams) {
			yield return new WaitForSeconds(this.timeUntilRemove);
			eventParams.combatant.RemoveItem(item: eventParams.item);
		}

		#region INTERFACE IMPLEMENTATION - IONITEMREMOVEEVENT
		public void OnItemRemove(CombatantEventParams eventParams) {
			Debug.Log("Attribute item removed.");
			Combatant combatant = eventParams.combatant;
			combatant.CombatantBody.AddGravityModifier(modifier: -this.gravityModifier);
		}
		#endregion

	}


}
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


		#region INTERFACE IMPLEMENTATION - IONITEMADDEVENT
		public void OnItemAdd(CombatantEventParams eventParams) {
			Debug.Log("Just using this as a way to confirm that an item has been added.");
		}
		#endregion

		#region INTERFACE IMPLEMENTATION - IONITEMREMOVEEVENT
		public void OnItemRemove(CombatantEventParams eventParams) {
			Debug.Log("Just using this as a way to confirm that an item has been removed.");
		}
		#endregion

	}


}
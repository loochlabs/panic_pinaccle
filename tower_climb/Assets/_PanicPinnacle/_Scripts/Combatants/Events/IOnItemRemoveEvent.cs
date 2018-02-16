using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Events {

	/// <summary>
	/// Gets called when an item is removed from the combatant's inventory.
	/// </summary>
	public interface IOnItemRemoveEvent {

		/// <summary>
		/// Gets called when an item is removed from the combatant's inventory.
		/// </summary>
		/// <param name="eventParams">The parameters that define how this event was called.</param>
		void OnItemRemove(CombatantEventParams eventParams);
	
	}


}
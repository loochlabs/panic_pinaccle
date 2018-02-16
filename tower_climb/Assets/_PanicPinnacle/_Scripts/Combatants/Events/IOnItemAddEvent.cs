using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Events {

	/// <summary>
	/// Gets called when this item is added to the combatant's inventory
	/// </summary>
	public interface IOnItemAddEvent {

		/// <summary>
		///  Gets called when this item is added to the combatant's inventory.
		/// </summary>
		/// <param name="eventParams">The parameters that define how this event was called.</param>
		void OnItemAdd(CombatantEventParams eventParams);
		
	}


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Events {

	/// <summary>
	/// Defines a behavior that should be called when a collision exits the combatant.
	/// </summary>
	public interface IOnTriggerExit2DEvent {

		/// <summary>
		/// Gets called when a collider exits this combatant.
		/// </summary>
		/// <param name="eventParams">The parameters that define how this event was called.</param>
		void OnTriggerExit2D(CombatantEventParams eventParams);
	}

}
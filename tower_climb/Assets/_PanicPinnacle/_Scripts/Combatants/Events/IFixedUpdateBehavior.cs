using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Events {

	/// <summary>
	/// Defines a behavior that should be called on the combatant's FixedUpdate.
	/// </summary>
	public interface IFixedUpdateEvent {

		/// <summary>
		/// Gets called every FixedUpdate from the combatant.
		/// </summary>
		/// <param name="eventParams">The parameters that define how this event was called.</param>
		void FixedUpdate(CombatantEventParams eventParams);

	}
}
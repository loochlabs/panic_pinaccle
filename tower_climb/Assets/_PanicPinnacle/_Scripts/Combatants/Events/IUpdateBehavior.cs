using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Events {

	/// <summary>
	/// Defines a behavior that should be called every Update on the combatant.
	/// </summary>
	public interface IUpdateEvent {

		/// <summary>
		/// Gets called every Update on the combatant.
		/// </summary>
		/// <param name="eventParams">The parameters defining the context in which this event was called.</param>
		void Update(CombatantEventParams eventParams);
	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Combatants;

namespace PanicPinnacle.Events {

	/// <summary>
	/// If a class needs to be alerted when the combatant is punched, it should implement this.
	/// </summary>
	public interface IPunchedEvent {

		/// <summary>
		/// Handles the situation where a combatant has been punched.
		/// </summary>
		/// <param name="eventParams">The parameters defining the context in which this event was called.</param>
		void OnPunched(CombatantEventParams eventParams);

	}
}


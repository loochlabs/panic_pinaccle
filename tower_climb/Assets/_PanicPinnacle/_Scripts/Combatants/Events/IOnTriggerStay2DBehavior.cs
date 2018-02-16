using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Events{

	/// <summary>
	/// Defines a behavior that should be called when a collision stays inside the combatant.
	/// </summary>
	public interface IOnTriggerStay2DEvent {

		/// <summary>
		/// Gets called when a collider stays inside this combatant.
		/// </summary>
		/// <param name="eventParams">The parameters that define how this event was called.</param>
		void OnTriggerStay2D(CombatantEventParams eventParams);
	}

}
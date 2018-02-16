using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Events {

	/// <summary>
	/// Defines a behavior that should be called when a collision enters the combatant.
	/// </summary>
	public interface IOnTriggerEnter2DEvent {

		/// <summary>
		/// Gets called when a collider enters this combatant.
		/// </summary>
		/// <param name="eventParams">The parameters that define how this event was called.</param>
		void OnTriggerEnter2D(CombatantEventParams eventParams);

	}
}
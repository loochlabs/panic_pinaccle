using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanicPinnacle.Combatants;
using PanicPinnacle.Items;

namespace PanicPinnacle.Events {

	/// <summary>
	/// A collection of data that describes the context in which an event occured.
	/// </summary>
	public class CombatantEventParams {

		#region FIELDS
		/// <summary>
		/// The combatant that the event is being invoked on.
		/// </summary>
		public Combatant combatant;
		/// <summary>
		/// The item that invoked the event.
		/// May be null.
		/// </summary>
		public Item item;
		/// <summary>
		/// The collision that invoked this event.
		/// May be null.
		/// </summary>
		public Collider2D collision;
		#endregion

		/// <summary>
		/// A collection of data that describes the context in which an event occured.
		/// </summary>
		/// <param name="combatant">The combatant that the event has been invoked on.</param>
		/// <param name="item">The item that invoked this event.</param>
		/// <param name="collision">The collision that invoked this event..</param>
		public CombatantEventParams(Combatant combatant, Item item, Collider2D collision) {
			this.combatant = combatant;
			this.item = item;
			this.collision = collision;
		}

	}


}
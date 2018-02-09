using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle {

	/// <summary>
	/// A class that provides an interface for explicitly calling garbage collection.
	/// Preferable over calling System.GC.Collect in the event that CollectGarbage() needs modifications of some kind.
	/// </summary>
	public static class GarbageCollectorHelper {

		/// <summary>
		/// Call the garbage collector.
		/// </summary>
		public static void CollectGarbage() {
			System.GC.Collect();
		}

	}

}
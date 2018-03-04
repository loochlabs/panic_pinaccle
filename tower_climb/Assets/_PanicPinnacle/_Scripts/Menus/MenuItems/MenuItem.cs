using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace PanicPinnacle.UI {

	/// <summary>
	/// The way in which items in menus should be implemented.
	/// </summary>
	[RequireComponent(typeof(Selectable))]
	public abstract class MenuItem : SerializedMonoBehaviour, ICancelHandler, ISelectHandler, ISubmitHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

		#region FIELDS - UNITY EVENTS
		/// <summary>
		/// The event to run when this item handles OnSubmit.
		/// </summary>
		[TabGroup("Events", "Events"), SerializeField]
		protected UnityEvent onSubmitEvent;
		/// <summary>
		/// The event to run when this item handles OnCancel.
		/// </summary>
		[TabGroup("Events", "Events"), SerializeField]
		protected UnityEvent onCancelEvent;
		#endregion

		#region INTERFACE IMPLEMENTATION - THE EVENTSYSTEM JUNK
		public abstract void OnCancel(BaseEventData eventData);
		public abstract void OnDeselect(BaseEventData eventData);
		public abstract void OnPointerEnter(PointerEventData eventData);
		public abstract void OnPointerExit(PointerEventData eventData);
		public abstract void OnSelect(BaseEventData eventData);
		public abstract void OnSubmit(BaseEventData eventData);

		public virtual void OnPointerClick(PointerEventData eventData) {
			// may or may not get rid of this its a long story
		}
		#endregion

	}
}
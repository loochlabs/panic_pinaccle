using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

namespace PanicPinnacle.UI {

	/// <summary>
	/// The way in which items in menus should be implemented.
	/// </summary>
	[RequireComponent(typeof(Selectable))]
	public abstract class MenuItem : SerializedMonoBehaviour, ICancelHandler, ISelectHandler, ISubmitHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler {

		#region INTERFACE IMPLEMENTATION - THE EVENTSYSTEM JUNK
		public abstract void OnCancel(BaseEventData eventData);
		public abstract void OnDeselect(BaseEventData eventData);
		public abstract void OnPointerEnter(PointerEventData eventData);
		public abstract void OnPointerExit(PointerEventData eventData);
		public abstract void OnSelect(BaseEventData eventData);
		public abstract void OnSubmit(BaseEventData eventData);
		#endregion

	}
}
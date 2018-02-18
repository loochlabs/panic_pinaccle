using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Legacy {
	public class CameraControls : MonoBehaviour {

		public static CameraControls instance;

		public Transform focusTransform;
		public Vector3 offset = new Vector3(0, 0, -10f);

		private void Awake() {
			instance = this;
		}

		void Update() {
			transform.position = focusTransform.position + offset;
		}
	}

}
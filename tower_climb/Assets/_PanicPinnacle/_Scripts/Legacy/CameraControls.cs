using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PanicPinnacle.Legacy {
	public class CameraControls : MonoBehaviour {

		public Transform boundsCenter;
		public Vector3 offset = new Vector3(0, 0, -10f);

		void Update() {
			transform.position = boundsCenter.position + offset;
		}
	}

}
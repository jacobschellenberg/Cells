using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform Target;
	public float Distance = -10;

	void LateUpdate() {
		if (Target != null) {
			Vector3 newPosition = Target.transform.position;
			newPosition.z = Distance;
			this.transform.position = newPosition;
		}
	}

	public void SetTarget(Transform target) {
		Target = target;
	}

	public void SetDistance(float distance) {
		Distance = distance;
	}
}

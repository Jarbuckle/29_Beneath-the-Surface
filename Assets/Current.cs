using UnityEngine;
using System.Collections;

public class Current : MonoBehaviour {

	void FixedUpdate () {
		Vector3 thisPos = transform.position;
		Vector3 thatPos = new Vector3(0, 0, 0);
		float dist = Mathf.Abs(Vector3.Distance(thisPos, thatPos));
		float strength = 0.0f;
		if (dist > 0) {
			strength = 0.5f / dist;
		}
		rigidbody.AddForce((thatPos - thisPos).normalized * strength);
	}
}

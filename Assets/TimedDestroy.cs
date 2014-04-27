using UnityEngine;
using System.Collections;

public class TimedDestroy : MonoBehaviour {

	public float duration;

	private float startTime;

	void Start() {
		startTime = Time.time;
	}

	void Update () {
		if ((Time.time - startTime) >= duration) {
			Destroy(gameObject);
		}
	}
}

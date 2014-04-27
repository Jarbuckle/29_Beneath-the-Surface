using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public Transform pivot;
	public Transform emitter;
	public Transform interfacePlane;
	public GameObject fireballPrefab;

	private GameObject fireball = null;
	private bool charging = false;
	private float chargingStarted = 0.0f;

	void Start () {
		if (fireballPrefab == null) {
			throw new UnassignedReferenceException("fireballPrefab");
		}
		if (fireballPrefab.GetComponent<ChargeUp>() == null) {
			throw new MissingComponentException("fireballPrefab must have a ChargeUp component");
		}
	}

	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		int layerMask = 1 << LayerMask.NameToLayer("InterfacePlane");
		bool hit = Physics.Raycast(ray, out hitInfo, 100.0f, layerMask);
		if (hit) {
			pivot.transform.LookAt(hitInfo.point);
		}

		if (Input.GetMouseButtonDown(0)) {
			fireball = Instantiate(fireballPrefab) as GameObject;
			fireball.transform.parent = emitter;
			fireball.transform.localPosition = Vector3.zero;
			charging = true;
			chargingStarted = Time.time;
		}

		if (charging) {
			ChargeUp fireballCharge = fireball.GetComponent<ChargeUp>();
			if (Input.GetMouseButtonUp(0)) {
				Vector3 pivotPos = pivot.position;
				Vector3 emitterPos = emitter.position;
				Vector3 direction = (emitterPos - pivotPos).normalized;
				fireball.transform.parent = null;
				fireballCharge.direction = direction;
				fireball.rigidbody.AddForce(direction * 20.0f, ForceMode.VelocityChange);
				charging = false;
			} else {
				float diffTime = Time.time - chargingStarted;
				float chargeLevel = Mathf.Min(diffTime / 3.0f, 1.0f);
				fireballCharge.chargeLevel = chargeLevel;
			}
		}
	}
}

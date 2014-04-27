using UnityEngine;
using System.Collections.Generic;

public class Magnet : MonoBehaviour {

	public int magnetId;

	private List<Magnet> closeMagnets;

	private Magnet attachedMagnet;
	private FixedJoint attachmentJoint;

	private Material normalMaterial;
	public Material glowMaterial;

	void Start () {
		closeMagnets = new List<Magnet>();
		normalMaterial = renderer.material;
	}

	public string FullIdentifier {
		get {
			return "" + transform.parent.gameObject.GetComponent<GamePiece>().gamePieceId + ":" + magnetId;
		}
	}

	void OnTriggerEnter(Collider other) {
		Magnet otherMagnet = null;
		otherMagnet = other.GetComponent<Magnet>();
		if ((otherMagnet != null) && (!closeMagnets.Contains(otherMagnet))) {
			closeMagnets.Add(otherMagnet);
		}
	}

	void OnTriggerExit(Collider other) {
		Magnet otherMagnet = null;
		otherMagnet = other.GetComponent<Magnet>();
		closeMagnets.Remove(otherMagnet);
	}

	void Update() {
		if (attachedMagnet == null) {
			Vector3 thisPos = transform.position;
			foreach (Magnet magnet in closeMagnets) {
				Vector3 thatPos = magnet.transform.position;

				Vector3 thisPos1 = transform.TransformPoint(transform.localPosition + (transform.localRotation * new Vector3(0f, 0f, 0.2f)));
				Vector3 thisPos2 = transform.TransformPoint(transform.localPosition + (transform.localRotation * new Vector3(0f, 0f, -0.2f)));

				Vector3 thatPos1 = magnet.transform.TransformPoint(magnet.transform.localPosition + (magnet.transform.localRotation * new Vector3(0f, 0f, 0.2f)));
				Vector3 thatPos2 = magnet.transform.TransformPoint(magnet.transform.localPosition + (magnet.transform.localRotation * new Vector3(0f, 0f, -0.2f)));

				float dist1 = Vector3.Distance(thisPos1, thatPos2);
				float dist2 = Vector3.Distance(thisPos2, thatPos1);
				bool pos1Close = dist1 < 0.3f;
				bool pos2Close = dist2 < 0.3f;
								
				Debug.DrawLine(thisPos1, thatPos1, Color.green * (1f/dist1));
				Debug.DrawLine(thisPos2, thatPos2, Color.red * (1f/dist2));

				if (pos1Close && pos2Close) {
					Debug.Log("Attaching magnet " + FullIdentifier + " to magnet " + magnet.FullIdentifier);
					attachedMagnet = magnet;
					attachmentJoint = collider.attachedRigidbody.gameObject.AddComponent<FixedJoint>();
					attachmentJoint.anchor = transform.localPosition;
					attachmentJoint.connectedBody = magnet.collider.attachedRigidbody;
					attachmentJoint.breakForce = 10f;
					attachmentJoint.breakTorque = 10f;

					renderer.material = glowMaterial;
					break;
				}
			}
		} else if (attachmentJoint == null) {
			Debug.Log("Magnet " + FullIdentifier + " no longer attached to magnet " + attachedMagnet.FullIdentifier);
			attachedMagnet = null;
			renderer.material = normalMaterial;
		} 
	}

	void FixedUpdate () {
		if (!attachedMagnet) {
			Vector3 thisPos = transform.position;
			foreach (Magnet magnet in closeMagnets) {
				Vector3 thatPos = magnet.transform.position;
				float dist = Vector3.Distance(thisPos, thatPos);
				if (dist >= 0.01f) {
					float forceStrength = 1f / (dist);
					magnet.collider.attachedRigidbody.AddForce((thisPos - thatPos).normalized * forceStrength);
				}
			}
		}
	}
}

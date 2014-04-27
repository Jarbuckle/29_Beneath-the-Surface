using UnityEngine;
using System.Collections;

public class ChargeUp : MonoBehaviour {

	public float chargeLevel = 0;
	public ParticleSystem fire;
	public ParticleSystem sparks;
	public Light glow;

	public GameObject explosionPrefab;

	public float minMass = 0.1f;
	public float maxMass = 50.0f;

	public bool freeFlying = false;
	public Vector3 direction;
	
	public float fireStartLifetimeModifier;
	public float fireStartSpeedModifier;
	public float fireStartSizeModifier;
	public float fireEmissionRateModifier;
	
	private float fireStartLifetimeDefault;
	private float fireStartSpeedDefault;
	private float fireStartSizeDefault;
	private float fireEmissionRateDefault;

	public float sparksStartLifetimeModifier;
	public float sparksStartSpeedModifier;
	public float sparksStartSizeModifier;
	public float sparksEmissionRateModifier;
	
	private float sparksStartLifetimeDefault;
	private float sparksStartSpeedDefault;
	private float sparksStartSizeDefault;
	private float sparksEmissionRateDefault;
	
	public float glowIntensityModifier;
	
	private float glowIntensityDefault;
	
	public int numAllowedRebounds = 2;

	private int numRebounds = 0;

	void Start() {
		fireStartLifetimeDefault = fire.startLifetime;
		fireStartSpeedDefault = fire.startSpeed;
		fireStartSizeDefault = fire.startSize;
		fireEmissionRateDefault = fire.emissionRate;
		
		sparksStartLifetimeDefault = sparks.startLifetime;
		sparksStartSpeedDefault = sparks.startSpeed;
		sparksStartSizeDefault = sparks.startSize;
		sparksEmissionRateDefault = sparks.emissionRate;

		glowIntensityDefault = glow.intensity;
	}

	void Update () {
		if (chargeLevel > 0) {
			fire.startLifetime = fireStartLifetimeDefault + (chargeLevel * fireStartLifetimeModifier);
			fire.startSpeed = fireStartSpeedDefault + (chargeLevel * fireStartSpeedModifier);
			fire.startSize = fireStartSizeDefault + (chargeLevel * fireStartSizeDefault);
			fire.emissionRate = fireEmissionRateDefault + (chargeLevel * fireEmissionRateModifier);

			sparks.startLifetime = sparksStartLifetimeDefault + (chargeLevel * sparksStartLifetimeModifier);
			sparks.startSpeed = sparksStartSpeedDefault + (chargeLevel * sparksStartSpeedModifier);
			sparks.startSize = sparksStartSizeDefault + (chargeLevel * sparksStartSizeModifier);
			sparks.emissionRate = sparksEmissionRateDefault + (chargeLevel * sparksEmissionRateModifier);

			glow.intensity = glowIntensityDefault + (chargeLevel * glowIntensityModifier);

			rigidbody.mass = Mathf.Lerp(minMass, maxMass, chargeLevel * chargeLevel);
		}
	}

	void OnCollisionEnter(Collision collision) {
		Vector3 collisionPosition = collision.contacts[0].point;
		if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Shapes")) {
			Instantiate(explosionPrefab, collisionPosition, Quaternion.identity);
			Destroy(gameObject);

		} else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Walls")) {
			numRebounds += 1;
			if (numRebounds > numAllowedRebounds) {
				Instantiate(explosionPrefab, collisionPosition, Quaternion.identity);
				Destroy(gameObject);
			}
			Vector3 collisionNormal = collision.contacts[0].normal;
			Vector3 direction = (2.0f * Vector3.Dot(direction, collisionNormal) * collisionNormal - direction).normalized;
			rigidbody.AddForce(direction * 20.0f, ForceMode.VelocityChange);
		}
	}
}

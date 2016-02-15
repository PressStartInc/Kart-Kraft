using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Axles {
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool motor;		//Is this wheel attached to a motor?
	public bool steering;	//Does this wheel apply steering?
}

public class KartController3 : MonoBehaviour {
	private Rigidbody rb;

	public List<Axles> axleInfos;
	public float maxMotoTorque;
	public float maxSteerAnfle;

	void Start () {
		rb = this.GetComponent<Rigidbody> ();
		rb.centerOfMass = new Vector3 (0.0f, -0.5f, 0.3f);	
	}

	void FixedUpdate () {
		float motor = maxMotoTorque * Input.GetAxis ("Vertical");
		float steer = maxSteerAnfle * Input.GetAxis ("Horizontal");
		float brake = Input.GetKey ("space") ? 500.0f : 0.0f;

		foreach (Axles axleInfo in axleInfos) {
			if (axleInfo.steering) {
				axleInfo.leftWheel.steerAngle = steer;
				axleInfo.rightWheel.steerAngle = steer;
			}
			if (axleInfo.motor) {
				axleInfo.leftWheel.motorTorque = motor;
				axleInfo.rightWheel.motorTorque = motor;
				axleInfo.leftWheel.brakeTorque = brake;
				axleInfo.rightWheel.brakeTorque = brake;
			}
		}
	}
}

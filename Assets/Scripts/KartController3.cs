using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Axles {
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public Transform lw;
	public Transform rw;
	public bool motor;		//Is this wheel attached to a motor?
	public bool steering;	//Does this wheel apply steering?
}

public class KartController3 : MonoBehaviour {
	private Rigidbody rb;
	
	public string player;
	public List<Axles> axleInfos;
	public float maxSpeed;
	public float maxMotoTorque;
	public float maxSteerAngle;

	void Start () {
		rb = this.GetComponent<Rigidbody> ();
		rb.centerOfMass = new Vector3 (0.0f, -0.5f, 0.3f);	
	}

	//Make the wheels spin
	void Update() {
		if (rb.velocity.magnitude > 0) {
			foreach (Axles axleInfo in axleInfos) {
				axleInfo.lw.Rotate (Vector3.down * Time.deltaTime* rb.velocity.magnitude);
				axleInfo.rw.Rotate (Vector3.down * Time.deltaTime* rb.velocity.magnitude);
			}
		}
		else if (rb.velocity.magnitude < 0) {
			foreach (Axles axleInfo in axleInfos) {
				axleInfo.lw.Rotate (Vector3.up * Time.deltaTime* rb.velocity.magnitude);
				axleInfo.rw.Rotate (Vector3.up * Time.deltaTime* rb.velocity.magnitude);
			}
		}
	}
	// The physics stuff
	void FixedUpdate () {
		float motor = 0;
		float speed = rb.velocity.sqrMagnitude;
		if (Input.GetKey ("joystick "+player+" button 1")) //Press x to accel
			motor = maxMotoTorque; 
		if (Input.GetKey ("joystick "+player+" button 2")) //Press o to reverse
			motor = -maxMotoTorque; 
		float steer = maxSteerAngle * Input.GetAxis ("p"+player+"Steer");
		float brake = Input.GetKey ("joystick "+player+" button 0") ? 1000.0f : 0.0f; // press sq to brake

		Debug.Log ("Motor:"+motor+" | Speed:"+speed+" | Steer:"+steer);

		foreach (Axles axleInfo in axleInfos) {
			if (axleInfo.steering) {
				axleInfo.leftWheel.steerAngle = steer;
				axleInfo.rightWheel.steerAngle = steer;
			}
			if (axleInfo.motor) {
				if (speed < maxSpeed) {
					axleInfo.leftWheel.motorTorque = motor;
					axleInfo.rightWheel.motorTorque = motor;
				} else {
					axleInfo.leftWheel.motorTorque = 0;
					axleInfo.rightWheel.motorTorque = 0;
				}
				axleInfo.leftWheel.brakeTorque = brake;
				axleInfo.rightWheel.brakeTorque = brake;
			}
		}

	}
}	
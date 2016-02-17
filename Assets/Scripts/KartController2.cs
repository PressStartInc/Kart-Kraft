using UnityEngine;
using System.Collections;

public class KartController2 : MonoBehaviour {
	private Rigidbody rb;
	private Transform[] wheels = new Transform[4];

	private float enginePower = 1500.0f;
	private float power = 0.0f;
	private float brake = 0.0f;
	private float steer = 0.0f;
	private float maxSteer = 25.0f;
	private float maxmotoTorque;

	void Start () {
		rb = this.GetComponent<Rigidbody> ();
		rb.centerOfMass = new Vector3 (0.0f, -0.5f, 0.3f);

		wheels [0] = GameObject.Find ("FrontRight").transform;
		wheels [1] = GameObject.Find ("FrontLeft").transform;
		wheels [2] = GameObject.Find ("RearRight").transform;
		wheels [3] = GameObject.Find ("RearLeft").transform;
	}

	void FixedUpdate () {
		power = Input.GetAxis ("Vertical") * enginePower * Time.deltaTime * 2500.0f;
		steer = Input.GetAxis ("Horizontal") * maxSteer;
		brake = Input.GetKey ("space") ? rb.mass * 500.0f : 0.0f;

		Debug.Log ("Power:" + power +" | Steer:" + steer + " | Brake:" + brake);

		GetCollider (0).steerAngle = steer;
		GetCollider (1).steerAngle = steer;

		if (brake > 0.0f) {
			GetCollider (0).brakeTorque = brake;
			GetCollider (1).brakeTorque = brake;
			GetCollider (2).brakeTorque = brake;
			GetCollider (3).brakeTorque = brake;
			GetCollider (0).motorTorque = 0.0f;
			GetCollider (1).motorTorque = 0.0f;
			GetCollider (2).motorTorque = 0.0f;
			GetCollider (3).motorTorque = 0.0f;
		} else {
			GetCollider (0).brakeTorque = 0.0f;
			GetCollider (1).brakeTorque = 0.0f;
			GetCollider (2).brakeTorque = 0.0f;
			GetCollider (3).brakeTorque = 0.0f;
			GetCollider (0).motorTorque = power;
			GetCollider (1).motorTorque = power;
			GetCollider (2).motorTorque = power;
			GetCollider (3).motorTorque = power;
		}
	}

	WheelCollider GetCollider(int n) {
		return wheels [n].gameObject.GetComponent<WheelCollider> ();
	}
}

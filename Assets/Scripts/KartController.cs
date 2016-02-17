using UnityEngine;
using System.Collections;

public class KartController : MonoBehaviour {
	private Rigidbody rb;
	private Vector3 fric;
	private Vector3 wFric;

//	private Vector2 loc;
//	private float heading;
//	private float steerAngle;
//	private float wheelBase; //Imagine two wheels for simplicity. This is distance between them.

	public float friction;
	public float wFriction;
	public float accel;
	public float maxSpeed;
	public float rotSpeed;
	public float maxRotSpeed;

	void Start () {
//		loc = new Vector2 (this.transform.position.x, this.transform.position.z);
//		wheelBase = 1.0f;
//		heading = (float)Mathf.Atan2 ((loc.y + wheelBase / 2) - (loc.y - wheelBase / 2), (loc.x + wheelBase / 2) - (loc.x - wheelBase / 2));
		accel = 5.0f;
//		wFric = new Vector3 (0.0f, wFriction, 0.0f);
		rb = this.GetComponent<Rigidbody> ();	
	}

	void FixedUpdate () {
		// velocity
		if (Input.GetKey ("up")) {
			rb.AddForce (transform.forward * accel * Time.deltaTime);
		} else if (Input.GetKey ("down")) {
			rb.AddForce (transform.forward * -accel * Time.deltaTime);
		} 
		
		//angular velocity
		if (Input.GetKey ("left")) {
			rb.AddTorque (transform.up * -rotSpeed * Time.deltaTime);
		} else if (Input.GetKey ("right")) {
			rb.AddTorque (transform.up * rotSpeed * Time.deltaTime);
		} 
		//		else {
		//			Debug.Log(rb.angularVelocity);
		//			if (rb.angularVelocity.magnitude < 0.0f) {
		//				rb.angularVelocity -= new Vector3 (0.0f, 3*Time.deltaTime, 0.0f);//Vector3.Min (Vector3.zero, rb.angularVelocity + wFric * Time.deltaTime);
		////				rb.angularVelocity = new Vector3(
		////					0.0f,
		////					Mathf.Lerp(rb.angularVelocity.magnitude, 0.0f, Time.deltaTime * wFriction),
		////					0.0f
		////				);
		//			} else if (rb.angularVelocity.magnitude > 0.0f) {
		//				rb.angularVelocity = new Vector3(
		//					0.0f,
		//					Mathf.Lerp(rb.angularVelocity.magnitude, 0.0f, Time.deltaTime * wFriction),
		//					0.0f
		//				);
		//			} 
		//			Debug.Log(rb.angularVelocity);
		//		} 

//		if (Input.GetKey ("left")) {
//			steerAngle = -5.0f;
//		} else if (Input.GetKey ("right")) {
//			steerAngle = 5.0f;
//		} else {
//			steerAngle = 0.0f;
//		}
//
//		Vector2 frontWheel = loc + wheelBase / 2 * new Vector2(Mathf.Cos(heading), Mathf.Sin(heading));
//		Vector2 backWheel  = loc - wheelBase / 2 * new Vector2(Mathf.Cos(heading), Mathf.Sin(heading));
//
//		backWheel  += speed * Time.deltaTime * new Vector2(Mathf.Cos(heading), Mathf.Sin(heading));
//		frontWheel += speed * Time.deltaTime * new Vector2(Mathf.Cos(heading + steerAngle), Mathf.Sin(heading + steerAngle));
//
//		
//		loc = (frontWheel + backWheel) / 2;
//		transform.forward = Vector3.Normalize (new Vector3 (loc.x, 0.5f, loc.y) - transform.position);
//		heading = Mathf.Atan2(frontWheel.y - backWheel.y, frontWheel.x - backWheel.x);
//		transform.position = new Vector3 (loc.x, 0.5f, loc.y);

	}
}

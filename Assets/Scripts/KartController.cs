using UnityEngine;
using System.Collections;

public class KartController : MonoBehaviour {
	//TODO: consolidate where raycast are being drawn to condense code; similiar to axleinfo in KartController3.cs
	// Ideally FixedUpdate() will have a for loop so there is no repeated code
	// Implement traction;
	private Rigidbody rb;
	private Vector3 frontLeft;
	private Vector3 frontRight;
	private Vector3 backLeft;
	private Vector3 backRight;

	public float width;
	public float height;
	public float length;

	public float accel;
	public float steer;

	public float distOffGround;
	public float liftForce;
	public float liftDamp;

	void Start() {
		rb = this.GetComponent<Rigidbody> ();
	}

	//Physics stuff
	void FixedUpdate() {
		// Accleration / Braking and Turning
		if (Input.GetAxis ("Vertical") != 0) 
			rb.AddForce(transform.forward * Input.GetAxis("Vertical") * accel, ForceMode.Acceleration);
		if (Input.GetAxis ("Horizontal") != 0)
			rb.AddRelativeTorque(transform.up * Input.GetAxis ("Horizontal") * steer);

		//Set suspensions at each corner of kart
		//-transform.up points ray down towards ground
		frontRight = transform.TransformPoint(width/2, -height/2, length/2);
		frontLeft = transform.TransformPoint(-width/2, -height/2, length/2);
		backRight = transform.TransformPoint(width/2, -height/2, -length/2);
		backLeft = transform.TransformPoint(-width/2, -height/2, -length/2);

		//Grab ray hit info
		RaycastHit frHit;
		RaycastHit flHit;
		RaycastHit rrHit;
		RaycastHit rlHit;

		//Apply suspension - Makes car act more like a hovercraft than a car with springs. Needs tweaking
		if (Physics.Raycast (frontRight, -transform.up, out frHit, distOffGround)) {
			float suspError = distOffGround - frHit.distance;
			if (suspError > 0) {
				float lift = suspError * liftForce - rb.velocity.y * liftDamp;
				rb.AddForceAtPosition(Vector3.up*lift, frontRight);
			}
		}
		if (Physics.Raycast (frontLeft, -transform.up, out flHit, distOffGround)) {
			float suspError = distOffGround - flHit.distance;
			if (suspError > 0) {
				float lift = suspError * liftForce - rb.velocity.y * liftDamp;
				rb.AddForceAtPosition(Vector3.up*lift, frontLeft);
			}
		}
		if (Physics.Raycast (backRight, -transform.up, out rrHit, distOffGround)) {
			float suspError = distOffGround - rrHit.distance;
			if (suspError > 0) {
				float lift = suspError * liftForce - rb.velocity.y * liftDamp;
				rb.AddForceAtPosition(Vector3.up*lift,backRight);
			}
		}
		if (Physics.Raycast (backLeft,-transform.up, out rlHit, distOffGround)) {
			float suspError = distOffGround - frHit.distance;
			if (suspError > 0) {
				float lift = suspError * liftForce - rb.velocity.y * liftDamp;
				rb.AddForceAtPosition(Vector3.up*lift, backLeft);
			}
		}


		Debug.DrawRay (frontRight, -transform.up * distOffGround);
		Debug.DrawRay (frontLeft, -transform.up * distOffGround);
		Debug.DrawRay (backRight, -transform.up * distOffGround);
		Debug.DrawRay (backLeft, -transform.up * distOffGround);


//		if (Input.GetKey("space"))
//			rb.AddForceAtPosition(Vector3.up*25,-length/2, ForceMode.Impulse);
	}
}

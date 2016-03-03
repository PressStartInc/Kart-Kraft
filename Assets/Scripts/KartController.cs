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
	//Min Velocity to be able to turn
	public float soYouWantToTurnHuh;

	public float distOffGround;
	public float liftForce;
	public float liftDamp;

	public Texture speedOMeterDial;
	public Texture speedOMeterPointer;
	public int place;

	void Start() {
		rb = this.GetComponent<Rigidbody> ();
		rb.centerOfMass = new Vector3 (0.0f, -1.0f, 0.0f);
	}

	//Physics stuff
	void FixedUpdate() {
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

		Debug.Log (rrHit.distance);

		// Accleration / Braking and Turning

		//Make sure vehicle is grounded before applying acceleration; Better way to project transform.fwd to ground
		if (rrHit.distance <= distOffGround && rrHit.distance > 0
		    && frHit.distance <= distOffGround && rrHit.distance > 0 
		    && flHit.distance <= distOffGround && rrHit.distance > 0 
		    && rlHit.distance <= distOffGround && rrHit.distance > 0) {
			if (Input.GetAxis ("Vertical") != 0) 
				rb.AddForce (transform.forward * Input.GetAxis ("Vertical") * accel, ForceMode.Acceleration);
		}
		// Reverse inverts steering direction; Mapping to controller should handle this
		if (Input.GetAxis ("Horizontal") != 0)
			rb.AddRelativeTorque(transform.up * Mathf.Lerp(
				0.0f, 
				Input.GetAxis ("Horizontal") * steer,
				rb.velocity.magnitude - soYouWantToTurnHuh //DO I WANT TO NOMALIZE THIS?
			));
	}
	/*
	void OnGUI () {
		GUI.DrawTexture (Rect (Screen.width - 300, Screen.height - 150, 300, 150), speedOMeterDial);
		float speedFactor = currentSpeed / topSpeed;
		float rotationAngle;
		if (currentSpeed >= 0) {
			rotationAngle = Mathf.Lerp (0, 180, speedFactor);
		} else {
			rotationAngle = Mathf.Lerp (0, 180, -speedFactor);
		}
		GUIUtility.RotateAroundPivot (rotationAngle, Vector2 (Screen.width - 150, Screen.height));
		GUI.DrawTexture (Rect (Screen.width - 300, Screen.height - 150, 300, 300), speedOMeterPointer);
	}

	void Placing () {
		if(place == 1) {
			//Display 1st Place Texture
		}
		else if(place == 2) {
			//Display 2nd Place Texture
		}
		else if(place == 3) {
			//Display 3rd Place Texture
		}
		else {
			//Display 4th Place Texture
		}
	}
	*/

}

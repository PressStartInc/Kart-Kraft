using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class KartController : MonoBehaviour {
	private Rigidbody rb;
	private Vector3[] suspPoints;
	private Vector3 localVelocity;
	private bool frGround;
	private bool flGround;
	private bool rlGround;
	private bool rrGround;
	private float currAccel;
	private float currSteer;

	private bool boosting;
	private float boostDuration;
	
	private int heldItem = 0;

	public string player;

	public float width;
	public float height;
	public float length;

	public float accel;
	public float steer;
	//Min Velocity to be able to turn
	public float soYouWantToTurnHuh;
	public float maxSpeed;

	public float distOffGround;
	public float liftForce;
	public float liftDamp;

	public Texture speedOMeterDial;
	public Texture speedOMeterPointer;
	public int place;
	public Text P1Speed;
	public Text P2Speed;

	void Start() {
		rb = this.GetComponent<Rigidbody> ();
		rb.centerOfMass = new Vector3 (0.0f, -1.0f, 0.0f);

		suspPoints = new Vector3[4];
	}

	//Gather Input
	void Update() {
		//Drive / Reverse
		currAccel = 0.0f;
		float accelAxis = 0.0f; //Input.GetAxis ("Vertical");
//		if (accelAxis != 0)
//			currAccel = accelAxis * accel;

		if (Input.GetKey ("joystick " + player + " button 1")) // x
			accelAxis = 1.0f;
		else if (Input.GetKey ("joystick "+player+" button 0")) // sq
			accelAxis = -1.0f;
		currAccel = accelAxis * accel;

		//Steer
		currSteer = 0.0f;
		float steerAxis = Input.GetAxis ("p"+player+"Steer");
		//Debug.Log ("P"+player+"Steer: " + steerAxis);
		if (Mathf.Abs (steerAxis) != 0) {
			currSteer = steerAxis;
		}

		//Use item in inventory 
		if (Input.GetKey ("joystick " + player + " button 2")) // o , i think
			useItem();
		SetPlayerSpeed ();
	}

	//Physics stuff
	void FixedUpdate() {
		//Apply suspension
		//Set suspensions at each corner of kart
		suspPoints[0] = transform.TransformPoint(width/2, -height/32, length/2);
		suspPoints[1] = transform.TransformPoint(-width/2, -height/32, length/2);
		suspPoints[2] = transform.TransformPoint(width/2, -height/32, -length/2);
		suspPoints[3] = transform.TransformPoint(-width/2, -height/32, -length/2); 

		frGround = SuspensionCalculation (suspPoints [0]);
		rlGround = SuspensionCalculation (suspPoints [1]);
		flGround = SuspensionCalculation (suspPoints [2]);
		rrGround = SuspensionCalculation (suspPoints [3]);

        // Accleration / Braking and Turning
        if (boosting) {
			if (boostDuration <= 0)
				boosting = false;
			boostDuration -= Time.deltaTime;
			//rb.velocity = Vector3.forward * (float)System.Math.Sqrt(maxSpeed);
			rb.velocity = -transform.forward * 100;
		} else if (frGround || flGround || rrGround || rlGround) {
			//Make sure vehicle is grounded before applying acceleration; Better way to project transform.fwd to ground

			if (rb.velocity.sqrMagnitude < maxSpeed)
				rb.AddForce (-transform.forward * currAccel, ForceMode.Acceleration);           
		}
		//Steering
        if (currSteer != 0)
				rb.AddTorque (transform.up * Mathf.Lerp (
				0.0f, 
				currSteer * steer,
				rb.velocity.magnitude - soYouWantToTurnHuh //DO I WANT TO NOMALIZE THIS?
				));

		// Pseudo-traction/handing

		//Prevent sideways sliding of rigidbody. Set this factor between 0 and 1.
		float LateralSpeedFactor = 0.1f;
		
		//Inverse transform rigidbody velocity from world to local coordinates
		localVelocity = transform.InverseTransformDirection(rb.velocity);
		
		//Remove X (sideways) component of local velocity
		localVelocity.x *= LateralSpeedFactor;
		
		//Apply new velocity to rigidbody by transforming local velocity back to world coordinates
		rb.velocity = transform.TransformDirection(localVelocity);
	}

	bool SuspensionCalculation(Vector3 wheelLoc) {
		RaycastHit wheelHit;

		if (Physics.Raycast (wheelLoc, -transform.up, out wheelHit, distOffGround)) {
			//float suspError = distOffGround - wheelHit.distance;
			float cRatio = 1.0f - (wheelHit.distance / distOffGround);
            //if(cRatio > 0.9) rb.velocity = new Vector3(rb.velocity.x,Mathf.Abs(rb.velocity.y),rb.velocity.z);
			//if (suspError > 0) {
			float lift = cRatio * liftForce/* - rb.velocity.y * liftDamp*/;
			rb.AddForceAtPosition (transform.up * lift, wheelLoc, ForceMode.Force);
			//}
		}

		Debug.DrawRay(wheelLoc, -transform.up, Color.cyan, distOffGround);
		
		if (wheelHit.distance <= distOffGround && wheelHit.distance > 0) {
			return true;
		} else {
			return false;
		}
	}

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            if(other.gameObject.transform.parent.CompareTag("Speed Boost"))
            {
                //other.gameObject.transform.parent.gameObject.SetActive(false);
                Destroy(other.gameObject.transform.parent.gameObject);
                heldItem = 1;
            }
        }
    }

    void useItem()
    {
        if (heldItem == 0)
            return;
        if (heldItem == 1)
        {
            boosting = true;
            boostDuration = 3.0f;
        }
    }

	void SetPlayerSpeed() {
		string speed = Mathf.Abs(Mathf.Round(localVelocity.z * 8)).ToString();
		if (transform.name.Equals ("p1")) {
			P1Speed.text = speed + "Mph";
			Debug.Log ("P1Speed");
		}
		else if (transform.name.Equals ("p2")) {
			P2Speed.text = speed + "Mph";
			Debug.Log ("P2Speed");
		}
	}

	/*
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

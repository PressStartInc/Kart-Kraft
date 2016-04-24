using UnityEngine;
using System.Collections;

public class CameraEffect : MonoBehaviour {
	public KartController_pat1 c_kartController;
	public Transform target;
	public Vector2 v2_pos;
	public Vector3 v3_lookPos,v3_targetPos,v3_modTargetPos, targetDir;
	public float f_yPos, f_ylookPos;
	public float f_followSpeed,f_lookSpeed,f_ySpeed,f_yLookSpeed,f_minDistanceFromGround, f_distanceFromGround,f_lookHeight;
	public float f_maxFollowDist;
	public string s_player;
	public RaycastHit hit;
	
	private float leanAngle = 35.0f;
	private float leanSpeed = 5.0f;
	private float leanBackSpeed = 6.0f;
	
	void Start () {
		transform.parent = null;
		target = c_kartController.transform;
	}
	
	void Update () {
		float f_upMod = 0;
		v3_modTargetPos = target.TransformPoint(v3_targetPos);
		v2_pos = new Vector2(v3_modTargetPos.x,v3_modTargetPos.z);
		Vector2 v2_newPos = Vector2.MoveTowards(new Vector2(transform.position.x,transform.position.z),v2_pos,Time.deltaTime*c_kartController.f_mMaxVelocity*f_followSpeed);
		float f_yNewPos = Mathf.MoveTowards(transform.position.y,target.TransformPoint(new Vector3(0,6,-10)).y,f_ySpeed*Time.deltaTime);

		if(Physics.Raycast(new Vector3(v2_pos.x,f_yNewPos,v2_pos.y),-Vector3.up,out hit)){
			f_distanceFromGround = f_yNewPos-(hit.point.y);
			if(f_distanceFromGround < f_minDistanceFromGround)  
				f_yNewPos=Mathf.MoveTowards(transform.position.y,hit.point.y+f_minDistanceFromGround,(f_ySpeed/f_distanceFromGround)*Time.deltaTime);
		}/*
		if(Physics.Raycast(target.position,transform.position-target.position,out hit,Vector3.Distance(target.position,transform.position))) {
			float f_yCorrection = 0f;
			while(Physics.Raycast(target.position,new Vector3(transform.position.x,transform.position.y+f_yCorrection,transform.position.z)-target.position,out hit,Vector3.Distance(target.position,transform.position))){
				f_yCorrection +=0.1f;
			}
			f_upMod+= f_yCorrection;
		}
		v3_pos = new Vector3(v3_pos.x,f_upMod,v3_pos.z);
		v3_pos = target.TransformPoint(new Vector3(0,6,-10));*/

		transform.position = new Vector3(v2_newPos.x,f_yNewPos,v2_newPos.y);//Vector3.MoveTowards(transform.position,v3_pos,Time.deltaTime*c_kartController.f_mMaxVelocity*f_followSpeed);
		targetDir = new Vector3(target.position.x,target.position.y+f_lookHeight,target.position.z) - transform.position;
		f_ylookPos = Mathf.MoveTowards(f_ylookPos,targetDir.y,f_yLookSpeed*Time.deltaTime);
		float step = f_lookSpeed * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(transform.forward, new Vector3(targetDir.x,f_ylookPos,targetDir.z), step, 0.0F);
		Debug.DrawRay(transform.position, newDir, Color.red);
		transform.rotation = Quaternion.LookRotation(newDir);
		GetComponent<Camera>().fieldOfView = Mathf.SmoothStep(60,100,Mathf.Clamp((c_kartController.f_mVelocity-c_kartController.f_mMaxVelocity/2f)/(c_kartController.f_mMaxVelocity-c_kartController.f_mMaxVelocity/2f),0f,Mathf.Infinity));
		//transform.localPosition = new Vector3(0,6+Mathf.MoveTowards(6,,c_kartController.f_mVelocity/c_kartController.f_mMaxVelocity),-10-Mathf.MoveTowards(0,20,c_kartController.f_mVelocity/c_kartController.f_mMaxVelocity));
		
		float f_dist = Vector3.Distance(target.TransformPoint(v3_targetPos), this.transform.position);
		Debug.Log(f_dist);
	}
	
	void FixedUpdate() {
		// if (Input.GetAxis("p"+s_player+"CamY") > 0)
			// this.transform.RotateAround(target.position, target.right , 10.0f * Time.deltaTime);
		// else if (Input.GetAxis("p"+s_player+"CamY") < 0)
			// this.transform.RotateAround(target.position, target.right , 10.0f * Time.deltaTime);
		// if (Input.GetAxis("p"+s_player+"CamX") > 0) {
			// this.transform.Translate(new Vector3(10.0f * Time.deltaTime, 0, 0)); 
			// Vector3.RotateTowards(transform.forward, new Vector3(targetDir.x,f_ylookPos,targetDir.z), 1.25f, 0.0F);
		// } else if (Input.GetAxis("p"+s_player+"CamX") < 0) {
			// this.transform.Translate(new Vector3(-10.0f * Time.deltaTime, 0, 0));
			// Vector3.RotateTowards(transform.forward, new Vector3(targetDir.x,f_ylookPos,targetDir.z), 1.25f, 0.0F);
		// }
		
		if (Input.GetAxis("p"+s_player+"CamY") > 0)
			transform.LookAt(target.transform.position+new Vector3(0, 1f, 0));// doLeanUp();
		else if (Input.GetAxis("p"+s_player+"CamY") < 0)
			transform.LookAt(target.transform.position+new Vector3(0, -0.5f, 0));// doLeanDown();
		else if (Input.GetAxis("p"+s_player+"CamX") > 0) 
			transform.LookAt(target.transform.position+new Vector3(-1f, 0, 0));		// doLeanRight();
		else if (Input.GetAxis("p"+s_player+"CamX") < 0) 
			transform.LookAt(target.transform.position+new Vector3(1f, 0, 0)); //doLeanLeft();
		else if (Input.GetButton("p"+s_player+"RevCam"))
			this.transform.LookAt(2*transform.position - target.position);
	}
	
	void doLeanLeft()
	{
	 // current Z-rotation
     float currAngle = transform.rotation.eulerAngles.z;
     //Quaternion rot  = transform.rotation;
     
     // target Z-rotation
     float targetAngle = leanAngle;
     
     if ( currAngle > 180.0 )
     {
         //targetAngle = 0.0 - leanAngle;
         currAngle = 360 - currAngle;
     }
     
     //lerp value from current to target
     float angle = Mathf.Lerp( currAngle, targetAngle, leanSpeed * Time.deltaTime );
     
     Debug.Log ( "Left : currAngle " + currAngle + " : targetAngle " + targetAngle + " : angle " + angle );
     
     // rotate char
     Quaternion rotAngle = Quaternion.Euler( transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, angle );
     transform.rotation = rotAngle;
	}
	
	void doLeanRight()
	{
	 // current Z-rotation
     float currAngle = transform.rotation.eulerAngles.z;
     
     // target Z-rotation
     float targetAngle = leanAngle - 360.0f;
     
     if ( currAngle > 180.0 )
     {
         targetAngle = 360.0f - leanAngle;
     }
     
     //lerp value from current to target
     float angle = Mathf.Lerp( currAngle, targetAngle, leanSpeed * Time.deltaTime );
     
     //Debug.Log ( "Right : currAngle " + currAngle + " : targetAngle " + targetAngle + " : angle " + angle );
     
     // rotate char
     Quaternion rotAngle = Quaternion.Euler( transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, angle );
     transform.rotation = rotAngle;
	}
	
	// void doLeanUp()
	// {
		// transform.rotation = Quaternion.Euler(0,0,-30);
	// }
	
	// void doLeanDown()
	// {
		// transform.rotation = Quaternion.Euler(0,0,30);
	// }
}

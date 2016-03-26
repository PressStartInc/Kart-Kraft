using UnityEngine;
using System.Collections;

public class CameraEffect : MonoBehaviour {
	public KartController_pat c_kartController;
	public Transform target;
	public Vector2 v2_pos;
	public Vector3 v3_lookPos,v3_targetPos,v3_modTargetPos;
	public float f_yPos, f_ylookPos;
	public float f_followSpeed,f_lookSpeed,f_ySpeed,f_yLookSpeed,f_minDistanceFromGround, f_distanceFromGround,f_lookHeight;
	public RaycastHit hit;
	// Use this for initialization
	void Start () {
		transform.parent = null;
		target = c_kartController.transform;
	}
	
	// Update is called once per frame
	void Update () {
	float f_upMod = 0;
	v3_modTargetPos = target.TransformPoint(v3_targetPos);
	v2_pos = new Vector2(v3_modTargetPos.x,v3_modTargetPos.z);
	Vector2 v2_newPos = Vector2.MoveTowards(new Vector2(transform.position.x,transform.position.z),v2_pos,Time.deltaTime*c_kartController.f_zMaxVelocity*f_followSpeed);
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

	transform.position = new Vector3(v2_newPos.x,f_yNewPos,v2_newPos.y);//Vector3.MoveTowards(transform.position,v3_pos,Time.deltaTime*c_kartController.f_zMaxVelocity*f_followSpeed);
    Vector3 targetDir = new Vector3(target.position.x,target.position.y+f_lookHeight,target.position.z) - transform.position;
    f_ylookPos = Mathf.MoveTowards(f_ylookPos,targetDir.y,f_yLookSpeed*Time.deltaTime);
    float step = f_lookSpeed * Time.deltaTime;
    Vector3 newDir = Vector3.RotateTowards(transform.forward, new Vector3(targetDir.x,f_ylookPos,targetDir.z), step, 0.0F);
    Debug.DrawRay(transform.position, newDir, Color.red);
    transform.rotation = Quaternion.LookRotation(newDir);
	GetComponent<Camera>().fieldOfView = Mathf.SmoothStep(60,100,Mathf.Clamp((c_kartController.f_zVelocity-c_kartController.f_zOrigMaxVelocity/2f)/(c_kartController.f_zOrigMaxVelocity-c_kartController.f_zOrigMaxVelocity/2f),0f,Mathf.Infinity));
	//transform.localPosition = new Vector3(0,6+Mathf.MoveTowards(6,,c_kartController.f_zVelocity/c_kartController.f_zMaxVelocity),-10-Mathf.MoveTowards(0,20,c_kartController.f_zVelocity/c_kartController.f_zMaxVelocity));
	}
}

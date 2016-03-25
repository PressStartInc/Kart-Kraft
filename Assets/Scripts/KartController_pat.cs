using UnityEngine;
using System.Collections;

public class KartController_pat : MonoBehaviour {
	public float f_zVelocity, f_yVelocity,f_realVelocity,f_zMaxVelocity,f_yMaxVelocity,f_downHill;
	public float f_zOrigMaxVelocity;
	public float f_gravity;
	public float f_zPrev, f_zCur;
	public float f_yPrev,f_yCur;
	public float f_accel, f_timeToStop;
	private float f_stopVelocity;
	public float f_selfBottom;
	public float f_groundPadding;
	public float f_angle,f_prevAngle,f_angleDelta;
	public float f_turnStrength,f_modTurnStrength;
	public bool b_grounded;
	private RaycastHit hit;
	public bool b_init;
	public LayerMask lm_rayLayers;
	// Use this for initialization
	void Start () {
		f_zOrigMaxVelocity = f_zMaxVelocity;
		f_yPrev = f_yCur = transform.position.y;
		Physics.Raycast(new Vector3(transform.position.x,transform.position.y-transform.localScale.y*2,transform.position.z),transform.up,out hit);
		f_selfBottom = transform.position.y-hit.point.y;
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
	if(!b_init) {
		Physics.Raycast(transform.position,-transform.up,out hit);
		if(hit.transform != null) {
			transform.position = new Vector3(hit.point.x,hit.point.y+f_selfBottom,hit.point.z);
			b_init = true;
		}
	}
	else {
		f_zCur = transform.position.z;
		f_yCur = transform.position.y;
		f_angle = Mathf.Atan2((f_yCur-f_yPrev),(f_zCur-f_zPrev));
		f_angleDelta = (f_angle-f_prevAngle)*Time.smoothDeltaTime;
		if(f_yCur > f_yPrev && b_grounded) {
			f_yVelocity = Mathf.Clamp((f_yCur-f_yPrev)/Time.smoothDeltaTime,-f_yMaxVelocity,Mathf.Infinity);
		}
		else if(b_grounded)f_yVelocity = 0f;
	if(Input.GetKey(KeyCode.W) && b_grounded)
		f_zVelocity = Mathf.Clamp(f_zVelocity + f_accel*Time.smoothDeltaTime,-f_zMaxVelocity,f_zMaxVelocity);
	//if(Input.GetKey(KeyCode.S) && b_grounded)
	//	f_zVelocity = Mathf.Clamp(f_zVelocity - f_accel*Time.smoothDeltaTime,-f_zMaxVelocity,f_zMaxVelocity);
	else if(b_grounded) {
		f_zVelocity = Mathf.Clamp(Mathf.SmoothDamp(f_zVelocity,0f,ref f_stopVelocity,f_timeToStop),-f_zMaxVelocity,f_zMaxVelocity);
		if(f_zVelocity < 0.01f) f_zVelocity = 0f;
	}
	//else f_zVelocity =
	if(!b_grounded) {
			f_yVelocity = Mathf.Clamp(f_yVelocity + f_gravity*Time.smoothDeltaTime,-f_yMaxVelocity,Mathf.Infinity);
			transform.Translate(0,(f_yVelocity)*Time.smoothDeltaTime,0);			
		}
	else {
		f_modTurnStrength = f_turnStrength*Mathf.Abs(1f-Mathf.Abs(Mathf.Abs(f_zVelocity)-f_zMaxVelocity*0.6f)/(f_zMaxVelocity*0.6f));
			if	(Input.GetKey(KeyCode.A)) transform.Rotate(0,-f_modTurnStrength*Time.smoothDeltaTime,0);
		else if(Input.GetKey(KeyCode.D)) transform.Rotate(0,f_modTurnStrength*Time.smoothDeltaTime,0);
	}
	//else f_yVelocity = 0;
	//Debug.Log(f_yVelocity*Time.smoothDeltaTime);
	if(f_yVelocity <= 0){
		if(b_grounded)
			Physics.Raycast(new Vector3(transform.position.x,transform.position.y+f_selfBottom-0.05f,transform.position.z),-transform.up,out hit,(-(f_yVelocity)+f_groundPadding)*Time.smoothDeltaTime+(f_selfBottom*2f),lm_rayLayers);
		else
			Physics.Raycast(new Vector3(transform.position.x,transform.position.y+f_selfBottom-0.05f,transform.position.z),-transform.up,out hit,(-(f_yVelocity))*Time.smoothDeltaTime+(f_selfBottom*2f),lm_rayLayers);
		if(hit.transform != null) {
			transform.position = new Vector3(transform.position.x,hit.point.y+f_selfBottom-0.05f,transform.position.z);
			b_grounded = true;
			}
		else if(b_grounded){
			//print("down: (-(f_yVelocity)+f_groundPadding)*Time.smoothDeltaTime+(f_selfBottom*2f))");
			//print("vals: " + "(-("+f_yVelocity+")+"+f_groundPadding+")*"+Time.smoothDeltaTime+"+("+f_selfBottom+"*"+2f+"))");
			//print("tots: " + (-(f_yVelocity)+f_groundPadding)*Time.smoothDeltaTime+(f_selfBottom*2f));
			b_grounded = false;
		}
	}
	else {
		//if(Physics.Raycast(new Vector3(transform.position.x,transform.position.y+f_selfBottom-0.05f,transform.position.z),-transform.up,out hit,(1f-(f_zVelocity/f_zMaxVelocity))*(f_groundPadding)*Time.smoothDeltaTime+f_selfBottom*2f,lm_rayLayers)) {
		//print((f_angleDelta) + ", " + f_yVelocity);
		Debug.DrawRay(transform.position,-Vector3.up*(f_selfBottom*2f+f_angleDelta-f_yVelocity*Time.smoothDeltaTime),Color.red);
		if(Physics.Raycast(new Vector3(transform.position.x,transform.position.y+f_selfBottom-0.05f,transform.position.z),-transform.up,out hit,(f_selfBottom*2f+f_angleDelta-f_yVelocity*Time.smoothDeltaTime),lm_rayLayers)) {
			//print(hit.transform.name);			
			transform.position = new Vector3(transform.position.x,hit.point.y+f_selfBottom,transform.position.z);
			b_grounded = true;
			}
		else if(b_grounded){
			b_grounded = false;
			print("ray: " + (f_groundPadding*(Mathf.Clamp(1f-f_angle/(Mathf.PI/4f),0f,1f))) + " * " + Time.smoothDeltaTime+ " + " + (f_selfBottom*2f));
			print("total: " + ((f_groundPadding*(Mathf.Clamp(1f-f_angle/(Mathf.PI/4f),0f,1f)))*Time.smoothDeltaTime+(f_selfBottom*2f)));
			print("angle: " + f_angle + ", " + f_yVelocity);
		}
	}
	//if(b_grounded)f_zVelocity+= -Mathf.Clamp(f_angle/(Mathf.PI/4f),-1f,1f)*f_accel*Time.smoothDeltaTime;
	//f_zMaxVelocity = f_zOrigMaxVelocity/2f+f_zOrigMaxVelocity*Mathf.Clamp(1-f_angle/(Mathf.PI/8f),0f,1f);

	f_downHill += Mathf.Clamp(-f_angle*Time.deltaTime,-1f,1f)*(Mathf.Abs(f_zVelocity))*1f*Time.smoothDeltaTime;
	f_downHill = Mathf.Clamp(f_downHill,0,f_zMaxVelocity*0.5f);
	if(f_yVelocity >= 0) {
		//f_realVelocity = (f_zVelocity/(1f/Mathf.Cos(f_angle)));
		f_realVelocity = f_zVelocity;

		}
	if(f_yVelocity < 0) {
		f_realVelocity = f_zVelocity;
		} 
	//else f_realVelocity = f_zVelocity;
	transform.Translate(Vector3.forward*(f_realVelocity+f_downHill)*Time.smoothDeltaTime);
	if(f_downHill > 0.1f)f_downHill+= -Time.smoothDeltaTime*0.1f;
	else if(f_downHill < -0.1f) f_downHill += Time.smoothDeltaTime*0.1f;

	//print(f_zVelocity/Mathf.Cos(f_angle));
	f_zPrev = f_zCur;
	f_yPrev = f_yCur;
	f_prevAngle = f_angle;
	}
}
}

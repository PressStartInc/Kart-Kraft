using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KartController_pat1 : MonoBehaviour {
	public c_terraingen_r6 c_terrainGen;
	public c_kartAngleCalc c_kartAngleCalc;
	public KartState state;
	public bool b_AI;
	public bool b_AIForward;
	public bool b_onTrack, b_bumperOnTrack;
	public int i_AIDirection = -1;
	public Vector2 v2_pos, v2_bumperPos;
	public List<Vector2> v2_tracks;
	public float f_radius;
	public float f_acceleration, f_gravity;
	public float f_mVelocity, f_zVelocity, f_yVelocity;
	public float f_mMaxVelocity;
	public float f_zAngle, f_zAngleDelta,f_angleBreak;
	public float f_prevY,f_curY,f_prevZ,f_curZ,f_yDelta,f_zDelta;
	public float f_ySample;
	public float f_slowDownValue, f_handbreakValue;
	public float f_turnStrength,f_modTurnStrength;
	public float f_bumperY;
	public string s_player;
	
	public enum KartState {
		init,
		grounded,
		airborne
	}
	
	public void Start(){
		f_radius = transform.lossyScale.y/2f;	
	}
	
	public void Update(){
	v2_pos = new Vector2(transform.position.x,transform.position.z);
	transform.Translate(0,0,f_zVelocity*Time.deltaTime);
	v2_tracks = c_terrainGen.v2_tracks;
	f_curY = transform.position.y;
	f_curZ = transform.position.z;
	b_onTrack = false;
	b_bumperOnTrack = false;
	if(f_mVelocity > 0f)
		v2_bumperPos = new Vector2(transform.TransformPoint(0,0,c_kartAngleCalc.f_kartLength/2f).x,transform.TransformPoint(0,0,c_kartAngleCalc.f_kartLength/2f).z);
	else if(f_mVelocity < 0f)
		v2_bumperPos= new Vector2(transform.TransformPoint(0,0,-c_kartAngleCalc.f_kartLength/2f).x,transform.TransformPoint(0,0,-c_kartAngleCalc.f_kartLength/2f).z);
	else v2_bumperPos = v2_pos;
	if(Physics.Raycast(transform.TransformPoint(0,10,0),-Vector3.up,c_terrainGen.i_yRes,c_terrainGen.lm_trackCheck)){
			b_onTrack = true;
		}
		if(f_mVelocity >= 0){
			if(b_bumperOnTrack)
				f_bumperY = c_terrainGen.SampleTerrain(v2_bumperPos,c_terrainGen.f_trackRoughness)*c_terrainGen.i_yRes+f_radius;
			else 
				f_bumperY = Mathf.Ceil(c_terrainGen.SampleTerrain(new Vector2(Mathf.Round(v2_bumperPos.x),Mathf.Round(v2_bumperPos.y)),c_terrainGen.f_blendAmount)*c_terrainGen.i_yRes)+f_radius;
			}
		else {
			if(b_bumperOnTrack)
				f_bumperY = c_terrainGen.SampleTerrain(v2_bumperPos,c_terrainGen.f_trackRoughness)*c_terrainGen.i_yRes+f_radius;
			else 
				f_bumperY = Mathf.Ceil(c_terrainGen.SampleTerrain(new Vector2(Mathf.Round(v2_bumperPos.x),Mathf.Round(v2_bumperPos.y)),c_terrainGen.f_blendAmount)*c_terrainGen.i_yRes)+f_radius;
			}

		f_zDelta = (f_curZ-f_prevZ)/Time.deltaTime;
		f_yDelta = (f_curY-f_prevY)/Time.deltaTime;


		switch(state) {
			case KartState.init:
				Init();
				break;
			case KartState.grounded:
				Grounded();
				break;
			case KartState.airborne:
				Airborne();
				break;
			}
		f_prevY = f_curY;
		f_prevZ = f_curZ;
	}
	
	public void Init() {
		f_ySample = c_terrainGen.SampleTerrain(v2_pos,c_terrainGen.f_trackRoughness)*c_terrainGen.i_yRes+f_radius;
		transform.position = new Vector3(transform.position.x,f_ySample,transform.position.z);	
		state = KartState.grounded;
	}
	
	public void Grounded() {
		f_zAngleDelta = (-Mathf.Atan2((f_yDelta),(f_zDelta))-f_zAngle);
		f_zAngle = -Mathf.Atan2((f_yDelta),(f_zDelta));//print(f_zAngle + " " + c_kartAngleCalc.f_xAngle);
		f_angleBreak = 20f*Mathf.Clamp(1f-Mathf.Clamp(f_mVelocity,0,f_mMaxVelocity)/(f_mMaxVelocity*1.01f),0,1f);
		
		float f_zRadians = c_kartAngleCalc.f_xAngle * Mathf.PI/180f;
		float f_zVelocityRatio = Mathf.Cos(f_zRadians);
		float f_yVelocityRatio = Mathf.Sin(f_zRadians);
		f_zVelocity = f_zVelocityRatio*f_mVelocity;

		//f_yVelocity = -f_yVelocityRatio*f_mVelocity;
		f_yVelocity = f_yDelta;

		f_modTurnStrength = f_turnStrength*Mathf.Abs(1f-Mathf.Abs(Mathf.Abs(f_mVelocity)-f_mMaxVelocity*0.75f)/(f_mMaxVelocity*0.75f));		

		if(b_onTrack) {
			f_ySample = c_terrainGen.SampleTerrain(v2_pos,c_terrainGen.f_trackRoughness)*c_terrainGen.i_yRes+f_radius;
		}
		else {
			float f_ySampleBL,f_ySampleTL,f_ySampleBR,f_ySampleTR;
			f_ySampleBL = Mathf.Ceil(c_terrainGen.SampleTerrain(new Vector2(Mathf.Round(v2_pos.x)-0.5f,Mathf.Round(v2_pos.y)-0.5f),c_terrainGen.f_blendAmount)*c_terrainGen.i_yRes)+f_radius; 
			f_ySampleTL = Mathf.Ceil(c_terrainGen.SampleTerrain(new Vector2(Mathf.Round(v2_pos.x)-0.5f,Mathf.Round(v2_pos.y)+0.5f),c_terrainGen.f_blendAmount)*c_terrainGen.i_yRes)+f_radius; 
			f_ySampleBR = Mathf.Ceil(c_terrainGen.SampleTerrain(new Vector2(Mathf.Round(v2_pos.x)+0.5f,Mathf.Round(v2_pos.y)-0.5f),c_terrainGen.f_blendAmount)*c_terrainGen.i_yRes)+f_radius; 
			f_ySampleTR = Mathf.Ceil(c_terrainGen.SampleTerrain(new Vector2(Mathf.Round(v2_pos.x)+0.5f,Mathf.Round(v2_pos.y)+0.5f),c_terrainGen.f_blendAmount)*c_terrainGen.i_yRes)+f_radius; 
			f_ySample = Mathf.Lerp(Mathf.Lerp(f_ySampleBL,f_ySampleTL,v2_pos.y-(Mathf.Round(v2_pos.y)-0.5f)),Mathf.Lerp(f_ySampleBR,f_ySampleTR,v2_pos.y-(Mathf.Round(v2_pos.y)-0.5f)),v2_pos.x-(Mathf.Round(v2_pos.x)-0.5f));
		}

		transform.position = new Vector3(transform.position.x,f_ySample,transform.position.z);	


		if	((!b_AI && (Input.GetAxis("p"+s_player+"Steer") < 0)|| Input.GetKey(KeyCode.A)) || (b_AI && i_AIDirection == 0)){
			if(f_mVelocity > 0)
				transform.Rotate(0,-f_modTurnStrength*Time.deltaTime,0);
			else
				transform.Rotate(0,f_modTurnStrength*Time.deltaTime,0);
		}
		else if((!b_AI && (Input.GetAxis("p"+s_player+"Steer") > 0) || Input.GetKey(KeyCode.D)) || (b_AI && i_AIDirection == 1)) {
			if(f_mVelocity > 0)
				transform.Rotate(0,f_modTurnStrength*Time.deltaTime,0);
			else 
				transform.Rotate(0,-f_modTurnStrength*Time.deltaTime,0);
		}
		
		if((!b_AI && (Input.GetAxis("p"+s_player+"Accel") > 0) || Input.GetKey(KeyCode.W)) || (b_AI && b_AIForward)) {
			f_mVelocity = f_mVelocity+f_acceleration*Time.deltaTime;
			if(f_mVelocity < 0)
				f_mVelocity = f_mVelocity+f_handbreakValue*Time.deltaTime;

		}
		else if((!b_AI && (Input.GetAxis("p"+s_player+"Accel") < 0) || Input.GetKey(KeyCode.S))) {
			f_mVelocity = f_mVelocity-f_acceleration*Time.deltaTime;
			if(f_mVelocity > 0)
				f_mVelocity = f_mVelocity-f_handbreakValue*Time.deltaTime;
		}
		else if(f_mVelocity > 0.1f) f_mVelocity = f_mVelocity-f_acceleration*f_slowDownValue*Time.deltaTime;
		else if(f_mVelocity < -0.1f) f_mVelocity = f_mVelocity+f_acceleration*f_slowDownValue*Time.deltaTime;
		else if(f_mVelocity >= -0.1f && f_mVelocity <= 0.1f) f_mVelocity = 0;
		else f_mVelocity = 0f;
		
		if (!b_AI && Input.GetButton("p"+s_player+"HBrake"))
			f_mVelocity = f_mVelocity - f_acceleration * 1.5f * Time.deltaTime;
		
		//if (Input.GetButton("p"+s_player+"Item"))
			//UseItem();
		if(b_onTrack) {
			if(f_mVelocity > f_mMaxVelocity) f_mVelocity = f_mVelocity-f_acceleration*1.1f*Time.deltaTime;
			if(f_mVelocity < -f_mMaxVelocity*0.5f) f_mVelocity = f_mVelocity+f_acceleration*1.1f*Time.deltaTime;
		}
		else {
			if(f_mVelocity > f_mMaxVelocity*0.75f) f_mVelocity = f_mVelocity-f_acceleration*1.1f*Time.deltaTime;
			if(f_mVelocity < -f_mMaxVelocity*0.5f*0.75f) f_mVelocity = f_mVelocity+f_acceleration*1.1f*Time.deltaTime;
		}
		//if(f_bumperY > f_curY+0.5f && !b_bumperOnTrack)
		//	f_mVelocity = -f_mVelocity*0.5f;
		
		if(b_onTrack && (f_zAngleDelta-f_zAngle*0.01f) > f_angleBreak)
			state = KartState.airborne;
	}

	public void Airborne() {
		f_yVelocity = f_yVelocity+f_gravity*Time.deltaTime;
		float f_ySample;
		if(b_onTrack) {
			f_ySample = c_terrainGen.SampleTerrain(v2_pos,c_terrainGen.f_trackRoughness)*c_terrainGen.i_yRes+f_radius;

		}
		else {
			f_ySample = Mathf.Ceil(c_terrainGen.SampleTerrain(v2_pos,c_terrainGen.f_blendAmount)*c_terrainGen.i_yRes)+f_radius;
		}
		if((transform.position.y+f_yVelocity*Time.deltaTime) < f_ySample && f_ySample-f_bumperY < 0.5f){
			transform.position = new Vector3(transform.position.x,f_ySample,transform.position.z);
			state = KartState.grounded;
		}
		else transform.Translate(0,f_yVelocity*Time.deltaTime,0);
	}

}

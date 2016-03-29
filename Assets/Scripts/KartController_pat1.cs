using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KartController_pat1 : MonoBehaviour {
	public c_terraingen_r4 c_terrainGen;
	public c_kartAngleCalc c_kartAngleCalc;
	public KartState state;
	public bool b_AI;
	public bool b_AIForward;
	public int i_AIDirection = -1;
	public Vector2 v2_pos;
	public List<Transform> l_track;
	private float f_radius;
	public float f_acceleration, f_gravity;
	public float f_mVelocity, f_zVelocity, f_yVelocity;
	public float f_mMaxVelocity;
	public float f_zAngle, f_zAngleDelta,f_angleBreak;
	public float f_prevY,f_curY,f_prevZ,f_curZ,f_yDelta,f_zDelta;
	public float f_turnStrength,f_modTurnStrength;
	public enum KartState {
		init,
		grounded,
		airborne
	}
	public void Start(){
		f_radius = transform.lossyScale.y/2f;
		state = KartState.grounded;
	}
	public void FixedUpdate(){
	v2_pos = new Vector2(transform.position.x,transform.position.z);
	transform.Translate(0,0,f_zVelocity*Time.deltaTime);
	l_track = c_terrainGen.l_track;
	f_curY = transform.position.y;
	f_curZ = transform.position.z;
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
	}
	public void Grounded() {

		
		bool b_onTrack = false;
		
		f_zAngleDelta = (-Mathf.Atan2((f_yDelta),(f_zDelta))-f_zAngle);
		f_zAngle = -Mathf.Atan2((f_yDelta),(f_zDelta));//print(f_zAngle + " " + c_kartAngleCalc.f_xAngle);
		f_angleBreak = 5f*Mathf.Clamp(1f-Mathf.Clamp(f_mVelocity,0,f_mMaxVelocity)/(f_mMaxVelocity*1.01f),0,1f);
		
		float f_zRadians = c_kartAngleCalc.f_xAngle * Mathf.PI/180f;
		float f_zVelocityRatio = Mathf.Cos(f_zRadians);
		float f_yVelocityRatio = Mathf.Sin(f_zRadians);
		f_zVelocity = f_zVelocityRatio*f_mVelocity;

		//f_yVelocity = -f_yVelocityRatio*f_mVelocity;
		f_yVelocity = f_yDelta;

		f_modTurnStrength = f_turnStrength*Mathf.Abs(1f-Mathf.Abs(Mathf.Abs(f_mVelocity)-f_mMaxVelocity*0.75f)/(f_mMaxVelocity*0.75f));
		float f_ySample;		
		for(int i = 0; i < l_track.Count;i++) {
			if(l_track[i].transform.position.x == Mathf.Floor(v2_pos.x) && l_track[i].transform.position.z == Mathf.Floor(v2_pos.y))
				b_onTrack = true;
		}
		
		if((f_zAngleDelta)-f_zAngle*0.01f > f_angleBreak)
			state = KartState.airborne;
		if(b_onTrack) {
			f_ySample = c_terrainGen.SampleTerrain(v2_pos,c_terrainGen.f_trackRoughness)*c_terrainGen.i_yRes+5f+f_radius;
		}
		else {
			f_ySample = Mathf.Ceil(c_terrainGen.SampleTerrain(v2_pos,c_terrainGen.f_blendAmount)*c_terrainGen.i_yRes+5f+f_radius);
		}

		transform.position = new Vector3(transform.position.x,f_ySample,transform.position.z);	


		if	(Input.GetKey(KeyCode.A) || (b_AI && i_AIDirection == 0))transform.Rotate(0,-f_modTurnStrength*Time.deltaTime,0);
		else if(Input.GetKey(KeyCode.D) || (b_AI && i_AIDirection == 1))transform.Rotate(0,f_modTurnStrength*Time.deltaTime,0);
		if(Input.GetKey(KeyCode.W) || (b_AI && b_AIForward)) {
			f_mVelocity = f_mVelocity+f_acceleration*Time.deltaTime;
		}
		else if(f_mVelocity > 0f) f_mVelocity = f_mVelocity-f_acceleration*2*Time.deltaTime;
		else f_mVelocity = 0f;
		if(f_mVelocity > f_mMaxVelocity) f_mVelocity = f_mVelocity-f_acceleration*1.1f*Time.deltaTime;
	}

	public void Airborne() {
		f_yVelocity = f_yVelocity+f_gravity*Time.deltaTime;

			bool b_onTrack = false;
		for(int i = 0; i < l_track.Count;i++) {
			if(l_track[i].transform.position.x == Mathf.Floor(v2_pos.x) && l_track[i].transform.position.z == Mathf.Floor(v2_pos.y))
				b_onTrack = true;
		}

		float f_ySample;
		if(b_onTrack) {
			f_ySample = c_terrainGen.SampleTerrain(v2_pos,c_terrainGen.f_trackRoughness)*c_terrainGen.i_yRes+5f+f_radius;
		}
		else {
			f_ySample = Mathf.Ceil(c_terrainGen.SampleTerrain(v2_pos,c_terrainGen.f_blendAmount)*c_terrainGen.i_yRes+5f+f_radius);
		}
		if((transform.position.y+f_yVelocity*Time.deltaTime) < f_ySample){
			transform.position = new Vector3(transform.position.x,f_ySample,transform.position.z);
			state = KartState.grounded;
		}
		else transform.Translate(0,f_yVelocity*Time.deltaTime,0);
	}

}